using System.Linq.Expressions;

namespace Info.Storage.Domain.Service.Shared
{
    public abstract partial class BaseDomainService<TEntity, TKey> where TEntity : class
    {
        // FreeSql 实例
        private IFreeSql _freeSql;

        #region 初始化部分

        public BaseDomainService()
        {
        }

        public BaseDomainService(IFreeSql freeSql)
        {
            this._freeSql = freeSql;
        }

        /// <summary>
        /// 获取IFreeSql
        /// </summary>
        /// <returns></returns>
        public IFreeSql GetOrm()
        {
            return this._freeSql;
        }

        /// <summary>
        /// 初始化FreeSql
        /// </summary>
        /// <param name="fsql"></param>
        public void Init(IFreeSql fsql)
        {
            this._freeSql = fsql;
        }

        #endregion 初始化部分

        #region 数据库相关

        /// <summary>
        /// 默认领域：添加单一实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体对象</returns>
        public async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            /* 异步插入操作
             * Insert<TEntity>() 表示要进行插入操作，并指定插入的实体类型为TEntity
             * AppendData(entity) 将要插入的实体数据追加到插入操作中
             * ExecuteInsertedAsync() 异步执行插入操作，并返回插入的数据(返回的是插入数据的列表)
             */
            List<TEntity> resultDatas = await this._freeSql.Insert<TEntity>().AppendData(entity).ExecuteInsertedAsync();

            if (resultDatas.Count > 0)
            {
                // TODO 设置缓存

                return resultDatas[0];
            }
            return null;
        }

        /// <summary>
        /// 默认领域：添加多个实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>实体对象集合</returns>
        public async Task<IEnumerable<TEntity>> AddEntitiesAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Count() > 0)
            {
                List<TEntity> resultDatas = await this._freeSql.Insert<TEntity>().AppendData(entities).ExecuteInsertedAsync();
                if (resultDatas.Count > 0)
                {
                    // TODO 设置缓存

                    return resultDatas;
                }
                else
                    return await Task.FromResult(Enumerable.Empty<TEntity>());
            }
            else
                return await Task.FromResult(Enumerable.Empty<TEntity>());
        }

        /// <summary>
        /// 默认领域：按主键删除实体
        /// </summary>
        /// <param name="idValue">主键值</param>
        /// <param name="primaryFieldName">主键字段</param>
        /// <returns>影响行数</returns>
        public async Task<int> DeleteEntityAsync(TKey idValue, string primaryFieldName = "id")
        {
            int effectRows = await this._freeSql.Delete<TEntity>().Where($"{primaryFieldName} = @id", new { id = idValue }).ExecuteAffrowsAsync();
            // TODO 设置缓存
            return effectRows;
        }

        /// <summary>
        /// 默认领域：按条件删除实体
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="condition">是否应用条件</param>
        /// <returns>影响行数</returns>
        public async Task<int> DeleteEntitiesAsync(Expression<Func<TEntity, bool>> exp, bool condition = true)
        {
            List<TEntity> resultDatas = await this._freeSql.Delete<TEntity>().WhereIf(condition, exp).ExecuteDeletedAsync();
            // TODO 设置缓存
            return resultDatas.Count;
        }

        /// <summary>
        /// 默认领域：更新单一实体
        /// </summary>
        /// <param name="entity">更新实体</param>
        /// <returns></returns>
        public async Task<int> UpdateEntityAsync(TEntity entity)
        {
            int effectRows = await this._freeSql.Update<TEntity>().SetSource(entity).ExecuteAffrowsAsync();
            // TODO 设置缓存
            return effectRows;
        }

        /// <summary>
        /// 默认领域：更新实体集合
        /// </summary>
        /// <param name="entities">更新实体集合</param>
        /// <returns></returns>
        public async Task<int> UpdateEntitiesAsync(IEnumerable<TEntity> entities)
        {
            int effectRows = await this._freeSql.Update<TEntity>().SetSource(entities).ExecuteAffrowsAsync();
            // TODO 设置缓存
            return effectRows;
        }

        /// <summary>
        /// 默认领域：插入或更新单一实体，依据主键判断
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertOrUpdateEntityAsync(TEntity entity)
        {
            int effectRows = await this._freeSql.InsertOrUpdate<TEntity>().SetSource(entity).ExecuteAffrowsAsync();
            // TODO 设置缓存
            return effectRows;
        }

        /// <summary>
        /// 默认领域：插入或更新实体集合，依据主键判断
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertOrUpdateEntitiesAsync(IEnumerable<TEntity> entities)
        {
            int effecRows = await this._freeSql.InsertOrUpdate<TEntity>().SetSource(entities).ExecuteAffrowsAsync();
            // TODO 设置缓存
            return effecRows;
        }

        /// <summary>
        /// 默认领域：查询唯一实体
        /// </summary>
        /// <param name="idValue"></param>
        /// <param name="primaryFieldName"></param>
        /// <returns></returns>
        public async Task<TEntity> QuerySingleEntityAsync(TKey idValue, string primaryFieldName = "id")
        {
            // TODO 缓存获取

            TEntity result = await this._freeSql.Select<TEntity>().Where($"{primaryFieldName} = @id", new { id = idValue }).FirstAsync();
            // TODO 不为空则保存至缓存
            return result;
        }

        /// <summary>
        /// 默认领域：查询实体
        /// </summary>
        /// <typeparam name="TResult">返回实体</typeparam>
        /// <param name="where">过滤条件表达式字典</param>
        /// <param name="filter">筛选Dto表达式</param> /* 对查询结果进行投影的Lambda表达式 */
        /// <param name="orderBySql">排序Sql</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>(数据总数，结果集)</returns>
        public async Task<(long, IEnumerable<TResult>)> QueryEntitiesAsync<TResult>(List<(bool, Expression<Func<TEntity, bool>>)> where,
            Expression<Func<TEntity, TResult>> filter, string orderBySql = null, int? pageIndex = null, int? pageSize = null)
        {
            List<TResult> lstResult = null;
            long dataCount = -1;
            var oSelect = this._freeSql.Select<TEntity>();
            if (where != null && where.Count > 0)
            {
                foreach (var item in where)
                    oSelect.WhereIf(item.Item1, item.Item2);
            }
            // 排序
            oSelect.OrderBy(!string.IsNullOrWhiteSpace(orderBySql), orderBySql);
            // 分页
            if (pageIndex != null && pageSize != null)
                oSelect.Count(out dataCount).Page(pageIndex.Value, pageSize: pageSize.Value);

            if (filter == null)
                lstResult = await oSelect.ToListAsync<TResult>();
            else
                lstResult = await oSelect.ToListAsync(filter);

            return (dataCount, lstResult);
        }

        /// <summary>
        /// 多查询条件领域：查询实体
        /// </summary>
        /// <typeparam name="TResult">返回实体</typeparam>
        /// <param name="where">过滤条件表达式字典</param>
        /// <param name="filter">筛选Dto表达式</param>
        /// <param name="orderBySqls">排序Sql列表</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(long, IEnumerable<TResult>)> QueryEntitiesAsync<TResult>(List<(bool, Expression<Func<TEntity, bool>>)> where,
            Expression<Func<TEntity, TResult>> filter, List<string> orderBySqls = null, int? pageIndex = null, int? pageSize = null)
        {
            List<TResult> lstResult = null;
            long dataCount = -1;
            var oSelect = this._freeSql.Select<TEntity>();
            if (where != null && where.Count > 0)
            {
                foreach (var item in where)
                    oSelect.WhereIf(item.Item1, item.Item2);
            }
            // 排序
            if (orderBySqls != null && orderBySqls.Count > 0)
                foreach (var item in orderBySqls)
                    oSelect.OrderBy(!string.IsNullOrWhiteSpace(item), item);
            // 分页
            if (pageIndex != null && pageSize != null)
                oSelect.Count(out dataCount).Page(pageIndex.Value, pageSize: pageSize.Value);

            if (filter == null)
                lstResult = await oSelect.ToListAsync<TResult>();
            else
                lstResult = await oSelect.ToListAsync(filter);

            return (dataCount, lstResult);
        }

        #endregion 数据库相关
    }
}