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
              <input
                :type="'text'"
                class="textbox-default input-form"
                id="employee-code"
                :value="'NV000001'"
                tabindex="1"
                ref="employeeCode"
                v-model="detail.EmployeeCode"
              />
            </div>
            <div class="input-field">
              <div class="input-label">
                Họ và tên (<span class="required">*</span>)
              </div>
              <input
                :type="'text'"
                class="textbox-default input-form"
                id="fullname"
                :value="'Nguyễn Ngọc Hưng'"
                tabindex="2"
                v-model="detail.FullName"
              />
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Ngày sinh</div>
              <input
                :type="'date'"
                class="textbox-default input-form input-form"
                id="dob"
                :value="'2000-01-08'"
                tabindex="3"
                v-model="detail.DateOfBirth"
              />
            </div>
            <div class="input-field">
              <div class="input-label">Giới tính</div>
              <BaseDropdown
                :direction="'down'"
                :data="[{Name:'Nam'}, {Name:'Nữ'}, {Name:'Không xác định'}]"
                :type="'form-dropdown'"
                :displayId="'gender'"
                :currProp="detail.GenderName"
                tabindex="4"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">
                Số CMTND/ Căn cước (<span class="required">*</span>)
              </div>
              <input
                :type="'text'"
                class="textbox-default input-form"
                id="identity-number"
                :value="'038200009999'"
                tabindex="5"
                v-model="detail.IdentityNumber"
              />
            </div>
            <div class="input-field">
              <div class="input-label">Ngày cấp</div>
              <input
                :type="'date'"
                class="textbox-default input-form"
                id="identity-date"
                :value="'2020-01-08'"
                tabindex="6"
                v-model="detail.IdentityDate"
              />
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Nơi cấp</div>
              <input
                :type="'text'"
                class="textbox-default input-form"
                id="identity-place"
                :value="'Tỉnh Thanh Hóa'"
                tabindex="7"
                v-model="detail.IdentityPlace"
              />
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">
                Email (<span class="required">*</span>)
              </div>
              <input
                :type="'text'"
                class="textbox-default input-form"
                id="email"
                :value="'nguyenngochung.ncth@gmail.com'"
                tabindex="8"
                v-model="detail.Email"
              />
            </div>
            <div class="input-field">
              <div class="input-label">
                Số điện thoại (<span class="required">*</span>)
              </div>
              <input
                :type="'text'"
                class="textbox-default input-form"
                id="phone-number"
                :value="'0334004655'"
                tabindex="9"
                v-model="detail.PhoneNumber"
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
                :typeData="'Position'"
                :displayId="'position-name'"
                tabindex="10"
                :currProp="detail.PositionName"
                id="form-positions"
                :api="'http://cukcuk.manhnv.net/v1/Positions'"
                v-if="isDataLoaded"
              ></BaseDropdown>
            </div>
            <div class="input-field">
              <div class="input-label">Phòng ban</div>
              <BaseDropdown
                :direction="'down'"
                :type="'form-dropdown'"
                :typeData="'Department'"
                :displayId="'department-name'"
                tabindex="11"
                :currProp="detail.DepartmentName"
                id="form-departments"
                :api="'http://cukcuk.manhnv.net/api/Department'"
                v-if="isDataLoaded"
                @dropSelect="selectDepartment"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Mã số thuế cá nhân</div>
              <input
                :type="'text'"
                class="textbox-default input-form"
                id="tax-code"
                :value="'123456789'"
                tabindex="12"
                v-model="detail.PersonalTaxCode"
              />
            </div>
            <div class="input-field">
              <div class="input-label">Mức lương cơ bản</div>
              <input
                :type="'text'"
                class="textbox-default input-form input-salary"
                id="salary"
                :value="'100.000.000'"
                tabindex="13"
                v-model="detail.Salary"
              />
            </div>
            <div class="money-unit">(VNĐ)</div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Ngày gia nhập công ty</div>
              <input
                :type="'date'"
                class="textbox-default input-form"
                id="join-date"
                :value="'2021-07-12'"
                tabindex="14"
              />
            </div>
            <div class="input-field">
              <div class="input-label">Tình trạng công việc</div>
              <BaseDropdown
                :direction="'down'"
                :data="['Đang làm việc', 'Đang thử việc', 'Sắp nghỉ việc']"
                :type="'form-dropdown'"
                :displayId="'work-status'"
                :currProp="String(detail.WorkStatus)"
                tabindex="15"
              ></BaseDropdown>
            </div>
          </div>
        </div>
      </div>

      <div class="form-footer">
        <BaseButton
          :value="'Hủy'"
          :type="'button-cancel'"
          :onclick="this.close"
        ></BaseButton>
        <BaseButtonIcon
          :value="'Lưu'"
          :type="'button-save'"
          :icon="'icon-save'"
          :onclick="this.BtnSaveClick"
        ></BaseButtonIcon>
      </div>
    </div>
  </div>
</template>

<script>
import ultis from "../../mixins/ultis";
import BaseButton from "../base/BaseButton.vue";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import BaseDropdown from "../base/BaseDropdown.vue";

export default {
  name: "Form",
  components: {
    BaseButton,
    BaseButtonIcon,
    BaseDropdown,
  },
  mixins: [ultis],
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
      default: 0, // 0: For POST action, 1: For PUT action
    },
    // detailId = id of employee / customer
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
      isDataLoaded: false
    };
  },
  mounted() {},
  computed: {},
  watch: {
    /**
     * Set auto focus on employee Code input field when open form
     */
    isOpen: function () {
      this.$nextTick(() => {
        if (this.isOpen) this.$refs.employeeCode.focus();
      });
      this.isDataLoaded = false;
      console.log("form " + (this.isOpen ? "open" : "close"));
      if (this.detailId) {
        this.axios
          .get(`http://cukcuk.manhnv.net/v1/Employees/${this.detailId}`)
          .then((res) => {
            this.detail = res.data;
            this.FormatData();
            this.detail.PositionName = this.moreDetail.PositionName;
            this.detail.DepartmentName = this.moreDetail.DepartmentName;
            this.isDataLoaded = true;
            // console.log("res", this.detail);
          })
          .catch((err) => {
            console.log(err);
          });
      } else {
        this.detail = {};
      }
    },
  },
  methods: {
    FormatData() {
      this.detail.DateOfBirth = this.DateFormat(this.detail.DateOfBirth, true);
      this.detail.IdentityDate = this.DateFormat(
        this.detail.IdentityDate,
        true
      );
      this.detail.WorkStatus = this.WorkStatusCode2Text(this.detail.WorkStatus);
      this.detail.Salary = this.FormatMoneyString(this.detail.Salary);
    },
    GetRawData() {
      let res = JSON.parse(JSON.stringify(this.detail));
      res.DateOfBirth = new Date(this.detail.DateOfBirth).toISOString();
      res.IdentityDate = new Date(
        this.detail.IdentityDate
      ).toISOString();
      res.WorkStatus = this.WorkStatusText2Code(this.detail.WorkStatus);
      let salary = this.detail.Salary;
      res.Salary = Number(salary.replaceAll(".", ""));
      return res;
    },
    BtnSaveClick() {
      
      console.log("save",this.GetRawData());
      // if(this.mode){
      //   this.axios.put(`http://cukcuk.manhnv.net/v1/Employees/${this.detailId}`, this.detail)
      //   .then(res => {
      //     if(res) alert("da SUA thanh cong");
      //   })
      //   .catch(err => {
      //     console.log(err);
      //   })
      // } else {
      //   // console.log(this.detail);
      //   this.axios.post(`http://cukcuk.manhnv.net/v1/Employees/`, this.detail)
      //   .then(res => {
      //     if(res) alert("da THEM thanh cong");
      //   })
      //   .catch(err => {
      //     console.log(err);
      //   })
      // }
    },
    // selectDepartment(depart){      
    //   this.detail.DepartmentName = depart;
    //   console.log("drop select",this.detail.DepartmentName);
    // }
  },
};
</script>

<style scoped>
@import "../../css/components/popup-form.css";
</style>