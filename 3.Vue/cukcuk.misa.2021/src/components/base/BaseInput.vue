<template>
  <input
    @blur="inputValidate"
    :type="valueType"
    :class="['textbox-default', type, {'unvalid': !isValidate}]"
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
    renderFlag:{
      type: Boolean,
      require: false
    }
  },
  data() {
    return {
      isValidate: true
    };
  },
  mounted(){
    // console.log('MOUNTED', this.label);
  },
  computed: {
    inputListeners: function () {
      var vm = this;
      // `Object.assign` merges objects together to form a new object
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
  watch:{
    // value: function(){
    //   this.inputValidate();
    // },
    renderFlag: function(){
      this.isValidate = true;
    }
  },
  methods: {
    inputValidate() {
      if (this.validates) {
        for (let func of this.validates) {
          this.isValidate = func(this.label, this.value);
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