<template>
  <div>
    <div class="content">
      <div class="content-heading">
        <b class="title">Danh sách nhân viên</b>
        <BaseButton
          value="Xóa nhân viên"
          type="button-delete"
          :onclick="delBtnClick"
          :class="{'hide': !delBtnActive}"
        ></BaseButton>
        <BaseButtonIcon
          value="Thêm nhân viên"
          icon="icon-add"
          :onclick="BtnAddClick"
        ></BaseButtonIcon>
      </div>

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

      <Table
        :type="'Employee'"
        :thead="thead"
        :dataMap="tmap"
        :api="'http://cukcuk.manhnv.net/v1/Employees/'"
        @dataLoaded="tableDataLoaded"
        @rowDblClick="rowDoubleClick"
        @rowClick="rowSelect"
        :key="tableKey"
      ></Table>

      <div class="content-page-navigator">
        <div class="navigator-left" id="current-pagesize">
          Hiển thị <b>1-20/{{ tableSize }}</b> nhân viên
        </div>
        <div class="navigator-center">
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
          <Dropdown
            :direction="'up'"
            :data="this.pageSizeDropData"
            :type="'drop-number-of-row'"
            :displayId="'number-of-rows'"
            :currProp="this.pageSizeDropData[0].Name"
          ></Dropdown>
        </div>
      </div>
    </div>    
    <Form
      :isOpen="this.formStatus"
      :close="this.CloseForm"
      :mode="action"
      @closeForm="this.CloseForm"
      :detailId="this.employeeId"
      :moreDetail="this.moreDetail"      
    ></Form>
    <div id="loader" :class="{'hide' : !isTableLoading}">
        <div class="spinner-wrapper">
            <div class="spinner"></div>
        </div>
    </div>
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
      tableSize: 0, // number of rows
      action: 0,
      employeeId: null,
      moreDetail: null,
      deleteList: [],
      tableKey: 0,
      pageSizeDropData: [
        {Name:"10 nhân viên/trang"},
        {Name:"20 nhân viên/trang"},
        {Name:"50 nhân viên/trang"},
        {Name:"100 nhân viên/trang"},
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
        "GenderName",
        "DateOfBirth",
        "PhoneNumber",
        "Email",
        "PositionName",
        "DepartmentName",
        "Salary",
        "WorkStatus",
      ],
      formStatus: false,
      isTableLoading: true
    };
  },
  created() {},
  computed: {
    delBtnActive: function(){
      return this.deleteList.length > 0;
    }
  },
  methods: {
    OpenForm(act) {
      this.action = act;
      this.formStatus = true;
    },
    CloseForm() {
      this.formStatus = false;
      this.employeeId = null;
    },
    /**
     * Change page label when number of rows change
     */
    tableDataLoaded(size) {
      this.tableSize = size;
      this.isTableLoading = false;
    },
    BtnAddClick(){      
      this.OpenForm(0);
    },
    rowDoubleClick(id, pos, dep){      
      console.log("row click", id);
      this.employeeId = id;
      this.moreDetail = {
        PositionName: pos,
        DepartmentName: dep
      }
      this.OpenForm(1);
    },
    rowSelect(id){
      if(this.deleteList.includes(id)){
        let i = this.deleteList.indexOf(id);
        this.deleteList.splice(i,1);
      }else{
        this.deleteList.push(id);
      }      
    },
    delBtnClick(){
      for(let id of this.deleteList){
        this.axios.delete(`http://cukcuk.manhnv.net/v1/Employees/${id}`)
        .then(() => {
          this.deleteList.splice(this.deleteList.indexOf(id));
          this.isTableLoading = true;
          this.ForceTableRerender();
        })
        .catch(err => {
          console.log(err);
        })
      }
    },
    ForceTableRerender(){
      this.tableKey++;
    }
  },
};
</script>

<style>
@import "../../css/layout/content.css";
@import "../../css/base/loader.css"
</style>
