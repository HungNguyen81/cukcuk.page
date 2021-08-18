export default {
    methods: {
        /**
         * Kiểm tra các trường bắt buộc nhập không được rỗng
         * @param {String} label Tên trường dữ liệu
         * @param {String} val Giá trị
         * @returns true, false
         * CreatedBy: HungNguyen81 (18-08-2021)
         * ModifiedBy: HungNguyen81 (18-08-2021)
         */
        required(label, val) {
            if (!val || !val.trim()) {
                // this.$emit('showToast', 'warning', 'Required', `<b>"${label}"</b> không được để trống`);
                return {
                    isValid: false,
                    msg: `"${label}" không được để trống`
                };
            }
            return {
                isValid: true,
                msg: ""
            };
        },
        
        /**
         * Validate định dạng email
         * @param {String} label tên input
         * @param {String} val Giá trị input
         * @returns True-False
         * CreatedBy: HungNguyen81 (18-08-2021)
         * ModifiedBy: HungNguyen81 (18-08-2021)
         */
        email(label, val) {
            let reg = /\S+@\S+\.\S+/;
            let res = reg.test(val);
            return {
                isValid: res,
                msg: res? "":`"${label}" không đúng định dạng !`
            };
        },

        /**
         * Validate định dạng số điện thoại
         * @param {String} label Tên input
         * @param {String} val Giá trị số điện thoại
         * @returns True/False
         * CreatedBy: HungNguyen81 (18-08-2021)
         * ModifiedBy: HungNguyen81 (18-08-2021)
         */
        phone(label, val) {
            let reg = /^0[0-9]{9,11}$/;
            let res = reg.test(val);
            // if (!res) {
            //     this.$emit('showToast', 'warning', 'Phone Number invalid', `<b>"${label}"</b> phải bắt đầu bằng 0 và dài 10-12 kí tự !`);
            // }
            return {
                isValid: res,
                msg: res? "":`"${label}" phải bắt đầu bằng 0 và dài 10-12 kí tự !`
            };
        },

        /**
         * Validate Ngày tháng
         * @param {string} label tên của trường dữ liệu ngày tháng
         * @param {string} dateString giá trị ngày tháng cần validate
         * @returns true- hợp lệ, false- không hợp lệ
         * CreatedBy: HungNguyen81 (18-08-2021)
         * ModifiedBy: HungNguyen81 (18-08-2021)
         */
        date(label, dateString) {
            if(!dateString){
                return {
                    isValid : true,
                    msg: ""
                };
            }
            if (!/^(0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|1[012])[-/.]\d{4}$/g.test(dateString)) {
                return {
                    isValid: false,
                    msg: `"${label}" ${dateString} không đúng định dạng !`
                };
            }
            return {
                isValid : true,
                msg: ""
            };
        }
    }
}