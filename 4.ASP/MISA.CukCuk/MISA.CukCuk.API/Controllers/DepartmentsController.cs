using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;

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

            try
            {
                // lấy dữ liệu
                var departments = _dbConnection.Query<object>(sqlQuery);

                if (departments == null)
                {
                    var response = new
                    {
                        userMsg = Properties.Resources.MISANoContentMsg,
                    };
                    return StatusCode(204, response);
                }

                // phản hồi về cho client
                return StatusCode(200, departments);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Lấy thông tin phòng ban theo id
        /// </summary>
        /// <param name="departmentId"> id phòng ban cần lấy thông tin</param>
        /// <returns></returns>
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

                if (department == null)
                {
                    var response = new
                    {
                        userMsg = Properties.Resources.MISANoContentMsg,
                    };
                    return StatusCode(204, response);
                }

                return StatusCode(200, department);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion
    }
}
