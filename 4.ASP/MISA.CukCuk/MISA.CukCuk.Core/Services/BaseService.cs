using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Resource;
using System;
using System.Collections.Generic;
namespace MISA.CukCuk.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region Fields

        IBaseRepository<MISAEntity> _repository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public BaseService(IBaseRepository<MISAEntity> repository)
        {
            _repository = repository;
            _serviceResult = new ServiceResult();
        }
        #endregion


        #region Các phương thức GET

        /// <summary>
        /// Lấy toàn bộ data
        /// </summary>
        /// <returns></returns>
        public virtual ServiceResult Get()
        {
            _serviceResult.Data = _repository.Get();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual ServiceResult GetById(Guid entityId)
        {
            _serviceResult.Data = _repository.GetById(entityId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        #endregion

        #region Thêm mới

        /// <summary>
        /// Thêm bản ghi mới vào bảng khách hàng
        /// </summary>
        /// <param name="entity">Dữ liệu thêm mới</param>
        /// <returns></returns>
        public virtual ServiceResult Add(MISAEntity entity)
        {
            // Kết nối infrastructure service làm việc với db

            _serviceResult.Data = _repository.Add(entity);
            if ((int)_serviceResult.Data > 0)
            {
                _serviceResult.IsValid = true;
            }
            else
            {
                _serviceResult.IsValid = false;
                _serviceResult.Msg = ResourceVN.ErrorMsg;
            }
            return _serviceResult;
        }

        #endregion

        #region Cập nhật

        /// <summary>
        /// Cập nhật dữ liệu khách hàng
        /// </summary>
        /// <param name="entity">     Dữ liệu cập nhật</param>
        /// <param name="entityId">   Id của khác hàng</param>
        /// <returns></returns>
        public virtual ServiceResult Update(MISAEntity entity, Guid entityId)
        {
            // Kết nối infrastructure service làm việc với db

            _serviceResult.Data = _repository.Update(entity, entityId);

            if ((int)_serviceResult.Data > 0)
            {
                _serviceResult.IsValid = true;
            }
            else
            {
                _serviceResult.IsValid = false;
                _serviceResult.Msg = ResourceVN.ErrorMsg;
            }
            return _serviceResult;
        }

        #endregion

        #region Các phương thức xóa

        /// <summary>
        /// Xóa một bản ghi với id tương ứng
        /// </summary>
        /// <param name="entityId">id của khách hàng cần xóa</param>
        /// <returns></returns>
        public virtual ServiceResult DeleteOne(Guid entityId)
        {
            _serviceResult.Data = _repository.DeleteOne(entityId);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;

            return _serviceResult;
        }

        /// <summary>
        /// Xóa nhiều khách hàng
        /// </summary>
        /// <param name="entityIds">List id cần xóa</param>
        /// <returns></returns>
        public virtual ServiceResult DeleteMany(List<Guid> entityIds)
        {
            _serviceResult.Data = _repository.DeleteMany(entityIds);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;
            return _serviceResult;
        }

        #endregion

    }
}
