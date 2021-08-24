<template>
  <div>
    <Header></Header>
    <Menu :route="route" :items="menuItems"></Menu>
    <Content
      entityName="Customer"
      :filterNames="['CustomerGroupId']"
      :thead="thead"
      :theadMap="theadMap"
      :featureButtons="featureButtons"
    ></Content>
    <input
      id="fileHolder"
      type="file"
      accept=".xlsx, .xls"
      @change="submitFile"
      hidden
    />
  </div>
</template>

<script>
import axios from 'axios';
import EventBus from "../../event-bus/EventBus";
import Header from "../layout/TheHeader.vue";
import Menu from "../layout/TheMenu.vue";
import Content from "../base/BaseContent.vue";

export default {
  name: "CustomerPage",
  components: {
    Header,
    Menu,
    Content,
  },
  data() {
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
      menuItems: [
        { icon: "item-dashboard", path: "#", text: "Tổng quan" },
        { icon: "item-report", path: "#", text: "Báo cáo" },
        { icon: "item-employee", path: "/employees", text: "Danh mục nhân viên" },
        { icon: "item-employee", path: "/customers", text: "Danh mục khách hàng" },
        { icon: "item-setting", path: "#", text: "Thiết lập hệ thống" },
      ],
      route: "/customers",
      featureButtons: [
        {
          Name: "Xuất khẩu",
          Icon: "icon-add",
          Callback: this.chooseFile,
        },
        {
          Name: "Nhập khẩu",
          Icon: "icon-save",
          Callback: this.chooseFile,
        },
      ],
      file: null,
    };
  },
  methods: {
    /**
     * handle khi click chọn nhập file
     * CreatedBy: HungNguyen81 (19-08-2021)
     */
    chooseFile() {
      this.file = document.getElementById("fileHolder");
      this.file.click();
      console.log("IMPORTED");
    },

    /**
     * Xác nhận chọn file
     * CreatedBy: HungNguyen81 (19-08-2021)
     */
    submitFile() {
      EventBus.$emit("showPopup", {
        title: `Xác nhận nhập khẩu file`,
        content: `Bạn có chắc chắn muốn nhập file <b>"${this.file.files[0].name}"</b> hay không ?!`,
        popupType: "info",
        okAction: "Nhập",
        isHide: false,
        icon: "icon-add",
        callback: this.sendFileToImport,
      });
    },

    /**
     * POST file đã chọn
     * CreatedBy: HungNguyen81 (19-08-2021)
     * ModifiedBy: HungNguyen81 (20-08-2021)
     */
    sendFileToImport() {
      console.table(this.file.files);

      // Gửi file lên API server
      var formData = new FormData();
      formData.append("formFile", this.file.files[0]);
      axios
        .post("https://localhost:44372/api/v1/Customers/import", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        })
        .then((res) => {
          console.groupCollapsed("Import file result");
          console.table(res.data);
          console.groupEnd();
        });

      // sau khi submit file, clear file value
      this.file = null;
    },
  },
};
</script>

<style>
</style>
