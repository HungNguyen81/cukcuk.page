using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class DepartmentsController : ControllerBase
    {
        #region Fields

        private readonly IConfiguration _config;

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public DepartmentsController(IConfiguration config)
        {
            _config = config;

            // Lấy thông tin truy cập db
            _connectionString = _config.GetValue<string>("ConnectionString:CukCuk");

            // Khởi tạo đối tượng kết nối db
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #endregion

        #region GET requests

        /// <summary>
        /// Lấy dữ liệu tất cả phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            // Chuẩn bị truy vấn dữ liệu
            var sqlQuery = "SELECT * FROM Department";

            // lấy dữ liệu
            var Departments = _dbConnection.Query<object>(sqlQuery);

            // phản hồi về cho client
            return StatusCode(200, Departments);
        }

        [HttpGet("{departmentId}")]
        public IActionResult GetDepartmentById(string departmentId)
        {
            var sqlQuery = $"SELECT * FROM Department p WHERE p.DepartmentId = @DepartmentId";

            var parameters = new DynamicParameters();

            parameters.Add("@DepartmentId", departmentId);

            // Lấy dữ liệu và phản hồi cho client
            try
            {
                var department = _dbConnection.QueryFirstOrDefault<object>(sqlQuery, param: parameters);
                if (department == null) return StatusCode(204, "not found !");
                return StatusCode(200, department);
            }
            catch (Exception)
            {
                return StatusCode(500, Properties.Resources.MISAErrorMessage);
            }
        }

        #endregion
    }
}
