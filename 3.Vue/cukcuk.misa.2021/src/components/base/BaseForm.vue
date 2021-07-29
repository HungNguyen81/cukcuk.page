<template>
  <div :class="['container', { open: isOpen, close: !isOpen }]" id="container">
    <div
      :class="['form-container', { open: isOpen, close: !isOpen }]"
      id="form-container"
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
                :type="'text'"
                class="input-form"
                id="employee-code"
                :value="'NV000001'"
                tabindex="1"
              ></BaseInput>
            </div>
            <div class="input-field">
              <div class="input-label">
                Họ và tên (<span class="required">*</span>)
              </div>
              <BaseInput
                :type="'text'"
                class="input-form"
                id="fullname"
                :value="'Nguyễn Ngọc Hưng'"
                tabindex="2"
              ></BaseInput>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Ngày sinh</div>
              <BaseInput
                :type="'date'"
                class="input-form input-form"
                id="dob"
                :value="'2000-01-08'"
                tabindex="3"
              ></BaseInput>
            </div>
            <div class="input-field">
              <div class="input-label">Giới tính</div>              
              <BaseDropdown
                :direction="'down'"
                :data="['Nam', 'Nữ', 'Không xác định']"
                :type="'form-dropdown'"
                :displayId="'gender'" tabindex="4"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">
                Số CMTND/ Căn cước (<span class="required">*</span>)
              </div>
              <BaseInput
                :type="'text'"
                class="input-form"
                id="identity-number"
                :value="'038200009999'"
                tabindex="5"
              ></BaseInput>
            </div>
            <div class="input-field">
              <div class="input-label">Ngày cấp</div>
              <BaseInput
                :type="'date'"
                class="input-form"
                id="identity-date"
                :value="'2020-01-08'"
                tabindex="6"
              ></BaseInput>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Nơi cấp</div>
              <BaseInput
                :type="'text'"
                class="input-form"
                id="identity-place"
                :value="'Tỉnh Thanh Hóa'"
                tabindex="7"
              />
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">
                Email (<span class="required">*</span>)
              </div>
              <BaseInput
                :type="'text'"
                class="input-form"
                id="email"
                :value="'nguyenngochung.ncth@gmail.com'"
                tabindex="8"
              />
            </div>
            <div class="input-field">
              <div class="input-label">
                Số điện thoại (<span class="required">*</span>)
              </div>
              <BaseInput
                :type="'text'"
                class="input-form"
                id="phone-number"
                :value="'0334004655'"
                tabindex="9"
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
                :displayId="'position-name'" tabindex="10"
                id="form-positions"
                :api="'http://cukcuk.manhnv.net/v1/Positions'"
              ></BaseDropdown>
            </div>
            <div class="input-field">
              <div class="input-label">Phòng ban</div>
              <BaseDropdown
                :direction="'down'"                
                :type="'form-dropdown'"
                :typeData="'Department'"
                :displayId="'department-name'" tabindex="11"
                id="form-departments"
                :api="'http://cukcuk.manhnv.net/api/Department'"
              ></BaseDropdown>
            </div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Mã số thuế cá nhân</div>
              <BaseInput
                :type="'text'"
                class="input-form"
                id="tax-code"
                :value="'123456789'"
                tabindex="12"
              />
            </div>
            <div class="input-field">
              <div class="input-label">Mức lương cơ bản</div>
              <BaseInput
                :type="'text'"
                class="input-form input-salary"
                id="salary"
                :value="'100.000.000'"
                tabindex="13"
              />
            </div>
            <div class="money-unit">(VNĐ)</div>
          </div>
          <div class="input-row">
            <div class="input-field">
              <div class="input-label">Ngày gia nhập công ty</div>
              <BaseInput
                :type="'date'"
                class="input-form"
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
                :displayId="'work-status'" tabindex="15"
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
          :onclick="this.close"
        ></BaseButtonIcon>
      </div>
    </div>
  </div>
</template>

<script>
import BaseButton from "../base/BaseButton.vue";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import BaseDropdown from "../base/BaseDropdown.vue";
import BaseInput from "../base/BaseTextInput.vue";

export default {
  name: "Form",
  components: {
    BaseButton,
    BaseButtonIcon,
    BaseDropdown,
    BaseInput
  },
  props: {
    isOpen: {
      type: Boolean,
      required: true,
    },
    close: {
      type: Function,
      require: false,
    },
  },
  data() {
    return {
      // isOpen: true,
    };
  },
};
</script>

<style scoped>
@import "../../css/components/popup-form.css";
</style>