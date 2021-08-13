using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Repositiories
{
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        List<Employee> Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        object GetById(Guid employeeId);

        /// <summary>
        /// Lấy và lọc dữ liệu
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        FilterResponse GetByFilter(int pageSize, int pageNumber, string filterString, Guid? departmentId, Guid? positionId);

        /// <summary>
        /// Lấy mã nv mới
        /// </summary>
        /// <returns></returns>
        string GetNewCode();

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int Add(Employee employee);


        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        int Update(Employee employee, Guid employeeId);

        /// <summary>
        /// Xóa một
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        int DeleteOne(Guid employeeId);

        /// <summary>
        /// Xóa nhiều
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        int DeleteMany(List<Guid> employeeIds);
    }
}
