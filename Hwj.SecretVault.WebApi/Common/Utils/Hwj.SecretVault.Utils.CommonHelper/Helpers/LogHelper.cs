using Info.Storage.Utils.CommonHelper.Extensions;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Info.Storage.Utils.CommonHelper.Helpers
{
    /// <summary>
    /// 日志工具类
    /// </summary>
    public static class LogHelper
    {
        #region 私有变量

        /// <summary>
        /// 线程安全队列
        /// </summary>
        private static readonly ConcurrentQueue<LogMessage> _queue;

        /// <summary>
        /// 信号
        /// </summary>
        private static readonly ManualResetEvent _mre;

        /// <summary>
        /// 日志写锁
        /// </summary>
        private static readonly ReaderWriterLockSlim _lock;

        #endregion 私有变量

        #region 构造函数

        static LogHelper()
        {
            _queue = new ConcurrentQueue<LogMessage>();
            _mre = new ManualResetEvent(false);
            _lock = new ReaderWriterLockSlim();
            Task.Run(() => Initalize());
        }

        #endregion 构造函数

        #region 信息日志

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="args">字符串格式化参数</param>
        public static void Info(string message, params object[] args)
        {
            StackFrame sf = new StackTrace(true).GetFrame(1);
            LogMessage logMessage = new LogMessage
            {
                Level = LogLevel.Info,
                Message = string.Format((message.Replace("{", "{{").Replace("}", "}}") ?? "").ReplaceOfRegex("{$1}", @"{{(\d+)}}"), args),
                StackFrame = sf
            };
            _queue.Enqueue(logMessage);
            _mre.Set();
        }

        #endregion 信息日志

        #region 错误日志

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error(string message, params object[] args)
        {
            StackFrame sf = new StackTrace(true).GetFrame(1);
            LogMessage logMessage = new LogMessage
            {
                Level = LogLevel.Error,
                Message = string.Format((message?.Replace("{", "{{").Replace("}", "}}") ?? "").ReplaceOfRegex("{$1}", @"{{(\d+)}}"), args),
                StackFrame = sf
            };
            _queue.Enqueue(logMessage);
            _mre.Set();
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error(Exception ex, string message = "", params object[] args)
        {
            StackFrame sf = null;
            if (ex != null)
            {
                StackFrame[] frames = new StackTrace(ex, true).GetFrames();
                sf = frames[frames.Length - 1];
            }
            else
                sf = new StackTrace(true).GetFrame(1);

            LogMessage logMessage = new LogMessage
            {
                Level = LogLevel.Error,
                Exception = ex,
                Message = string.Format((message?.Replace("{", "{{").Replace("}", "}}") ?? "").ReplaceOfRegex("{$1}", @"{{(\d+)}}"), args),
                StackFrame = sf
            };
            _queue.Enqueue(logMessage);
            _mre.Set();
        }

        #endregion 错误日志

        #region 日志初始化

        private static void Initalize()
        {
            while (true)
            {
                // 等待信号通知
                _mre.WaitOne();
                // 重新设置新信号
                _mre.Reset();
                Thread.Sleep(1);
            }
        }

        #endregion 日志初始化

        #region 写入日志

        private static void Write()
        {
            // 获取日志文件的物理路径
            string infoDir = (ConfigHelper.Configuration.GetSection("Logs")["Info"] ?? @"Logs/Info").GetPhysicalPath();
            string errorDir = (ConfigHelper.Configuration.GetSection("Logs")["Error"] ?? @"Logs/Error").GetPhysicalPath();

            // 根据当天日期创建日志文件
            string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}.log";
            string infoPath = infoDir + fileName;
            string errorPath = errorDir + fileName;
            try
            {
                // 进入写锁
                _lock.EnterWriteLock();
                // 判断目录是否存在，不存在则重新创建
                if (!Directory.Exists(infoDir)) Directory.CreateDirectory(infoDir);
                if (!Directory.Exists(errorDir)) Directory.CreateDirectory(errorDir);
                // 创建StreamWriter
                StreamWriter swInfo = null;
                StreamWriter swError = null;

                if (_queue?.ToList().Exists(log => log.Level == LogLevel.Info) == true)
                {
                    swInfo = new StreamWriter(infoPath, true, encoding: Encoding.UTF8);
                }
                if (_queue?.ToList().Exists(log => log.Level == LogLevel.Error) == true)
                {
                    swError = new StreamWriter(errorPath, true, encoding: Encoding.UTF8);
                }
                // 判断日志队列中是否有内容，从队列中获取内容，并删除队列中的内容
                while (_queue?.Count > 0 && _queue.TryDequeue(out LogMessage logMessage))
                {
                    StackFrame sf = logMessage.StackFrame;
                    // Info
                    if (swInfo != null && logMessage.Level == LogLevel.Info)
                    {
                        swInfo.WriteLine($"[级别：Info]");
                        swInfo.WriteLine($"[时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}]");
                        swInfo.WriteLine($"[类名：{sf?.GetMethod().DeclaringType.FullName}]");
                        swInfo.WriteLine($"[方法：{sf?.GetMethod().Name}]");
                        swInfo.WriteLine($"[行号：{sf?.GetFileLineNumber()}]");
                        swInfo.WriteLine($"[内容：{logMessage.Message}]");
                        swInfo.WriteLine("------------------------------------------------------------------------------------------");
                        swInfo.WriteLine(string.Empty);
                    }
                    // Error
                    if (swError != null && logMessage.Level == LogLevel.Error)
                    {
                        swError.WriteLine($"[级别：Error]");
                        swError.WriteLine($"[时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}]");
                        swError.WriteLine($"[类名：{sf?.GetMethod().DeclaringType.FullName}]");
                        swError.WriteLine($"[方法：{sf?.GetMethod().Name}]");
                        swError.WriteLine($"[行号：{sf?.GetFileLineNumber()}]");
                        if (!string.IsNullOrWhiteSpace(logMessage.Message))
                        {
                            swError.WriteLine($"[内容：{logMessage.Message}]");
                        }
                        if (logMessage.Exception != null)
                        {
                            swError.WriteLine($"[异常：{logMessage.Exception.ToString()}]");
                        }
                        swError.WriteLine("------------------------------------------------------------------------------------------");
                        swError.WriteLine(string.Empty);
                    }
                }
                // 关闭释放资源
                if (swInfo != null)
                {
                    swInfo.Close();
                    swInfo.Dispose();
                }
                if (swError != null)
                {
                    swError.Close();
                    swError.Dispose();
                }
            }
            finally
            {
                // 退出写锁
                _lock.ExitWriteLock();
            }
        }

        #endregion 写入日志

        #region 日志消息实体

        /// <summary>
        /// 日志级别
        /// </summary>
        private enum LogLevel
        {
            Info, Error
        }

        /// <summary>
        /// 消息实体
        /// </summary>
        private class LogMessage
        {
            /// <summary>
            /// 日志级别
            /// </summary>
            public LogLevel Level { get; set; }

            /// <summary>
            /// 消息内容
            /// </summary>
            public string Message { get; set; } = string.Empty;

            /// <summary>
            /// 异常对象
            /// </summary>
            public Exception Exception { get; set; }

            /// <summary>
            /// 堆栈帧信息
            /// </summary>
            public StackFrame StackFrame { get; set; }
        }

        #endregion 日志消息实体
    }
}