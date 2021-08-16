﻿using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;

namespace MISA.CukCuk.Core.Interfaces.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        /// <summary>
        /// Lấy và lọc dữ liệu
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId);
    }
}
