<template>
  <div class="input-field tooltip">
    <span class="tooltip-text" v-if="!isValidate">{{ invalidTooltip }}</span>
    <div class="input-label">
      {{ label }} <span v-if="required">(<span class="required">*</span>)</span>
    </div>

    <div
      :class="[
        'date-input',
        'textbox-default',
        type,
        { 'input-focus': isDateFocus },
        { invalid: !isValidate },
      ]"
    >
      <input
        type="text"
        placeholder="dd / mm / yyyy"
        class="date-edit"
        :tabindex="tabindex"
        v-model="formatedValue"
        @focus="isDateFocus = true"
        @blur="
          isDateFocus = false;
          inputValidate();
        "
        ref="dateView"
      />
      <input
        @blur="
          isDateFocus = false;
          inputValidate();
        "
        @focus="isDateFocus = true"
        type="date"
        v-bind:value="value"
        v-on="inputListeners"
        :tabindex="Number(tabindex) + 1"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "BaseDateInput",
  components: {},
  props: {
    inputKey: {
      type: String,
      required: false,
    },
    value: {
      type: String,
    },
    type: {
      type: String,
    },
    validates: {
      type: Array,
    },
    label: {
      type: String,
    },
    required: {
      type: Boolean,
    },
    tabindex: {
      type: Number,
    },
    rerenderFlag: {
      type: Boolean,
      require: false,
    },
  },
  data() {
    return {
      isValidate: true,
      isDateFocus: false,
      formatedValue: null,
      dateTimeOut: null,
      invalidTooltip: "",
    };
  },
  mounted() {
    // console.log("MOUNTED", this.label);
  },
  computed: {
    inputListeners: function () {
      var vm = this;
      return Object.assign(
        {},
        // We add all the listeners from the parent
        this.$listeners,
        // Then we can add custom listeners or override the
        // behavior of some listeners.
        {
          // This ensures that the component works with v-model
          input: function (event) {
            vm.$emit("input", event.target.value);
          },
        }
      );
    },
  },
  watch: {
    /**
     * Tắt border cảnh báo invalid khi mở form (hoặc re-render form)
     */
    rerenderFlag: function () {
      this.isValidate = true;
    },

    /**
     * Watch sự thay đổi của input value
     */
    value: function (val) {
      if (!val) {
        this.formatedValue = null;
        return;
      }
      let data = val.split("-");
      let yyyy = this.zeroPad(data[0], 4);
      let mm = this.zeroPad(data[1], 2);
      let dd = this.zeroPad(data[2], 2);
      this.formatedValue = `${dd}/${mm}/${yyyy}`;
    },
    
    /**
     * Watch sự thay đổi của giá trị ngày đc hiển thị lên input
     */
    formatedValue: function (val) {
      clearTimeout(this.dateTimeOut);
      let newVal;
      if (!val) {
        newVal = "";
      } else {
        let data = val.split("/");
        if (
          data.length < 3 ||
          !data[2] ||
          !data[1] ||
          !data[0] ||
          data[2].length < 4
        )
          return;
        let yyyy = this.zeroPad(data[2], 4);
        let mm = this.zeroPad(data[1], 2);
        let dd = this.zeroPad(data[0], 2);

        newVal = `${yyyy}-${mm}-${dd}`;
      }

      this.dateTimeOut = setTimeout(() => {
        this.$emit(
          "dateChange",
          this.inputKey,
          newVal,
          this.$refs.dateView,
          val
        );
      }, 500);
    },
  },
  methods: {
      /**
       * Thêm số 0 ở đầu chuỗi, vd zeroPad("123", 5) => "00123"
       */
    zeroPad: function (num, places) {
      let res = String(num).padStart(places, "0");
      return res.substr(res.length - places);
    },

    /**
     * Kiểm tra tính hợp lệ bằng cách gọi lần lượt các hàm validate truyền vào từ props
     */
    inputValidate() {
      if (this.validates) {
        let res = true;
        for (let func of this.validates) {
          let valid = func(this.label, this.formatedValue);
          res = res && valid.isValid;
          this.invalidTooltip = valid.msg;
          if (!res) break;
        }
        this.isValidate = res;

        this.$emit("valid", this.inputKey, res);
      } else {
        console.log("NO validations");
      }
    },
  },
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
@import "../../css/base/tooltip.css";
</style>