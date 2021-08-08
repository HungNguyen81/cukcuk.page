<template>
  <div
    :class="['container', { open: isOpen, close: !isOpen }]"
    id="container"
    @click="$emit('closeForm')"
  >
    <div
      :class="['form-container', { open: isOpen, close: !isOpen }]"
      id="form-container"
      @click.stop=""
    >
      <div class="form-header" id="form-container-header">
        <div class="header">THÔNG TIN NHÂN VIÊN</div>
        <div id="close-button" @click="this.close"></div>
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
            <BaseInput
              :valueType="'text'"
              type="input-form"
              id="employee-code"
              :tabindex="1"
              ref="employeeCode"
              v-model="detail.EmployeeCode"
              label="Mã nhân viên"
              :required="true"
              :validates="[required]"
              :renderFlag="isRerender"
              @valid="validate.employeeCode = true"
              @invalid="validate.employeeCode = false"
            />
            <BaseInput
              :valueType="'text'"
              type="input-form"
              id="fullname"
              tabindex="2"
              v-model="detail.FullName"
              label="Họ và tên"
              :required="true"
              ref="fullName"
              :validates="[required]"
              :renderFlag="isRerender"
              @valid="validate.fullName = true"
              @invalid="validate.fullName = false"
            />
          </div>
          <div class="input-row">
            <BaseInput
              :valueType="'date'"
              :type="'input-form'"
              id="dob"
              tabindex="3"
              :renderFlag="isRerender"
              v-model="detail.DateOfBirth"
              :label="'Ngày sinh'"
              @dateChange="dateChange"
              :validates="[date]"
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
                tabindex="4"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <BaseInput
              :valueType="'text'"
              type="input-form"
              id="identity-number"
              tabindex="5"
              v-model="detail.IdentityNumber"
              :label="'Số CMTND/ Căn cước'"
              :validates="[required]"
              ref="identityNumber"
              :renderFlag="isRerender"
              @valid="validate.identityNumber = true"
              @invalid="validate.identityNumber = false"
            />
            <BaseInput
              :valueType="'date'"
              type="input-form"
              id="identity-date"
              tabindex="6"
              v-model="detail.IdentityDate"
              :label="'Ngày cấp'"
              @dateChange="dateChange"
            />
          </div>
          <div class="input-row">
            <BaseInput
              :valueType="'text'"
              type="input-form"
              id="identity-place"
              tabindex="7"
              v-model="detail.IdentityPlace"
              :label="'Nơi cấp'"
            />
          </div>
          <div class="input-row">
            <BaseInput
              :valueType="'text'"
              type="input-form"
              id="email"
              tabindex="8"
              v-model="detail.Email"
              :label="'Email'"
              :validates="[required, email]"
              :renderFlag="isRerender"
              ref="email"
              :required="true"
              @valid="validate.email = true"
              @invalid="validate.email = false"
            />
            <BaseInput
              :valueType="'text'"
              type="input-form"
              id="phone-number"
              tabindex="9"
              v-model="detail.PhoneNumber"
              :label="'Số điện thoại'"
              :validates="[required, phone]"
              :renderFlag="isRerender"
              ref="phoneNumber"
              :required="true"
              @valid="validate.phoneNumber = true"
              @invalid="validate.phoneNumber = false"
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
                tabindex="10"
                :value="detail.PositionName"
                id="form-positions"
                :api="'https://localhost:44372/api/Positions'"
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
                tabindex="11"
                :value="detail.DepartmentName"
                id="form-departments"
                :api="'https://localhost:44372/api/Departments'"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <BaseInput
              :valueType="'text'"
              type="input-form"
              id="tax-code"
              tabindex="12"
              v-model="detail.PersonalTaxCode"
              :label="'Mã số thuế cá nhân'"
            />
            <BaseInput
              :valueType="'tel'"
              :type="'input-form input-salary'"
              id="salary"
              :tabindex="13"
              v-model="detail.Salary"
              :label="'Mức lương cơ bản'"
              @input="formatSalaryOnInput"
              ref="salary"
            />
            <div class="money-unit">(VNĐ)</div>
          </div>
          <div class="input-row">
            <BaseInput
              :valueType="'date'"
              type="input-form"
              id="join-date"
              tabindex="14"
              v-model="detail.JoinDate"
              :label="'Ngày gia nhập công ty'"
              @dateChange="dateChange"
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
                tabindex="15"
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
          :onclick="close"
          tabindex="16"
        ></BaseButtonIcon>
        <BaseButtonIcon
          :value="'Lưu'"
          :type="'button-save'"
          :icon="'icon-save'"
          :onclick="BtnSaveClick"
          tabindex="17"
        ></BaseButtonIcon>
      </div>
    </div>
  </div>
</template>

<script>
import ultis from "../../mixins/ultis";
import validate from "../../mixins/validate";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import BaseDropdown from "../base/BaseDropdown.vue";
import BaseInput from "../base/BaseInput.vue";

export default {
  name: "Form",
  components: {
    BaseButtonIcon,
    BaseDropdown,
    BaseInput,
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
      isDataLoaded: false,
      isRerender: false,
      validate: {
        employeeCode: false,
        fullName: false,
        identityNumber: false,
        email: false,
        phoneNumber: false,
      },
      validateRefs: [
        "employeeCode",
        "fullName",
        "identityNumber",
        "email",
        "phoneNumber",
      ],
      dateMap: {
        dob: "DateOfBirth",
        "identity-date": "IdentityDate",
        "join-date": "JoinDate",
      },
      dateName: {
        dob: "Ngày sinh",
        "identity-date": "Ngày cấp",
        "join-date": "Ngày gia nhập",
      },
    };
  },
  mounted() {},
  computed: {},
  watch: {
    isValidate: function (v) {
      console.log("v:", v);
    },
    /**
     * Watch sự thay đổi của isOpen để
     * - Auto focus vào trường mã nhân viên khi mở form
     * - Gọi API lấy thông tin đối tượng nếu form mode = 0 <sửa>
     */
    isOpen: function (val) {
      this.$nextTick(() => {
        if (val) this.$refs.employeeCode.$el.children[1].focus();
        if(this.mode == 0) this.detail = {};
      });

      this.isDataLoaded = false;
      this.isRerender = !this.isRerender;
      console.log("form " + (val ? "open" : "close"), this.mode);

      if (this.isOpen)
        // mode : 1 = thêm nv, mode : 0 = sửa nv <0 = không thêm>
        if (this.mode == 1 && this.detailId) {
          this.axios
            .get(`https://localhost:44372/api/Employees/${this.detailId}`)
            .then((res) => {
              this.detail = Object.assign({}, res.data);
              console.log(this.detail);
              this.FormatData();
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
              this.isDataLoaded = true;
            })
            .catch((err) => {
              console.log(err);
            });
        } else if (this.mode == 0) {
          this.isDataLoaded = true;
          this.axios
            .get("http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode")
            .then((res) => {
              this.$refs.employeeCode.$el.value = res.data;
              if(!res.data){
                this.$emit("showToast","error", "GET error", `Không thể lấy mã nhân viên mới !`);
              }
              this.$set(this.detail, "EmployeeCode", res.data);
            })
            .catch(() => {
              this.$emit("showToast","error", "GET error", `Không thể lấy mã nhân viên mới !`);
              let newCode = `NV-${Math.round(Math.random() * 100000)}`;
              this.$refs.employeeCode.$el.value = newCode;

              // Thay đổi giá trị bằng phép gán làm mất tính reactivity của component
              this.$set(this.detail, "EmployeeCode", newCode);
            });
        }
    },
  },
  methods: {
    /**
     * Handle mỗi khi date input value thay đổi
     */
    dateChange(key, val, input, formatedVal) {
      console.log(key, val, input);
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
     */
    FormatData() {
      this.$set(
        this.detail,
        "DateOfBirth",
        this.DateFormat(this.detail.DateOfBirth, true)
      );
      this.$set(
        this.detail,
        "IdentityDate",
        this.DateFormat(this.detail.IdentityDate, true)
      );
      this.$set(
        this.detail,
        "JoinDate",
        this.DateFormat(this.detail.JoinDate, true)
      );
      this.$set(
        this.detail,
        "WorkStatus",
        this.WorkStatusCode2Text(this.detail.WorkStatus)
      );
      this.$set(
        this.detail,
        "Salary",
        this.FormatMoneyString(this.detail.Salary)
      );
    },

    /**
     * Kiểm tra tính hợp lệ của các trường dữ liệu trên form
     */
    isValidate() {
      let res = true;
      Object.keys(this.validate).forEach((key) => {
        res = res && this.validate[key];
      });
      console.log("RES:", res);
      return res;
    },

    /**
     *
     * Định dạng tiền tệ trong khi nhập
     */
    formatSalaryOnInput() {
      let salaryInput = this.$refs.salary.$el.children[1];
      let selecStart = salaryInput.selectionStart - 1;
      let selecEnd = salaryInput.selectionEnd - 1;
      let oldLen = salaryInput.value.length - 1;
      this.$set(
        this.detail,
        "Salary",
        this.FormatMoneyString(this.detail.Salary)
      );

      // Giữ vị trí dấu nháy khi nhập (ko bị nhảy về cuối)
      this.$nextTick(() => {
        let offset = salaryInput.value.length - oldLen;
        salaryInput.setSelectionRange(selecStart + offset, selecEnd + offset);
      });
    },


    /**
     * Lấy dữ liệu thô để post/put
     */
    GetRawData() {
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
      res.WorkStatus = this.WorkStatusText2Code(this.detail.WorkStatus);
      res.Gender = this.GenderText2Code(this.detail.GenderName);
      res.Salary = salary == null ? null : Number(salary.replaceAll(".", ""));
      return res;
    },

    /**
     * Handle khi click nút lưu
     */
    BtnSaveClick() {
      console.log("save", this.detail);

      for(let ref of this.validateRefs){
        this.$refs[ref].inputValidate();
      }
      if (!this.isValidate()) {
        this.$emit('showToast', 'warning', 'NOT VALIDATE', `Dữ liệu không hợp lệ !`);
        this.$emit("showPopup", {
          title: "Thông báo",
          content: `Dữ liệu không hợp lệ, vui lòng nhập lại`,
          popupType: "warning",
          okAction: "OK",
          isHide: false,
          callback: null,
        });
        return;
      }
      console.log("EMIT");
      this.$emit("saveClicked", this.mode, this.detailId, this.GetRawData());
    },

    /**
     * Khi select trong dropdown, chỉ có DepartmentName thay đổi mà DepartmentId không thay đổi
     * Tương tự với PositionId
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
    emitShowToast(type, header, msg) {
      this.$emit("showToast", type, header, msg);
    },
  },
};
</script>

<style scoped>
@import "../../css/components/popup-form.css";
</style>