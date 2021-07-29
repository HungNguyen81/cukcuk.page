export default {
    methods: {
        /**
        * Convert salary : 1000 => 1.000
        */
        FormatMoneyString(text) {
            text = String(text);
            text = text.replace(/\s+/, "");
            text = text.replace(/[a-zA-Z@#$%^&*()<>?:";'{[}\]|\\/]+/, "");
            return text.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        },

        /**
         * convert date to display type or input value type
         */
        DateFormat(data, isForDateInput) {
            let date = new Date(data);
            let dd = date.getDate();
            let mm = date.getMonth() + 1;
            let yyyy = date.getFullYear();

            if (isForDateInput) {
                return `${yyyy}-${mm < 10 ? "0" + mm : mm}-${dd < 10 ? "0" + dd : dd}`;
            }
            return `${dd < 10 ? "0" + dd : dd}/${mm < 10 ? "0" + mm : mm}/${yyyy}`;
        },
        WorkStatusCode2Text(statusCode) {
            switch (statusCode) {
                case 1: return "Đang làm việc";
                case 2: return "Đang thử việc";
                case 3: return "Sắp nghỉ việc";
                default: return "Đã nghỉ việc";
            }
        },  
        WorkStatusText2Code(text) {
            switch (text) {
                case "Đang làm việc": return 1;
                case "Đang thử việc": return 2;
                case "Sắp nghỉ việc": return 3;
                default: return 3;
            }
        }
    }
}