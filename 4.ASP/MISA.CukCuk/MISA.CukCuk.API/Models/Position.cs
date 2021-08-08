using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Models
{
    public class Position : BaseEntity
    {
        /// <summary>
        /// Id vị trí/ chức vụ
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Tên vị trí/ chức vụ
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Mã vị trí/ chức vụ
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
