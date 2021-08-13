using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Entities
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
