<template>
    <div class="toast-wrapper">
      <div :class="['toast', toastType]">
        <div class="toast-icon"></div>
        <div class="toast-content">
          <div class="toast-header">
            <b>{{ toastHeader }}</b>
          </div>
          <div class="toast-message" v-html="toastMsg">
          </div>
        </div>
        <div class="toast-close" @click="closeToast">
          <div :class="['toast-close-button', toastType]">
            <i class="fas fa-times"></i>
          </div>
        </div>
      </div>
      <div class="line">
        <div class="line-loader"></div>
      </div>
    </div>
</template>

<script>
export default {
  name: "Toast",
  components: {},
  props: {
    type: {
      type: String,
      require: true,
    },
    header: {
      type: String,
      require: false,
    },
    msg: {
      type: String,
      require: false,
    },
  },
  data() {
    return {
      toastType: "",
      toastHeader: "",
      toastMsg: "",
      timeOut: null,
    };
  },
  mounted() {
    this.toastType = this.type;
    this.toastHeader = this.header;
    this.toastMsg = this.msg;
    this.timeOut = setTimeout(this.closeToast, 7000);
  },
  methods: {
    /**
     * Đóng toast và xóa khỏi DOM
     * @ CreatedBy: HungNguyen81 (08-2021)
     */
    closeToast() {
      console.log("close toast");

      clearTimeout(this.timeOut)

      // destroy the vue listeners, etc
      this.$destroy();

      // remove the element from the DOM
      // console.log(this.$el.children[0]);
      this.$el.parentNode.removeChild(this.$el);
    },
  },
};
</script>

<style scoped>
@import "../../css/base/toast.css";
</style>