<template>
  <div class="content">
    <div class="content-heading">
      <b class="title">Danh sách nhân viên</b>
      <div
        class="button"
        id="button-delete"
        onclick="DeleteSelectedEmployees()"
      >
        Xóa nhân viên
      </div>
      <div class="button button-icon" onclick="OpenPopup()">
        <div class="icon-button"></div>
        <div>Thêm nhân viên</div>
      </div>
      <!-- <BaseButton></BaseButton> -->
    </div>
    <div class="content-search">
      <div class="search-icon"></div>
      <input
        type="text"
        class="textbox-default search-field"
        id="search-box"
        placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại"
        onkeyup="TableLiveSearch(this)"
      />
      <div class="combobox-container dropdown-departments">
        <div class="combobox">
          <input
            type="text"
            class="combobox-input textbox-default"
            value="Tất cả phòng ban"
            filter="department"
            onkeyup="ComboboxInputChange(this)"
          />
          <div class="x-icon" hidden="true" onclick="ClearInputText(this)">
            <i class="fas fa-times"></i>
          </div>
          <div class="combobox-icon-container" onclick="ShowDropData(this)">
            <div class="combobox-icon"></div>
          </div>
        </div>
        <div class="dropdown-data departments" id="departments" hidden="true">
          <!-- <div class="dropdown-item item-selected" onclick="ItemSelect(this)">
                        <i class="fas fa-check item-icon"></i>
                        <div class="item-text">Tất cả phòng ban</div>
                    </div> -->
        </div>
      </div>

      <div class="combobox-container dropdown-positions">
        <div class="combobox">
          <input
            type="text"
            class="combobox-input textbox-default"
            value="Tất cả vị trí"
            filter="position"
            onkeyup="ComboboxInputChange(this)"
          />
          <div class="x-icon" hidden="true" onclick="ClearInputText(this)">
            <i class="fas fa-times"></i>
          </div>
          <div class="combobox-icon-container" onclick="ShowDropData(this)">
            <div class="combobox-icon"></div>
          </div>
        </div>
        <div class="dropdown-data positions" id="positions" hidden="true">
          <!-- <div class="dropdown-item  item-selected" onclick="ItemSelect(this)">
                        <i class="fas fa-check item-icon"></i>
                        <div class="item-text">Tất cả vị trí</div>
                    </div> -->
        </div>
      </div>
      <div
        class="button-refresh"
        onclick="GetNumberOfEmployees(UpdateEmployeeTable);"
      ></div>
    </div>
    <div class="content-table">
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
          <!-- Employee data goes here -->
          <tr v-for="e in employees" :key="e.EmployeeCode">
            <td>
              <span class="checkbox-container"
                ><input type="checkbox" name="delete" />
                <span class="checkmark"><i class="fas fa-check check"></i></span
              ></span>
            </td>
            <td v-bind:title="[e.EmployeeCode]">{{e.EmployeeCode}}</td>
            <td v-bind:title="[e.FullName]">{{e.FullName}}</td>
            <td v-bind:title="[e.Gender]">{{e.Gender}}</td>
            <td v-bind:title="[e.DateOfBirth]">{{e.DateOfBirth}}</td>
            <td v-bind:title="[e.PhoneNumber]">{{e.PhoneNumber}}</td>
            <td v-bind:title="[e.Email]">{{e.Email}}</td>
            <td v-bind:title="[e.PositionName]">{{e.PositionName}}</td>
            <td v-bind:title="[e.DepartmentName]">{{e.DepartmentName}}</td>
            <td v-bind:title="[e.Salary]">{{e.Salary}}</td>
            <td v-bind:title="[e.WorkStatus]">{{e.WorkStatus}}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="content-page-navigator">
      <div class="navigator-left" id="current-pagesize">
        Hiển thị <b>1-20/1000</b> nhân viên
      </div>
      <div class="navigator-center" currentpage="1">
        <div class="button-firstpage button-navigator"></div>
        <div class="button-prev-page button-navigator"></div>

        <div class="page-buttons">
          <div class="button-page-number button-current-page first-page">1</div>
        </div>

        <div class="button-next-page button-navigator"></div>
        <div class="button-lastpage button-navigator"></div>
      </div>
      <div class="navigator-right">
        <div class="input-field">
          <div class="dropdown-container drop-number-of-row">
            <div class="dropdown">
              <div id="number-of-rows"><b>20</b> nhân viên/trang</div>
              <div class="dropdown-icon number-of-rows-icon">
                <i class="fas fa-chevron-up"></i>
              </div>
            </div>
            <div class="dropdown-data" hidden="true">
              <div class="dropdown-item paging-item">
                <i class="fas fa-check item-icon"></i>
                <div class="item-text"><b>10</b> nhân viên/trang</div>
              </div>
              <div class="dropdown-item paging-item item-selected">
                <i class="fas fa-check item-icon"></i>
                <div class="item-text"><b>20</b> nhân viên/trang</div>
              </div>
              <div class="dropdown-item paging-item">
                <i class="fas fa-check item-icon"></i>
                <div class="item-text"><b>50</b> nhân viên/trang</div>
              </div>
              <div class="dropdown-item paging-item">
                <i class="fas fa-check item-icon"></i>
                <div class="item-text"><b>100</b> nhân viên/trang</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
// import BaseButton from '../base/BaseButton.vue'

export default {
  name: "Content",
  components: {
    //   BaseButton
  },
  data() {
    return {
      employees: [],
    };
  },
  created() {
    this.axios
      .get("http://cukcuk.manhnv.net/v1/Employees/")
      .then((res) => {
        console.log(res.data);
        this.employees = res.data;
      })
      .catch((err) => {
        console.log(err);
      });
  },
};
</script>

<style>
@import "../../css/layout/content.css";
</style>
