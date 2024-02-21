using FreeSql;
using System.Linq.Expressions;

namespace Info.Storage.Infa.Repository.Extension
{
    /// <summary>
    /// 扩展 BaseRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseRepositoryExtend<T, TKey> : BaseRepository<T, TKey> where T : class, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fsql"></param>
        /// <param name="filter"></param>
        /// <param name="asTable"></param>
        public BaseRepositoryExtend(IFreeSql fsql, Expression<Func<T, bool>> filter, Func<string, string> asTable = null) : base(fsql, filter, asTable)
        {
        }
    }
}