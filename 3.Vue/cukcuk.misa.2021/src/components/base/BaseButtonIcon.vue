<template>
  <div class="button button-icon" @click="OpenPopup()">
    <div class='icon-button' :class='icon'></div>
    <div>{{value}}</div>
  </div>
</template>

<script>

export default {
  name: "BaseButtonIcon",
  props: ['value', 'icon'],
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
