using MISA.CukCuk.Core.CustomAttrs;
using System;
using System.Collections.Generic;

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
        [MISAColumnForImport]
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
        [MISAColumnForImport]
        public string FullName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [MISAColumnForImport]
        public string Address { get; set; }

        /// <summary>
        /// Giới tính (int)
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// Giới tính (string)
        /// </summary>
        [MISADbColumnNotMatch]
        public string GenderName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [MISAColumnForImport]
        public DateTime? DateOfBirth { get; set; }


        /// <summary>
        /// Địa chỉ email
        /// </summary>
        [MISAColumnForImport]
        public string Email { get; set; }


        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MISAColumnForImport]
        public string PhoneNumber { get; set; }


        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid? CustomerGroupId { get; set; }

        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        [MISADbColumnNotMatch]
        [MISAColumnForImport]
        public string CustomerGroupName { get; set; }


        /// <summary>
        /// Số tiền nợ
        /// </summary>
        public double? DebitAmount { get; set; }


        /// <summary>
        /// Mã thẻ thành viên
        /// </summary>
        [MISAColumnForImport]
        public string MemberCardCode { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        [MISAColumnForImport]
        public string CompanyName { get; set; }


        /// <summary>
        /// Mã số thuế công ty
        /// </summary>
        [MISAColumnForImport]
        public string CompanyTaxCode { get; set; }


        /// <summary>
        /// Đã ngừng theo dõi hay chưa
        /// </summary>
        public bool IsStopFollow { get; set; } = false;

        /// <summary>
        /// Danh sách thông báo validate
        /// </summary>
        [MISADbColumnNotMatch]
        public List<string> InValids { get; set; }

        #endregion

    }
}
