using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Entities
{
    public class ServiceResult
    {
        /// <summary>
        /// Tính hợp lệ của dữ liệu
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object Data { get; set; }


        /// <summary>
        /// Thông điệp trả về
        /// </summary>
        public string Msg { get; set; }


        /// <summary>
        /// Mã lỗi
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Id truy vết lỗi
        /// </summary>
        public Guid? TraceId { get; set; }
    }
}
