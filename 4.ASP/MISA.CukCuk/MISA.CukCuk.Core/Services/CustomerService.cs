using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Helpers;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Services
{
    public class CustomerService : ICustomerService
    {
        #region Fields

        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Thêm mới

        /// <summary>
        /// Thêm bản ghi mới vào bảng khách hàng
        /// </summary>
        /// <param name="customer">Dữ liệu thêm mới</param>
        /// <returns></returns>
        public ServiceResult Add(Customer customer)
        {
            // Xử lý nghiệp vụ

            // 1. Kiểm tra mã khách hàng rỗng   

            if (!Validations.Required(customer.CustomerCode))
            {
                _serviceResult = new ServiceResult
                {
                    IsValid = false,
                    Data = new
                    {
                        userMsg = ResourceVN.CustomerCodeInvalidMsg,
                    }
                };

                return _serviceResult;
            }

            // 2. Validate định dạng email

            if (!Validations.EmailValidate(customer.Email))
            {
                _serviceResult = new ServiceResult
                {
                    IsValid = false,
                    Data = new
                    {
                        userMsg = ResourceVN.EmailInvalidMsg
                    }
                };
                return _serviceResult;
            }

            // 3. Kiểm tra trùng mã nv
            // ...

            // Kết nối infrastructure service làm việc với db

            _serviceResult.Data = _customerRepository.Add(customer);
            if ((int)_serviceResult.Data > 0)
            {
                _serviceResult.IsValid = true;
            }
            else
            {
                _serviceResult.IsValid = false;
                _serviceResult.Msg = ResourceVN.ErrorMsg;
            }
            return _serviceResult;
        }

        #endregion

        #region Cập nhật

        /// <summary>
        /// Cập nhật dữ liệu khách hàng
        /// </summary>
        /// <param name="customer">     Dữ liệu cập nhật</param>
        /// <param name="customerId">   Id của khác hàng</param>
        /// <returns></returns>
        public ServiceResult Update(Customer customer, Guid customerId)
        {
            // Xử lý nghiệp vụ

            // 1. Kiểm tra mã khách hàng rỗng   

            if (!Validations.Required(customer.CustomerCode))
            {
                _serviceResult = new ServiceResult
                {
                    IsValid = false,
                    Data = new
                    {
                        userMsg = ResourceVN.CustomerCodeInvalidMsg,
                    }
                };

                return _serviceResult;
            }

            // 2. Validate định dạng email

            if (!Validations.EmailValidate(customer.Email))
            {
                _serviceResult = new ServiceResult
                {
                    IsValid = false,
                    Data = new
                    {
                        userMsg = ResourceVN.EmailInvalidMsg
                    }
                };
                return _serviceResult;
            }

            // 3. Kiểm tra trùng mã nv
            // ...

            // Kết nối infrastructure service làm việc với db

            _serviceResult.Data = _customerRepository.Update(customer, customerId);

            if((int)_serviceResult.Data > 0)
            {
                _serviceResult.IsValid = true;
            } 
            else
            {
                _serviceResult.IsValid = false;
                _serviceResult.Msg = ResourceVN.ErrorMsg;
            }
            return _serviceResult;
        }

        #endregion

        #region Các phương thức xóa

        /// <summary>
        /// Xóa một bản ghi với id tương ứng
        /// </summary>
        /// <param name="customerId">id của khách hàng cần xóa</param>
        /// <returns></returns>
        public ServiceResult DeleteOne(Guid customerId)
        {
            _serviceResult.Data = _customerRepository.DeleteOne(customerId);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;

            return _serviceResult;
        }

        /// <summary>
        /// Xóa nhiều khách hàng
        /// </summary>
        /// <param name="customerIds">List id cần xóa</param>
        /// <returns></returns>
        public ServiceResult DeleteMany(List<Guid> customerIds)
        {
            _serviceResult.Data = _customerRepository.DeleteMany(customerIds);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;
            return _serviceResult;
        }

        #endregion

        #region Các phương thức GET

        /// <summary>
        /// Lấy toàn bộ data
        /// </summary>
        /// <returns></returns>
        public ServiceResult Get()
        {
            _serviceResult.Data = _customerRepository.Get();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ServiceResult GetById(Guid customerId)
        {
            _serviceResult.Data = _customerRepository.GetById(customerId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

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
