using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.API.Models;
using MISA.CukCuk.API.Responses;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
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

            try
            {
                var customers = _dbConnection.Query<Customer>(sqlQuery);

                if (customers == null)
                {
                    var response = new
                    {
                        userMsg = Properties.Resources.MISANoContentMsg,
                    };
                    return StatusCode(204, response);
                }

                return StatusCode(200, customers);
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
        /// Lấy dữ liệu khách hàng ứng với id
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <returns></returns>
        [HttpGet("{CustomerId}")]
        public IActionResult GetCustomerByid(string customerId)
        {
            var sqlQuery = $"SELECT * FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", customerId);

            // Lấy dữ liệu và phản hồi cho client
            try
            {
                var customer = _dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);

                if (customer == null)
                {
                    var response = new
                    {
                        userMsg = Properties.Resources.MISANoContentMsg,
                    };
                    return StatusCode(204, response);
                }

                return StatusCode(200, customer);
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
        /// Lọc danh sách khách hàng theo các tiêu chí: phân trang, tìm kiếm, lọc theo nhóm khách hàng
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên một trang</param>
        /// <param name="pageNumber">Chỉ số trang cần xem</param>
        /// <param name="filterString">Chuỗi cần tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("customerFilter")]
        public IActionResult GetCustomerFilter(int pageSize, int pageNumber, string filterString, string customerGroupId)
        {
            var sqlSelectCount = "SELECT COUNT(*) FROM Customer c ";

            var sqlQuery = $"SELECT c.*, cg.CustomerGroupName, CASE " +
                                $"WHEN c.Gender = 0 THEN 'Nữ' " +
                                $"WHEN c.Gender = 1 THEN 'Nam' " +
                                $"ELSE 'Không xác định' " +
                                $"END as GenderName " +
                            $"FROM Customer c LEFT JOIN CustomerGroup cg ON cg.CustomerGroupId=c.CustomerGroupId ";
            var parameters = new DynamicParameters();

            parameters.Add("@pageSize", pageSize);
            parameters.Add("@pageStart", pageNumber * pageSize);

            if (filterString == null) filterString = "";
            var sqlWhere = "WHERE ( UPPER(c.FullName) LIKE CONCAT('%',@filter,'%') " +
                        "OR UPPER(c.CustomerCode) LIKE CONCAT('%',@filter,'%') " +
                        "OR c.PhoneNumber LIKE CONCAT('%',@filter,'%') ) ";

            parameters.Add("@filter", filterString.ToUpper());

            // Lọc theo id nhóm KH
            if (customerGroupId != null)
            {
                sqlWhere += "AND c.CustomerGroupId=@CustomerGroupId ";
                parameters.Add("@CustomerGroupId", customerGroupId);
            }
            sqlQuery += sqlWhere;
            sqlSelectCount += sqlWhere;

            // Sắp xếp theo chiều giảm dần mã KH
            sqlQuery += "ORDER BY c.CustomerCode DESC ";

            // Phân trang cho kết quả truy vấn
            sqlQuery += (pageSize > 0) ? "LIMIT @pageStart, @pageSize;" : "";
            sqlSelectCount += "ORDER BY c.CustomerId";

            try
            {
                // Thực hiện truy vấn lấy dữ liệu
                var customers = _dbConnection.Query<object>(sqlQuery, param: parameters);

                if (customers == null)
                {
                    var response = new
                    {
                        userMsg = Properties.Resources.MISANoContentMsg,
                    };
                    return StatusCode(204, response);
                }

                var totalRecord = _dbConnection.QueryFirstOrDefault<int>(sqlSelectCount, param: parameters);
                var totalPage = (int)(totalRecord / pageSize) + ((totalRecord % pageSize != 0) ? 1 : 0);

                var filterResponse = new FilterResponse
                {
                    TotalRecord = totalRecord,
                    TotalPage = totalPage,
                    Data = (List<object>)customers
                };
                // Trả dữ liệu về cho client
                return StatusCode(200, filterResponse);
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

                if (numberRowAffects < 1)
                {
                    return StatusCode(500, new
                    {
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
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(400, response);
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
            var sqlQuery =  $"UPDATE Customer SET {String.Join(", ", queryLine.ToArray())} " +
                            $"WHERE CustomerId = @OldCustomerId";


            // Thực thi truy vấn và trả về kết quả cho client
            try
            {
                var numberRowAffects = _dbConnection.Execute(sqlQuery, param: parameters);

                if (numberRowAffects < 1)
                {
                    return StatusCode(500, new
                    {
                        userMsg = Properties.Resources.MISAErrorMessage
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
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(400, response);
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
                var numberRowAffects = _dbConnection.Execute(sqlQuery, param: parameters);

                if (numberRowAffects < 1)
                {
                    return StatusCode(500, new
                    {
                        userMsg = Properties.Resources.MISAErrorMessage
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
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(400, response);
            }
        }


        /// <summary>
        /// Xóa nhiều bản ghi qua một request
        /// </summary>
        /// <param name="customerIds">Mảng id của các bản ghi cần xóa</param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteCustomers([FromBody] List<string> customerIds)
        {
            var parameters = new DynamicParameters();
            var paramName = new List<string>();

            for (int i = 0; i < customerIds.Count; i++)
            {
                var id = customerIds[i];
                paramName.Add($"@id{i}");
                parameters.Add($"@id{i}", id);
            }

            var sql = $"Delete from Customer where CustomerId In ({String.Join(", ", paramName.ToArray())})";

            try
            {
                var numberRowAffects = _dbConnection.Execute(sql, param: parameters);

                if (numberRowAffects < 1)
                {
                    return StatusCode(500, new
                    {
                        userMsg = Properties.Resources.MISAErrorMessage
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
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(400, response);
            }

            
        }

        #endregion
    }
}
