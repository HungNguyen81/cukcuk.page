using System;

namespace MISA.CukCuk.Core.Entities
{
    public class Customer : BaseEntity
    {

        #region Properties

        /// <summary>
        /// Id khách hàng
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// Họ và tên đệm
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string FullName { get; set; }


        /// <summary>
        /// Giới tính
        /// </summary>
        public int? Gender { get; set; }


        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }


        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid? CustomerGroupId { get; set; }


        /// <summary>
        /// Số tiền nợ
        /// </summary>
        public double? DebitAmount { get; set; }


        /// <summary>
        /// Mã thẻ thành viên
        /// </summary>
        public string MemberCardCode { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }


        /// <summary>
        /// Mã số thuế công ty
        /// </summary>
        public string CompanyTaxCode { get; set; }


        /// <summary>
        /// Đã ngừng theo dõi hay chưa
        /// </summary>
        public bool IsStopFollow { get; set; }

        #endregion

    }
}
