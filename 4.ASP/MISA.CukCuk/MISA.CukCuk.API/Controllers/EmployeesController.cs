using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Fields

        private readonly IEmployeeService _employeeService;

        private ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #endregion

        #region Get Requests

        /// <summary>
        /// Lấy dữ liệu toàn bộ nv
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                _serviceResult = _employeeService.Get();

                if (_serviceResult.IsValid == false)
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

        /// <summary>
        /// Lấy dữ liệu nv ứng với id
        /// </summary>
        /// <param name="employeeId">Id nv</param>
        /// <returns></returns>
        [HttpGet("{EmployeeId}")]
        public IActionResult GetEmployeeByid(Guid employeeId)
        {
            // Lấy dữ liệu và phản hồi cho client
            try
            {
                _serviceResult = _employeeService.GetById(employeeId);
                if (_serviceResult.IsValid == false)
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


        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmPloyeeCode()
        {
            try
            {
                _serviceResult = _employeeService.GetNewCode();

                return Ok(_serviceResult);
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
        [HttpGet("employeeFilter")]
        public IActionResult GetEmployeeFilter(int pageSize, int pageNumber,
                                                string filterString, Guid? departmentId, Guid? positionId)
        {
            
            try
            {
                _serviceResult = _employeeService.GetByFilter(pageSize, pageNumber, filterString, departmentId, positionId);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return Ok(_serviceResult);
                }

                // Trả dữ liệu về cho client
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

        #region Post Requests

        /// <summary>
        /// Thêm thông tin nv mới vào db
        /// </summary>
        /// <param name="employee">Object thông tin nv muốn thêm vào db</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertEmployee(Employee employee)
        {
            try
            {
                _serviceResult = _employeeService.Add(employee);
                if (_serviceResult.IsValid == false)
                {
                    return StatusCode(400, _serviceResult);
                }

                _serviceResult.Msg = Properties.Resources.MISAInsertMsg;
                return StatusCode(201, _serviceResult);
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

        #region PUT requests

        /// <summary>
        /// Cập nhật thông tin của 1 nv vào trong db
        /// </summary>
        /// <param name="employeeId">Id nv</param>
        /// <param name="employee">Dữ liệu muốn cập nhật</param>
        /// <returns></returns>
        [HttpPut("{EmployeeId}")]
        public IActionResult EditEmployee(Guid employeeId, Employee employee)
        {
            // Thực thi truy vấn và trả về kết quả cho client
            try
            {
                var serverResult = _employeeService.Update(employee, employeeId);

                if (serverResult.IsValid == false)
                {
                    return StatusCode(400, serverResult);
                }

                serverResult.Msg = Properties.Resources.MISAUpdateMsg;
                return StatusCode(200, serverResult);
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

        #region Delete Requests

        /// <summary>
        /// Xóa nv với id tương ứng
        /// </summary>
        /// <param name="employeeId">Id của nv muốn xóa</param>
        /// <returns></returns>

        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployeeById(Guid employeeId)
        {
            try
            {
                _serviceResult = _employeeService.DeleteOne(employeeId);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return StatusCode(400, _serviceResult);
                }

                _serviceResult.Msg = Properties.Resources.MISADeleteMsg;
                return StatusCode(200, _serviceResult);
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
        /// Xóa nhiều bản ghi qua 1 request
        /// </summary>
        /// <param name="employeeIds">List<string> chứa mảng id ứng với các bản ghi cần xóa</string></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteEmployees([FromBody] List<Guid> employeeIds)
        {
            try
            {
                _serviceResult = _employeeService.DeleteMany(employeeIds);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return StatusCode(400, _serviceResult);
                }

                _serviceResult.Msg = Properties.Resources.MISADeleteMsg;

                return StatusCode(200, _serviceResult);
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

