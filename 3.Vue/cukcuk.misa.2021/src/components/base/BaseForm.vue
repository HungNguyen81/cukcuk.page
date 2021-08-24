<template>
  <div
    :class="['container', { open: isOpen, close: !isOpen }]"
    id="container"
    @click="closeForm"
  >
    <div
      :class="['form-container', { open: isOpen, close: !isOpen }]"
      id="form-container"
      @click.stop=""
    >
      <div class="form-header" id="form-container-header">
        <div class="header">THÔNG TIN NHÂN VIÊN</div>
        <div id="close-button" @click="closeForm"></div>
      </div>
      <div class="form-body">
        <div class="body-avatar body-column">
          <div class="avatar"></div>
          <div class="note">
            (Vui lòng chọn ảnh có định dạng <br />.jpg, .jpeg, .png, .gif)
          </div>
        </div>
        <div class="body-form body-column">
          <div class="header-row">
            <div class="header-2">
              A. THÔNG TIN CHUNG
              <div class="header-line"></div>
            </div>
          </div>
          <div class="input-row">
            <BaseTextInput
              :valueType="'text'"
              type="input-form"
              inputKey="employee-code"
              :tabindex="1"
              ref="employeeCode"
              v-model="detail.EmployeeCode"
              label="Mã nhân viên"
              :required="true"
              :validates="[required]"
              :rerenderFlag="isRerender"
              @valid="validateForm"
            />
            <BaseTextInput
              :valueType="'text'"
              type="input-form"
              inputKey="fullname"
              :tabindex="2"
              v-model="detail.FullName"
              label="Họ và tên"
              :required="true"
              ref="fullName"
              :validates="[required]"
              :rerenderFlag="isRerender"
              @valid="validateForm"
            />
          </div>
          <div class="input-row">
            <BaseDateInput
              :type="'input-form'"
              inputKey="date-of-birth"
              ref="dateOfBirth"
              :tabindex="3"
              v-model="detail.DateOfBirth"
              :label="'Ngày sinh'"
              :validates="[date]"
              :rerenderFlag="isRerender"
              @valid="validateForm"
              @dateChange="dateChange"
            />
            <div class="input-field">
              <div class="input-label">Giới tính</div>
              <BaseDropdown
                :direction="'down'"
                :data="[
                  { GenderName: 'Nam' },
                  { GenderName: 'Nữ' },
                  { GenderName: 'Không xác định' },
                ]"
                :type="'form-dropdown'"
                :displayId="'gender'"
                :value="detail.GenderName"
                :typeData="'GenderName'"
                :tabindex="4"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <BaseTextInput
              :valueType="'text'"
              type="input-form"
              inputKey="identity-number"
              :tabindex="5"
              v-model="detail.IdentityNumber"
              :label="'Số CMTND/ Căn cước'"
              :validates="[required]"
              :required="true"
              :rerenderFlag="isRerender"
              ref="identityNumber"
              @valid="validateForm"
            />
            <BaseDateInput
              type="input-form"
              inputKey="identity-date"
              ref="identityDate"
              :tabindex="6"
              v-model="detail.IdentityDate"
              :label="'Ngày cấp'"
              :validates="[date]"
              :rerenderFlag="isRerender"
              @dateChange="dateChange"
              @valid="validateForm"
            />
          </div>
          <div class="input-row">
            <BaseTextInput
              :valueType="'text'"
              type="input-form"
              id="identity-place"
              :tabindex="7"
              v-model="detail.IdentityPlace"
              :label="'Nơi cấp'"
              :rerenderFlag="isRerender"
            />
          </div>
          <div class="input-row">
            <BaseTextInput
              :valueType="'text'"
              type="input-form"
              inputKey="email"
              :tabindex="8"
              v-model="detail.Email"
              :label="'Email'"
              :validates="[required, email]"
              ref="email"
              :required="true"
              :rerenderFlag="isRerender"
              @valid="validateForm"
            />
            <BaseTextInput
              :valueType="'text'"
              type="input-form"
              inputKey="phone-number"
              :tabindex="9"
              v-model="detail.PhoneNumber"
              :label="'Số điện thoại'"
              :validates="[required, phone]"
              ref="phoneNumber"
              :required="true"
              :rerenderFlag="isRerender"
              @valid="validateForm"
            />
          </div>
          <div class="header-row">
            <div class="header-2">
              B. THÔNG TIN CÔNG VIỆC
              <div class="header-line"></div>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Vị trí</div>
              <BaseDropdown
                :direction="'down'"
                :type="'form-dropdown'"
                :typeData="'PositionName'"
                :displayId="'position-name'"
                :tabindex="10"
                :value="detail.PositionName"
                id="form-positions"
                :api="`${$config.BASE_API}/Positions`"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
                @showToast="emitShowToast"
              ></BaseDropdown>
            </div>
            <div class="input-field">
              <div class="input-label">Phòng ban</div>
              <BaseDropdown
                :direction="'down'"
                :type="'form-dropdown'"
                :typeData="'DepartmentName'"
                :displayId="'department-name'"
                :tabindex="11"
                :value="detail.DepartmentName"
                id="form-departments"
                :api="`${$config.BASE_API}/Departments`"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <BaseTextInput
              :valueType="'text'"
              type="input-form"
              inputKey="tax-code"
              :tabindex="12"
              v-model="detail.PersonalTaxCode"
              :rerenderFlag="isRerender"
              :label="'Mã số thuế cá nhân'"
            />
            <BaseTextInput
              :valueType="'tel'"
              :type="'input-form input-salary'"
              inputKey="salary"
              :tabindex="13"
              v-model="detail.Salary"
              :label="'Mức lương cơ bản'"
              :rerenderFlag="isRerender"
              @input="formatSalaryOnInput"
              ref="salary"
            />
            <div class="money-unit">(VNĐ)</div>
          </div>
          <div class="input-row">
            <BaseDateInput
              type="input-form"
              inputKey="join-date"
              ref="joinDate"
              :tabindex="14"
              v-model="detail.JoinDate"
              :label="'Ngày gia nhập công ty'"
              :validates="[date]"
              :rerenderFlag="isRerender"
              @dateChange="dateChange"
              @valid="validateForm"
            />
            <div class="input-field">
              <div class="input-label">Tình trạng công việc</div>
              <BaseDropdown
                :direction="'down'"
                :data="[
                  { WorkStatus: 'Đang làm việc' },
                  { WorkStatus: 'Đang thử việc' },
                  { WorkStatus: 'Sắp nghỉ việc' },
                ]"
                :type="'form-dropdown'"
                :typeData="'WorkStatus'"
                :displayId="'work-status'"
                :value="String(detail.WorkStatus)"
                :tabindex="15"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
              ></BaseDropdown>
            </div>
          </div>
        </div>
      </div>

      <div class="form-footer">
        <BaseButtonIcon
          :value="'Hủy'"
          :type="'button-cancel'"
          :onclick="closeForm"
          tabindex="16"
        ></BaseButtonIcon>
        <BaseButtonIcon
          :value="'Lưu'"
          :type="'button-save'"
          :icon="'icon-save'"
          :onclick="btnSaveClick"
          :disable="isDisableSaveButton"
          tabindex="17"
        ></BaseButtonIcon>
      </div>
    </div>
  </div>
</template>

<script>
import EventBus from '../../event-bus/EventBus';
import axios from "axios";
import ultis from "../../mixins/ultis";
import validate from "../../mixins/validate";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import BaseDropdown from "../base/BaseDropdown.vue";
import BaseTextInput from "../base/BaseTextInput.vue";
import BaseDateInput from "../base/BaseDateInput.vue";

export default {
  name: "Form",
  components: {
    BaseButtonIcon,
    BaseDropdown,
    BaseTextInput,
    BaseDateInput,
  },
  mixins: [ultis, validate],
  props: {
    isOpen: {
      type: Boolean,
      required: true,
    },
    close: {
      type: Function,
      require: false,
    },
    mode: {
      type: Number,
      required: false,
      default() {
        return 0;
      }, // 0: For POST action, 1: For PUT action
    },
    detailId: {
      type: String,
      required: false,
    },
    moreDetail: {
      type: Object,
      required: false,
    },
  },
  data() {
    return {
      detail: {},
      initDetail: {},
      isDetailChange: false,
      isDataLoaded: false,
      isRerender: false,
      isDisableSaveButton: false,
      validate: {
        "employee-code": false,
        fullname: false,
        email: false,
        "identity-number": false,
        "phone-number": false,
        "date-of-birth": false,
        "identity-date": false,
        "join-date": false,
      },
      validateRefs: [
        "employeeCode",
        "fullName",
        "identityNumber",
        "email",
        "phoneNumber",
        "dateOfBirth",
        "identityDate",
        "joinDate",
      ],
      dateMap: {
        "date-of-birth": "DateOfBirth",
        "identity-date": "IdentityDate",
        "join-date": "JoinDate",
      },
      dateName: {
        "date-of-birth": "Ngày sinh",
        "identity-date": "Ngày cấp",
        "join-date": "Ngày gia nhập",
      },
      dateInputFormat: "yyyy-mm-dd"
    };
  },
  mounted() {},
  computed: {
    isChange: function() {
      return (
        Object.entries(this.initDetail).toString() !=
        Object.entries(this.detail).toString()
      );
    },
  },
  watch: {
    isValidate: function (v) {
      console.log("v:", v);
    },

    // detail: function(d){
    //   this.isDetailChange = this.initDetail != d;
    // },

    /**
     * Watch sự thay đổi của isOpen để
     * - Auto focus vào trường mã nhân viên khi mở form
     * - Gọi API lấy thông tin đối tượng nếu form mode = 0 <sửa>
     * CreatedBy: HungNguyen81 (07-2021)
     */
    isOpen: function (val) {
      this.$nextTick(() => {
        if (val) this.$refs.employeeCode.$el.children[1].focus();
        if (this.mode == 0) {
          this.detail = {};
          this.initDetail = {};
        }
        this.isDisableSaveButton = false;
      });

      this.isDataLoaded = false;
      this.isRerender = !this.isRerender;
      console.log("form " + (val ? "open" : "close"), this.mode);

      if (this.isOpen) {
        console.log(
          "open",
          this.mode,
          this.detailId,
          `${this.$config.BASE_API}/Employees/${this.detailId}`
        );
        // mode : 0 = thêm nv, mode : 1 = sửa nv
        if (this.mode == 1 && this.detailId) {
          axios
            .get(`${this.$config.BASE_API}/Employees/${this.detailId}`)
            .then((res) => {
              if (res.data.IsValid === false) {
                this.$emit("showToast", "warning", "NO Content", res.data.Msg);
                return;
              }

              this.detail = Object.assign({}, res.data);

              console.groupCollapsed("Data form");
              console.table(res.data);
              console.groupEnd();

              this.formatData();
              this.$set(
                this.detail,
                "PositionName",
                this.moreDetail.PositionName
              );
              this.$set(
                this.detail,
                "DepartmentName",
                this.moreDetail.DepartmentName
              );
              this.initDetail = Object.assign({}, this.detail);
              this.isDataLoaded = true;
            })
            .catch((err) => {
              console.log(err);
            });
        } else if (this.mode == 0) {
          this.isDataLoaded = true;
          axios
            .get(`${this.$config.BASE_API}/Employees/NewEmployeeCode`)
            .then((res) => {
              let newCode = res.data.Data;
              this.$refs.employeeCode.$el.value = newCode;
              if (!newCode) {
                this.$emit(
                  "showToast",
                  "error",
                  "GET error",
                  `Không thể lấy mã nhân viên mới !`
                );
              }
              this.$set(this.detail, "EmployeeCode", newCode);

              this.initDetail = Object.assign({}, this.detail);
            })
            .catch(() => {
              this.$emit(
                "showToast",
                "error",
                "GET error",
                `Không thể lấy mã nhân viên mới !`
              );
              let newCode = `NV${Math.round(Math.random() * 100000)}`;
              this.$refs.employeeCode.$el.value = newCode;
              this.$set(this.detail, "EmployeeCode", newCode);

              this.initDetail = Object.assign({}, this.detail);
            });
        }
      }
    },
  },
  methods: {
    closeForm() {
      this.close(this.isChange);
    },

    /**
     * Handle mỗi khi date input value thay đổi
     * CreatedBy: HungNguyen81 (07-2021)
     */
    dateChange(key, val, input, formatedVal) {
      let keyName = this.dateMap[key];
      let oldVal = this.detail[keyName];
      if (oldVal != val && val) {
        // validate định dạng ngày
        !this.date(this.dateName[key], formatedVal);

        let start = input.selectionStart;
        this.$set(this.detail, keyName, val);
        this.$nextTick(() => {
          input.setSelectionRange(start + 1, start + 1);
        });
      }
    },

    /**
     * Format dữ liệu để hiển thị
     * CreatedBy: HungNguyen81 (07-2021)
     */
    formatData() {
      this.$set(
        this.detail,
        "DateOfBirth",
        this.dateFormatVer2(this.detail.DateOfBirth, this.dateInputFormat)
      );
      this.$set(
        this.detail,
        "IdentityDate",
        this.dateFormatVer2(this.detail.IdentityDate, this.dateInputFormat)
      );
      this.$set(
        this.detail,
        "JoinDate",
        this.dateFormatVer2(this.detail.JoinDate, this.dateInputFormat)
      );
      this.$set(
        this.detail,
        "WorkStatus",
        this.workStatusCode2Text(this.detail.WorkStatus)
      );
      this.$set(
        this.detail,
        "Salary",
        this.formatMoneyString(this.detail.Salary)
      );
    },

    /**
     * Kiểm tra tính hợp lệ của các trường dữ liệu trên form
     * CreatedBy: HungNguyen81 (07-2021)
     */
    isValidate() {
      let res = true;
      Object.keys(this.validate).forEach((key) => {
        res = res && this.validate[key];
      });
      console.groupCollapsed("Validate info");
      console.table(JSON.parse(JSON.stringify(this.validate)));
      console.groupEnd();
      return res;
    },

    /**
     *
     * Định dạng tiền tệ trong khi nhập
     * CreatedBy: HungNguyen81 (07-2021)
     */
    formatSalaryOnInput() {
      let salaryInput = this.$refs.salary.$el.children[1];
      let selecStart = salaryInput.selectionStart - 1;
      let selecEnd = salaryInput.selectionEnd - 1;
      let oldLen = salaryInput.value.length - 1;
      this.$set(
        this.detail,
        "Salary",
        this.formatMoneyString(this.detail.Salary)
      );

      // Giữ vị trí dấu nháy khi nhập (ko bị nhảy về cuối)
      this.$nextTick(() => {
        let offset = salaryInput.value.length - oldLen;
        salaryInput.setSelectionRange(selecStart + offset, selecEnd + offset);
      });
    },

    /**
     * Lấy dữ liệu thô để post/put
     * CreatedBy: HungNguyen81 (07-2021)
     */
    getRawData() {
      let dob = this.detail.DateOfBirth;
      let identityDate = this.detail.IdentityDate;
      let joinDate = this.detail.JoinDate;
      let salary = this.detail.Salary;
      let res = JSON.parse(JSON.stringify(this.detail));

      res.DateOfBirth = dob ? new Date(dob).toISOString() : null;
      res.IdentityDate = identityDate
        ? new Date(identityDate).toISOString()
        : null;
      res.JoinDate = joinDate ? new Date(joinDate).toISOString() : null;
      res.WorkStatus = this.workStatusText2Code(this.detail.WorkStatus);
      res.Gender = this.genderText2Code(this.detail.GenderName);
      res.Salary = salary == null ? null : Number(salary.replaceAll(".", ""));
      return res;
    },

    /**
     * Handle khi click nút lưu
     * CreatedBy: HungNguyen81 (07-2021)
     */
    btnSaveClick() {
      this.isDisableSaveButton = true;
      for (let ref of this.validateRefs) {
        this.$refs[ref].inputValidate();
      }
      if (!this.isValidate()) {
        this.$emit(
          "showToast",
          "warning",
          "NOT VALIDATE",
          `Dữ liệu không hợp lệ !`
        );
        this.$emit("showPopup", {
          title: "Thông báo",
          content: `Dữ liệu không hợp lệ, vui lòng nhập lại`,
          popupType: "warning",
          okAction: "OK",
          isHide: false,
          callback: null,
        });
        this.isDisableSaveButton = false;
        return;
      }
      if(this.isChange){
        this.isDisableSaveButton = true;
        this.$emit("saveClicked", this.mode, this.detailId, this.getRawData());
        EventBus.$on('PopupClose', () => {
          this.isDisableSaveButton = false;
        })
      } else {
        this.closeForm(this.isChange);
      }
    },

    /**
     * Khi select trong dropdown, chỉ có DepartmentName thay đổi mà DepartmentId không thay đổi
     * Tương tự với PositionId
     * CreatedBy: HungNguyen81 (07-2021)
     */
    dropDataChange(typeName, obj) {
      this.$set(this.detail, typeName, obj[typeName]);

      if (obj.DepartmentId) {
        this.$set(this.detail, "DepartmentId", obj.DepartmentId);
      }
      if (obj.PositionId) {
        this.$set(this.detail, "PositionId", obj.PositionId);
      }
    },

    /**
     * Emit sự kiện showToast cho parent(base content)
     * CreatedBy: HungNguyen81 (07-2021)
     */
    emitShowToast(type, header, msg) {
      this.$emit("showToast", type, header, msg);
    },

    /**
     * Handle khi validate input
     * CreatedBy: HungNguyen81 (07-2021)
     */
    validateForm(key, isValid) {
      this.$set(this.validate, key, isValid);
    },
  },
};
</script>

<style scoped>
@import "../../css/components/popup-form.css";
</style>