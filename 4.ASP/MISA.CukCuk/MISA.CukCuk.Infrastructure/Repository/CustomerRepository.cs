using Dapper;
using MISA.CukCuk.Core.CustomAttrs;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            filterString ??= "";
            var parameters = new DynamicParameters();
            parameters.Add("m_pageSize", pageSize);
            parameters.Add("m_pageStart", pageNumber * pageSize);
            parameters.Add("m_filterString", filterString);
            parameters.Add("m_customerGroupId", customerGroupId == null ? "" : customerGroupId.ToString());

            _dbConnection.Open();

            var procName = "Proc_CustomerFilter";
            var result = _dbConnection.QueryMultiple(procName, param: parameters, 
                    commandType: CommandType.StoredProcedure);

            var customers = result.Read<object>().ToList();
            var totalRecord = result.Read<int>().ToList()[0];
            var totalPage = (int) Math.Ceiling((double)totalRecord / pageSize);

            _dbConnection.Close();

            if (customers == null)
            {
                return new FilterResponse
                {
                    TotalRecord = 0,
                    TotalPage = 0,
                    Data = null
                };
            }
            return new FilterResponse
            {
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                Data = customers
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
