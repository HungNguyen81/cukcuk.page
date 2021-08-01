<template>
  <div :class="['button', {'button-icon': icon}, type]" @click="onclick" @keydown.enter="onclick">
    <div class='icon-button' :class='icon' v-if="icon"></div>
    <div>{{value}}</div>
  </div>
</template>

<script>

export default {
  name: "BaseButtonIcon",
  props: {
    value : {
      type: String
    }, 
    type:{
      type: String
    }, 
    icon:{
      type: String
    }, 
    onclick:{
      type: Function
    }
  },
  methods:{}
};
</script>

<style>
@import '../../css/base/button.css';
</style>
