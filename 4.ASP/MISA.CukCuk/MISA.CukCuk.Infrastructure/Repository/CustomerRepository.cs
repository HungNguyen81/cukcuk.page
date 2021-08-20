using Dapper;
using MISA.CukCuk.Core.CustomAttrs;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Responses;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace MISA.CukCuk.Infrastructure.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {

        #region Các phương thức GET của riêng Customer

        /// <summary>
        /// Lọc dữ liệu theo chuỗi tìm kiếm hoặc nhóm khách hàng, kết hợp phân trang
        /// </summary>
        /// <param name="pageSize">         số bản ghi 1 trang</param>
        /// <param name="pageNumber">       chỉ số trang</param>
        /// <param name="filterString">     chuỗi tìm kiếm</param>
        /// <param name="customerGroupId">  id nhóm khách hàng</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (17-08-2021)
        //@ Modified_By: HungNguyen81 (17-08-2021)
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
            _dbConnection.Open();
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
            _dbConnection.Close();
            return new FilterResponse
            {
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                Data = (List<object>)customers
            };
        }

        public CustomerGroup GetCustomerGroupInfo(string customerGroupName)
        {
            // Lấy dữ liệu
            var sqlQuery = $"SELECT * FROM {_entityName}Group WHERE {_entityName}GroupName = @group";
            var parameter = new DynamicParameters();

            parameter.Add("@group", customerGroupName);

            _dbConnection.Open();

            var customerGroup = _dbConnection.QueryFirstOrDefault<CustomerGroup>(sqlQuery, param: parameter);

            _dbConnection.Close();
            return customerGroup;
        }



        #endregion

        #region Import Data từ file

        public int InsertMany(List<Customer> customers)
        {
            var procName = "Proc_ImportCustomer";
            var rowAffects = 0;

            _dbConnection.Open();

            foreach (var customer in customers)
            {
                // Nếu customer ko hợp lệ thì bỏ qua
                if (customer.InValids != null) continue;

                using (var trans = _dbConnection.BeginTransaction())
                {
                    try
                    {
                        // Đọc từng property của object
                        var properties = customer.GetType().GetProperties();
                        var parameters = new DynamicParameters();

                        foreach (var prop in properties)
                        {
                            var propAttr = prop.GetCustomAttributes(typeof(MISAColumnForImport), false);

                            // nếu k phải cột lấy từ file thì bỏ qua
                            if (propAttr.Length == 0) continue;

                            // Tên thuộc tính
                            var propName = prop.Name;

                            // Giá tri thuộc tính
                            var propValue = prop.GetValue(customer);

                            parameters.Add($"@{propName}", propValue);
                        }

                        rowAffects += _dbConnection.Execute(
                            procName,
                            customer,
                            commandType: System.Data.CommandType.StoredProcedure,
                            transaction: trans
                        );
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
            }

            _dbConnection.Close();

            return rowAffects;
        }

        #endregion
    }
}
