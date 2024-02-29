using Info.Storage.Application.ModuleUserManagement;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Entity.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Info.Storage.HttpApi.Host.Controllers.ModuleUserManagement
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Policy = "Policy.Default")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Initialize

        private readonly IUserService _userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion Initialize

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUser")]
        [ProducesResponseType(typeof(BaseResult<UserDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto, [FromQuery] bool responseData = false)
        {
            BaseResult<UserDto?> br = await this._userService.AddUser(userDto, responseData);
            return Ok(br);
        }
    }
}