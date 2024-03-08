using AutoMapper;
using Hwj.SecretVault.Domain.Service.ModuleUserManagement;
using Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Dtos;
using Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Params;
using Hwj.SecretVault.Infra.Entity.Shared.Attributes;
using Hwj.SecretVault.Infra.Entity.Shared.Constants;
using Hwj.SecretVault.Infra.Entity.Shared.Dtos;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.Aow.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Hwj.SecretVault.Application.ModuleUserManagement
{
    public interface IUserService
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="responseData">是否返回对象</param>
        /// <returns></returns>
        Task<BaseResult<UserDto?>> AddUser(UserDto userDto, bool responseData = false);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="deleteUserParam"></param>
        /// <returns></returns>
        Task<BaseResult> DelUser(DeleteUserParam deleteUserParam);

        /// <summary>
        /// 清除逻辑删除用户-管理员权限
        /// </summary>
        /// <returns></returns>
        Task<BaseResult> ClearUser();

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateUser(UserDto userDto, bool responseData = false);

        /// <summary>
        /// 查询单一用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto?>> GetUser(long userId);

        /// <summary>
        /// 查询用户集合
        /// </summary>
        /// <param name="queryUserParam"></param>
        /// <returns></returns>
        Task<BaseResult<IEnumerable<UserDto>>> GetUsers(QueryUserParam queryUserParam);

        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        Task<BaseResult> IsUserAccountExist(string userAccount);
    }

    [AutoInject(serviceLifetime: ServiceLifetime.Scoped, key: "app")]
    public class UserService : IUserService
    {
        #region Initialize

        private readonly IUserDomainService _userDomainService;
        private readonly IMapper _mapper;

        public UserService(IUserDomainService userDomainService, IMapper mapper)
        {
            _userDomainService = userDomainService;
            _mapper = mapper;
        }

        #endregion Initialize

        #region Implements

        public async Task<BaseResult<UserDto?>> AddUser(UserDto userDto, bool responseData = false)
        {
            try
            {
                BaseResult<UserDto?> br = new BaseResult<UserDto?>();
                userDto.UserId = Yitter.IdGenerator.YitIdHelper.NextId();

                // 默认AutoMapper转换规则
                AppUser oAppUser = _mapper.Map<UserDto, AppUser>(userDto);
                AppUser result = await _userDomainService.AddUserAsync(oAppUser);
                bool isNullResult = result == null;
                br.IsSuccess = !isNullResult;
                if (responseData && !isNullResult)
                    br.Data = userDto;
                br.Message = isNullResult ? Msg.DbError : Msg.Success;
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<UserDto?>(false, null, ex.Message));
            }
        }

        public async Task<BaseResult> ClearUser()
        {
            try
            {
                BaseResult br = new BaseResult();
                int effectRows = await _userDomainService.DelUserPhysicallyAsync();
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

        public async Task<BaseResult> DelUser(DeleteUserParam deleteUserParam)
        {
            try
            {
                BaseResult br = new BaseResult();
                int effectRows = await _userDomainService.DelUserAsync(deleteUserParam);
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

        public async Task<BaseResult<UserDto?>> GetUser(long userId)
        {
            try
            {
                BaseResult<UserDto?> br = new BaseResult<UserDto?>();
                AppUser appUser = await _userDomainService.GetUserAsync(userId);
                bool isNullResult = appUser == null;
                br.IsSuccess = !isNullResult;
                br.DataCount = br.IsSuccess ? 1 : -1;
                if (!isNullResult)
                    br.Data = _mapper.Map<AppUser, UserDto>(appUser);
                br.Message = isNullResult ? Msg.DataNotFound : Msg.Success;
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<UserDto?>(false, null, ex.Message));
            }
        }

        public async Task<BaseResult<IEnumerable<UserDto>>> GetUsers(QueryUserParam queryUserParam)
        {
            try
            {
                BaseResult<IEnumerable<UserDto>> br = new BaseResult<IEnumerable<UserDto>>();
                (long DataCount, IEnumerable<UserDto> Data) result = await _userDomainService.GetUsersAsync(queryUserParam);
                br.IsSuccess = result.Data != null;
                br.Message = !br.IsSuccess ? Msg.DataNotFound : Msg.Success;
                if (br.IsSuccess)
                {
                    br.Data = result.Data;
                    br.DataCount = result.DataCount == -1 ? result.Data.Count() : result.DataCount;
                }

                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<IEnumerable<UserDto>>(false, Enumerable.Empty<UserDto>(), ex.Message));
            }
        }

        public async Task<BaseResult> IsUserAccountExist(string userAccount)
        {
            try
            {
                BaseResult br = new BaseResult();
                if (!string.IsNullOrWhiteSpace(userAccount))
                {
                    // 用户名存在则返回False
                    br.IsSuccess = !await _userDomainService.IsUserAccountExistAsync(userAccount);
                    br.Message = br.IsSuccess ? Msg.Success : Msg.ExistAccount;
                }
                else
                {
                    br.IsSuccess = false;
                    br.Message = Msg.ParamError;
                }
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult(false, null, ex.Message));
            }
        }

        public async Task<BaseResult> UpdateUser(UserDto userDto, bool responseData = false)
        {
            try
            {
                BaseResult br = new BaseResult();
                AppUser appUser = _mapper.Map<UserDto, AppUser>(userDto);
                userDto.UpdateTime = appUser.UpdateTime;

                int effectRows = await _userDomainService.UpdateUserAsync(appUser);
                br.IsSuccess = effectRows > 0;
                br.DataCount = effectRows;

                if (responseData && br.IsSuccess)
                    br.Data = userDto;
                br.Message = !br.IsSuccess ? Msg.DbError : Msg.Success;
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