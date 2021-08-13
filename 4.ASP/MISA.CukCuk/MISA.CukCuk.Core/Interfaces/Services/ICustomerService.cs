using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Lấy toàn bộ 
        /// </summary>
        /// <returns></returns>
        ServiceResult Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ServiceResult GetById(Guid customerId);

        /// <summary>
        /// Lấy và lọc dữ liệu
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId);

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        ServiceResult Add(Customer customer);

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ServiceResult Update(Customer customer, Guid customerId);

        /// <summary>
        /// Xóa một bản ghi theo id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ServiceResult DeleteOne(Guid customerId);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        ServiceResult DeleteMany(List<Guid> customerIds);
    }
}
