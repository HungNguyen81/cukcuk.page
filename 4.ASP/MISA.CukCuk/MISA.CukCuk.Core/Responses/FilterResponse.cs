using System.Collections.Generic;
namespace MISA.CukCuk.Core.Responses
{
    public class FilterResponse
    {
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// Dữ liệu
        /// </summary>
        public List<object> Data { get; set; }
    }
}
