export default {
    methods: {
        /**
         * Định dạng chuỗi tiền tệ
         * @param {String} text giá trị tiền tệ
         * @returns chuỗi tiền tệ ngăn cách bởi dấu .
         * CreatedBy: HungNguyen81 (18-08-2021)
         */
        formatMoneyString(text) {
            if(!text) return null;
            text = String(text);
            text = text.replaceAll('.', '');
            text = text.replace(/\s+/, "");
            text = text.replace(/[^\d]+/, "");
            return text.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        },

        /**
         * Định dạng chuỗi ngày tháng để hiển thị hoặc đưa vào value của date input
         * @param {String} data Chuỗi gt ngày tháng
         * @param {String} isForDateInput Dùng cho dateinput hay không
         * @returns Định dạng ngày tháng mong muốn (cho dateinput hoặc cho mục đích hiển thị)
         * CreatedBy: HungNguyen81 (18-08-2021)
         */
        dateFormat(data, isForDateInput) {
            if(!data) return "";
            let date = new Date(data);
            let dd = date.getDate();
            let mm = date.getMonth() + 1;
            let yyyy = date.getFullYear();

            if (isForDateInput) {
                return `${yyyy}-${mm < 10 ? "0" + mm : mm}-${dd < 10 ? "0" + dd : dd}`;
            }
            return `${dd < 10 ? "0" + dd : dd}/${mm < 10 ? "0" + mm : mm}/${yyyy}`;
        },

        /**
         * Lấy tên ttcv ứng với mã
         * @param {Number} statusCode mã số tương ứng với trạng thái công việc
         * @returns Tên trạng thái cv
         * CreatedBy: HungNguyen81 (18-08-2021)
         */
        workStatusCode2Text(statusCode) {
            switch (statusCode) {
                case 1: return "Đang làm việc";
                case 2: return "Đang thử việc";
                case 3: return "Sắp nghỉ việc";
                default: return "Đã nghỉ việc";
            }
        },  

        /**
         * Trả về mã ứng với tên ttcv
         * @param {String} text Tên trạng thái cv
         * @returns Mã trạng thái cv
         * CreatedBy: HungNguyen81 (18-08-2021)
         */
        workStatusText2Code(text) {
            switch (text) {
                case "Đang làm việc": return 1;
                case "Đang thử việc": return 2;
                case "Sắp nghỉ việc": return 3;
                default: return 3;
            }
        },

        /**
         * Lấy tên giới tính theo mã
         * @param {Number} code 
         * @returns {String} Giới tính
         * CreatedBy: HungNguyen81 (18-08-2021)
         */
        genderCode2Text(code) {
            switch (code) {
                case 0: return "Nữ";
                case 1: return "Nam";
                case 2: return "Không xác định";
                default: return "";
            }
        },

        /**
         * Lấy mã theo tên giới tính
         * @param {String} text 
         * @returns {Number} mã giới tính
         * CreatedBy: HungNguyen81 (18-08-2021)
         */
        genderText2Code(text) {
            switch (text) {
                case "Nữ": return 0;
                case "Nam": return 1;
                case "Không xác định": return 2;
                default: return 3;
            }
        },

        /**
         * Chuyển TV có dấu thành không dấu
         * @param {String} str chuỗi Tiếng Việt có dấu
         * @returns {String} chuỗi TV không dấu 
         * CreatedBy: HungNguyen81 (18-08-2021)
         */
        removeAccents(str) {
            return str.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
        }
    }
}