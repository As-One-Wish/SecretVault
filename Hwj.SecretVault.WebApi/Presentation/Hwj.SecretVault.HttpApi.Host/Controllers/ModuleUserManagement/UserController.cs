using Info.Storage.Application.ModuleUserManagement;
using Info.Storage.Infra.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infra.Entity.ModuleUserManagement.Params;
using Info.Storage.Infra.Entity.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("AddUser")]
        [ProducesResponseType(typeof(BaseResult<UserDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto, [FromQuery] bool responseData = false)
        {
            BaseResult<UserDto?> br = await this._userService.AddUser(userDto, responseData);
            return Ok(br);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="deleteUserParam"></param>
        /// <returns></returns>
        [HttpDelete("DelUser")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> DelUser([FromBody] DeleteUserParam deleteUserParam)
        {
            BaseResult br = await this._userService.DelUser(deleteUserParam);
            return Ok(br);
        }

        /// <summary>
        /// 清空已逻辑删除用户(管理员权限)
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ClearUser")]
        [Authorize(Policy = "Policy.Admin")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearUser()
        {
            BaseResult br = await this._userService.ClearUser();
            return Ok(br);
        }

        /// <summary>
        /// 查询用户信息ById
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetUserById")]
        [ProducesResponseType(typeof(BaseResult<UserDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser([FromQuery] long userId)
        {
            BaseResult<UserDto?> br = await this._userService.GetUser(userId);
            return Ok(br);
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="queryUserParam"></param>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(BaseResult<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromQuery] QueryUserParam queryUserParam)
        {
            BaseResult<IEnumerable<UserDto>> br = await this._userService.GetUsers(queryUserParam);
            return Ok(br);
        }

        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        [HttpPost("IsUserAccountExist")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsUserNameExist([FromBody] string userAccount)
        {
            BaseResult br = await this._userService.IsUserAccountExist(userAccount);
            return Ok(br);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto, [FromQuery] bool responseData = false)
        {
            BaseResult br = await this._userService.UpdateUser(userDto, responseData);
            return Ok(br);
        }
    }
}