<template>
  <div id="app">
    <Employee></Employee>
  </div>
</template>

<script>
// import Header from "./components/layout/TheHeader.vue"
// import Menu   from "./components/layout/TheMenu.vue"
// import Content from "./components/layout/TheContent.vue"
import Employee from './components/pages/PageEmployee.vue';

export default {
  name: "App",
  components: {
    // Header,
    // Menu,
    // Content
    Employee
  },
};
</script>

<style>
@import './css/main.css';
</style>
