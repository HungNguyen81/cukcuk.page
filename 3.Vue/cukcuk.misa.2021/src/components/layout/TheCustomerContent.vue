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
          :onclick="BtnAddClick"
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

        <div class="button-refresh" @click="RefreshTable"></div>
      </div>

      <div class="table-wrap">
        <Table
          type="Employee"
          :thead="thead"
          :dataMap="tmap"
          :api="`https://localhost:44372/api/Employees/employeeFilter?pageSize=${pageSize}&pageNumber=${pageNumber}&filterString=${searchInput}&departmentId=${
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
      :mode="formMode"
      @closeForm="this.closeFormWithoutSave"
      :detailId="this.employeeId"
      :moreDetail="this.moreDetail"
      @saveClicked="this.FormSaveButtonClick"
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
      @closePopup="ClosePopup"
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
      formMode: 0,
      employeeId: null,
      employeeDetail: null,
      moreDetail: null,
      // Delete info
      deleteIdList: [],
      deleteCodeList: [],
      tableFlag: false,
      // page
      pageSize: 20,
      pageNumber: 0, // = current - 1
      totalRecord: 20,
      totalPage: 1,
      pageItems: [],
      // search
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
  },
  watch: {
    // watch sự thay đổi của nội dung tìm kiếm
    searchInput: function (c) {
      if (this.searchTimeOut) clearTimeout(this.searchTimeOut);
      this.searchTimeOut = setTimeout(() => {
        // re-render table
        this.pageNumber = 0;
        this.tableFlag = !this.tableFlag;
        console.log("c:", c);
      }, 500);
    },

    // khi kích thước trang thay đổi thì render lại bảng
    pageSize: function () {
      this.ForceTableRerender();
    },
    // chuyển trang => render bảng
    pageNumber: function () {
      this.ForceTableRerender();
    },
  },
  methods: {
    /**
     * Handle khi client chọn filter từ combobox
     */
    activeFilter(type, id) {
      // this.filter[type + "Id"] = id;
      this.$set(this.filter, type + "Id", id);
      console.log("filter:", this.filter);
      this.ForceTableRerender();
    },

    /**
     * Handle khi chuyển trang trên paging component
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
     * khi dữ liệu table load xong, gửi totalPage, totalRecord sang paging component
     */
    transPagingInfo(numPage, numRecord) {
      this.totalRecord = numRecord;
      this.totalPage = numPage;
    },

    OnPageChange(size, num) {
      this.pageSize = size;
      this.pageNumber = num;
    },

    OpenForm(act) {
      this.formMode = act;
      this.formStatus = true;
    },
    CloseForm() {
      this.formStatus = false;
      this.popup.isHide = true;
    },

    /**
     * Đóng form mà k có popup hiện lên
     */
    closeFormWithoutSave() {
      this.popup = {
        title: "Xác nhận hủy",
        content: `Bạn có chắc chắn muốn <b>HỦY</b> nhập liệu hay không ?!`,
        popupType: "warning",
        okAction: "Có",
        isHide: false,
        callback: this.CloseForm,
      };
    },

    OpenPopup() {
      this.popup.isHide = false;
    },
    ClosePopup() {
      this.popup.isHide = true;
    },

    /**
     * Hiển thị thông báo toast
     * params: {String}
     */
    showToast(type, header, msg, delay) {
      const Show = () => {
        this.toasts.push({
          type: type,
          header: header.toUpperCase(),
          msg: msg,
        });
      };
      if (delay) {
        setTimeout(Show, delay);
      } else {
        Show();
      }
    },

    /**
     * Hiển thị popup khi có callback
     */
    showPopup(options) {
      if (!options.callback) {
        options.callback = this.ClosePopup;
      }
      this.popup = options;
    },

    /**
     * Hiện popup thông báo xác nhận thêm/sửa
     */
    FormSaveButtonClick(mode, id, detail) {
      this.popup = {
        title: "Thông báo",
        content: `Bạn có chắc chắn muốn <b>${
          mode ? "sửa" : "thêm"
        }</b> nhân viên hay không ?!`,
        popupType: "",
        okAction: "Lưu",
        isHide: false,
        callback: mode ? this.SendPutRequest : this.SendPostRequest,
      };
      this.employeeId = id;
      this.employeeDetail = detail;
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode sửa nhân viên
     */
    SendPutRequest() {
      this.axios
        .put(
          `https://localhost:44372/api/Employees/${this.employeeId}`,
          this.employeeDetail
        )
        .then(() => {
          this.showToast(
            "success",
            "PUT Success",
            `Sửa nhân viên <b>"${this.employeeDetail.FullName}"</b> thành công`
          );
          this.ClosePopup();
          this.CloseForm();
          this.RefreshTable();
        })
        .catch(() => {
          this.showToast(
            "error",
            "PUT error",
            `Sửa nhân viên <b>"${this.employeeDetail.FullName}"</b> không thành công`
          );
          this.ClosePopup();
        });
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode thêm nhân viên
     */
    SendPostRequest() {
      this.axios
        .post(`https://localhost:44372/api/Employees/`, this.employeeDetail)
        .then(() => {
          this.showToast(
            "success",
            "POST success",
            `Thêm nhân viên <b>"${this.employeeDetail.FullName}"</b> thành công`
          );
          this.ClosePopup();
          this.CloseForm();
          this.RefreshTable();
        })
        .catch(() => {
          this.showToast(
            "error",
            "POST error",
            `Thêm nhân viên <b>"${this.employeeDetail.FullName}"</b> không thành công`
          );
          this.ClosePopup();
        });
    },

    /**
     * Làm cho table render lại và hiện spinner
     */
    RefreshTable() {
      this.isTableLoading = true;
      this.ForceTableRerender();
    },

    /**
     * Thay đổi nhãn số trang khi chỉ số trang hiện tại thay đổi
     */
    tableDataLoaded() {
      console.log("LOADED");
      this.isTableLoading = false;
    },

    /**
     * Gọi hàm khi bấm nút Thêm nhân viên/ Thêm Khách hàng
     */
    BtnAddClick() {
      this.OpenForm(0);
    },

    // Xử lí sự kiện click đúp vào table row
    rowDoubleClick(id, pos, dep) {
      this.employeeId = id;
      this.moreDetail = {
        PositionName: pos,
        DepartmentName: dep,
      };
      this.OpenForm(1);
    },

    /**
     *  Xử lí khi select 1 table row
     */
    rowSelect(id, code, name) {
      if (this.deleteIdList.includes(id)) {
        let i = this.deleteIdList.indexOf(id);
        this.deleteIdList.splice(i, 1);
        this.deleteCodeList.splice(i, 1);
      } else {
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

    /**
     * Xử lí sự kiện bấm nút xóa các hàng đã chọn trên table
     */
    delBtnClick() {
      this.popup = {
        title: "Xác nhận xóa",
        content: `Bạn có chắc chắn muốn xóa "<b>${this.deleteCodeList.join(
          ", "
        )}</b>" hay không ?!`,
        popupType: "error",
        okAction: "Xóa",
        isHide: false,
        callback: this.SendDeleteRequests,
      };
      this.OpenPopup();
    },

    /**
     * Callback khi bấm OK trong popup
     */
    SendDeleteRequests() {
      for (const [i, id] of this.deleteIdList.entries()) {
        let index = this.deleteIdList.indexOf(id);
        let name = this.deleteCodeList[index];
        this.axios
          .delete(`https://localhost:44372/api/Employees/${id}`)
          .then(() => {
            let index = this.deleteIdList.indexOf(id);
            this.deleteIdList.splice(index, 1);
            this.deleteCodeList.splice(index, 1);

            // Đóng popup và làm mới bảng
            this.ClosePopup();
            this.RefreshTable();

            // hiển thị toast thông báo đã xóa thành công
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

    /**
     * Khiến table phải re-render
     */
    ForceTableRerender() {
      this.isTableLoading = true;
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
