using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Interfaces.Repositiories
{
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Lấy toàn bộ
        /// </summary>
        /// <returns></returns>
        List<object> Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        Department GetById(Guid departmentId);
    }
}
