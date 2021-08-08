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

        #region GET Requests

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

        #region POST Requests

        /// <summary>
        /// Thêm nhóm khách hàng mới
        /// </summary>
        /// <param name="group">Đối tượng chứa thông tin của nhóm khách hàng mới</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertCustomerGroup(CustomerGroup group)
        {
            // Tạo id mới
            group.CustomerGroupId = Guid.NewGuid();

            var columnsName = new List<string>();
            var columnsParam = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = group.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(group);

                columnsName.Add(propName);
                columnsParam.Add($"@{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            var sqlQuery = $"INSERT INTO CustomerGroup({String.Join(", ", columnsName.ToArray())}) " +
                            $"VALUES({String.Join(", ", columnsParam.ToArray())})";

            // Thự thi truy vấn
            try
            {
                var numberRowAffects = _dbConnection.Execute(sqlQuery, param: parameters);

                // Trả lại số dòng được thêm vào db cho client
                return StatusCode(200, $"{numberRowAffects} row(s) affected");
            }
            catch (Exception)
            {
                // Có exception xảy ra, trả về thông báo lỗi
                return StatusCode(400, Properties.Resources.MISAErrorMessage);
            }
        }

        #endregion

        #region PUT Requests

        /// <summary>
        /// Sửa thông tin của một nhóm khách hàng với id tương ứng
        /// </summary>
        /// <param name="id">id của nhóm khách hàng</param>
        /// <param name="group">Dữ liệu cập nhật</param>
        /// <returns></returns>
        [HttpPut("{CustomerGroupId}")]
        public IActionResult UpdateCustomerGroup(string CustomerGroupId, CustomerGroup group)
        {
            var queryLine = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = group.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(group);

                queryLine.Add($"{propName} = @{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            parameters.Add("@OldCustomerGroupId", CustomerGroupId);
            var sqlQuery = $"UPDATE CustomerGroup SET {String.Join(", ", queryLine.ToArray())} " +
                           $"WHERE CustomerGroupId = @OldCustomerGroupId";


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

        #region DELETE Requests

        /// <summary>
        /// Xóa nhóm khách hàng ứng với id
        /// </summary>
        /// <param name="id"> id của nhóm khách hàng cần xóa </param>
        /// <returns></returns>
        [HttpDelete("{CustomerGroupId}")]
        public IActionResult DeleteCustomerGroup(string CustomerGroupId)
        {
            var sqlQuery = $"DELETE FROM CustomerGroup WHERE CustomerGroupId = @id";
            var parameters = new DynamicParameters();

            parameters.Add("@id", CustomerGroupId);

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
