using Info.Storage.Application.ModuleUserManagement;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Entity.ModuleUserManagement.Params;
using Info.Storage.Infa.Entity.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Info.Storage.HttpApi.Host.Controllers.ModuleUserManagement
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Policy = "Policy.Admin")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region Initialize

        private readonly IRoleService _roleService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleService"></param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion Initialize

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("AddRole")]
        [ProducesResponseType(typeof(BaseResult<RoleDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto, [FromQuery] bool responseData = false)
        {
            BaseResult<RoleDto?> br = await this._roleService.AddRole(roleDto, responseData);
            return Ok(br);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteRoleParam"></param>
        /// <returns></returns>
        [HttpDelete("DelRole")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> DelRole([FromBody] DeleteRoleParam deleteRoleParam)
        {
            BaseResult br = await this._roleService.DelRole(deleteRoleParam);
            return Ok(br);
        }

        /// <summary>
        /// 查询角色信息ById
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("GetRoleById")]
        [ProducesResponseType(typeof(BaseResult<RoleDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRole([FromQuery] long roleId)
        {
            BaseResult<RoleDto?> br = await this._roleService.GetRole(roleId);
            return Ok(br);
        }

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="queryRoleParam"></param>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        [ProducesResponseType(typeof(BaseResult<IEnumerable<RoleDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles([FromQuery] QueryRoleParam queryRoleParam)
        {
            BaseResult<IEnumerable<RoleDto>> br = await this._roleService.GetRoles(queryRoleParam);
            return Ok(br);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("UpdateRole")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto roleDto, [FromQuery] bool responseData = false)
        {
            BaseResult br = await this._roleService.UpdateRole(roleDto, responseData);
            return Ok(br);
        }
    }
}