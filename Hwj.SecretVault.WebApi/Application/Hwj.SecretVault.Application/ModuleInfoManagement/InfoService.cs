using AutoMapper;
using Hwj.SecretVault.Domain.Service.ModuleInfoManagement;
using Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Dtos;
using Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Params;
using Hwj.SecretVault.Infra.Entity.Shared.Attributes;
using Hwj.SecretVault.Infra.Entity.Shared.Constants;
using Hwj.SecretVault.Infra.Entity.Shared.Dtos;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Hwj.SecretVault.Application.ModuleInfoManagement
{
    public interface IInfoService
    {
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="infoDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        Task<BaseResult<InfoDto?>> AddInfo(InfoDto infoDto, bool responseData = false);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="deleteInfoParam"></param>
        /// <returns></returns>
        Task<BaseResult> DelInfo(DeleteInfoParam deleteInfoParam);

        /// <summary>
        /// 清除逻辑删除的信息-管理员权限
        /// </summary>
        /// <returns></returns>
        Task<BaseResult> ClearInfo();

        /// <summary>
        /// 查询单一信息
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        Task<BaseResult<InfoDto?>> GetInfo(long infoId);

        /// <summary>
        /// 查询信息集合
        /// </summary>
        /// <param name="queryInfoParam"></param>
        /// <returns></returns>
        Task<BaseResult<IEnumerable<InfoDto>>> GetInfos(QueryInfoParam queryInfoParam);

        /// <summary>
        /// 更新信息内容
        /// </summary>
        /// <param name="infoDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateInfo(InfoDto infoDto, bool responseData = false);
    }

    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class InfoService : IInfoService
    {
        #region Initialize

        private readonly IInfoDomainService _infoDomainService;
        private readonly IMapper _mapper;

        public InfoService(IInfoDomainService domainService, IMapper mapper)
        {
            _infoDomainService = domainService;
            _mapper = mapper;
        }

        #endregion Initialize

        #region Implements

        public async Task<BaseResult<InfoDto?>> AddInfo(InfoDto infoDto, bool responseData = false)
        {
            try
            {
                BaseResult<InfoDto?> br = new BaseResult<InfoDto?>();
                infoDto.InfoId = Yitter.IdGenerator.YitIdHelper.NextId();

                AppInfo appInfo = _mapper.Map<InfoDto, AppInfo>(infoDto);
                AppInfo result = await _infoDomainService.AddInfoAsync(appInfo);

                bool isNullResult = result == null;
                br.IsSuccess = !isNullResult;
                if (responseData && !isNullResult)
                    br.Data = infoDto;
                br.Message = isNullResult ? Msg.DbError : Msg.Success;
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<InfoDto?>(false, null, ex.Message));
            }
        }

        public async Task<BaseResult> ClearInfo()
        {
            try
            {
                BaseResult br = new BaseResult();
                int effectRows = await _infoDomainService.DelInfoPhysicallyAsync();
                br.IsSuccess = effectRows > 0;
                br.DataCount = effectRows;
                br.Message = br.IsSuccess ? Msg.Success : Msg.DbError;
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult(false, null, ex.Message));
            }
        }

        public async Task<BaseResult> DelInfo(DeleteInfoParam deleteInfoParam)
        {
            try
            {
                BaseResult br = new BaseResult();
                int effectRows = await _infoDomainService.DelInfoAsync(deleteInfoParam);
                br.IsSuccess = effectRows > 0;
                br.DataCount = effectRows;
                br.Message = br.IsSuccess ? Msg.Success : Msg.DbError;

                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult(false, null, ex.Message));
            }
        }

        public async Task<BaseResult<InfoDto?>> GetInfo(long infoId)
        {
            try
            {
                BaseResult<InfoDto?> br = new BaseResult<InfoDto?>();
                AppInfo appInfo = await _infoDomainService.GetInfoAsync(infoId);
                bool isNullResult = appInfo == null;
                br.IsSuccess = !isNullResult;
                br.Message = isNullResult ? Msg.DataNotFound : Msg.Success;
                br.DataCount = isNullResult ? -1 : 1;
                if (!isNullResult)
                    br.Data = _mapper.Map<AppInfo, InfoDto>(appInfo);
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<InfoDto?>(false, null, ex.Message));
            }
        }

        public async Task<BaseResult<IEnumerable<InfoDto>>> GetInfos(QueryInfoParam queryInfoParam)
        {
            try
            {
                BaseResult<IEnumerable<InfoDto>> br = new BaseResult<IEnumerable<InfoDto>>();
                (long DataCount, IEnumerable<AppInfo> Data) result = await _infoDomainService.GetInfosAsync(queryInfoParam);
                br.IsSuccess = result.Data != null;
                br.Message = br.IsSuccess ? Msg.Success : Msg.DataNotFound;
                if (br.IsSuccess)
                {
                    br.Data = result.Data.Select(_mapper.Map<AppInfo, InfoDto>);
                    br.DataCount = result.DataCount == -1 ? result.Data.Count() : result.DataCount;
                }

                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<IEnumerable<InfoDto>>(false, Enumerable.Empty<InfoDto>(), ex.Message));
            }
        }

        public async Task<BaseResult> UpdateInfo(InfoDto infoDto, bool responseData = false)
        {
            try
            {
                BaseResult br = new BaseResult();
                AppInfo appInfo = _mapper.Map<InfoDto, AppInfo>(infoDto);

                int effectRows = await _infoDomainService.UpdateInfoAsync(appInfo);
                infoDto.UpdateTime = appInfo.UpdateTime;
                br.IsSuccess = effectRows > 0;
                br.DataCount = effectRows;

                if (responseData && br.IsSuccess)
                    br.Data = infoDto;
                br.Message = br.IsSuccess ? Msg.Success : Msg.DbError;

                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult(false, null, ex.Message));
            }
        }

        #endregion Implements
    }
}