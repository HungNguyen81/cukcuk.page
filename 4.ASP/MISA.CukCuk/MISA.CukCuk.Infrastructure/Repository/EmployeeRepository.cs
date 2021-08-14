using Dapper;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Resource;
using MISA.CukCuk.Core.Responses;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region Fields        

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public EmployeeRepository()
        {

            // Lấy thông tin truy cập db
            _connectionString = ResourceVN.ConnectionString;

            // nvởi tạo đối tượng kết nối db
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #endregion

        #region Thêm mới

        /// <summary>
        /// Thêm mới nv
        /// </summary>
        /// <param name="employee"> Data thêm mới</param>
        /// <returns></returns>
        public int Add(Employee employee)
        {
            var columnsName = new List<string>();
            var columnsParam = new List<string>();
            var parameters = new DynamicParameters();

            // Tạo id mới
            employee.EmployeeId = Guid.NewGuid();

            // Đọc từng property của object
            var properties = employee.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(employee);

                columnsName.Add(propName);
                columnsParam.Add($"@{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            var sqlQuery = $"INSERT INTO Employee({String.Join(", ", columnsName.ToArray())}) " +
                            $"VALUES({String.Join(", ", columnsParam.ToArray())})";

            return _dbConnection.Execute(sqlQuery, param: parameters);
        }

        #endregion

        #region Xóa

        /// <summary>
        /// Xóa nhiều nv
        /// </summary>
        /// <param name="employeeIds">List id của các nv cần xóa</param>
        /// <returns></returns>
        public int DeleteMany(List<Guid> employeeIds)
        {
            var parameters = new DynamicParameters();
            var paramName = new List<string>();

            for (int i = 0; i < employeeIds.Count; i++)
            {
                var id = employeeIds[i];
                paramName.Add($"@id{i}");
                parameters.Add($"@id{i}", id.ToString());
            }

            var sql = $"Delete from Employee where EmployeeId In ({String.Join(", ", paramName.ToArray())})";

            return _dbConnection.Execute(sql, param: parameters);
        }


        /// <summary>
        /// Xóa một nv với id tương ứng
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int DeleteOne(Guid employeeId)
        {
            var sqlQuery = $"DELETE FROM Employee WHERE EmployeeId = @employeeId";
            var parameters = new DynamicParameters();

            parameters.Add("@employeeId", employeeId);

            return _dbConnection.Execute(sqlQuery, param: parameters);
        }

        #endregion

        #region Các phương thức GET

        /// <summary>
        /// Lấy tất cả data
        /// </summary>
        /// <returns></returns>
        public List<Employee> Get()
        {
            // Lấy dữ liệu
            var sqlQuery = "SELECT e.*, CASE " +
                           "WHEN e.Gender=0 THEN 'Nữ' " +
                           "WHEN e.Gender=1 THEN 'Nam' " +
                           "ELSE 'Không xác định' END as GenderName " +
                           "FROM Employee e";
            return (List<Employee>)_dbConnection.Query<Employee>(sqlQuery);
        }

        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public object GetById(Guid employeeId)
        {
            var sqlQuery = $"SELECT e.*, CASE " +
                $"WHEN e.Gender=0 THEN 'Nữ' " +
                $"WHEN e.Gender=1 THEN 'Nam' " +
                $"ELSE 'Không xác định' END as GenderName " +
                $"FROM Employee e WHERE e.EmployeeId = @employeeId";
            var parameters = new DynamicParameters();

            parameters.Add("@employeeId", employeeId);

            // Lấy dữ liệu và phản hồi cho client
            return _dbConnection.QueryFirstOrDefault<object>(sqlQuery, param: parameters);
        }

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

        #region Cập nhật

        /// <summary>
        /// Cập nhật thông tin nv
        /// </summary>
        /// <param name="employee">     Data</param>
        /// <param name="employeeId">   Id</param>
        /// <returns></returns>
        public int Update(Employee employee, Guid employeeId)
        {
            var queryLine = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = employee.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(employee);

                queryLine.Add($"{propName} = @{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            parameters.Add("@oldEmployeeId", employeeId);
            var sqlQuery = $"UPDATE Employee SET {String.Join(", ", queryLine.ToArray())} " +
                            $"WHERE EmployeeId = @oldEmployeeId";

            return _dbConnection.Execute(sqlQuery, param: parameters);
        }

        #endregion
    }
}
