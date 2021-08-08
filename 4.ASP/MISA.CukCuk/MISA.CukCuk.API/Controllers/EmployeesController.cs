using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.API.Models;
using MISA.CukCuk.API.Response;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Fields

        private readonly IConfiguration _config;

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public EmployeesController(IConfiguration config)
        {
            _config = config;

            // Lấy thông tin truy cập db
            _connectionString = _config.GetValue<string>("ConnectionString:CukCuk");

            // Khởi tạo đối tượng kết nối db
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #endregion

        #region Get Requests

        /// <summary>
        /// Lấy dữ liệu toàn bộ khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees()
        {
            // Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Employee";

            var employees = _dbConnection.Query<Employee>(sqlQuery);

            return StatusCode(200, employees);
        }

        /// <summary>
        /// Lấy dữ liệu khách hàng ứng với id
        /// </summary>
        /// <param name="EmployeeId">Id khách hàng</param>
        /// <returns></returns>
        [HttpGet("{EmployeeId}")]
        public IActionResult GetEmployeeByid(string employeeId)
        {
            var sqlQuery = $"SELECT e.*, CASE " +
                $"WHEN e.Gender=0 THEN 'Nữ' " +
                $"WHEN e.Gender=1 THEN 'Nam' " +
                $"ELSE 'Không xác định' " +
                $"END as GenderName " +
                $"FROM Employee e WHERE e.EmployeeId = @EmployeeId";

            var parameters = new DynamicParameters();

            parameters.Add("@EmployeeId", employeeId);

            // Lấy dữ liệu và phản hồi cho client
            try
            {
                var employee = _dbConnection.QueryFirstOrDefault<object>(sqlQuery, param: parameters);
                return StatusCode(200, employee != null ? employee : "Not found!");
            }
            catch (Exception)
            {
                return StatusCode(500, Properties.Resources.MISAErrorMessage);
            }
        }


        /// <summary>
        /// Lọc danh sách khách hàng theo các tiêu chí: phân trang, tìm kiếm, lọc theo nhóm khách hàng
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên một trang</param>
        /// <param name="pageNumber">Chỉ số trang cần xem</param>
        /// <param name="filterString">Chuỗi cần tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("employeeFilter")]
        public IActionResult GetEmployeeFilter( int pageSize, int pageNumber, 
                                                string filterString, string departmentId, string positionId )
        {
            var sqlSelectCount = "SELECT COUNT(e.EmployeeId) FROM Employee e ";

            var sqlQuery = $"SELECT e.*, d.DepartmentName, p.PositionName, CASE " +
                                $"WHEN e.Gender = 0 THEN 'Nữ' " +
                                $"WHEN e.Gender = 1 THEN 'Nam' " +
                                $"ELSE 'Không xác định' " +
                                $"END as GenderName "  +
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

            // Phân trang cho kết quả truy vấn
            sqlQuery += (pageSize > 0)? "LIMIT @pageStart, @pageSize;" : "" ;
            sqlSelectCount += "ORDER BY e.EmployeeId";

            // Thực hiện truy vấn lấy dữ liệu
            var employees = _dbConnection.Query<object>(sqlQuery, param: parameters);

            var totalRecord = _dbConnection.QueryFirstOrDefault<int>(sqlSelectCount, param:parameters);

            var totalPage = (int)(totalRecord / pageSize) + ((totalRecord % pageSize != 0) ? 1 : 0);



            var response = new EmployeeFilterResponse
            {
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                Data = (List<object>)employees
            };
            // Trả dữ liệu về cho client
            return StatusCode(200, response);
        }

        #endregion

        #region Post Requests

        /// <summary>
        /// Thêm thông tin khách hàng mới vào db
        /// </summary>
        /// <param name="Employee">Object thông tin khách hàng muốn thêm vào db</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertEmployee(Employee employee)
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

            // Thự thi truy vấn
            //try
            {
                var numberRowAffects = _dbConnection.Execute(sqlQuery, param: parameters);

                // Trả lại số dòng được thêm vào db cho client
                return StatusCode(200, $"{numberRowAffects} row(s) affected");
            }
            //catch (Exception)
            //{
            //    return StatusCode(400, Properties.Resources.MISAErrorMessage);
            //}
        }

        #endregion

        #region PUT requests

        /// <summary>
        /// Cập nhật thông tin của 1 khách hàng vào trong db
        /// </summary>
        /// <param name="EmployeeId">Id khách hàng</param>
        /// <param name="Employee">Dữ liệu muốn cập nhật</param>
        /// <returns></returns>
        [HttpPut("{EmployeeId}")]
        public IActionResult EditEmployee(string EmployeeId, Employee Employee)
        {
            var queryLine = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = Employee.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(Employee);

                queryLine.Add($"{propName} = @{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            parameters.Add("@OldEmployeeId", EmployeeId);
            var sqlQuery = $"UPDATE Employee SET {String.Join(", ", queryLine.ToArray())} " +
                            $"WHERE EmployeeId = @OldEmployeeId";


            // Thực thi truy vấn và trả về kết quả cho client
            try
            {
                var numberRowAffects = _dbConnection.Execute(sqlQuery, param: parameters);
                return StatusCode(200, $"Đã cập nhật {numberRowAffects} dòng trong database.");
            }
            catch (Exception)
            {
                return StatusCode(400, Properties.Resources.MISAErrorMessage);
            }
        }

        #endregion

        #region Delete Requests

        /// <summary>
        /// Xóa khách hàng với id tương ứng
        /// </summary>
        /// <param name="EmployeeId">Id của khách hàng muốn xóa</param>
        /// <returns></returns>

        [HttpDelete("{EmployeeId}")]
        public IActionResult DeleteEmployeeById(string EmployeeId)
        {
            var sqlQuery = $"DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
            var parameters = new DynamicParameters();

            parameters.Add("@EmployeeId", EmployeeId);

            try
            {
                var rowAffects = _dbConnection.Execute(sqlQuery, param: parameters);
                return StatusCode(200, $"Đã xóa {rowAffects} bản ghi.");
            }
            catch (Exception)
            {
                return StatusCode(500, Properties.Resources.MISAErrorMessage);
            }
        }

        #endregion
    }
}

