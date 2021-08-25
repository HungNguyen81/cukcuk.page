<template>
  <div>
    <div class="content">
      <div class="content-heading">
        <b class="title">Danh sách {{ entityMap[entityName].toLowerCase() }}</b>
        <div class="buttons-containner">
          <BaseButtonIcon
            :value="'Xóa ' + entityMap[entityName].toLowerCase()"
            type="button-delete"
            icon="icon-delete"
            :onclick="delBtnClick"
            :class="{ 'hidden': !delBtnActive }"
          ></BaseButtonIcon>
          <BaseButtonIcon
            :value="'Thêm ' + entityMap[entityName].toLowerCase()"
            icon="icon-add"
            :onclick="btnAddClick"
          ></BaseButtonIcon>
        </div>
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
          :api="`${$config.BASE_API}/${filterNames[index].split('Id')[0]}s`"
          :type="filterNames[index].split('Id')[0]"
          mode="1"
          @filterActive="activeFilter"
          @showToast="showToast"
          v-for="(f, index) in filterNames"
          :key="index"
        ></Combobox>

        <div class="more-feature-btn-wrap">
          <BaseButtonIcon
            v-for="(btn, i) in featureButtons" :key="i"
            :value="btn.Name"
            :icon="btn.Icon"
            :onclick="btn.Callback"
          ></BaseButtonIcon>
          <div class="button button-refresh" @click="refreshTableSelected">
            <i class="fas fa-sync-alt" style="transform: translate(0.5px, 0.5px);"></i>
          </div>
        </div>
      </div>

      <div class="table-wrap">
        <Table
          :type="entityName"
          :thead="thead"
          :dataMap="theadMap"
          :api="`${$config.BASE_API}/${entityName}s/${entityName.toLowerCase()}Filter?pageSize=${pageSize}&pageNumber=${pageNumber}&filterString=${
            searchInput + getApiFilterQuery()
          }`"
          @dataLoaded="isTableLoading = false"
          @rowDblClick="rowDoubleClick"
          @rowClick="rowSelect"
          :tableKey="tableFlag"
          :key="tableKey"
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
        :pageNumber="pageNumber"
        :pageSize="pageSize"
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
      :isOpen="formStatus"
      :close="closeForm"
      :mode="formMode"
      :detailId="entityId"
      :moreDetail="moreDetail"
      @saveClicked="formSaveButtonClick"
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
      :icon="popup.icon"
      @closePopup="closePopup"
      @popupCallbackFinish="finishPopupCallback"
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
import axios from 'axios';
import EventBus from '../../event-bus/EventBus';
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
    filterNames: {
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
    featureButtons: {
      type: Array,
      required: false
    }
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
      tableKey: false,
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
        icon: null,
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
  created() {
    EventBus.$on("showPopup", (args) => {
      this.showPopup(args);
    })
  },
  computed: {
    /**
     * Toggle trạng thái nút xóa nv theo số dòng được select
     * CreatedBy: HungNguyen81 (07-2021)
     */
    delBtnActive() {
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

    // chuyển trang => render bảng
    pageNumber: function () {
      this.forceTableRerender();
    }
  },
  methods: {
    //#region Xử lí liên quan đến Form

    /**
     * Mở form thông tin nv
     */
    // CreatedBy: HungNguyen81 (07-2021)
    // ModifiedBy: HungNguyen81 (18-08-2021)
    
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
          callback: this.closePopup,
        });
      }
    },

    /**
     * Đóng form
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    closeForm(isChange) {
      if(isChange){
        this.closeFormChanged();
      } else {
        this.formStatus = false;
        this.popup.isHide = true;
      }
    },

    /**
     * Đóng form mà k lưu
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (21-08-2021) - Đổi tên method
     */
    closeFormChanged() {
      this.popup = {
        title: "Xác nhận hủy",
        content: `Bạn có chắc chắn muốn <b>HỦY</b> nhập liệu hay không ?`,
        popupType: "warning",
        okAction: "Có",
        isHide: false,
        callback: this.closeForm,
        icon: null,
      };
    },
    //#endregion

    //#region Xử lý liên quan đến popup thông báo

    /**
     * Đóng popup
     * CreatedBy: HungNguyen81 (07-2021)
     */
    closePopup() {
      this.popup.isHide = true;
      EventBus.$emit('PopupClose');
    },
    /**
     * Hiển thị popup khi có callback
     * CreatedBy: HungNguyen81 (07-2021)
     */
    showPopup(options) {
      if (!options.callback) {
        options.callback = this.closePopup;
      }
      this.popup = options;
    },

    /**
     * Ẩn popup sau khi callback của nút OK chạy xong
     * CreatedBy: HungNguyen81 (07-2021)
     */
    finishPopupCallback(){
      this.popup.isHide = true;
    },
    //#endregion

    //#region Xử lý liên quan đến toast message
    /**
     * Hiển thị toast message
     * @param {String} type
     * @param {String} header
     * @param {String} msg
     * @param {Number} delay Thời gian trễ khi hiển thị toast, mặc định là 0
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
    //#endregion

    //#region Handle sự kiện emit
    /**
     * Handle khi client chọn filter từ combobox
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    activeFilter(type, id) {
      this.$set(this.filter, type + "Id", id);

      console.groupCollapsed("Filters");
      console.table(Object.assign({}, this.filter));
      console.groupEnd();

      this.refreshTableSelected();
    },

    /**
     * Handle khi chuyển trang trên paging component
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    onPageSizeChange(size) {
      this.pageSize = size;
      this.pageNumber = 0;
      this.refreshTableSelected();
    },

    onPageNumChange(num) {
      console.log("PAGE NUMBER: ", num);
      this.pageNumber = num;
    },

    /**
     * khi dữ liệu table load xong, gửi totalPage, totalRecord sang paging component
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    transPagingInfo(numPage, numRecord) {
      this.totalRecord = numRecord;
      this.totalPage = numPage;
    },

    //#endregion

    //#region xử lí sự kiện DOM
    /**
     * Hiện popup thông báo xác nhận thêm/sửa
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    formSaveButtonClick(mode, id, detail) {
      this.popup = {
        title: "Thông báo",
        content: `Bạn có chắc chắn muốn <b>${
          mode ? "sửa" : "thêm"
        }</b> nhân viên hay không ?`,
        popupType: "",
        okAction: "Lưu",
        isHide: false,
        icon: null,
        callback: mode ? this.sendPutRequest : this.sendPostRequest,
      };
      this.entityId = id;
      this.entityDetail = detail;
    },

    /**
     * Gọi hàm khi bấm nút Thêm nhân viên/ Thêm Khách hàng
     * CreatedBy: HungNguyen81 (07-2021)
     */
    btnAddClick() {
      this.OpenForm(0);
    },

    // Xử lí sự kiện click đúp vào table row
    // CreatedBy: HungNguyen81 (07-2021)
    rowDoubleClick(id, pos, dep) {
      this.entityId = id;
      this.moreDetail = {
        PositionName: pos,
        DepartmentName: dep,
      };
      console.log("id", id);
      this.OpenForm(1);
    },

    /**
     *  Xử lí khi select 1 table row
     * CreatedBy: HungNguyen81 (07-2021)
     */
    rowSelect(id, code, name) {
      if (this.deleteIdList.includes(id)) {
        let i = this.deleteIdList.indexOf(id);
        this.deleteIdList.splice(i, 1);
        this.deleteCodeList.splice(i, 1);
      } else {
        this.deleteIdList.push(id);
        this.deleteCodeList.push(name);
      }
    },

    /**
     * Xử lí sự kiện bấm nút xóa các hàng đã chọn trên table
     * CreatedBy: HungNguyen81 (07-2021)
     */
    delBtnClick() {
      this.showPopup({
        title: "Xác nhận xóa",
        content: `Bạn có chắc chắn muốn xóa "<b>${this.deleteCodeList.join(
          ", "
        )}</b>" hay không ?`,
        popupType: "error",
        okAction: "Xóa",
        isHide: false,
        icon: "icon-delete",
        callback: this.sendDeleteRequests,
      });
    },
    //#endregion

    //#region Requests
    /**
     * Callback khi bấm OK trong popup, gửi toàn bộ id cần xóa lên 1 request
     * CreatedBy: HungNguyen81 (07-2021)
     */
    sendDeleteRequests() {
      axios
        .delete(`${this.$config.BASE_API}/${this.entityName}s/`, {
          data: this.deleteIdList,
        })
        .then((res) => {
          this.deleteIdList = [];
          this.deleteCodeList = [];

          // Đóng popup và làm mới bảng
          this.closePopup();
          this.forceTableRerender();

          // hiển thị toast thông báo đã xóa thành công
          this.showToast("info", "DELETE successfully", res.data.Msg);
        })
        .catch(() => {
          this.showToast("error", "Delete error", `Xóa không thành công`);
        });
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode sửa nhân viên
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    sendPutRequest() {
      axios
        .put(
          `${this.$config.BASE_API}/${this.entityName}s/${this.entityId}1`,
          this.entityDetail
        )
        .then(() => {
          this.closePopup();
          this.closeForm();

          this.showToast(
            "success",
            "PUT Success",
            `Sửa thông tin của <b>"${this.entityDetail.FullName}"</b> thành công`
          );

          this.forceTableRerender();
        })
        .catch((err) => {
          this.showToast("error", "PUT error", err.response.data.Msg);
          this.closePopup();
        });
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode thêm nhân viên
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    sendPostRequest() {
      axios
        .post(
          `${this.$config.BASE_API}/${this.entityName}s/`,
          this.entityDetail
        )
        .then((res) => {
          this.showToast(
            "success",
            "POST success",
            res.data.Msg
          );
          this.closePopup();
          this.closeForm();
          this.forceTableRerender();
        })
        .catch((err) => {
          this.showToast(
            "error",
            "POST error",
            err.response.data.Msg
          );
          this.closePopup();
        });
    },
    //#endregion

    //#region Làm mới bảng dữ liệu
    /**
     * Refresh table và bỏ các dòng đã selected
     * CreatedBy: HungNguyen81 (07-2021)
     */
    refreshTableSelected() {
      localStorage.setItem("select", JSON.stringify([]));
      this.deleteIdList = [];
      this.deleteCodeList = [];
      this.forceTableRerender();
    },

    /**
     * Khiến table phải re-render
     * CreatedBy: HungNguyen81 (07-2021)
     */
    forceTableRerender() {
      this.isTableLoading = true;
      this.tableFlag = !this.tableFlag;
    },

    //#endregion

    /**
     * Lấy thông số filter cho api query
     * CreatedBy: HungNguyen81 (07-2021)
     */
    getApiFilterQuery() {
      var res = "";
      for (let i = 0; i < this.filterNames.length; i++) {
        res += `&${this.filterNames[i]}=${this.filter[this.filterNames[i]]}`;
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
