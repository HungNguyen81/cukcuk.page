using Microsoft.AspNetCore.Http;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Helpers;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Resource;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace MISA.CukCuk.Core.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region Fields

        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public CustomerService(ICustomerRepository customerRepository):base(customerRepository)
        {
            _customerRepository = customerRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion


        #region Các phương thức GET

        /// <summary>
        /// Lấy theo chuỗi tìm kiếm hoặc phân trang
        /// </summary>
        /// <param name="pageSize">         Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">       Chỉ số của trang</param>
        /// <param name="filterString">     chuỗi tìm kiếm</param>
        /// <param name="customerGroupId">  Id của nhóm khách hàng</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (17-08-2021)
        //@ Modified_By: HungNguyen81 (17-08-2021)
        public ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString, Guid? customerGroupId)
        {
            _serviceResult.Data = _customerRepository.GetByFilter(pageSize, pageNumber, filterString, customerGroupId);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        #endregion

        #region Nhập khẩu từ file excel

        /// <summary>
        /// Thêm nhiều bản ghi trong một request
        /// </summary>
        /// <param name="customers">danh sách các bản ghi</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (20-08-2021)
        public ServiceResult InsertMany(List<Customer> customers)
        {
            _serviceResult.Data = _customerRepository.InsertMany(customers);
            _serviceResult.IsValid = (int)_serviceResult.Data > 0;
            return _serviceResult;
        }


        /// <summary>
        /// Nhập khẩu từ file
        /// </summary>
        /// <param name="formFile">File dữ liệu</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (19-08-2021)
        public ServiceResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            // kiểm tra file rỗng
            if (formFile == null || formFile.Length <= 0)
            {
                return new ServiceResult
                {
                    IsValid = false,
                    Msg = ResourceVN.FileEmptyMsg
                };
            }

            // kiểm tra định dạng file có là xls, xlsx
            if (!(Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)
                 || Path.GetExtension(formFile.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase)))
            {
                return new ServiceResult
                {
                    IsValid = false,
                    Msg = ResourceVN.NotSupportFileExtMsg
                };
            }

            var list = new List<Customer>();

            using (var stream = new MemoryStream())
            {
                formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var obj = GetCustomerFromExcelRow(worksheet, row);

                        // VALIDATE CHUNG: 1. ktra mã kh rỗng, 2. Ktra trùng mã, 3. Ktra định dạng email
                        _serviceResult = Validate(obj, (int)Mode.Add);

                        // VALIDATE CUSTOM: 1. Nhóm kh ko có trong db, 2. Trùng mã KH trong file/db, 3. Trùng sđt trong file/db
                        _serviceResult = ValidateForImport(obj, worksheet, row);

                        // Kết thúc validate
                        if (!_serviceResult.IsValid)
                        {
                            obj.InValids = _serviceResult.InvalidMsg;
                        }

                        list.Add(obj);
                    }
                }
            }

            var rowAffects = _customerRepository.InsertMany(list);
            
            return new ServiceResult
            {
                IsValid = rowAffects > 0,
                Data = new
                {
                    RowAffects = rowAffects,
                    Result = list
                }
            };
        }

        /// <summary>
        /// Lấy data từ một hàng trong file để tạo đối tượng Customer
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (20-08-2021)
        protected Customer GetCustomerFromExcelRow(ExcelWorksheet worksheet, int row)
        {
            var customerCode = worksheet.Cells[row, 1].Value;
            var fullName = worksheet.Cells[row, 2].Value;
            var memberCardCode = worksheet.Cells[row, 3].Value;
            var customerGroupName = worksheet.Cells[row, 4].Value;
            var phoneNumber = worksheet.Cells[row, 5].Value;
            var dateOfBirth = worksheet.Cells[row, 6].Value;
            var companyName = worksheet.Cells[row, 7].Value;
            var companyTaxCode = worksheet.Cells[row, 8].Value;
            var email = worksheet.Cells[row, 9].Value;
            var address = worksheet.Cells[row, 10].Value;

            var sCustomerCode = customerCode.ToString().Trim();
            var sFullName = fullName == null ? null : fullName.ToString().Trim();
            var sMemberCardCode = memberCardCode == null ? null : memberCardCode.ToString().Trim();
            var sCustomerGroupName = customerGroupName == null ? null : customerGroupName.ToString().Trim();
            var sPhoneNumber = phoneNumber == null ? null : phoneNumber.ToString().Trim().Replace(".", "");
            var sDateOfBirth = dateOfBirth == null ? null : dateOfBirth.ToString().Trim();
            var sCompanyName = companyName == null ? null : companyName.ToString().Trim();
            var sCompanyTaxCode = companyTaxCode == null ? null : companyTaxCode.ToString().Trim();
            var sEmail = email == null ? null : email.ToString().Trim();
            var sAddress = address == null ? null : address.ToString().Trim();

            DateTime dob;
            var isDobNull = false;
            string[] formats = { "dd/MM/yyyy", "MM/yyyy", "yyyy" };
            if (!DateTime.TryParseExact(sDateOfBirth, formats, CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dob))
            {
                isDobNull = true;
            }

            return new Customer
            {
                CustomerCode = sCustomerCode,
                FullName = sFullName,
                MemberCardCode = sMemberCardCode,
                CustomerGroupName = sCustomerGroupName,
                PhoneNumber = sPhoneNumber,
                DateOfBirth = isDobNull ? null : dob,
                CompanyName = sCompanyName,
                CompanyTaxCode = sCompanyTaxCode,
                Email = sEmail,
                Address = sAddress
            };
        }

        #endregion

        #region Validate riêng của Customer

        /// <summary>
        /// Validate khi nhập khẩu từ file
        /// </summary>
        /// <param name="obj">Đối tượng chứa dữ liệu cần validate</param>
        /// <param name="worksheet">data 1 sheet của file excel</param>
        /// <param name="row">chỉ số dòng trong sheet</param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (20-08-2021)
        protected ServiceResult ValidateForImport(Customer obj, ExcelWorksheet worksheet, int row)
        {
            //  Nhóm kh không có trong csdl
            if (_customerRepository.GetCustomerGroupInfo(obj.CustomerGroupName) == null)
            {
                _serviceResult.IsValid = false;
                _serviceResult.InvalidMsg.Add(ResourceVN.CustomerGroupNotExistMsg);
            }

            // Mã KH đã tồn tại trong file
            if (IsFoundValueInFile(worksheet, 1, row, obj.CustomerCode))
            {
                _serviceResult.IsValid = false;
                _serviceResult.InvalidMsg.Add(ResourceVN.CustomerCodeExistInFileMsg);
            }

            // SĐT đã tồn tại trong csdl
            if (!string.IsNullOrEmpty(obj.PhoneNumber) && _customerRepository.GetByPhoneNumber(obj.PhoneNumber) != null)
            {
                _serviceResult.IsValid = false;
                _serviceResult.InvalidMsg.Add(ResourceVN.PhoneNumberDuplicateMsg);
            }

            // SĐT đã tồn tại trong file
            if (!string.IsNullOrEmpty(obj.PhoneNumber) && IsFoundValueInFile(worksheet, 5, row, obj.PhoneNumber))
            {
                _serviceResult.IsValid = false;
                _serviceResult.InvalidMsg.Add(ResourceVN.PhoneNumberExistInFileMsg);
            }

            return _serviceResult;
        }


        /// <summary>
        /// Tìm kiếm một giá trị trong file excel theo cột
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="column">Chỉ số cột muốn tìm</param>
        /// <param name="currentRow">Dòng hiện tại, chỉ tìm kiếm các dòng khác dòng này</param>
        /// <param name="compareVal">Giá trị cần tìm</param>
        /// <returns>True- Giá trị cần tìm xuất hiện nhiều hơn 1 lần trong sheet</returns>
        //@ Created_By: HungNguyen81 (19-08-2021)
        protected bool IsFoundValueInFile(ExcelWorksheet worksheet, int column, int currentRow, string compareVal)
        {
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                if (row == currentRow) continue;
                var valObj = worksheet.Cells[row, column].Value;
                string val = valObj == null ? null : valObj.ToString().Trim();

                if (!string.IsNullOrEmpty(val) && val.Equals(compareVal)) return true;
            }
            return false;
        }

        #endregion
    }
}
