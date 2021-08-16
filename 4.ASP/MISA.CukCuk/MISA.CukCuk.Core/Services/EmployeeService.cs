using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Helpers;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Resource;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Fields

        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public EmployeeService(IEmployeeRepository employeeRepository):base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Override Thêm mới

        public override ServiceResult Add(Employee employee)
        {
            // Xử lý nghiệp vụ

            // 1. Kiểm tra mã khách hàng rỗng   

            if (!Validations.Required(employee.EmployeeCode))
            {
                _serviceResult = new ServiceResult
                {
                    IsValid = false,
                    Data = new
                    {
                        userMsg = ResourceVN.EmployeeCodeInvalidMsg,
                    }
                };

                return _serviceResult;
            }

            // 2. Validate định dạng email

            if (!Validations.EmailValidate(employee.Email))
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

            return base.Add(employee);
        }

        #endregion


        #region Override Cập nhật

        public override ServiceResult Update(Employee employee, Guid employeeId)
        {
            // Xử lý nghiệp vụ

            // 1. Kiểm tra mã khách hàng rỗng   

            if (!Validations.Required(employee.EmployeeCode))
            {
                _serviceResult = new ServiceResult
                {
                    IsValid = false,
                    Msg = ResourceVN.EmployeeCodeInvalidMsg
                };

                return _serviceResult;
            }

            // 2. Validate định dạng email

            if (!Validations.EmailValidate(employee.Email))
            {
                _serviceResult = new ServiceResult
                {
                    IsValid = false,
                    Msg = ResourceVN.EmailInvalidMsg
                };
                return _serviceResult;
            }

            // 3. Kiểm tra trùng mã nv
            // ...

            return base.Update(employee, employeeId);
        }

        #endregion

        #region Các phương thức GET của riêng Employee

        /// <summary>
        /// Lấy mã nv mới
        /// </summary>
        /// <returns></returns>
        public ServiceResult GetNewCode()
        {
            _serviceResult.Data = _employeeRepository.GetNewCode();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString, Guid? departmentId, Guid? positionId)
        {
            _serviceResult.Data = _employeeRepository.GetByFilter(pageSize, pageNumber, filterString, departmentId, positionId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        #endregion
    }
}
