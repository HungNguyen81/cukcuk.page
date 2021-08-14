using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        #region Fields

        private readonly IDepartmentService _departmentService;

        private ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        #endregion


        #region GET requests

        /// <summary>
        /// Lấy dữ liệu tất cả phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            try
            {
                // lấy dữ liệu
                _serviceResult = _departmentService.Get();

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return StatusCode(200, _serviceResult);
                }

                // phản hồi về cho client
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

        /// <summary>
        /// Lấy thông tin phòng ban theo id
        /// </summary>
        /// <param name="departmentId"> id phòng ban cần lấy thông tin</param>
        /// <returns></returns>
        [HttpGet("{departmentId}")]
        public IActionResult GetDepartmentById(Guid departmentId)
        {
            try
            {
                _serviceResult = _departmentService.GetById(departmentId);

                if (!_serviceResult.IsValid)
                {

                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return StatusCode(204, _serviceResult);
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
