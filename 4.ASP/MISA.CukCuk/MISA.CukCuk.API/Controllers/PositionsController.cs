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
    public class PositionsController : ControllerBase
    {
        #region Fields

        private readonly IConfiguration _config;

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public PositionsController(IConfiguration config)
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
        /// Lấy dữ liệu tất cả vị trí/ chức vụ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            // Chuẩn bị truy vấn dữ liệu
            var sqlQuery = "SELECT * FROM Position";

            // lấy dữ liệu
            var positions = _dbConnection.Query<object>(sqlQuery);

            // phản hồi về cho client
            return StatusCode(200, positions);
        }

        [HttpGet("{PositionId}")]
        public IActionResult GetPositionById(string positionId)
        {
            var sqlQuery = $"SELECT * FROM Position p WHERE p.PositionId = @PositionId";

            var parameters = new DynamicParameters();

            parameters.Add("@PositionId", positionId);

            // Lấy dữ liệu và phản hồi cho client
            try
            {
                var position = _dbConnection.QueryFirstOrDefault<object>(sqlQuery, param: parameters);
                if (position == null) return StatusCode(204, "not found !");
                return StatusCode(200, position);
            }
            catch (Exception)
            {
                return StatusCode(500, Properties.Resources.MISAErrorMessage);
            }
        }

        #endregion

    }
}
