using Hwj.SecretVault.Application.ModuleInfoManagement;
using Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Dtos;
using Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Params;
using Hwj.SecretVault.Infra.Entity.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hwj.SecretVault.HttpApi.Host.Controllers.ModuleInfoManagement
{
    /// <summary>
    /// 信息控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Policy = "Policy.Default")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        #region Initialize

        private readonly IInfoService _infoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="infoService"></param>
        public InfoController(IInfoService infoService)
        {
            _infoService = infoService;
        }

        #endregion Initialize

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="infoDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("AddInfo")]
        [ProducesResponseType(typeof(BaseResult<InfoDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddInfo([FromBody] InfoDto infoDto, [FromQuery] bool responseData = false)
        {
            BaseResult<InfoDto?> result = await _infoService.AddInfo(infoDto, responseData);
            return Ok(result);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="deleteInfoParam"></param>
        /// <returns></returns>
        [HttpDelete("DelInfo")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> DelInfo([FromBody] DeleteInfoParam deleteInfoParam)
        {
            BaseResult result = await _infoService.DelInfo(deleteInfoParam);
            return Ok(result);
        }

        /// <summary>
        /// 清除已逻辑删除信息(管理员权限)
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ClearInfo")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        [Authorize(Policy = "Policy.Admin")]
        public async Task<IActionResult> ClearInfo()
        {
            BaseResult result = await _infoService.ClearInfo();
            return Ok(result);
        }

        /// <summary>
        /// 查询信息ById
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        [HttpGet("GetInfo")]
        [ProducesResponseType(typeof(BaseResult<InfoDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo([FromQuery] long infoId)
        {
            BaseResult<InfoDto?> result = await _infoService.GetInfo(infoId);
            return Ok(result);
        }

        /// <summary>
        /// 查询信息列表
        /// </summary>
        /// <param name="queryInfoParam"></param>
        /// <returns></returns>
        [HttpGet("GetInfos")]
        [ProducesResponseType(typeof(BaseResult<IEnumerable<InfoDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfos([FromQuery] QueryInfoParam queryInfoParam)
        {
            BaseResult<IEnumerable<InfoDto>> result = await _infoService.GetInfos(queryInfoParam);
            return Ok(result);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="infoDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("UpdateInfo")]
        [ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateInfo([FromBody] InfoDto infoDto, [FromQuery] bool responseData = false)
        {
            BaseResult result = await _infoService.UpdateInfo(infoDto, responseData);
            return Ok(result);
        }
    }
}