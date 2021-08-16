using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Helpers;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Resource;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region Fields

        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public CustomerService(ICustomerRepository customerRepository):base(customerRepository)
        {
            _customerRepository = customerRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Các phương thức GET

        /// <summary>
        /// Lấy theo chuỗi tìm kiếm hoặc phân trang
        /// </summary>
        /// <param name="pageSize">         Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">       Chỉ số của trang</param>
        /// <param name="filterString">     chuỗi tìm kiếm</param>
        /// <param name="customerGroupId">  Id của nhóm khách hàng</param>
        /// <returns></returns>
        public ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId)
        {
            _serviceResult.Data = _customerRepository.GetByFilter(pageSize, pageNumber, filterString, customerGroupId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        #endregion
    }
}
