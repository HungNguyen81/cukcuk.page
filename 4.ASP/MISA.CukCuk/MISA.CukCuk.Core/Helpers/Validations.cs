using System.Text.RegularExpressions;

namespace MISA.CukCuk.Core.Helpers
{
    public class Validations
    {
        /// <summary>
        /// Validate định dạng email
        /// </summary>
        /// <param name="email">Email cần validate</param>
        /// <returns>True: ddúng định dạng, False: Không hợp lệ</returns>
        //@ Created_By: HungNguyen81 (18-08-2021)
        //@ Modified_By: HungNguyen81 (18-08-2021)
        public static bool EmailValidate(string email)
        {
            if (string.IsNullOrEmpty(email)) return true;
            var emailFormat = @"\S+@\S+\.\S+";
            return Regex.IsMatch(email, emailFormat, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Validate chuỗi rỗng hay không
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (18-08-2021)
        //@ Modified_By: HungNguyen81 (18-08-2021)
        public static bool Required(string val)
        {
            return !(val == null || val.Equals(""));
        }
    }
}
