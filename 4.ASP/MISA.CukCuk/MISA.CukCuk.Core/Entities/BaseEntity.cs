using System;

namespace MISA.CukCuk.Core.Entities
{
    public class BaseEntity
    {
        #region Properties

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa đổi
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa đổi
        /// </summary>
        public string ModifiedBy { get; set; }

        #endregion
    }
}
