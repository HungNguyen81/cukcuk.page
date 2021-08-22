using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MISA.CukCuk.API.Controllers
{
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
        //@ CreateBy  : Hungnn
        //@ ModifiedBy: Hungnn

        [HttpGet("customerFilter")]
        public IActionResult GetCustomerFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId)
        {
            try {
                var serviceResult  = _customerService.GetByFilter(pageSize, pageNumber, filterString, customerGroupId);

                if(serviceResult.IsValid == false)
                {
                    serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                }
                // Trả dữ liệu về cho client
                return StatusCode(200, serviceResult.Data);
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

        #region Import Data

        /// <summary>
        /// Import data from file to db
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (20-08-2021)
        [HttpPost("import")]
        public IActionResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            try
            {
                var serviceResult = _customerService.Import(formFile, cancellationToken);

                //if (_serviceResult.IsValid == false)
                //{
                //    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                //}
                // Trả dữ liệu về cho client
                return StatusCode(200, serviceResult);
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

        /// <summary>
        /// Thêm mới nhiều bản ghi trong 1 request
        /// </summary>
        /// <param name="customers">DS các bản ghi</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (20-08-2021)
        [HttpPost("importMany")]
        public IActionResult ImportMany(List<Customer> customers)
        {
            try
            {   
                var serviceResult = _customerService.InsertMany(customers);

                if (serviceResult.IsValid == false)
                {
                    serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return NoContent();
                }
                // Trả dữ liệu về cho client
                return StatusCode(200, serviceResult);
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
