using Dapper;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Các phương thức GET của riêng Employee

        /// <summary>
        /// Lọc dữ liệu theo chuỗi tìm kiếm hoặc nhóm nv, kết hợp phân trang
        /// </summary>
        /// <param name="pageSize">         số bản ghi 1 trang</param>
        /// <param name="pageNumber">       chỉ số trang</param>
        /// <param name="filterString">     chuỗi tìm kiếm</param>
        /// <param name="employeeGroupId">  id nhóm nv</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public FilterResponse GetByFilter(int pageSize, int pageNumber, string filterString, 
                                            Guid? departmentId, Guid? positionId)
        {
            filterString ??= "";
            var parameters = new DynamicParameters();
            parameters.Add("m_pageSize", pageSize);
            parameters.Add("m_pageStart", pageNumber * pageSize);
            parameters.Add("m_filterString", filterString);
            parameters.Add("m_departmentId", departmentId == null ? "" : departmentId.ToString());
            parameters.Add("m_positionId", positionId == null ? "" : positionId.ToString());

            _dbConnection.Open();

            var procName = "Proc_EmployeeFilter";
            var result = _dbConnection.QueryMultiple(procName, param: parameters,
                    commandType: CommandType.StoredProcedure);

            var employees = result.Read<object>().ToList();
            var totalRecord = result.Read<int>().ToList()[0];
            var totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            _dbConnection.Close();

            if (employees == null)
            {
                return new FilterResponse
                {
                    TotalRecord = 0,
                    TotalPage = 0,
                    Data = null
                };
            }
            return new FilterResponse
            {
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                Data = employees
            };
        }

        /// <summary>
        /// Lấy mã mới
        /// </summary>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public string GetNewCode()
        {
            _dbConnection.Open();
            var sqlQuery = "SELECT e.EmployeeCode FROM Employee e ORDER BY e.EmployeeCode DESC LIMIT 1";
            var employeeCode = _dbConnection.QueryFirstOrDefault<string>(sqlQuery);
            int employeeNumber;
            string newEmployeeCode;

            if (employeeCode != null)
            {
                employeeNumber = int.Parse(employeeCode.Split("NV")[1]);
                employeeNumber++;
                newEmployeeCode = $"NV{employeeNumber.ToString().PadLeft(5, '0')}";
            } 
            else
            {
                newEmployeeCode = "NV00001";
            }
            _dbConnection.Close();

            return newEmployeeCode;
        }

        #endregion
    }
}
