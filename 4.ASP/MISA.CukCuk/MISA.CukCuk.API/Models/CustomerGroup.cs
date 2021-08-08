using System;

namespace MISA.CukCuk.API.Models
{
    public class CustomerGroup : BaseEntity
    {

        #region Properties

        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid CustomerGroupId { get; set; }


        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        public string CustomerGroupName { get; set; }


        /// <summary>
        /// Mô tả nhóm khách hàng
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
