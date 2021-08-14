using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Lấy toàn bộ 
        /// </summary>
        /// <returns></returns>
        ServiceResult Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ServiceResult GetById(Guid employeeId);

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

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        ServiceResult Add(Employee employee);

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ServiceResult Update(Employee employee, Guid employeeId);

        /// <summary>
        /// Xóa một bản ghi theo id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ServiceResult DeleteOne(Guid employeeId);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        ServiceResult DeleteMany(List<Guid> employeeIds);
    }
}
