using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Responses;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Interfaces.Repositiories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
       
        //List<Employee> Get();

        //object GetById(Guid employeeId);

        /// <summary>
        /// Lọc dữ liệu trả về
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
       
        //int Add(Employee employee);

        //int Update(Employee employee, Guid employeeId);

        //int DeleteOne(Guid employeeId);

        //int DeleteMany(List<Guid> employeeIds);
    }
}
