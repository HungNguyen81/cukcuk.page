using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Interfaces.Services
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        /// <summary>
        /// Lấy và lọc data
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString, Guid? departmentId, Guid? positionId);

        /// <summary>
        /// Lấy mã nv mới
        /// </summary>
        /// <returns></returns>
        ServiceResult GetNewCode();
    }
}
