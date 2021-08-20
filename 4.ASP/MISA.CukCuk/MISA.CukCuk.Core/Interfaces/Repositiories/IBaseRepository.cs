using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Repositiories
{
    public interface IBaseRepository<MISAEntity>
    {
        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        List<MISAEntity> Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        MISAEntity GetById(Guid entityId);

        /// <summary>
        /// Lấy theo mã 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MISAEntity GetByCode(string code);

        MISAEntity GetByPhoneNumber(string phoneNumber);

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(MISAEntity entity);


        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        int Update(MISAEntity entity, Guid entityId);

        /// <summary>
        /// Xóa một
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        int DeleteOne(Guid entityId);

        /// <summary>
        /// Xóa nhiều
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        int DeleteMany(List<Guid> entityIds);
    }
}
