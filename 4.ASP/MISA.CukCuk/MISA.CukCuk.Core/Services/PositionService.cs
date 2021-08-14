using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.Core.Services
{
    public class PositionService : IPositionService
    {

        #region Fields

        IPositionRepository _positionRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
            _serviceResult = new ServiceResult();
        }

        #endregion

        #region GET methods
        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        public ServiceResult Get()
        {
            _serviceResult.Data = _positionRepository.Get();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public ServiceResult GetById(Guid positionId)
        {
            _serviceResult.Data = _positionRepository.GetById(positionId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        #endregion

    }
}

