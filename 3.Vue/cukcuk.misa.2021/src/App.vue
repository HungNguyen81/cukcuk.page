<template>
  <div id="app">
    <Employee></Employee>
  </div>
</template>

<script>
import Employee from './components/pages/PageEmployee.vue';

export default {
  name: "App",
  components: {
    Employee
  },
};
</script>

<style>
@import './css/main.css';
</style>
