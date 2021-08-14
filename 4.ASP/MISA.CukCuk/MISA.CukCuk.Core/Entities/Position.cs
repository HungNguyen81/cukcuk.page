using System;

namespace MISA.CukCuk.Core.Entities
{
    public class Position : BaseEntity
    {
        #region Properties

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

        #endregion

    }
}
