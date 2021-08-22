using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.API.Controllers
{
    //[Route("api/v1/employees")]
    //[ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Fields

        private readonly IEmployeeService _employeeService;

        #endregion

        #region Constructors

        public EmployeesController(IEmployeeService employeeService):base(employeeService)
        {
            _employeeService = employeeService;
        }

        #endregion

        #region Get Requests

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>

        //@ CreateBy  : Hungnn
        //@ ModifiedBy: Hungnn

        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmPloyeeCode()
        {
            try
            {
                var serviceResult = _employeeService.GetNewCode();

                return Ok(serviceResult);
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
        /// Lọc và phân trang theo tiêu chí tìm kiếm, theo phòng ban, vị trí
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>

        //@ CreateBy  : Hungnn
        //@ ModifiedBy: Hungnn

        [HttpGet("employeeFilter")]
        public IActionResult GetEmployeeFilter(int pageSize, int pageNumber,
                                                string filterString, Guid? departmentId, Guid? positionId)
        {
            try
            {
                var serviceResult = _employeeService.GetByFilter(pageSize, pageNumber, filterString, departmentId, positionId);

                if (!serviceResult.IsValid)
                {
                    serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return Ok(serviceResult);
                }

                // Trả dữ liệu về cho client
                return StatusCode(200, serviceResult.Data);
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

