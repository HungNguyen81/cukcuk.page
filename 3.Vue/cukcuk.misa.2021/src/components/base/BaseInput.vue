<template>
  <div class="input-field tooltip">
    <span class="tooltip-text" v-if="!isValidate">{{invalidTooltip}}</span>
    <div class="input-label" v-if="required">
      {{ label }} (<span class="required">*</span>)
    </div>
    <div class="input-label" v-else>
      {{ label }}
    </div>

    <div
      :class="[
        'date-input',
        'textbox-default',
        type,
        { 'input-focus': isDateFocus },
        { invalid: !isValidate }
      ]"
      v-if="valueType == 'date'"
    >
      <input
        type="text"
        placeholder="dd / mm / yyyy"
        class="date-edit"
        :tabindex="tabindex"
        v-model="formatedValue"
        @focus="isDateFocus = true"
        @blur="isDateFocus = false;inputValidate();"
        ref="dateView"
      />
      <input
        @blur="
          isDateFocus = false;
          inputValidate();
        "
        @focus="isDateFocus = true"
        :type="valueType"
        v-bind:value="value"
        v-on="inputListeners"
        :tabindex="Number(tabindex) + 1"
      />
    </div>
    <input
      v-else
      @blur="inputValidate()"
      :type="valueType"
      :class="['textbox-default', type, { invalid: !isValidate }]"
      v-bind:value="value"
      v-on="inputListeners"
      :tabindex="tabindex"
    />
  </div>
</template>

<script>
export default {
  name: "BaseInput",
  components: {},
  props: {
    inputKey:{
      type: String,
      required: false
    },
    value: {
      type: String,
    },
    valueType: {
      type: String,
    },
    type: {
      type: String,
    },
    validates: {
      type: Array, // Array of Function
    },
    label: {
      type: String,
    },
    renderFlag: {
      type: Boolean,
      require: false,
    },
    required: {
      type: Boolean,
    },
    tabindex: {},
  },
  data() {
    return {
      isValidate: true,
      isDateFocus: false,
      formatedValue: null,
      dateTimeOut: null,
      invalidTooltip: '',
    };
  },
  mounted() {
    // console.log('MOUNTED', this.label);
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
    renderFlag: function () {
      this.isValidate = true;
    },
    // isValidate: function (isValid) {
    //   console.log("validate", this.label, isValid);
    // },
    value: function (val) {
      if (this.valueType == "date") {
        if (!val) {
          this.formatedValue = null;
          return;
        }
        let data = val.split("-");
        let yyyy = this.zeroPad(data[0], 4);
        let mm = this.zeroPad(data[1], 2);
        let dd = this.zeroPad(data[2], 2);
        this.formatedValue = `${dd}/${mm}/${yyyy}`;
      }
    },
    formatedValue: function (val) {      
      // console.log("format value editing", val);

      clearTimeout(this.dateTimeOut);

      let newVal;
      if (!val) {
        newVal = "";
      } else {
        let data = val.split("/");
        if (data.length < 3 || !data[2] || !data[1] || !data[0] || data[2].length < 4) return;
        let yyyy = this.zeroPad(data[2], 4);
        let mm = this.zeroPad(data[1], 2);
        let dd = this.zeroPad(data[0], 2);
        
        newVal = `${yyyy}-${mm}-${dd}`;
      }
      
      this.dateTimeOut = setTimeout(() => {
        this.$emit("dateChange", this.inputKey, newVal, this.$refs.dateView, val);
      }, 500);
    },
  },
  methods: {
    zeroPad: function (num, places) {
      let res = String(num).padStart(places, "0");
      return res.substr(res.length - places);
    },
    inputValidate() {
      if (this.validates) {
        let res = true;
        for (let func of this.validates) {
          let valid = func(this.label, this.valueType=='date'?this.formatedValue:this.value);
          res = res && valid.isValid;
          this.invalidTooltip = valid.msg;
          if(!res) break;
        }
        this.isValidate = res;

        this.$emit("valid", this.inputKey, res);
      }
    },
  },
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
@import "../../css/base/tooltip.css";
</style>