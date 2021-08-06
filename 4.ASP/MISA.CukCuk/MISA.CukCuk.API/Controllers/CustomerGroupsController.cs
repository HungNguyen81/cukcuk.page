using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.API.Models;
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
    public class CustomerGroupsController : ControllerBase
    {
        #region Fields

        private readonly IConfiguration _config;

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public CustomerGroupsController(IConfiguration config)
        {
            // Khởi tạo thông tin kết nối đến db
            _config = config;
            _connectionString = config.GetValue<string>("ConnectionString:CukCuk");
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #endregion

        #region GET Methods

        /// <summary>
        /// Lấy dữ liệu tất cả nhóm khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllCustomerGroup()
        {            
            var sqlQuery = "SELECT * FROM CustomerGroup";

            try
            {
                var customerGroups = _dbConnection.Query<CustomerGroup>(sqlQuery);
                return StatusCode(200, customerGroups);
            }
            catch (Exception)
            {
                return StatusCode(404, "Không tìm thấy nhóm khách hàng nào!");
            }
        }

        /// <summary>
        /// Lấy thông tin 1 nhóm khách hàng theo id từ db
        /// </summary>
        /// <param name="customerGroupId">id nhóm khách hàng trong db</param>
        /// <returns></returns>
        [HttpGet("{CustomerGroupId}")]
        public IActionResult GetCustomerGroupById(string customerGroupId)
        {
            // Lấy dữ liệu từ db
            var paramName = "CustomerGroupId";
            var sqlQuery = $"SELECT * FROM CustomerGroup WHERE CustomerGroupId = @{paramName}";

            var parameters = new DynamicParameters();
            parameters.Add($"@{paramName}", customerGroupId);

            // Phản hồi lại cho client
            try
            {
                var customerGroup = _dbConnection.QueryFirstOrDefault<CustomerGroup>(sqlQuery, param: parameters);
                return StatusCode(200, customerGroup);
            }
            catch (Exception)
            {
                return StatusCode(404, $"Không tìm thấy nhóm khách hàng có id = \"{customerGroupId}\"!");
            }
        }

        #endregion
    }
}
