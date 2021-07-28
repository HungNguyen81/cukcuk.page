<template>
  <div class="content-search">
    <div class="search-icon"></div>
    <input
      type="text"
      class="textbox-default search-field"
      id="search-box"
      placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại"
    />
    <Combobox
      :api="'https://cukcuk-app.herokuapp.com/api/Department'"
      type="Department"
      mode="1"
    ></Combobox>
    <Combobox
      :api="'http://cukcuk.manhnv.net/v1/Positions'"
      type="Position"
      mode="1"
    ></Combobox>

    <div
      class="button-refresh"
      onclick="GetNumberOfEmployees(UpdateEmployeeTable);"
    ></div>
  </div>
</template>

<script>
import Combobox from "./BaseCombobox.vue";
export default {
  name: "SearchBar",
  components: {
    Combobox,
  },
};
</script>

<style scoped>
</style>