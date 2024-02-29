using AutoMapper;
using Info.Storage.Domain.Service.ModuleUserManagement;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Entity.ModuleUserManagement.Params;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Infa.Entity.Shared.Constants;
using Info.Storage.Infa.Entity.Shared.Dtos;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Info.Storage.Application.ModuleUserManagement
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
            this._userDomainService = userDomainService;
            this._mapper = mapper;
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
                AppUser oAppUser = this._mapper.Map<UserDto, AppUser>(userDto);
                AppUser result = await this._userDomainService.AddUserAsync(oAppUser);
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

        public async Task<BaseResult> DelUser(DeleteUserParam deleteUserParam)
        {
            try
            {
                BaseResult br = new BaseResult();
                int effectRows = await this._userDomainService.DelUserAsync(deleteUserParam);
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
                AppUser appUser = await this._userDomainService.GetUserAsync(userId);
                bool isNullResult = appUser == null;
                br.IsSuccess = !isNullResult;

                if (!isNullResult)
                    br.Data = this._mapper.Map<AppUser, UserDto>(appUser);
                br.Message = isNullResult ? Msg.DbError : Msg.Success;
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
                (long DataCount, IEnumerable<UserDto> Data) result = await this._userDomainService.GetUsersAsync(queryUserParam);
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
                    br.IsSuccess = !await this._userDomainService.IsUserAccountExistAsync(userAccount);
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
                AppUser appUser = this._mapper.Map<UserDto, AppUser>(userDto);
                userDto.UpdateTime = appUser.UpdateTime;

                int effectRows = await this._userDomainService.UpdateUserAsync(appUser);
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