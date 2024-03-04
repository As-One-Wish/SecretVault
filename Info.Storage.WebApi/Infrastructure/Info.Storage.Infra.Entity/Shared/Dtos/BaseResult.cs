namespace Info.Storage.Infra.Entity.Shared.Dtos
{
    public record BaseResult<T>
    {
        #region 字段

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 错误说明
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 数据数量
        /// </summary>
        public long? DataCount { get; set; }

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseResult()
        {
            IsSuccess = true;
            Message = null;
            Data = default;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isSuccess"></param>
        public BaseResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
            Message = null;
            Data = default;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="data"></param>
        public BaseResult(bool isSuccess, T data)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = null;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <Param name="isSuccess"></Param>
        /// <Param name="data"></Param>
        /// <Param name="message"></Param>
        public BaseResult(bool isSuccess, T data, string message)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <Param name="isSuccess"></Param>
        /// <Param name="data"></Param>
        /// <Param name="message"></Param>
        /// <Param name="dataCount"></Param>
        public BaseResult(bool isSuccess, T data, string message, long dataCount)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            DataCount = dataCount;
        }

        #endregion 构造函数

        #region 返回

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BaseResult<T> Ok(T value)
        {
            return new BaseResult<T>()
            {
                IsSuccess = true,
                Data = value
            };
        }
        /// <summary>
        /// 返回成功，不附带数据
        /// </summary>
        /// <returns></returns>
        public static BaseResult Ok()
        {
            return new BaseResult() { IsSuccess = true };
        }
        /// <summary>
        /// 返回失败结果
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BaseResult NotOk(string message, object data = null)
        {
            return new BaseResult()
            {
                IsSuccess = false,
                Message = message,
                Data = data
            };
        }

        #endregion 返回
    }

    public record BaseResult : BaseResult<object>
    {
        public BaseResult() : base() { }
        public BaseResult(bool isSuccess) : base(isSuccess) { }
        public BaseResult(bool isSuccess, object data) : base(isSuccess, data) { }
        public BaseResult(bool isSuccess, object data, string message) : base(isSuccess, data, message) { }
        public BaseResult(bool isSuccess, object data, string message, long dataCount) : base(isSuccess, data, message, dataCount) { }
    }
}