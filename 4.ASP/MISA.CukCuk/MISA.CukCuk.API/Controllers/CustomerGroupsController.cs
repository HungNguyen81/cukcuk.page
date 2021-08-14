using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerGroupsController : ControllerBase
    {
        #region Fields

        private readonly ICustomerGroupService _customerGroupService;

        private ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public CustomerGroupsController(ICustomerGroupService customerGroupService)
        {
            _customerGroupService = customerGroupService;
        }

        #endregion

        #region GET Requests

        /// <summary>
        /// Lấy dữ liệu tất cả nhóm khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllCustomerGroup()
        {            
            try
            {
                _serviceResult = _customerGroupService.Get();

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
                    errorCode = "MISA_001",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Lấy thông tin 1 nhóm khách hàng theo id từ db
        /// </summary>
        /// <param name="customerGroupId">id nhóm khách hàng trong db</param>
        /// <returns></returns>
        [HttpGet("{CustomerGroupId}")]
        public IActionResult GetCustomerGroupById(Guid customerGroupId)
        {
            
            // Phản hồi lại cho client
            try
            {
                _serviceResult = _customerGroupService.GetById(customerGroupId);

                if(!_serviceResult.IsValid)
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
                    errorCode = "MISA_002",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion

        #region POST Requests

        /// <summary>
        /// Thêm nhóm khách hàng mới
        /// </summary>
        /// <param name="group">Đối tượng chứa thông tin của nhóm khách hàng mới</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertCustomerGroup(CustomerGroup customerGroup)
        {
            try
            {
                _serviceResult = _customerGroupService.Add(customerGroup);

                if(!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return BadRequest(_serviceResult);
                }
                _serviceResult.Msg = Properties.Resources.MISAInsertMsg;
                return StatusCode(201, _serviceResult);
            }
            catch (Exception e)
            {
                // Có exception xảy ra, trả về thông báo lỗi
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISABadRequestMsg,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion

        #region PUT Requests

        /// <summary>
        /// Sửa thông tin của một nhóm khách hàng với id tương ứng
        /// </summary>
        /// <param name="id">    id của nhóm khách hàng</param>
        /// <param name="group"> Dữ liệu cập nhật</param>
        /// <returns></returns>
        [HttpPut("{CustomerGroupId}")]
        public IActionResult UpdateCustomerGroup(Guid customerGroupId, CustomerGroup customerGroup)
        {
            try
            {
                _serviceResult = _customerGroupService.Update(customerGroup, customerGroupId);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return BadRequest(_serviceResult);
                }

                _serviceResult.Msg = Properties.Resources.MISAUpdateMsg;
                return StatusCode(200, _serviceResult);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISABadRequestMsg,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(400, response);
            }
        }

        #endregion

        #region DELETE Requests

        /// <summary>
        /// Xóa nhóm khách hàng ứng với id
        /// </summary>
        /// <param name="id"> id của nhóm khách hàng cần xóa </param>
        /// <returns></returns>
        [HttpDelete("{CustomerGroupId}")]
        public IActionResult DeleteCustomerGroup(Guid customerGroupId)
        {
            try
            {
                _serviceResult = _customerGroupService.DeleteOne(customerGroupId);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return BadRequest(_serviceResult);
                }
                _serviceResult.Msg = Properties.Resources.MISADeleteMsg;
                return StatusCode(200, _serviceResult);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISABadRequestMsg,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion
    }
}
