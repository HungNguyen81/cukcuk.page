using Dapper;
using MISA.CukCuk.Core.CustomAttrs;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Resource;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        #region Fields        

        protected readonly string _connectionString;

        protected readonly IDbConnection _dbConnection;

        protected readonly string _entityName;

        #endregion

        #region Constructors

        public BaseRepository()
        {
            // Lấy thông tin truy cập db
            _connectionString = ResourceVN.LocalConnectionString;

            // Khởi tạo đối tượng kết nối db
            _dbConnection = new MySqlConnection(_connectionString);

            _entityName = typeof(MISAEntity).Name;
        }

        #endregion


        #region Các phương thức GET

        /// <summary>
        /// Lấy tất cả data
        /// </summary>
        /// <returns></returns>
        public virtual List<MISAEntity> Get()
        {
            // Lấy dữ liệu
            var sqlQuery = $"SELECT * FROM {_entityName}";

            _dbConnection.Open();

            var res = (List<MISAEntity>)_dbConnection.Query<MISAEntity>(sqlQuery);

            _dbConnection.Close();
            return res;
        }

        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public virtual MISAEntity GetById(Guid entityId)
        //:                                       ^ id của nv/kh
        {
            var sqlQuery = $"SELECT e.*, CASE " +
                $"WHEN e.Gender=0 THEN 'Nữ' " +
                $"WHEN e.Gender=1 THEN 'Nam' " +
                $"ELSE 'Không xác định' " +
                $"END as GenderName FROM {_entityName} e WHERE e.{_entityName}Id = @entityId";
            var parameters = new DynamicParameters();

            parameters.Add("@entityId", entityId);

            // Lấy dữ liệu và phản hồi cho client

            _dbConnection.Open();

            var res = _dbConnection.QueryFirstOrDefault<MISAEntity>(sqlQuery, param: parameters);

            _dbConnection.Close();

            return res;
        }

        /// <summary>
        /// Lấy thông tin theo mã 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public MISAEntity GetByCode(string code)
        //:                                 ^ mã nv/mã kh
        {
            // Lấy dữ liệu
            var sqlQuery = $"SELECT * FROM {_entityName} WHERE {_entityName}Code = @code";
            var parameter = new DynamicParameters();

            parameter.Add("@code", code);

            _dbConnection.Open();

            var res = _dbConnection.QueryFirstOrDefault<MISAEntity>(sqlQuery, param: parameter);

            _dbConnection.Close();
            return res;
        }

        public MISAEntity GetByPhoneNumber(string phoneNumber)
        {
            // Lấy dữ liệu
            var sqlQuery = $"SELECT * FROM {_entityName} WHERE PhoneNumber = @phone";
            var parameter = new DynamicParameters();

            parameter.Add("@phone", phoneNumber);

            _dbConnection.Open();

            var res = _dbConnection.QueryFirstOrDefault<MISAEntity>(sqlQuery, param: parameter);

            _dbConnection.Close();
            return res;
        }

        #endregion


        #region Thêm mới

        /// <summary>
        /// Thêm mới khách hagnf
        /// </summary>
        /// <param name="entity"> Data thêm mới</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public virtual int Add(MISAEntity entity)
        {
            var columnsName = new List<string>();
            var columnsParam = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = entity.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var propAttr = prop.GetCustomAttributes(typeof(MISADbColumnNotMatch), false);
                if (propAttr.Length != 0) continue;

                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(entity);

                // Tạo id mới
                if (propName.Equals($"{_entityName}Id") && prop.PropertyType == typeof(Guid))
                {
                    propValue = Guid.NewGuid();
                }

                columnsName.Add(propName);
                columnsParam.Add($"@{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            var sqlQuery = $"INSERT INTO {_entityName} ({String.Join(", ", columnsName.ToArray())}) " +
                            $"VALUES({String.Join(", ", columnsParam.ToArray())})";

            _dbConnection.Open();
            
            var res = _dbConnection.Execute(sqlQuery, param: parameters);

            _dbConnection.Close();
            return res;
        }

        #endregion


        #region Cập nhật

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="entity">     Data</param>
        /// <param name="entityId">   Id</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public virtual int Update(MISAEntity entity, Guid entityId)
        //:                                    ^ thực thể     ^ id
        {
            var queryLine = new List<string>();
            var parameters = new DynamicParameters();

            // Đọc từng property của object
            var properties = entity.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var propAttr = prop.GetCustomAttributes(typeof(MISADbColumnNotMatch), false);
                if (propAttr.Length != 0) continue;

                // Tên thuộc tính
                var propName = prop.Name;

                // Giá tri thuộc tính
                var propValue = prop.GetValue(entity);
                if (propName.Equals($"{_entityName}Id"))
                {
                    propValue = entityId;
                }

                queryLine.Add($"{propName} = @{propName}");
                parameters.Add($"@{propName}", propValue);
            }

            parameters.Add("@oldEntityId", entityId);
            var sqlQuery = $"UPDATE {_entityName} SET {String.Join(", ", queryLine.ToArray())} " +
                            $"WHERE {_entityName}Id = @oldEntityId";

            _dbConnection.Open();
            var res = _dbConnection.Execute(sqlQuery, param: parameters);

            _dbConnection.Close();
            return res;
        }
        #endregion


        #region Xóa

        /// <summary>
        /// Xóa nhiều khách hàng
        /// </summary>
        /// <param name="entityIds">List id của các KH cần xóa</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public virtual int DeleteMany(List<Guid> entityIds)
        //:                                          ^ danh sách các id
        {
            var parameters = new DynamicParameters();
            var paramName = new List<string>();

            for (int i = 0; i < entityIds.Count; i++)
            {
                var id = entityIds[i];
                paramName.Add($"@id{i}");
                parameters.Add($"@id{i}", id.ToString());
            }

            var sql = $"delete from {_entityName} where {_entityName}Id In ({String.Join(", ", paramName.ToArray())})";

            using (var trans = _dbConnection.BeginTransaction())
            {
                _dbConnection.Open();

                var res = _dbConnection.Execute(sql, param: parameters, transaction: trans);

                trans.Commit();
                _dbConnection.Close();
                return res;
            }

        }


        /// <summary>
        /// Xóa một KH với id tương ứng
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (08-2021)
        public virtual int DeleteOne(Guid entityId)
        //:                                  ^ Id của thực thể cần xóa
        {
            var sqlQuery = $"DELETE FROM {_entityName} WHERE {_entityName}Id = @entityId";
            var parameters = new DynamicParameters();

            parameters.Add("@entityId", entityId);

            _dbConnection.Open();
            var res = _dbConnection.Execute(sqlQuery, param: parameters);

            _dbConnection.Close();
            return res;
        }

        #endregion
    }
}
