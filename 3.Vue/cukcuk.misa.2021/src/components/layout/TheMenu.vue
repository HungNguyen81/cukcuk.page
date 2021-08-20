<template>
  <div class="menu">
    <a
      :href="item.path"
      class="menu-item"
      :class="[item.icon, { 'menu-item-active': route == item.path }]"
      v-for="(item, i) in items"
      :key="i"
    >
      <div class="menu-icon"></div>
      <div class="menu-item-text">{{ item.text }}</div>
    </a>

    <!-- [
    {icon: 'item-dashboard', path: '#', text: 'Tổng quan' },
    {icon: 'item-report', path: '#', text: 'Báo cáo' },
    {icon: 'item-employee', path: '#', text: 'Danh mục nhân viên' },
    {icon: 'item-employee', path: '#', text: 'Danh mục khách hàng' },
    {icon: 'item-setting', path: '#', text: 'Thiết lập hệ thống' },

    ] -->
    <!-- <div class="menu-item item-dashboard">
      <div class="menu-icon"></div>
      <div class="menu-item-text">Tổng quan</div>
    </div>
    <div class="menu-item item-report">
      <div class="menu-icon"></div>
      <div class="menu-item-text">Báo cáo</div>
    </div>
    <a href="/employees" class="menu-item item-employee">
      <div class="menu-icon"></div>
      <div class="menu-item-text">Danh mục nhân viên</div>
    </a>
    <a href="/customers" class="menu-item item-employee">
      <div class="menu-icon"></div>
      <div class="menu-item-text">Danh mục khách hàng</div>
    </a>
    <div class="menu-item item-setting">
      <div class="menu-icon"></div>
      <div class="menu-item-text">Thiết lập hệ thống</div>
    </div> -->
  </div>
</template>

<script>
export default {
  name: "Menu",
  components: {},
  props: {
    items: {
      type: Array,
      required: false,
    },
    route:{
        type: String,
        require: false
    }
  },
};
</script>

<style scoped>
@import "../../css/layout/menu.css";
</style>
