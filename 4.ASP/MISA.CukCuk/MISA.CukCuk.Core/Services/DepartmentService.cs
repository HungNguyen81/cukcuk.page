using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields

        IDepartmentRepository _departmentRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
            _serviceResult = new ServiceResult();
        }

        #endregion

        #region GET methods
        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        public ServiceResult Get()
        {
            _serviceResult.Data = _departmentRepository.Get();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ServiceResult GetById(Guid departmentId)
        {
            _serviceResult.Data = _departmentRepository.GetById(departmentId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }
        #endregion

    }
}
