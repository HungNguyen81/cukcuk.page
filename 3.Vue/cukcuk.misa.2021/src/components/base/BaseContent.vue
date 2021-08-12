<template>
  <div>
    <div class="content">
      <div class="content-heading">
        <b class="title">Danh sách {{ entityMap[entityName].toLowerCase() }}</b>
        <BaseButtonIcon
          :value="'Xóa ' + entityMap[entityName].toLowerCase()"
          type="button-delete"
          :onclick="delBtnClick"
          :class="{ hide: !delBtnActive }"
        ></BaseButtonIcon>
        <BaseButtonIcon
          :value="'Thêm ' + entityMap[entityName].toLowerCase()"
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
          :api="`https://localhost:44372/api/v1/${
            filterName[index].split('Id')[0]
          }s`"
          :type="filterName[index].split('Id')[0]"
          mode="1"
          @filterActive="activeFilter"
          @showToast="showToast"
          v-for="(f, index) in filterName"
          :key="index"
        ></Combobox>
        <div class="button-refresh" @click="RefreshTable"></div>
      </div>

      <div class="table-wrap">
        <Table
          :type="entityName"
          :thead="thead"
          :dataMap="theadMap"
          :api="`https://localhost:44372/api/v1/${entityName}s/${entityName.toLowerCase()}Filter?pageSize=${pageSize}&pageNumber=${pageNumber}&filterString=${
            searchInput + GetApiFilterQuery()
          }`"
          @dataLoaded="tableDataLoaded"
          @rowDblClick="rowDoubleClick"
          @rowClick="rowSelect"
          :tableKey="tableFlag"
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
        :entityName="entityName"
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
      :detailId="this.entityId"
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
import BaseButtonIcon from "./BaseButtonIcon.vue";
import Combobox from "./BaseCombobox.vue";
import Form from "./BaseForm.vue";
import Table from "./BaseTable.vue";
import Popup from "./BasePopup.vue";
import Toast from "./BaseToast.vue";
import Paging from "./BasePaging.vue";

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
  props: {
    entityName: {
      type: String,
      required: true,
      default() {
        return "Employee";
      },
    },
    filterName: {
      type: Array,
      required: false,
    },
    thead: {
      type: Array,
      require: true,
    },
    theadMap: {
      type: Array,
      require: true,
    },
  },
  data() {
    return {
      formMode: 0,
      entityId: null,
      entityDetail: null,
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
        CustomerGroupId: "",
      },
      popup: {
        title: "Empty Title",
        content: "Empty Content",
        popupType: "error",
        okAction: "OK",
        isHide: true,
        callback: null,
      },
      formStatus: false,
      isTableLoading: true,
      toasts: [],
      entityMap: {
        Customer: "Khách hàng",
        Employee: "Nhân viên",
      },
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
        console.log("search:", c);
      }, 500);
    },

    // khi kích thước trang thay đổi thì render lại bảng
    pageSize: function () {
      // this.ForceTableRerender();
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

      console.groupCollapsed("Filters");
      console.table(Object.assign({}, this.filter));
      console.groupEnd();

      this.ForceTableRerender();
    },

    /**
     * Handle khi chuyển trang trên paging component
     */
    onPageSizeChange(size) {
      this.pageSize = size;
      this.pageNumber = 0;
      this.ForceTableRerender();
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

    OpenForm(mode) {
      //   this.formMode = act;
      //   this.formStatus = true;
      console.log("mode:", mode);
      if (this.entityName == "Employee") {
        // this.OpenForm(0);
        this.formMode = mode;
        this.formStatus = true;
      } else {
        this.showPopup({
          title: "Thông báo",
          content: `Chức năng ${mode ? "Sửa" : "Thêm"}  ${
            this.entityMap[this.entityName]
          } chưa được thực hiện,<br> Vui lòng liên hệ MISA !`,
          popupType: "warning",
          okAction: "OK",
          isHide: false,
          callback: this.ClosePopup,
        });
      }
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
      this.entityId = id;
      this.entityDetail = detail;
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode sửa nhân viên
     */
    SendPutRequest() {
      this.axios
        .put(
          `https://localhost:44372/api/v1/${this.entityName}s/${this.entityId}`,
          this.entityDetail
        )
        .then(() => {
          this.showToast(
            "success",
            "PUT Success",
            `Sửa nhân viên <b>"${this.entityDetail.FullName}"</b> thành công`
          );
          this.ClosePopup();
          this.CloseForm();
          this.RefreshTable();
        })
        .catch(() => {
          this.showToast(
            "error",
            "PUT error",
            `Sửa nhân viên <b>"${this.entityDetail.FullName}"</b> không thành công`
          );
          this.ClosePopup();
        });
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode thêm nhân viên
     */
    SendPostRequest() {
      this.axios
        .post(
          `https://localhost:44372/api/v1/${this.entityName}s/`,
          this.entityDetail
        )
        .then(() => {
          this.showToast(
            "success",
            "POST success",
            `Thêm nhân viên <b>"${this.entityDetail.FullName}"</b> thành công`
          );
          this.ClosePopup();
          this.CloseForm();
          this.RefreshTable();
        })
        .catch(() => {
          this.showToast(
            "error",
            "POST error",
            `Thêm nhân viên <b>"${this.entityDetail.FullName}"</b> không thành công`
          );
          this.ClosePopup();
        });
    },

    /**
     * Làm cho table render lại và hiện spinner
     */
    RefreshTable() {
      // this.isTableLoading = true;
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
      //   if (this.entityName == "Employee") {
      this.OpenForm(0);
      //   } else {
      //     this.showPopup({
      //       title: "Thông báo",
      //       content: `Chức năng thêm ${this.entityMap[this.entityName]} chưa được thực hiện,<br> Vui lòng liên hệ MISA !`,
      //       popupType: "warning",
      //       okAction: "OK",
      //       isHide: false,
      //       callback: this.ClosePopup,
      //     })
      //   }
    },

    // Xử lí sự kiện click đúp vào table row
    rowDoubleClick(id, pos, dep) {
      this.entityId = id;
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
     * Callback khi bấm OK trong popup, gửi toàn bộ id cần xóa lên 1 request
     */
    SendDeleteRequests() {
      console.table(this.deleteIdList);
      this.axios
        .delete(`https://localhost:44372/api/v1/${this.entityName}s/`, {
          data: this.deleteIdList,
        })
        .then((res) => {
          this.deleteIdList = [];
          this.deleteCodeList = [];

          // Đóng popup và làm mới bảng
          this.ClosePopup();
          this.RefreshTable();

          // hiển thị toast thông báo đã xóa thành công
          this.showToast(
            "info",
            "DELETE successfully",
            res.data.userMsg
          );
        })
        .catch(() => {
          this.showToast("error", "Delete error", `Xóa không thành công`);
        });
    },

    /**
     * Khiến table phải re-render
     */
    ForceTableRerender() {
      this.isTableLoading = true;
      this.tableFlag = !this.tableFlag;
    },

    /**
     * Lấy thông số filter cho api query
     */
    GetApiFilterQuery() {
      var res = "";
      for (let i = 0; i < this.filterName.length; i++) {
        res += `&${this.filterName[i]}=${this.filter[this.filterName[i]]}`;
      }      
      return res;
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
