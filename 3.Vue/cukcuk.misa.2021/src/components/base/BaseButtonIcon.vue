<template>
  <div class="button button-icon" :class='type' @click="onclick">
    <div class='icon-button' :class='icon'></div>
    <div>{{value}}</div>
  </div>
</template>

<script>

export default {
  name: "BaseButtonIcon",
  props: ['value', 'type', 'icon', 'onclick'],
  methods:{
    OpenPopup(){
      console.log("open popup");
    }
  }
};
</script>

<style>
/* @import '../../css/base/button.css' */
</style>
