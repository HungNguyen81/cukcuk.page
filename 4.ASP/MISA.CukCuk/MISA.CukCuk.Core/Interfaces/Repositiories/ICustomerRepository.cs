using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Responses;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Interfaces.Repositiories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        List<Customer> Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetById(Guid customerId);

        /// <summary>
        /// Lấy data và lọc
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        FilterResponse GetByFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId);


        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        int Add(Customer customer);


        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        int Update(Customer customer, Guid customerId);

        /// <summary>
        /// Xóa một
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        int DeleteOne(Guid customerId);

        /// <summary>
        /// Xóa nhiều
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        int DeleteMany(List<Guid> customerIds);
    }
}
