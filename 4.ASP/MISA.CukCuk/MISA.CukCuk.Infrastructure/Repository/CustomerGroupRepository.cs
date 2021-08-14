using Dapper;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class CustomerGroupRepository : ICustomerGroupRepository
    {

        #region Fields        

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public CustomerGroupRepository()
        {
            // Lấy thông tin truy cập db
            _connectionString = MISA.CukCuk.Core.Resource.ResourceVN.ConnectionString;

            // Khởi tạo đối tượng kết nối db
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #endregion

        #region Các phương thức GET

        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        public List<CustomerGroup> Get()
        {
            var sqlQuery = "SELECT * FROM CustomerGroup";
            return (List<CustomerGroup>)_dbConnection.Query<CustomerGroup>(sqlQuery);
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public CustomerGroup GetById(Guid customerGroupId)
        {
            // Lấy dữ liệu từ db
            var paramName = "CustomerGroupId";
            var sqlQuery = $"SELECT * FROM CustomerGroup WHERE CustomerGroupId = @{paramName}";

            var parameters = new DynamicParameters();
            parameters.Add($"@{paramName}", customerGroupId);
            
            return _dbConnection.QueryFirstOrDefault<CustomerGroup>(sqlQuery, param: parameters);
        }

        #endregion

        #region Thêm mới

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <returns></returns>
        public int Add(CustomerGroup customerGroup)
        {
            // Tạo id mới
            customerGroup.CustomerGroupId = Guid.NewGuid();

            var columnsName = new List<string>();
            var columnsParam = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = customerGroup.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(customerGroup);

                columnsName.Add(propName);
                columnsParam.Add($"@{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            var sqlQuery = $"INSERT INTO CustomerGroup({String.Join(", ", columnsName.ToArray())}) " +
                            $"VALUES({String.Join(", ", columnsParam.ToArray())})";

            // Thự thi truy vấn
            return _dbConnection.Execute(sqlQuery, param: parameters);
        }

        #endregion

        #region Cập nhật

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public int Update(CustomerGroup customerGroup, Guid customerGroupId)
        {
            var queryLine = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = customerGroup.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(customerGroup);

                queryLine.Add($"{propName} = @{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            parameters.Add("@oldCustomerGroupId", customerGroupId);
            var sqlQuery = $"UPDATE CustomerGroup SET {String.Join(", ", queryLine.ToArray())} " +
                           $"WHERE CustomerGroupId = @oldCustomerGroupId";

            // Thực thi truy vấn và trả về kết quả cho client
            return _dbConnection.Execute(sqlQuery, param: parameters);

        }

        #endregion

        #region Xóa

        /// <summary>
        /// Xóa một bản ghi
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int DeleteOne(Guid customerGroupId)
        {
            var sqlQuery = $"DELETE FROM CustomerGroup WHERE CustomerGroupId = @id";
            var parameters = new DynamicParameters();

            parameters.Add("@id", customerGroupId);

            return _dbConnection.Execute(sqlQuery, param: parameters);
        }

        #endregion
    }
}
