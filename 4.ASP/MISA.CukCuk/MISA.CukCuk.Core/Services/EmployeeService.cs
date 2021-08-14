using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Helpers;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Resource;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        #region Fields

        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Thêm mới

        /// <summary>
        /// Thêm bản ghi mới vào bảng nv
        /// </summary>
        /// <param name="employee">Dữ liệu thêm mới</param>
        /// <returns></returns>
        public ServiceResult Add(Employee employee)
        {
            // Xử lý nghiệp vụ

            // 1. Kiểm tra mã nv rỗng   

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

            // Kết nối infrastructure service làm việc với db

            _serviceResult.Data = _employeeRepository.Add(employee);
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
        /// Cập nhật dữ liệu nv
        /// </summary>
        /// <param name="employee">     Dữ liệu cập nhật</param>
        /// <param name="employeeId">   Id của khác hàng</param>
        /// <returns></returns>
        public ServiceResult Update(Employee employee, Guid employeeId)
        {
            // Xử lý nghiệp vụ

            if (employee.EmployeeId == Guid.Empty)
            {
                return new ServiceResult
                {
                    IsValid = false,
                    Data = null,
                    Msg = ResourceVN.ErrorMsg
                };
            } 

            // 1. Kiểm tra mã nv rỗng   

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

            // Kết nối infrastructure service làm việc với db

            _serviceResult.Data = _employeeRepository.Update(employee, employeeId);

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

        #region Các phương thức xóa

        /// <summary>
        /// Xóa một bản ghi với id tương ứng
        /// </summary>
        /// <param name="employeeId">id của nv cần xóa</param>
        /// <returns></returns>
        public ServiceResult DeleteOne(Guid employeeId)
        {
            _serviceResult.Data = _employeeRepository.DeleteOne(employeeId);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;

            return _serviceResult;
        }

        /// <summary>
        /// Xóa nhiều nv
        /// </summary>
        /// <param name="employeeIds">List id cần xóa</param>
        /// <returns></returns>
        public ServiceResult DeleteMany(List<Guid> employeeIds)
        {
            _serviceResult.Data = _employeeRepository.DeleteMany(employeeIds);
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
            _serviceResult.Data = _employeeRepository.Get();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public ServiceResult GetById(Guid employeeId)
        {
            _serviceResult.Data = _employeeRepository.GetById(employeeId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

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
