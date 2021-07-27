<template>
  <div id="app">
    <Header></Header>
    <Menu></Menu>
    <Content></Content>
  </div>
</template>

<script>
import Header from "./components/layout/TheHeader.vue"
import Menu   from "./components/layout/TheMenu.vue"
import Content from "./components/layout/TheContent.vue"

export default {
  name: "App",
  components: {
    Header,
    Menu,
    Content
  },
};
</script>

<style>
@import './css/main.css'
</style>
