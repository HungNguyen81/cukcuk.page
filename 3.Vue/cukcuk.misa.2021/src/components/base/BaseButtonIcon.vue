<template>
  <div :class="['button', {'button-icon': icon}, type]" @click="onclick" @keydown="handleButtonPress">
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
  methods:{
    OpenPopup(){
      console.log("open popup");
    },
    handleButtonPress(event){
      if(event.code == 'Enter'){
        // console.log("enter");
        this.onclick();
      }
    }
  }
};
</script>

<style>
@import '../../css/base/button.css';
</style>
