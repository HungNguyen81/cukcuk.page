using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.Core.Services
{
    public class PositionService : BaseService<Position>, IPositionService
    {
        #region Constructors

        public PositionService(IPositionRepository positionRepository):base(positionRepository)
        {
        }

        #endregion
    }
}

