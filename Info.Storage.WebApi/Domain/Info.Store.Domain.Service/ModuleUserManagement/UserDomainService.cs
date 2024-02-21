using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Databases.Repositories;

namespace Info.Storage.Domain.Service.ModuleUserManagement
{
    /// <summary>
    /// 用户管理领域接口
    /// </summary>
    public interface IUserDomainService
    {
        /// <summary>
        /// 异步添加用户
        /// </summary>
        /// <param name="appUser">用户数据库实体</param>
        /// <returns>数据库实体</returns>
        Task<AppUser> AddUserAsync(AppUser appUser);
    }

    /// <summary>
    /// 用户管理领域实现
    /// </summary>
    public class UserDomainService : IUserDomainService
    {
        private readonly AppUserRepository _appUserRepository;

        public UserDomainService(AppUserRepository appUserRepository)
        {
            this._appUserRepository = appUserRepository;
        }

        public Task<AppUser> AddUserAsync(AppUser appUser)
        {
            throw new NotImplementedException();
        }
    }
}