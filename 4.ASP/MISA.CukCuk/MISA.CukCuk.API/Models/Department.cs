using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Models
{
    public class Department : BaseEntity
    {
        /// <summary>
        /// Id phòng ban
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }        
    }
}
