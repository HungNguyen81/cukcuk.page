<template>
  <input
    @blur="inputValidate()"
    :type="valueType"
    :class="['textbox-default', type, { unvalid: !isValidate }]"
    v-bind:value="value"
    v-on="inputListeners"
  />
</template>

<script>
export default {
  name: "BaseInput",
  components: {},
  props: {
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
  },
  data() {
    return {
      isValidate: true,
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
    renderFlag: function () {
      this.isValidate = true;
    },
    isValidate: function (isValid) {
      console.log("validate", this.label, isValid);
      if (isValid) {
        this.$emit("valid");
      } else {
        this.$emit("invalid");
      }
    },
    value: function () {
      // console.log("v: ", v);
      // this.inputValidate();  
    },
  },
  methods: {
    inputValidate() {
      if (this.validates) {
        let res = true;
        for (let func of this.validates) {
          res = res && func(this.label, this.value);
        }
        this.isValidate = res;
        if (this.isValidate) {
          this.$emit("valid");
        } else {
          this.$emit("invalid");
        }
      } else {
        console.log("NO validations");
      }
    },
  },
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
</style>