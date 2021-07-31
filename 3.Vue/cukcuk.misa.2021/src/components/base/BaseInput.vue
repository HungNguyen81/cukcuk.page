<template>
  <input
    :type="valueType"
    :class="['textbox-default', type]"
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
  },
  data() {
    return {};
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
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
</style>