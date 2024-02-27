namespace Info.Storage.Infa.Repository.Extension
{
    /// <summary>
    /// SingleFreeSql接口定义
    /// </summary>
    /// <typeparam name="TDBKey"></typeparam>
    public interface IBaseSingleFreeSql<TDBKey>
    {
        /// <summary>
        /// 获取单个FreeSql
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IFreeSql Get(TDBKey key);

        /// <summary>
        /// SingleFreeSql注册
        /// </summary>
        /// <param name="key"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public IBaseSingleFreeSql<TDBKey> Register(TDBKey key, Func<IFreeSql> create);
    }

    /// <summary>
    /// BaseSingleFreeSql实现
    /// </summary>
    /// <typeparam name="TDBKey"></typeparam>
    public class BaseSingleFreeSql<TDBKey> : IBaseSingleFreeSql<TDBKey>
    {
        #region Initialize

        /// <summary>
        /// 空闲对象管理器
        /// </summary>
        internal IdleBus<TDBKey, IFreeSql> _ib;

        public BaseSingleFreeSql()
        {
            _ib = new IdleBus<TDBKey, IFreeSql>(TimeSpan.MaxValue);
            _ib.Notice += (_, __) => { };
        }

        #endregion Initialize

        public IFreeSql Get(TDBKey key)
        {
            return _ib.Get(key);
        }

        public IBaseSingleFreeSql<TDBKey> Register(TDBKey key, Func<IFreeSql> create)
        {
            if (!_ib.TryRegister(key, create))
                throw new Exception("注册失败");
            return this;
        }
    }
}