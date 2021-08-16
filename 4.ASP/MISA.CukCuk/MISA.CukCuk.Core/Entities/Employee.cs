using MISA.CukCuk.Core.CustomAttrs;
using System;

namespace MISA.CukCuk.Core.Entities
{
    public class Employee : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Id nhân viên
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Họ và tên đệm
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// Tên
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string FullName { get; set; }


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
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Số cmnd/căn cước
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Ngày gia nhập công ty
        /// </summary>
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// Trạng thái hôn nhân
        /// </summary>
        //public int? MartialStatus { get; set; }


        /// <summary>
        /// Id Phòng ban
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// id vị trí
        /// </summary>
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Trạng thái công việc
        /// </summary>
        public int? WorkStatus { get; set; }


        /// <summary>
        /// Mã số thuế cá nhân
        /// </summary>
        public string PersonalTaxCode { get; set; }

        /// <summary>
        /// Lương
        /// </summary>
        public double? Salary { get; set; }

        #endregion
    }
}
