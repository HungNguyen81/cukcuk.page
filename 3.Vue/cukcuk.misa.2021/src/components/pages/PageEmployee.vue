<template>
  <div>
    <Header></Header>
    <Menu></Menu>
    <Content></Content>
  </div>
</template>

<script>
import Header from "../layout/TheHeader.vue"
import Menu   from "../layout/TheMenu.vue"
import Content from "../layout/TheContent.vue"

export default {
  name: "EmployeePage",
  components: {
    Header,
    Menu,
    Content
  },
};
</script>

<style>
</style>
