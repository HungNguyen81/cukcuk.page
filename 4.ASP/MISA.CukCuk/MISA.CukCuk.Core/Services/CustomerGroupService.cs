using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.Core.Services
{
    public class CustomerGroupService : ICustomerGroupService
    {
        #region Fields

        ICustomerGroupRepository _customerGroupRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public CustomerGroupService(ICustomerGroupRepository customerGroupRepository)
        {
            _customerGroupRepository = customerGroupRepository;
            _serviceResult = new ServiceResult();
        }

        #endregion

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <returns></returns>
        public ServiceResult Add(CustomerGroup customerGroup)
        {
            _serviceResult.Data = _customerGroupRepository.Add(customerGroup);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;
            return _serviceResult;
        }

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ServiceResult DeleteOne(Guid customerId)
        {
            _serviceResult.Data = _customerGroupRepository.DeleteOne(customerId);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;
            return _serviceResult;
        }

        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        public ServiceResult Get()
        {
            _serviceResult.Data = _customerGroupRepository.Get();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public ServiceResult GetById(Guid customerGroupId)
        {
            _serviceResult.Data = _customerGroupRepository.GetById(customerGroupId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public ServiceResult Update(CustomerGroup customerGroup, Guid customerGroupId)
        {
            _serviceResult.Data = _customerGroupRepository.Update(customerGroup, customerGroupId);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;
            return _serviceResult;
        }
    }
}
