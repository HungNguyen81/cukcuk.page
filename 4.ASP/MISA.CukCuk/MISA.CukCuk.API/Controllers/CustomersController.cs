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
    public class CustomersController : ControllerBase
    {
        #region Fields

        private readonly IConfiguration _config;

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public CustomersController(IConfiguration config)
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
        public IActionResult GetCustomers()
        {           
            // Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer";

            var customers = _dbConnection.Query<Customer>(sqlQuery);

            return StatusCode(200, customers);
        }

        [HttpGet("{CustomerId}")]
        public IActionResult GetCustomerByid(string customerId)
        {
            var sqlQuery = $"SELECT * FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", customerId);

            try
            {
                var customer = _dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);
                return StatusCode(200, customer != null? customer : "Not found!");
            }
            catch (Exception)
            {
                return StatusCode(500, Properties.Resources.MISAErrorMessage);
            }
        }

        #endregion

        #region Post Requests

        /// <summary>
        /// Thêm thông tin khách hàng mới vào db
        /// </summary>
        /// <param name="customer">Object thông tin khách hàng muốn thêm vào db</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
            var columnsName = new List<string>();
            var columnsParam = new List<string>();
            var parameters = new DynamicParameters();

            // Tạo id mới
            customer.CustomerId = Guid.NewGuid();

            // Đọc từng property của object
            var properties = customer.GetType().GetProperties();

            foreach (var prop in properties)    
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(customer);

                columnsName.Add(propName);
                columnsParam.Add($"@{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            var sqlQuery =  $"INSERT INTO Customer({String.Join(", ", columnsName.ToArray())}) " +
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
                return StatusCode(400, Properties.Resources.MISAErrorMessage);
            }
        }

        #endregion

        #region PUT requests

        /// <summary>
        /// Cập nhật thông tin của 1 khách hàng vào trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <param name="customer">Dữ liệu muốn cập nhật</param>
        /// <returns></returns>
        [HttpPut("{CustomerId}")]
        public IActionResult EditCustomer(string customerId, Customer customer)
        {
            var queryLine = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = customer.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(customer);

                queryLine.Add($"{propName} = @{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            parameters.Add("@OldCustomerId", customerId);
            var sqlQuery = $"UPDATE Customer SET {String.Join(", ", queryLine.ToArray())} WHERE CustomerId = @OldCustomerId";


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
        /// <param name="customerId">Id của khách hàng muốn xóa</param>
        /// <returns></returns>

        [HttpDelete("{CustomerId}")]
        public IActionResult DeleteCustomerById(string customerId)
        {
            var sqlQuery = $"DELETE FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", customerId);

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
