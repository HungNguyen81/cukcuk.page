<template>
  <div>
    <Header></Header>
    <Menu></Menu>
    <Content entityName="Customer" :filterName="['CustomerGroupId']" :thead="thead" :theadMap="theadMap"></Content>
  </div>
</template>

<script>
import Header from "../layout/TheHeader.vue"
import Menu   from "../layout/TheMenu.vue"
import Content from "../base/BaseContent.vue"

export default {
  name: "CustomerPage",
  components: {
    Header,
    Menu,
    Content
  },
  data(){
    return {
      thead: [
        "Mã khách hàng",
        "Họ và tên",
        "Giới tính",
        "Ngày sinh",
        "Điện thoại",
        "Email",
        "Nhóm khách hàng",
        "Tên công ty",
        "Dừng theo dõi",
      ],
      theadMap: [
        "CustomerCode",
        "FullName",
        "GenderName",
        "DateOfBirth",
        "PhoneNumber",
        "Email",
        "CustomerGroupName",
        "CompanyName",
        "IsStopFollow",
      ],
    }
  }
};
</script>

<style>
</style>
