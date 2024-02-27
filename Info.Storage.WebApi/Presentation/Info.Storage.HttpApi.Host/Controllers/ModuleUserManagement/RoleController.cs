using Info.Storage.Application.ModuleUserManagement;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Entity.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Info.Storage.HttpApi.Host.Controllers.ModuleUserManagement
{
    [Route("api/[controller]")]
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

        [HttpPost]
        [Route("AddRole")]
        [ProducesResponseType(typeof(BaseResult<RoleDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto, [FromQuery] bool responseData = false)
        {
            BaseResult<RoleDto?> br = await this._roleService.AddRole(roleDto, responseData);
            return Ok(br);
        }
    }
}