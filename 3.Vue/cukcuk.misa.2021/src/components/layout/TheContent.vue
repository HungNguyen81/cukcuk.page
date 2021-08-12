<template>
  <div>
    <div class="content">
      <div class="content-heading">
        <b class="title">Danh sách nhân viên</b>
        <BaseButtonIcon
          value="Xóa nhân viên"
          type="button-delete"
          :onclick="delBtnClick"
          :class="{ hide: !delBtnActive }"
        ></BaseButtonIcon>
        <BaseButtonIcon
          value="Thêm nhân viên"
          icon="icon-add"
          :onclick="btnAddClick"
        ></BaseButtonIcon>
      </div>

      <div class="content-search">
        <div class="search-icon"></div>
        <input
          type="text"
          class="textbox-default search-field"
          placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại"
          v-model="searchInput"
        />
        <Combobox
          :api="'http://cukcuk.manhnv.net/api/Department'"
          type="Department"
          mode="1"
          @filterActive="activeFilter"
          @showToast="showToast"
        ></Combobox>
        <Combobox
          :api="'http://cukcuk.manhnv.net/v1/Positions'"
          type="Position"
          mode="1"
          @filterActive="activeFilter"
          @showToast="showToast"
        ></Combobox>

        <div class="button-refresh" @click="refreshTable"></div>
      </div>

      <div class="table-wrap">
        <Table
          :type="'Employee'"
          :thead="thead"
          :dataMap="tmap"
          :api="`http://cukcuk.manhnv.net/v1/Employees/employeeFilter?pageSize=${pageSize}&pageNumber=${
            pageNumber * pageSize
          }&employeeFilter=${searchContent}&departmentId=${
            filter.DepartmentId ? filter.DepartmentId : ''
          }&positionId=${filter.PositionId ? filter.PositionId : ''}`"
          @dataLoaded="tableDataLoaded"
          @rowDblClick="rowDoubleClick"
          @rowClick="rowSelect"
          :key="tableFlag"
          @getPagingInfo="transPagingInfo"
          @showToast="showToast"
        ></Table>
        <div id="loader" :class="{ hide: !isTableLoading }">
          <div class="spinner-wrapper">
            <div class="spinner"></div>
          </div>
        </div>
      </div>

      <Paging
        :pageNumber="this.pageNumber"
        :pageSize="this.pageSize"
        :totalPage="totalPage"
        :totalRecord="totalRecord"
        :items="pageItems"
        @pageSizeChange="onPageSizeChange"
        @pageNumChange="onPageNumChange"
        @showToast="showToast"
      ></Paging>
    </div>
    <Form
      :isOpen="this.formStatus"
      :close="this.closeFormWithoutSave"
      :mode="action"
      @closeForm="this.closeFormWithoutSave"
      :detailId="this.employeeId"
      :moreDetail="this.moreDetail"
      @saveClicked="this.formSaveButtonClick"
      @getNewCodeError="failInGetNewCode"
      @showToast="showToast"
      @showPopup="showPopup"
    ></Form>

    <Popup
      :title="popup.title"
      :content="popup.content"
      :ok="popup.okAction"
      :isHide="popup.isHide"
      :popupType="popup.popupType"
      :callback="popup.callback"
      @closePopup="closePopup"
    ></Popup>
    <div class="toast-stack">
      <Toast
        :type="t.type"
        :header="t.header"
        :msg="t.msg"
        v-for="(t, i) in toasts"
        :key="i"
        transition="fade"
        stagger="1000"
      ></Toast>
    </div>
  </div>
</template>

<script>
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import Combobox from "../base/BaseCombobox.vue";
import Form from "../base/BaseForm.vue";
import Table from "../base/BaseTable.vue";
import Popup from "../base/BasePopup.vue";
import Toast from "../base/BaseToast.vue";
import Paging from "../base/BasePaging.vue";

export default {
  name: "Content",
  components: {
    // BaseButton,
    BaseButtonIcon,
    Combobox,
    Paging,
    Form,
    Table,
    Popup,
    Toast,
  },
  data() {
    return {
      action: 0,
      employeeId: null,
      employeeDetail: null,
      moreDetail: null,
      deleteIdList: [],
      deleteCodeList: [],
      tableFlag: false,
      // page
      pageSize: 20,
      pageNumber: 0, // = current - 1
      totalRecord: 20,
      totalPage: 1,
      pageItems: [],
      //
      // searchContent: "nv",
      searchInput: "",
      searchTimeOut: null,
      // Filter
      filter: {
        DepartmentId: "",
        PositionId: "",
      },
      popup: {
        title: "Empty Title",
        content: "Empty Content",
        popupType: "error",
        okAction: "OK",
        isHide: true,
        callback: null,
      },
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
      isTableLoading: true,
      toasts: [],
    };
  },
  created() {},
  computed: {
    /**
     * Toggle trạng thái nút xóa nv theo số dòng được select
     */
    delBtnActive: function () {
      return this.deleteIdList.length > 0;
    },
    searchContent: function () {
      return this.searchInput ? this.searchInput : "nv";
    },
  },
  watch: {
    searchContent: function (c) {
      if (this.searchTimeOut) clearTimeout(this.searchTimeOut);
      this.searchTimeOut = setTimeout(() => {
        // re-render table
        this.pageNumber = 0;
        this.tableFlag = !this.tableFlag;
        console.log("c:", c);
      }, 500);
    },
    pageSize: function () {
      this.forceTableRerender();
    },
    pageNumber: function () {
      this.forceTableRerender();
    },
  },
  methods: {
    activeFilter(type, id) {
      this.filter[type + "Id"] = id;
      console.log("filter:", this.filter.DepartmentId);
      this.forceTableRerender();
    },

    /**
     * When page size change from paging bar
     */
    onPageSizeChange(size) {
      this.pageSize = size;
      this.pageNumber = 0;
    },

    onPageNumChange(num) {
      console.log("PAGE NUMBER: ", num);
      this.pageNumber = num;
    },

    /**
     * Handle when table load completely, send totalPage, totalRecord to paging component
     */
    transPagingInfo(numPage, numRecord) {
      this.totalRecord = numRecord;
      this.totalPage = numPage;
    },

    onPageChange(size, num) {
      this.pageSize = size;
      this.pageNumber = num;
    },

    openForm(act) {
      this.action = act;
      this.formStatus = true;
    },
    closeForm() {
      this.formStatus = false;
      this.popup.isHide = true;
    },

    /**
     * Close form without display a popup, handle click event for Cancel and Close Form buttons
     */
    closeFormWithoutSave() {
      this.popup = {
        title: "Xác nhận hủy",
        content: `Bạn có chắc chắn muốn <b>HỦY</b> nhập liệu hay không ?!`,
        popupType: "warning",
        okAction: "Có",
        isHide: false,
        callback: this.closeForm,
      };
    },

    openPopup() {
      this.popup.isHide = false;
      // this.popup.callback = func;
    },
    closePopup() {
      this.popup.isHide = true;
    },

    /**
     * show toast to notice to the client
     * params: {String}
     */
    showToast(type, header, msg, delay) {
      const show = () => {
        this.toasts.push({
          type: type,
          header: header.toUpperCase(),
          msg: msg,
        });
      };
      if (delay) {
        setTimeout(show, delay);
      } else {
        show();
      }
    },

    showPopup(options) {
      if (!options.callback) {
        options.callback = this.closePopup;
      }
      this.popup = options;
    },

    /**
     * Display a popup, notice that the app is going to send put/post request to server
     */
    formSaveButtonClick(mode, id, detail) {
      this.popup = {
        title: "Thông báo",
        content: `Bạn có chắc chắn muốn <b>${
          mode ? "sửa" : "thêm"
        }</b> nhân viên hay không ?!`,
        popupType: "",
        okAction: "Lưu",
        isHide: false,
        callback: mode ? this.sendPutRequest : this.sendPostRequest,
      };
      this.employeeId = id;
      this.employeeDetail = detail;
    },

    /**
     * Callback for Form's Save Button, when edit employee
     */
    sendPutRequest() {
      this.axios
        .put(
          `http://cukcuk.manhnv.net/v1/Employees/${this.employeeId}`,
          this.employeeDetail
        )
        .then(() => {
          // if (res) console.log("da SUA thanh cong");
          this.showToast(
            "success",
            "PUT Success",
            `Sửa nhân viên <b>"${this.employeeDetail.FullName}"</b> thành công`
          );
          this.closePopup();
          this.closeForm();
          this.refreshTable();
        })
        .catch(() => {
          this.showToast(
            "error",
            "PUT error",
            `Sửa nhân viên <b>"${this.employeeDetail.FullName}"</b> không thành công`
          );
          this.closePopup();
          // this.closeForm();
        });
    },

    /**
     * Callback for Form's Save Button, when add new employee
     */
    sendPostRequest() {
      this.axios
        .post(`http://cukcuk.manhnv.net/v1/Employees/`, this.employeeDetail)
        .then(() => {
          // if (res) console.log("da THEM thanh cong");
          this.showToast(
            "success",
            "POST success",
            `Thêm nhân viên <b>"${this.employeeDetail.FullName}"</b> thành công`
          );
          this.closePopup();
          this.closeForm();
          this.refreshTable();
        })
        .catch(() => { 
          // console.log(err);
          this.showToast(
            "error",
            "POST error",
            `Thêm nhân viên <b>"${this.employeeDetail.FullName}"</b> không thành công`
          );
          this.closePopup();
          // this.closeForm();
        });
    },

    /**
     * Make the table re-render and display loading spinner
     */
    refreshTable() {
      // this.isTableLoading = true;
      this.forceTableRerender();
    },

    /**
     * Change page label when number of rows change
     */
    tableDataLoaded() {
      console.log("LOADED");
      // this.isTableLoading = false;
    },

    btnAddClick() {
      this.openForm(0);
    },

    rowDoubleClick(id, pos, dep) {
      // console.log("row click", id);
      this.employeeId = id;
      this.moreDetail = {
        PositionName: pos,
        DepartmentName: dep,
      };
      this.openForm(1);
    },

    /**
     * Handle when click table row
     */
    rowSelect(id, code, name) {
      if (this.deleteIdList.includes(id)) {
        let i = this.deleteIdList.indexOf(id);
        this.deleteIdList.splice(i, 1);
        this.deleteCodeList.splice(i, 1);
      } else {
        // console.log(id, code, name);
        this.deleteIdList.push(id);
        this.deleteCodeList.push(name);

        if (this.deleteIdList.length > 100) {
          this.showToast(
            "warning",
            "OUT OF LIMITATION",
            `Số nhân viên vượt quá <b>"100"</b> người !`
          );
        }
      }
    },

    delBtnClick() {
      this.popup = {
        title: "Xác nhận xóa",
        content: `Bạn có chắc chắn muốn xóa "<b>${this.deleteCodeList.join(
          ", "
        )}</b>" hay không ?!`,
        popupType: "error",
        okAction: "Xóa",
        isHide: false,
        callback: this.sendDeleteRequests,
      };
      this.openPopup();
    },

    /**
     * Callback for delete popup
     */
    sendDeleteRequests() {
      // console.log("send delete");
      for (const [i, id] of this.deleteIdList.entries()) {
        let index = this.deleteIdList.indexOf(id);
        let name = this.deleteCodeList[index];
        this.axios
          .delete(`http://cukcuk.manhnv.net/v1/Employees/${id}`)
          .then(() => {
            this.deleteIdList.splice(index, 1);
            this.deleteCodeList.splice(index, 1);
            // console.log(id);
            this.closePopup();
            this.refreshTable();
            this.showToast(
              "info",
              "DELETE successfully",
              `Đã xóa "${name}"`,
              i * 1000
            );
          })
          .catch(() => {
            this.showToast(
              "error",
              "Delete error",
              `Xóa nhân viên "<b>${name}</b>" không thành công`,
              i * 1000
            );
          });
      }
    },

    failInGetNewCode() {
      this.showToast("error", "GET error", `Không thể lấy mã nhân viên mới !`);
    },

    /**
     * Force the table re-render
     */
    forceTableRerender() {
      this.isTableLoading = true;
      // this.tableKey = (this.tableKey + 1) % 100;
      this.tableFlag = !this.tableFlag;
    },
  },
};
</script>

<style scoped>
@import "../../css/layout/content.css";
@import "../../css/base/loader.css";

.toast-stack {
  position: fixed;
  bottom: 0;
  right: 0;
  height: calc(100vh - 49px);
  width: 8px;
  display: flex;
  flex-direction: column;
  align-items: center;
  z-index: 100;
}
</style>
