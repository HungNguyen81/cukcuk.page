using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.API.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
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

                if (customerGroups == null)
                {
                    var response = new
                    {
                        userMsg = Properties.Resources.MISANoContentMsg,
                    };

                    return StatusCode(204, response);
                }

                return StatusCode(200, customerGroups);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_001",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
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

                if(customerGroup == null)
                {
                    var response = new
                    {
                        userMsg = Properties.Resources.MISANoContentMsg,
                    };
                    return StatusCode(204, response);
                }

                return StatusCode(200, customerGroup);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_002",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
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

                if(numberRowAffects < 1)
                {
                    return StatusCode(400, new {
                        userMsg = Properties.Resources.MISAErrorMessage
                    });
                }
                var response = new
                {
                    userMsg = Properties.Resources.MISAInsertMsg
                };
                return StatusCode(201, response);
            }
            catch (Exception e)
            {
                // Có exception xảy ra, trả về thông báo lỗi
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISABadRequestMsg,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion

        #region PUT Requests

        /// <summary>
        /// Sửa thông tin của một nhóm khách hàng với id tương ứng
        /// </summary>
        /// <param name="id">    id của nhóm khách hàng</param>
        /// <param name="group"> Dữ liệu cập nhật</param>
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

                if (numberRowAffects < 1)
                {
                    return StatusCode(500, new
                    {
                        devMsg = Properties.Resources.MISASqlErrorMsg,
                        userMsg = Properties.Resources.MISAErrorMessage,
                        errorCode = "MISA_004",
                        traceId = Guid.NewGuid().ToString()
                    });
                }

                var response = new
                {
                    userMsg = Properties.Resources.MISAUpdateMsg
                };
                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISABadRequestMsg,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(400, response);
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
                var numberRowAffects = _dbConnection.Execute(sqlQuery, param: parameters);

                if (numberRowAffects < 1)
                {
                    return StatusCode(400, new
                    {
                        devMsg = Properties.Resources.MISASqlErrorMsg,
                        userMsg = Properties.Resources.MISAErrorMessage,
                        errorCode = "MISA_001",
                        traceId = Guid.NewGuid().ToString()
                    });
                }
                var response = new
                {                    
                    userMsg = Properties.Resources.MISADeleteMsg
                };
                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISABadRequestMsg,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion
    }
}
