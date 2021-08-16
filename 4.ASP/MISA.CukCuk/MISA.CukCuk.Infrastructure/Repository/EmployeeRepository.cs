using Dapper;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Responses;
using System;
using System.Collections.Generic;

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
        public FilterResponse GetByFilter(int pageSize, int pageNumber, string filterString, Guid? departmentId, Guid? positionId)
        {
            var sqlSelectCount = "SELECT COUNT(*) FROM Employee e ";

            var sqlQuery = $"SELECT e.*, d.DepartmentName, p.PositionName, CASE " +
                                $"WHEN e.Gender = 0 THEN 'Nữ' " +
                                $"WHEN e.Gender = 1 THEN 'Nam' " +
                                $"ELSE 'Không xác định' " +
                                $"END as GenderName " +
                            $"FROM Employee e LEFT JOIN Department d ON d.DepartmentId=e.DepartmentId " +
                            $"LEFT JOIN Position p ON p.PositionId=e.PositionId ";
            var parameters = new DynamicParameters();

            parameters.Add("@pageSize", pageSize);
            parameters.Add("@pageStart", pageNumber * pageSize);

            if (filterString == null) filterString = "";
            var sqlWhere = "WHERE ( UPPER(e.FullName) LIKE CONCAT('%',@filter,'%') " +
                        "OR UPPER(e.EmployeeCode) LIKE CONCAT('%',@filter,'%') " +
                        "OR e.PhoneNumber LIKE CONCAT('%',@filter,'%') ) ";

            parameters.Add("@filter", filterString.ToUpper());

            // Lọc theo id phòng ban và vị trí
            if (departmentId != null)
            {
                sqlWhere += "AND e.DepartmentId=@departmentId ";
                parameters.Add("@departmentId", departmentId);
            }
            if (positionId != null)
            {
                sqlWhere += "AND e.PositionId=@positionId ";
                parameters.Add("@positionId", positionId);
            }

            sqlQuery += sqlWhere;
            sqlSelectCount += sqlWhere;

            // Sắp xếp theo chiều giảm dần mã nv
            sqlQuery += "ORDER BY e.EmployeeCode DESC ";

            // Phân trang cho kết quả truy vấn
            sqlQuery += (pageSize > 0) ? "LIMIT @pageStart, @pageSize;" : "";
            sqlSelectCount += "ORDER BY e.EmployeeId";

            // Thực hiện truy vấn lấy dữ liệu
            var employees = _dbConnection.Query<object>(sqlQuery, param: parameters);

            if (employees == null)
            {
                return new FilterResponse
                {
                    TotalRecord = 0,
                    TotalPage = 0,
                    Data = null
                };
            }

            var totalRecord = _dbConnection.QueryFirstOrDefault<int>(sqlSelectCount, param: parameters);
            var totalPage = (int)(totalRecord / pageSize) + ((totalRecord % pageSize != 0) ? 1 : 0);

            return new FilterResponse
            {
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                Data = (List<object>)employees
            };
        }

        /// <summary>
        /// Lấy mã mới
        /// </summary>
        /// <returns></returns>
        public string GetNewCode()
        {
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

            return newEmployeeCode;
        }

        #endregion
    }
}
