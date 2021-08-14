﻿using MISA.CukCuk.Core.Entities;
using System;

namespace MISA.CukCuk.Core.Interfaces.Services
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Lấy toàn bộ 
        /// </summary>
        /// <returns></returns>
        ServiceResult Get();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        ServiceResult GetById(Guid departmentId);
    }
}
