<template>
  <div id="app" @click="clicked">
    <router-view></router-view>
  </div>
</template>

<script>
// import Entity from "./components/pages/PageEmployee.vue";
import EventBus from './event-bus/EventBus';
export default {
  name: "App",
  components: {
    
  },
  methods:{
    clicked(event){
      EventBus.$emit("appClick", event.target);
    }
  }
};
</script>

<style>
@import "./css/main.css";
</style>
