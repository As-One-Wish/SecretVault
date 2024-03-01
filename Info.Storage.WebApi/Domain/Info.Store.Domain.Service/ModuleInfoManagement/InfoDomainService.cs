using Info.Storage.Infa.Entity.ModuleInfoManagement.Params;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Databases.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Info.Storage.Domain.Service.ModuleInfoManagement
{
    /// <summary>
    /// 信息管理领域接口
    /// </summary>
    public interface IInfoDomainService
    {
        /// <summary>
        /// 异步添加信息
        /// </summary>
        /// <param name="appInfo"></param>
        /// <returns></returns>
        Task<AppInfo> AddInfoAsync(AppInfo appInfo);

        /// <summary>
        /// 异步删除信息
        /// </summary>
        /// <param name="deleteInfoParam"></param>
        /// <returns></returns>
        Task<int> DelInfoAsync(DeleteInfoParam deleteInfoParam);

        /// <summary>
        /// 异步清空已逻辑删除的信息
        /// </summary>
        /// <returns></returns>
        Task<int> DelInfoPhysicallyAsync();

        /// <summary>
        /// 异步查询单一信息
        /// </summary>
        /// <returns></returns>
        Task<AppInfo> GetInfoAsync(long infoId);

        /// <summary>
        /// 查询信息集合
        /// </summary>
        /// <param name="queryInfoParam"></param>
        /// <returns></returns>
        Task<(long, IEnumerable<AppInfo>)> GetInfosAsync(QueryInfoParam queryInfoParam);

        /// <summary>
        /// 异步更新信息
        /// </summary>
        /// <param name="appInfo"></param>
        /// <returns></returns>
        Task<int> UpdateInfoAsync(AppInfo appInfo);
    }

    /// <summary>
    /// 信息管理领域实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class InfoDomainService : IInfoDomainService
    {
        #region Initialize

        private readonly AppInfoRepository _appInfoRepository;

        public InfoDomainService(AppInfoRepository appInfoRepository)
        {
            _appInfoRepository = appInfoRepository;
        }

        #endregion Initialize

        #region Implements

        public async Task<AppInfo> AddInfoAsync(AppInfo info)
        {
            AppInfo appInfo = await this._appInfoRepository.InsertAsync(info);
            return appInfo;
        }

        public async Task<int> DelInfoPhysicallyAsync()
        {
            int effectRows = await this._appInfoRepository.DeleteAsync(info => info.IsDeleted);
            return effectRows;
        }

        public async Task<int> DelInfoAsync(DeleteInfoParam deleteInfoParam)
        {
            List<AppInfo> infosToDel = new List<AppInfo>();
            if (deleteInfoParam.InfoId != null)
            {
                infosToDel = await this._appInfoRepository.Where(info => info.InfoId == deleteInfoParam.InfoId.Value && !info.IsDeleted).ToListAsync();
            }
            if (deleteInfoParam.InfoIds != null && deleteInfoParam.InfoIds.Length > 0)
            {
                infosToDel = await this._appInfoRepository.Where(info => deleteInfoParam.InfoIds.Contains(info.InfoId) && !info.IsDeleted).ToListAsync();
            }
            if (infosToDel != null && infosToDel.Count > 0)
                foreach (AppInfo appInfo in infosToDel)
                {
                    appInfo.IsDeleted = true;
                    await this._appInfoRepository.UpdateAsync(appInfo);
                }
            return infosToDel.Count > 0 ? infosToDel.Count : -1;
        }

        public async Task<AppInfo> GetInfoAsync(long infoId)
        {
            AppInfo appInfo = await this._appInfoRepository.Where(info => info.InfoId == infoId && !info.IsDeleted).ToOneAsync();
            return appInfo;
        }

        public async Task<(long, IEnumerable<AppInfo>)> GetInfosAsync(QueryInfoParam queryInfoParam)
        {
            List<AppInfo> lstResult = null;
            long dataCount = -1;
            var select = this._appInfoRepository.Select;

            #region 条件查询

            // 根据文本
            select.WhereIf(!string.IsNullOrWhiteSpace(queryInfoParam.SearchText),
                info => info.InfoName.Contains(queryInfoParam.SearchText) ||
                info.InfoAccount.Contains(queryInfoParam.SearchText) ||
                info.InfoRemark.Contains(queryInfoParam.SearchText) ||
                info.InfoContent.Contains(queryInfoParam.SearchText));
            // 根据Id
            select.WhereIf(queryInfoParam.InfoId != null, info => info.InfoId.Equals(queryInfoParam.InfoId.Value));
            // 按照分类
            select.WhereIf(queryInfoParam.InfoType != null, info => info.InfoType.Equals(queryInfoParam.InfoType.Value));
            select.Where(info => !info.IsDeleted);

            #endregion 条件查询

            #region 排序

            // 多表关联
            select.OrderBy(!string.IsNullOrWhiteSpace(queryInfoParam.OrderBy),
                "a." + queryInfoParam.OrderBy + (string.IsNullOrWhiteSpace(queryInfoParam.OrderByType) ?
                " asc" : queryInfoParam.OrderByType == "ascend" ? " asc" : " desc"));

            #endregion 排序

            #region 分页

            if (queryInfoParam.PageIndex != null && queryInfoParam.PageSize != null)
                select.Count(out dataCount).Page(queryInfoParam.PageIndex.Value, queryInfoParam.PageSize.Value);

            #endregion 分页

            lstResult = await select.ToListAsync();

            return (dataCount, lstResult);
        }

        public async Task<int> UpdateInfoAsync(AppInfo appInfo)
        {
            int effectRows = await this._appInfoRepository.UpdateAsync(appInfo);
            return effectRows;
        }

        #endregion Implements
    }
}