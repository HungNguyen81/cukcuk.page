<template>
  <div>
    <div class="content">
      <div class="content-heading">
        <b class="title">Danh sách nhân viên</b>
        <BaseButton
          value="Xóa nhân viên"
          type="button-delete"
          :onclick="OpenForm"
        ></BaseButton>
        <BaseButtonIcon
          value="Thêm nhân viên"
          icon="icon-add"
          :onclick="OpenForm"
        ></BaseButtonIcon>
      </div>
      <!-- <SearchBar></SearchBar> -->
      <div class="content-search">
        <div class="search-icon"></div>
        <input
          type="text"
          class="textbox-default search-field"
          id="search-box"
          placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại"
        />
        <Combobox
          :api="'http://cukcuk.manhnv.net/api/Department'"
          type="Department"
          mode="1"
        ></Combobox>
        <Combobox
          :api="'http://cukcuk.manhnv.net/v1/Positions'"
          type="Position"
          mode="1"
        ></Combobox>

        <div class="button-refresh"></div>
      </div>
      <!-- <div class="content-table">
        <table class="table-employee" id="table-employee">
          <thead>
            <tr>
              <th></th>
              <th>Mã nhân viên</th>
              <th>Họ và tên</th>
              <th>Giới tính</th>
              <th>Ngày sinh</th>
              <th>Điện thoại</th>
              <th>Email</th>
              <th>Chức vụ</th>
              <th>Phòng ban</th>
              <th>Mức lương cơ bản</th>
              <th>Tình trạng công việc</th>
            </tr>
          </thead>
          <tbody>
            
            <tr v-for="e in employees" :key="e.EmployeeCode">
              <td>
                <span class="checkbox-container"
                  ><input type="checkbox" name="delete" />
                  <span class="checkmark"
                    ><i class="fas fa-check check"></i></span
                ></span>
              </td>
              <td v-bind:title="[e.EmployeeCode]">{{ e.EmployeeCode }}</td>
              <td v-bind:title="[e.FullName]">{{ e.FullName }}</td>
              <td v-bind:title="[e.Gender]">{{ e.Gender }}</td>
              <td v-bind:title="[e.DateOfBirth]">{{ e.DateOfBirth }}</td>
              <td v-bind:title="[e.PhoneNumber]">{{ e.PhoneNumber }}</td>
              <td v-bind:title="[e.Email]">{{ e.Email }}</td>
              <td v-bind:title="[e.PositionName]">{{ e.PositionName }}</td>
              <td v-bind:title="[e.DepartmentName]">{{ e.DepartmentName }}</td>
              <td v-bind:title="[e.Salary]">{{ e.Salary }}</td>
              <td v-bind:title="[e.WorkStatus]">{{ e.WorkStatus }}</td>
            </tr>
          </tbody>
        </table>
      </div> -->
      <Table
        :type="'Employee'"
        :thead="thead"
        :dataMap="tmap"
        :api="'http://cukcuk.manhnv.net/v1/Employees/'"
      ></Table>

      <div class="content-page-navigator">
        <div class="navigator-left" id="current-pagesize">
          Hiển thị <b>1-20/{{ employees.length }}</b> nhân viên
        </div>
        <div class="navigator-center" currentpage="1">
          <div class="button-firstpage button-navigator"></div>
          <div class="button-prev-page button-navigator"></div>

          <div class="page-buttons">
            <div class="button-page-number button-current-page first-page">
              1
            </div>
          </div>

          <div class="button-next-page button-navigator"></div>
          <div class="button-lastpage button-navigator"></div>
        </div>
        <div class="navigator-right">
          <!-- <div class="input-field"> -->
          <Dropdown
            direction="up"
            :data="this.pageSize"
            :type="'drop-number-of-row'"
            :displayId="'number-of-rows'"
          ></Dropdown>
        </div>
      </div>
    </div>
    <Form :isOpen="this.formStatus" :close="this.CloseForm"></Form>
  </div>
</template>

<script>
import BaseButton from "../base/BaseButton.vue";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import Dropdown from "../base/BaseDropdown.vue";
import Combobox from "../base/BaseCombobox.vue";
import Form from "../base/BaseForm.vue";
import Table from "../base/BaseTable.vue";

export default {
  name: "Content",
  components: {
    BaseButton,
    BaseButtonIcon,
    Combobox,
    Dropdown,
    Form,
    Table,
  },
  data() {
    return {
      employees: [],
      pageSize: [
        "10 nhân viên/trang",
        "20 nhân viên/trang",
        "50 nhân viên/trang",
        "100 nhân viên/trang",
      ],
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
      tmap: [
        "EmployeeCode",
        "FullName",
        "Gender",
        "DateOfBirth",
        "PhoneNumber",
        "Email",
        "PositionName",
        "DepartmentName",
        "Salary",
        "WorkStatus",
      ],
      formStatus: false,
    };
  },
  created() {},
  methods: {
    OpenForm() {
      this.formStatus = true;
      console.log("open form");
    },
    CloseForm() {
      this.formStatus = false;
    },
  },
};
</script>

<style>
@import "../../css/layout/content.css";
</style>
