using System.Text.RegularExpressions;

namespace MISA.CukCuk.Core.Helpers
{
    public class Validations
    {
        /// <summary>
        /// Validate định dạng email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool EmailValidate(string email)
        {
            var emailFormat = @"\S+@\S+\.\S+";
            return Regex.IsMatch(email, emailFormat, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Validate chuỗi rỗng hay không
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool Required(string val)
        {
            return !(val == null || val.Equals(""));
        }
    }
}
