<template>
  <div>
    <Header></Header>
    <Menu :route="route" :items="menuItems"></Menu>
    <Content entityName="Employee" :filterNames="['DepartmentId', 'PositionId']" :thead="thead" :theadMap="theadMap"></Content>
  </div>
</template>

<script>
import Header from "../layout/TheHeader.vue"
import Menu   from "../layout/TheMenu.vue"
import Content from "../base/BaseContent.vue"

export default {
  name: "EmployeePage",
  components: {
    Header,
    Menu,
    Content
  },
  data(){
    return {
      thead: [
        "Mã nhân viên",
        "Họ và tên",
        "Giới tính",
        "Ngày sinh",
        "Điện thoại",
        "Email",
        "Chức vụ",
        "Phòng ban",
        "Mức lương cơ bản",
        "Tình trạng công việc",
      ],
      theadMap: [
        "EmployeeCode",
        "FullName",
        "GenderName",
        "DateOfBirth",
        "PhoneNumber",
        "Email",
        "PositionName",
        "DepartmentName",
        "Salary",
        "WorkStatus",
      ],
      menuItems: [
        { icon: "item-dashboard", path: "#", text: "Tổng quan" },
        { icon: "item-report", path: "#", text: "Báo cáo" },
        { icon: "item-employee", path: "/employees", text: "Danh mục nhân viên" },
        { icon: "item-employee", path: "/customers", text: "Danh mục khách hàng" },
        { icon: "item-setting", path: "#", text: "Thiết lập hệ thống" },
      ],
      route: "/employees",
    }
  }
};
</script>

<style>
</style>
