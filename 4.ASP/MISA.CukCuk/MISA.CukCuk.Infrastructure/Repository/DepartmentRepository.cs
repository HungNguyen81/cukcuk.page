using Dapper;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Resource;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {

        #region Fields        

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public DepartmentRepository()
        {
            // Lấy thông tin truy cập db
            _connectionString = ResourceVN.ConnectionString;

            // Khởi tạo đối tượng kết nối db
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region GET methods

        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        public List<object> Get()
        {
            // Chuẩn bị truy vấn dữ liệu
            var sqlQuery = "SELECT * FROM Department";
            return (List<object>)_dbConnection.Query<object>(sqlQuery);
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public Department GetById(Guid departmentId)
        {
            var sqlQuery = $"SELECT * FROM Department p WHERE p.DepartmentId = @DepartmentId";

            var parameters = new DynamicParameters();

            parameters.Add("@DepartmentId", departmentId);

            // Lấy dữ liệu và phản hồi cho client
            return _dbConnection.QueryFirstOrDefault<Department>(sqlQuery, param: parameters);
        }
        #endregion

    }
}
