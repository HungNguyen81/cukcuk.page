<template>
    <div class="menu">
        <div class="menu-item item-dashboard">
            <div class="menu-icon"></div>
            <div class="menu-item-text">Tổng quan</div>
        </div>
        <div class="menu-item item-report">
            <div class="menu-icon"></div>
            <div class="menu-item-text">Báo cáo</div>
        </div>
        <div class="menu-item item-employee menu-item-active">
            <div class="menu-icon"></div>
            <div class="menu-item-text">Danh mục nhân viên</div>
        </div>
        <div class="menu-item item-employee">
            <div class="menu-icon"></div>
            <div class="menu-item-text">Danh mục khách hàng</div>
        </div>
        <div class="menu-item item-setting">
            <div class="menu-icon"></div>
            <div class="menu-item-text">Thiết lập hệ thống</div>
        </div>
    </div>
</template>

<script>
export default {
  name: 'Menu',
  components: { }
}
</script>

<style scoped>
@import '../../css/layout/menu.css';
</style>
