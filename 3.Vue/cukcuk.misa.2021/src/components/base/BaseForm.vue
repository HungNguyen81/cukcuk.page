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
            <div class="input-field">
              <div class="input-label">
                Mã nhân viên (<span class="required">*</span>)
              </div>
              <BaseInput
                :valueType="'text'"
                type="input-form"
                id="employee-code"
                tabindex="1"
                ref="employeeCode"
                v-model="detail.EmployeeCode"
                label="Mã nhân viên"
                :validates="[required]"
                :renderFlag="isRerender"
              />
            </div>
            <div class="input-field">
              <div class="input-label">
                Họ và tên (<span class="required">*</span>)
              </div>
              <BaseInput
                :valueType="'text'"
                type="input-form"
                id="fullname"
                tabindex="2"
                v-model="detail.FullName"
                label="Họ và tên"
                :validates="[required]"
                :renderFlag="isRerender"
              />
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Ngày sinh</div>
              <BaseInput
                :valueType="'date'"
                class="input-form"
                id="dob"
                tabindex="3"
                v-model="detail.DateOfBirth"
                :label="'Ngày sinh'"
              />
            </div>
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
            <div class="input-field">
              <div class="input-label">
                Số CMTND/ Căn cước (<span class="required">*</span>)
              </div>
              <BaseInput
                :valueType="'text'"
                class="input-form"
                id="identity-number"
                tabindex="5"
                v-model="detail.IdentityNumber"
                :label="'Số CMTND/ Căn cước'"
                :validates="[required]"
                :renderFlag="isRerender"
              />
            </div>
            <div class="input-field">
              <div class="input-label">Ngày cấp</div>
              <BaseInput
                :valueType="'date'"
                class="input-form"
                id="identity-date"
                tabindex="6"
                v-model="detail.IdentityDate"
                :label="'Ngày cấp'"
              />
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Nơi cấp</div>
              <BaseInput
                :valueType="'text'"
                class="input-form"
                id="identity-place"
                tabindex="7"
                v-model="detail.IdentityPlace"
                :label="'Nơi cấp'"
              />
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">
                Email (<span class="required">*</span>)
              </div>
              <BaseInput
                :valueType="'text'"
                class="input-form"
                id="email"
                tabindex="8"
                v-model="detail.Email"
                :label="'Email'"
                :validates="[required]"
                :renderFlag="isRerender"
              />
            </div>
            <div class="input-field">
              <div class="input-label">
                Số điện thoại (<span class="required">*</span>)
              </div>
              <BaseInput
                :valueType="'text'"
                class="input-form"
                id="phone-number"
                tabindex="9"
                v-model="detail.PhoneNumber"
                :label="'Số điện thoại'"
                :validates="[required]"
                :renderFlag="isRerender"
              />
            </div>
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
                :api="'http://cukcuk.manhnv.net/v1/Positions'"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
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
                :api="'http://cukcuk.manhnv.net/api/Department'"
                v-if="isDataLoaded"
                @itemChange="dropDataChange"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Mã số thuế cá nhân</div>
              <BaseInput
                :valueType="'text'"
                class="input-form"
                id="tax-code"
                tabindex="12"
                v-model="detail.PersonalTaxCode"
                :label="'Mã số thuế cá nhân'"
              />
            </div>
            <div class="input-field">
              <div class="input-label">Mức lương cơ bản</div>
              <BaseInput
                :valueType="'text'"
                class="textbox-default input-form input-salary"
                id="salary"
                tabindex="13"
                v-model="detail.Salary"
                :label="'Mức lương cơ bản'"
                @input="formatSalaryOnInput"
                ref="salary"
              />
            </div>
            <div class="money-unit">(VNĐ)</div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Ngày gia nhập công ty</div>
              <BaseInput
                :valueType="'date'"
                class="input-form"
                id="join-date"
                tabindex="14"
                v-model="detail.JoinDate"
                :label="'Ngày gia nhập công ty'"
              />
            </div>
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
          :onclick="this.close"
          tabindex="16"
        ></BaseButtonIcon>
        <BaseButtonIcon
          :value="'Lưu'"
          :type="'button-save'"
          :icon="'icon-save'"
          :onclick="this.BtnSaveClick"
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
import BaseInput from '../base/BaseInput.vue';

export default {
  name: "Form",
  components: {    
    BaseButtonIcon,
    BaseDropdown,
    BaseInput
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
      default(){
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
      isRerender: false
    };
  },
  mounted() {},
  computed: { },
  watch: {
    /**
     * Set auto focus on employee Code input field when open form
     */
    isOpen: function (val) {
      this.$nextTick(() => {
        if (val) this.$refs.employeeCode.$el.focus();
      });
      
      this.isDataLoaded = false;
      this.isRerender = !this.isRerender;
      console.log("form " + (val ? "open" : "close"), this.mode);

      if (this.isOpen) 
      if (this.mode == 1 && this.detailId) {
        this.axios
          .get(`http://cukcuk.manhnv.net/v1/Employees/${this.detailId}`)
          .then((res) => {
            this.detail = Object.assign({}, res.data);
            this.FormatData();
            this.$set(this.detail, 'PositionName', this.moreDetail.PositionName);
            this.$set(this.detail, 'DepartmentName', this.moreDetail.DepartmentName)
            this.isDataLoaded = true;
            // console.log(this.detail);
          })
          .catch((err) => {
            console.log(err);
          });
      } else if (this.mode == 0) {
        this.detail = Object.assign({});
        this.isDataLoaded = true;
        this.axios
          .get("http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode")
          .then((res) => {            
            this.$refs.employeeCode.$el.value = res.data;            
            this.$set(this.detail, 'EmployeeCode', res.data)
          })
          .catch(() => {
            this.$emit('getNewCodeError');
            let newCode = `NV-${Math.round(Math.random()*100000)}`
            this.$refs.employeeCode.$el.value = newCode;

            // Thay đổi giá trị bằng phép gán làm mất tính reactivity của component
            this.$set(this.detail, 'EmployeeCode', newCode)

          });
      }
    },    
  },
  methods: {
    FormatData() {
      this.$set(this.detail, 'DateOfBirth', this.DateFormat(this.detail.DateOfBirth, true));
      this.$set(this.detail, 'IdentityDate', this.DateFormat(this.detail.IdentityDate,true));
      this.$set(this.detail, 'JoinDate', this.DateFormat(this.detail.JoinDate, true));
      this.$set(this.detail, 'WorkStatus', this.WorkStatusCode2Text(this.detail.WorkStatus));
      this.$set(this.detail, 'Salary', this.FormatMoneyString(this.detail.Salary));
    },

    formatSalaryOnInput(){
      let salaryInput = this.$refs.salary.$el;
      let selecStart = salaryInput.selectionStart;
      let selecEnd = salaryInput.selectionEnd;
      this.$set(this.detail, 'Salary', this.FormatMoneyString(this.detail.Salary));
      console.log(this.$refs.salary.$el.selectionEnd);
      this.$nextTick(() => {
        let offset = (salaryInput.value.replaceAll('.', '').length % 3 == 1)? 1: 0;        
        salaryInput.setSelectionRange(selecStart + offset, selecEnd + offset);
      });
    },

    GetRawData() {

      let dob           = this.detail.DateOfBirth;
      let identityDate  = this.detail.IdentityDate;
      let joinDate      = this.detail.JoinDate;
      let salary        = this.detail.Salary;
      let res           = JSON.parse(JSON.stringify(this.detail));

      res.DateOfBirth   = dob         ? new Date(dob).toISOString()           :null;
      res.IdentityDate  = identityDate? new Date(identityDate).toISOString()  :null;
      res.JoinDate      = joinDate    ? new Date(joinDate).toISOString()      :null;
      res.WorkStatus    = this.WorkStatusText2Code(this.detail.WorkStatus);
      res.Gender        = this.GenderText2Code(this.detail.GenderName);      
      res.Salary        = salary==null? null : Number(salary.replaceAll(".", ""));
      return res;
    },

    BtnSaveClick() {
      this.$emit("saveClicked", this.mode, this.detailId, this.GetRawData());
    },

    /**
     * Make Department Id, Position Id sync with its names
     */
    dropDataChange(typeName, obj) {
      this.$set(this.detail, typeName, obj[typeName])

      if (obj.DepartmentId) {
        this.$set(this.detail, 'DepartmentId', obj.DepartmentId);
      }
      if (obj.PositionId) {
        this.$set(this.detail, 'PositionId', obj.PositionId);
      }
    },
  },
};
</script>

<style scoped>
@import "../../css/components/popup-form.css";
</style>