using MISA.CukCuk.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Responses
{
    public class EmployeeFilterResponse
    {
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// Dữ liệu
        /// </summary>
        public List<object> Data { get; set; }
    }
}
