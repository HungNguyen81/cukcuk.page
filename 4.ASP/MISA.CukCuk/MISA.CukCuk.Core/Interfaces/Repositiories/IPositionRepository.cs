using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Interfaces.Repositiories
{
    public interface IPositionRepository
    {
        /// <summary>
        /// Lấy toàn bộ
        /// </summary>
        /// <returns></returns>
        List<object> Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Position GetById(Guid positionId);
    }
}
