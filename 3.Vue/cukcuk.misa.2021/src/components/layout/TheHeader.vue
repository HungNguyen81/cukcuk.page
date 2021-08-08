<template>
  <div class="header">
    <div class="header-left">
      <div class="toggle"></div>
      <div class="logo"></div>
    </div>
    <div class="header-right">
      <div class="dropdown-container drop-down-restaurant" @click="isHide = !isHide">
        <div class="dropdown">
          <div>Nhà hàng Biển Đông</div>
          <div class="dropdown-icon"></div>
        </div>
        <div :class="['dropdown-data', {hide : isHide}]" id="restaurants">
          <div class="dropdown-item item-selected">
            <i class="fas fa-check item-icon"></i>
            <div class="item-text">Nhà hàng Biển Đông</div>
          </div>
          <div class="dropdown-item">
            <i class="fas fa-check item-icon"></i>
            <div class="item-text">Nhà hàng Biển Tây</div>
          </div>
          <div class="dropdown-item">
            <i class="fas fa-check item-icon"></i>
            <div class="item-text">Nhà hàng Bách Khoa</div>
          </div>
        </div>
      </div>
      <div class="user-options">
        <div class="notification"></div>
        <div class="user-avatar"></div>
        <div class="username">Nguyễn Ngọc Hưng</div>
        <div class="icon-options"></div>
      </div>
    </div>
  </div>
</template>

<script>

export default {
  name: "Header",
  components: {},
  data(){
    return {
      isHide: true,
    }
  }
};
</script>

<style scoped>
@import "../../css/layout/header.css";
@import "../../css/base/dropdown.css";
</style>
