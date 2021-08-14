using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        #region Fields

        private readonly IPositionService _positionService;

        private ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public PositionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        #endregion

        #region GET requests

        /// <summary>
        /// Lấy dữ liệu tất cả vị trí/ chức vụ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            try
            {
                // lấy dữ liệu
                _serviceResult = _positionService.Get();

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return StatusCode(200, _serviceResult);
                }

                // phản hồi về cho client
                return StatusCode(200, _serviceResult.Data);
            }
            catch (Exception e )
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Lấy thông tin vị trí/ chức vụ theo id
        /// </summary>
        /// <param name="positionId"> id vị trí cần lấy thông tin</param>
        /// <returns></returns>
        [HttpGet("{PositionId}")]
        public IActionResult GetPositionById(Guid positionId)
        {
            // Lấy dữ liệu và phản hồi cho client
            try
            {
                _serviceResult = _positionService.GetById(positionId);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return StatusCode(200, _serviceResult);
                }

                return StatusCode(200, _serviceResult.Data);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion

    }
}
