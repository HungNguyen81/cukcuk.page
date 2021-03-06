//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MISA.CukCuk.Core.Resource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourceVN {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceVN() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MISA.CukCuk.Core.Resource.ResourceVN", typeof(ResourceVN).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Host = 47.241.69.179;Database = Web07.CUKCUK.HUNGNN; User Id = dev; Password = 12345678;.
        /// </summary>
        public static string ConnectionString {
            get {
                return ResourceManager.GetString("ConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã khách hàng đã trùng với khách hàng khác trong tệp nhập khẩu..
        /// </summary>
        public static string CustomerCodeExistInFileMsg {
            get {
                return ResourceManager.GetString("CustomerCodeExistInFileMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã khách hàng không được để trống !.
        /// </summary>
        public static string CustomerCodeInvalidMsg {
            get {
                return ResourceManager.GetString("CustomerCodeInvalidMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nhóm khách hàng không có trong hệ thống..
        /// </summary>
        public static string CustomerGroupNotExistMsg {
            get {
                return ResourceManager.GetString("CustomerGroupNotExistMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã khách hàng đã tồn tại trong hệ thống !.
        /// </summary>
        public static string DuplicateCustomerCodeMsg {
            get {
                return ResourceManager.GetString("DuplicateCustomerCodeMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã nhân viên đã tồn tại, xin vui lòng tạo mã mới !.
        /// </summary>
        public static string DuplicateEmployeeCodeMsg {
            get {
                return ResourceManager.GetString("DuplicateEmployeeCodeMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email không đúng định dạng !.
        /// </summary>
        public static string EmailInvalidMsg {
            get {
                return ResourceManager.GetString("EmailInvalidMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã nhân viên không được để trống !.
        /// </summary>
        public static string EmployeeCodeInvalidMsg {
            get {
                return ResourceManager.GetString("EmployeeCodeInvalidMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi xảy ra, vui lòng liên hệ MISA !.
        /// </summary>
        public static string ErrorMsg {
            get {
                return ResourceManager.GetString("ErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bạn chưa chọn file nhập khẩu hoặc file nhập khẩu bị lỗi !.
        /// </summary>
        public static string FileEmptyMsg {
            get {
                return ResourceManager.GetString("FileEmptyMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ID không được để trống !.
        /// </summary>
        public static string IdEmptyMsg {
            get {
                return ResourceManager.GetString("IdEmptyMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Host = localhost;Database = web07.cukcuk.hungnn; User Id = hungdev; Password = hung;.
        /// </summary>
        public static string LocalConnectionString {
            get {
                return ResourceManager.GetString("LocalConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Định dạng file không được hỗ trợ !.
        /// </summary>
        public static string NotSupportFileExtMsg {
            get {
                return ResourceManager.GetString("NotSupportFileExtMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SĐT đã tồn tại trong hệ thống..
        /// </summary>
        public static string PhoneNumberDuplicateMsg {
            get {
                return ResourceManager.GetString("PhoneNumberDuplicateMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SĐT đã trùng với SĐT của khách hàng khác trong tệp nhập khẩu..
        /// </summary>
        public static string PhoneNumberExistInFileMsg {
            get {
                return ResourceManager.GetString("PhoneNumberExistInFileMsg", resourceCulture);
            }
        }
    }
}
