using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        #region Fields

        private readonly ICustomerService _customerService;

        private ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #endregion

        #region Get Requests

        /// <summary>
        /// Lấy dữ liệu toàn bộ khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomers()
        {           
            try
            {
                _serviceResult = _customerService.Get();

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
        /// Lấy dữ liệu khách hàng ứng với id
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <returns></returns>
        [HttpGet("{CustomerId}")]
        public IActionResult GetCustomerByid(Guid customerId)
        {
            // Lấy dữ liệu và phản hồi cho client
            try
            {
                _serviceResult = _customerService.GetById(customerId);
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
        /// Lọc danh sách khách hàng theo các tiêu chí: phân trang, tìm kiếm, lọc theo nhóm khách hàng
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên một trang</param>
        /// <param name="pageNumber">Chỉ số trang cần xem</param>
        /// <param name="filterString">Chuỗi cần tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("customerFilter")]
        public IActionResult GetCustomerFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId)
        {
            try {
                _serviceResult = _customerService.GetByFilter(pageSize, pageNumber, filterString, customerGroupId);

                if(_serviceResult.IsValid == false)
                {
                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                }
                // Trả dữ liệu về cho client
                return StatusCode(200, _serviceResult.Data);
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

        #region Post Requests

        /// <summary>
        /// Thêm thông tin khách hàng mới vào db
        /// </summary>
        /// <param name="customer">Object thông tin khách hàng muốn thêm vào db</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
            try
            {
                _serviceResult = _customerService.Add(customer);
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
        /// Cập nhật thông tin của 1 khách hàng vào trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <param name="customer">Dữ liệu muốn cập nhật</param>
        /// <returns></returns>
        [HttpPut("{CustomerId}")]
        public IActionResult EditCustomer(Guid customerId, Customer customer)
        {

            // Thực thi truy vấn và trả về kết quả cho client
            try
            {
                var serverResult = _customerService.Update(customer, customerId);

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
        /// Xóa khách hàng với id tương ứng
        /// </summary>
        /// <param name="customerId">Id của khách hàng muốn xóa</param>
        /// <returns></returns>

        [HttpDelete("{CustomerId}")]
        public IActionResult DeleteCustomerById(Guid customerId)
        {
            try
            {
                _serviceResult = _customerService.DeleteOne(customerId);

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
        /// Xóa nhiều bản ghi qua một request
        /// </summary>
        /// <param name="customerIds">Mảng id của các bản ghi cần xóa</param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteCustomers([FromBody] List<Guid> customerIds)
        {
            try
            {
                _serviceResult = _customerService.DeleteMany(customerIds);

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
