export default {
    methods: {
        // validate functions
        required(label, val) {
            if (!val || !val.trim()) {
                this.$emit('showToast', 'warning', 'Required', `<b>"${label}"</b> không được để trống`);
                return false;
            }
            return true;
        },
        email(label, val) {
            let reg = /\S+@\S+\.\S+/;
            let res = reg.test(val);
            if(!res){
                this.$emit('showToast', 'warning', 'Email invalid', `<b>"${label}"</b> không đúng định dạng !`);
            }
            return res;
        },
        phone(label, val) {
            let reg = /0[0-9]{9,11}$/;
            let res = reg.test(val);
            if(!res){
                this.$emit('showToast', 'warning', 'Phone Number invalid', `<b>"${label}"</b> phải bắt đầu bằng 0 và dài 10-12 kí tự !`);
            }
            return res;
        }
    }
}