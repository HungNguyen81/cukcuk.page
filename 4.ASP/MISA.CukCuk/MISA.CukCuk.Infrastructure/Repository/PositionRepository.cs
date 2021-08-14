using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Resource;
using MySqlConnector;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class PositionRepository : IPositionRepository
    {

        #region Fields        

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;

        #endregion

        #region Constructors

        public PositionRepository()
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
            var sqlQuery = "SELECT * FROM Position";
            return (List<object>)_dbConnection.Query<object>(sqlQuery);
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public Position GetById(Guid positionId)
        {
            var sqlQuery = $"SELECT * FROM Position p WHERE p.PositionId = @positionId";

            var parameters = new DynamicParameters();

            parameters.Add("@positionId", positionId);

            // Lấy dữ liệu và phản hồi cho client
            return _dbConnection.QueryFirstOrDefault<Position>(sqlQuery, param: parameters);
        }
        #endregion

    }
}
