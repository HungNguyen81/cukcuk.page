using Dapper;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Responses;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        #region Fields        

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public CustomerRepository()
        {

            // Lấy thông tin truy cập db
            _connectionString = MISA.CukCuk.Core.Resource.ResourceVN.ConnectionString;

            // Khởi tạo đối tượng kết nối db
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #endregion

        #region Thêm mới

        /// <summary>
        /// Thêm mới khách hagnf
        /// </summary>
        /// <param name="customer"> Data thêm mới</param>
        /// <returns></returns>
        public int Add(Customer customer)
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

            var sqlQuery = $"INSERT INTO Customer({String.Join(", ", columnsName.ToArray())}) " +
                            $"VALUES({String.Join(", ", columnsParam.ToArray())})";

            return _dbConnection.Execute(sqlQuery, param: parameters);        
        }

        #endregion

        #region Xóa

        /// <summary>
        /// Xóa nhiều khách hàng
        /// </summary>
        /// <param name="customerIds">List id của các KH cần xóa</param>
        /// <returns></returns>
        public int DeleteMany(List<Guid> customerIds)
        {
            var parameters = new DynamicParameters();
            var paramName = new List<string>();

            for (int i = 0; i < customerIds.Count; i++)
            {
                var id = customerIds[i];
                paramName.Add($"@id{i}");
                parameters.Add($"@id{i}", id.ToString());
            }

            var sql = $"Delete from Customer where CustomerId In ({String.Join(", ", paramName.ToArray())})";

            return _dbConnection.Execute(sql, param: parameters);
        }


        /// <summary>
        /// Xóa một KH với id tương ứng
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
            public int DeleteOne(Guid customerId)
        {
            var sqlQuery = $"DELETE FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", customerId);

            return _dbConnection.Execute(sqlQuery, param: parameters);
        }

        #endregion

        #region Các phương thức GET

        /// <summary>
        /// Lấy tất cả data
        /// </summary>
        /// <returns></returns>
        public List<Customer> Get()
        {
            // Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer";
            return (List<Customer>)_dbConnection.Query<Customer>(sqlQuery);
        }

        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetById(Guid customerId)
        {
            var sqlQuery = $"SELECT * FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", customerId);

            // Lấy dữ liệu và phản hồi cho client
            return _dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);
        }

        /// <summary>
        /// Lọc dữ liệu theo chuỗi tìm kiếm hoặc nhóm khách hàng, kết hợp phân trang
        /// </summary>
        /// <param name="pageSize">         số bản ghi 1 trang</param>
        /// <param name="pageNumber">       chỉ số trang</param>
        /// <param name="filterString">     chuỗi tìm kiếm</param>
        /// <param name="customerGroupId">  id nhóm khách hàng</param>
        /// <returns></returns>
        public FilterResponse GetByFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId)
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

            // Thực hiện truy vấn lấy dữ liệu
            var customers = _dbConnection.Query<object>(sqlQuery, param: parameters);

            if (customers == null)
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
                Data = (List<object>)customers
            };
        }

        #endregion

        #region Cập nhật

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customer">     Data</param>
        /// <param name="customerId">   Id</param>
        /// <returns></returns>
        public int Update(Customer customer, Guid customerId)
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

            parameters.Add("@oldCustomerId", customerId);
            var sqlQuery = $"UPDATE Customer SET {String.Join(", ", queryLine.ToArray())} " +
                            $"WHERE CustomerId = @oldCustomerId";

            return _dbConnection.Execute(sqlQuery, param: parameters);
        }
        #endregion
    }
}
