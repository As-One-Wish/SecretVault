using Info.Storage.Domain.Service.ModuleUserManagement;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Entity.ModuleUserManagement.Params;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Infa.Entity.Shared.Dtos;
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
        Task<BaseResult> UpdateUser(UserDto userDto);

        /// <summary>
        /// 查询单一用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> GetUser(long userId);

        /// <summary>
        /// 查询用户集合
        /// </summary>
        /// <param name="queryUserParam"></param>
        /// <returns></returns>
        Task<BaseResult<IEnumerable<UserDto>>> GetUsers(QueryUserParam queryUserParam);

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<BaseResult> IsUserNameExist(string userName);
    }

    [AutoInject(serviceLifetime: ServiceLifetime.Scoped, key: "app")]
    public class UserService : IUserService
    {
        private readonly IUserDomainService _userDomainService;

        public UserService(IUserDomainService userDomainService)
        {
            this._userDomainService = userDomainService;
        }

        public Task<BaseResult<UserDto?>> AddUser(UserDto userDto, bool responseData = false)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult> DelUser(DeleteUserParam deleteUserParam)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UserDto>> GetUser(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<IEnumerable<UserDto>>> GetUsers(QueryUserParam queryUserParam)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult> IsUserNameExist(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult> UpdateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}