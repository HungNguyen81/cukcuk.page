export default {
    methods: {
        // validate functions
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
        email(label, val) {
            let reg = /\S+@\S+\.\S+/;
            let res = reg.test(val);
            // if (!res) {
            //     this.$emit('showToast', 'warning', 'Email invalid', `<b>"${label}"</b> không đúng định dạng !`);
            // }
            return {
                isValid: res,
                msg: res? "":`"${label}" không đúng định dạng !`
            };
        },
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