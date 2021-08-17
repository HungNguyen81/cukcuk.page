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


        #region Các phương thức GET của riêng Employee

        /// <summary>
        /// Lấy mã nv mới
        /// </summary>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (17-08-2021)
        //@ Modified_By: HungNguyen81 (17-08-2021)
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
        //@ Created_By: HungNguyen81 (17-08-2021)
        //@ Modified_By: HungNguyen81 (17-08-2021)
        public ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString, Guid? departmentId, Guid? positionId)
        {
            _serviceResult.Data = _employeeRepository.GetByFilter(pageSize, pageNumber, filterString, departmentId, positionId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }
        
        #endregion
    }
}
