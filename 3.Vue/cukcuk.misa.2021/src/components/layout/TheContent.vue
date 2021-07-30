<template>
  <div>
    <div class="content">
      <div class="content-heading">
        <b class="title">Danh sách nhân viên</b>
        <!-- <div class="button" style="position: fixed" @click="ShowToast">click</div> -->
        <BaseButton
          value="Xóa nhân viên"
          type="button-delete"
          :onclick="delBtnClick"
          :class="{ hide: !delBtnActive }"
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

        <div class="button-refresh" @click="RefreshTable"></div>
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
            :typeData="'Name'"
            :value="this.pageSizeDropData[0].Name"
          ></Dropdown>
        </div>
      </div>
    </div>
    <Form
      :isOpen="this.formStatus"
      :close="this.CloseFormWithoutSave"
      :mode="action"
      @closeForm="this.CloseFormWithoutSave"
      :detailId="this.employeeId"
      :moreDetail="this.moreDetail"
      @saveClicked="this.FormSaveButtonClick"
    ></Form>

    <div id="loader" :class="{ hide: !isTableLoading }">
      <div class="spinner-wrapper">
        <div class="spinner"></div>
      </div>
    </div>

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
import BaseButton from "../base/BaseButton.vue";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import Dropdown from "../base/BaseDropdown.vue";
import Combobox from "../base/BaseCombobox.vue";
import Form from "../base/BaseForm.vue";
import Table from "../base/BaseTable.vue";
import Popup from "../base/BasePopup.vue";
import Toast from "../base/BaseToast.vue";

export default {
  name: "Content",
  components: {
    BaseButton,
    BaseButtonIcon,
    Combobox,
    Dropdown,
    Form,
    Table,
    Popup,
    Toast,
  },
  data() {
    return {
      tableSize: 0, // number of rows
      action: 0,
      employeeId: null,
      employeeDetail: null,
      moreDetail: null,
      deleteIdList: [],
      deleteCodeList: [],
      tableKey: 0,
      popup: {
        title: "Empty Title",
        content: "Empty Content",
        popupType: "error",
        okAction: "OK",
        isHide: true,
        callback: null,
      },
      pageSizeDropData: [
        { Name: "10 nhân viên/trang" },
        { Name: "20 nhân viên/trang" },
        { Name: "50 nhân viên/trang" },
        { Name: "100 nhân viên/trang" },
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
      isTableLoading: true,
      toasts: [],
    };
  },
  created() {},
  computed: {
    delBtnActive: function () {
      return this.deleteIdList.length > 0;
    },
  },
  methods: {
    OpenForm(act) {
      this.action = act;
      this.formStatus = true;
    },
    CloseForm() {
      this.formStatus = false;
      // this.employeeId = null;
      this.popup.isHide = true;
      // this.RefreshTable();
    },

    /**
     * Close form without display a popup, handle click event for Cancel and Close Form buttons
     */
    CloseFormWithoutSave() {
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
      // this.popup.callback = func;
    },
    ClosePopup() {
      this.popup.isHide = true;
    },

    /**
     * Show toast to notice to the client
     * params: {String}
     */
    ShowToast(type, header, msg, delay) {
      const Show = () => {
        this.toasts.push({
          type: type,
          header: header.toUpperCase(),
          msg: msg,
        });
      }
      if(delay){
        setTimeout(Show, delay);
      } else { 
        Show();
      }
    },

    /**
     * Display a popup, notice that the app is going to send put/post request to server
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
     * Callback for Form's Save Button, when edit employee
     */
    SendPutRequest() {
      this.axios
        .put(
          `http://cukcuk.manhnv.net/v1/Employees/${this.employeeId}`,
          this.employeeDetail
        )
        .then(() => {
          // if (res) console.log("da SUA thanh cong");
          this.ShowToast(
            "success",
            "PUT Success",
            `Sửa nhân viên "<b>${this.employeeDetail.FullName}</b>" thành công`
          );
          this.ClosePopup();
          this.CloseForm();
          this.RefreshTable();
        })
        .catch(() => {
          this.ShowToast(
            "error",
            "PUT error",
            `Sửa nhân viên "<b>${this.employeeDetail.FullName}</b>" không thành công`
          );
        });
    },

    /**
     * Callback for Form's Save Button, when add new employee
     */
    SendPostRequest() {
      this.axios
        .post(`http://cukcuk.manhnv.net/v1/Employees/`, this.employeeDetail)
        .then(() => {
          // if (res) console.log("da THEM thanh cong");
          this.ShowToast(
            "success",
            "POST success",
            `Thêm nhân viên "<b>${this.employeeDetail.FullName}</b>" thành công`
          );
          this.ClosePopup();
          this.CloseForm();
          this.RefreshTable();
        })
        .catch(() => {
          // console.log(err);
          this.ShowToast(
            "error",
            "POST error",
            `Thêm nhân viên "<b>${this.employeeDetail.FullName}</b>" không thành công`
          );
        });
    },

    /**
     * Make the table re-render and display loading spinner
     */
    RefreshTable() {
      this.isTableLoading = true;
      this.ForceTableRerender();
    },

    /**
     * Change page label when number of rows change
     */
    tableDataLoaded(size) {
      this.tableSize = size;
      this.isTableLoading = false;
    },

    BtnAddClick() {
      this.OpenForm(0);
    },

    rowDoubleClick(id, pos, dep) {
      console.log("row click", id);
      this.employeeId = id;
      this.moreDetail = {
        PositionName: pos,
        DepartmentName: dep,
      };
      this.OpenForm(1);
    },

    rowSelect(id, code) {
      if (this.deleteIdList.includes(id)) {
        let i = this.deleteIdList.indexOf(id);
        this.deleteIdList.splice(i, 1);
        this.deleteCodeList.splice(i, 1);
      } else {
        this.deleteIdList.push(id);
        this.deleteCodeList.push(code);
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
        callback: this.SendDeleteRequests,
      };
      this.OpenPopup();
    },

    /**
     * Callback for delete popup
     */
    SendDeleteRequests() {
      console.log("send delete");
      for (const [i, id] of this.deleteIdList.entries()) {        
        this.axios
          .delete(`http://cukcuk.manhnv.net/v1/Employees/${id}`)
          .then(() => {
            let index = this.deleteIdList.indexOf(id);
            this.deleteIdList.splice(index, 1);
            this.deleteCodeList.splice(index, 1);
            console.log(id);
            this.ClosePopup();
            this.RefreshTable();
            this.ShowToast("info", "DELETED", `Đã xóa "${id}"`, i*700);
          })
          .catch(() => {
            this.ShowToast(
              "error",
              "Delete error",
              `Xóa nhân viên "<b>${id}</b>" không thành công`, i*700
            );
          });
      }
    },

    /**
     * Force the table re-render
     */
    ForceTableRerender() {
      this.tableKey++;
    },
  },
};
</script>

<style scoped>
@import "../../css/layout/content.css";
@import "../../css/base/loader.css";
@import "../../css/components/popup.css";

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
