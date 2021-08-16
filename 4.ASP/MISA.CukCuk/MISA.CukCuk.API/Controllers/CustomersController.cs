using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : BaseController<Customer>
    {
        #region Fields

        private readonly ICustomerService _customerService;

        #endregion

        #region Constructors

        public CustomersController(ICustomerService customerService):base(customerService)
        {
            _customerService = customerService;
        }

        #endregion

        #region Get Requests

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
    }
}
