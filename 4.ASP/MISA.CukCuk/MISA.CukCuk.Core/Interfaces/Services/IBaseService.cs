using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Services
{
    public interface IBaseService<MISAEntity>
    {
        /// <summary>
        /// Lấy toàn bộ 
        /// </summary>
        /// <returns></returns>
        ServiceResult Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ServiceResult GetById(Guid entityId);

        /// <summary>
        /// Lấy và lọc dữ liệu
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="entityGroupId"></param>
        /// <returns></returns>

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ServiceResult Add(MISAEntity entity);

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ServiceResult Update(MISAEntity entity, Guid entityId);

        /// <summary>
        /// Xóa một bản ghi theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ServiceResult DeleteOne(Guid entityId);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        ServiceResult DeleteMany(List<Guid> entityIds);
    }
}
