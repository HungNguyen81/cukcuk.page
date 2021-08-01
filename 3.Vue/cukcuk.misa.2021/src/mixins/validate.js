export default {
    methods: {
        // validate functions
        required(label, val) {
            if (!val) {
                this.$emit('showToast', 'warning', 'Required', `<b>"${label}"</b> không được để trống`);
                return false;
            }
            return true;
        }
    }
}