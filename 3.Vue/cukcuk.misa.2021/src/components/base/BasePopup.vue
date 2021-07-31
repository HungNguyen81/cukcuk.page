<template>
  <div :class="['popup-wrapper', { open: !isHide, close: isHide }]">
    <div :class="['popup-container', { open: !isHide, close: isHide }]">
      <div
        class="close-button"
        tabindex="17"
        @click="$emit('closePopup')"
        @keydown.enter="$emit('closePopup')"
      ></div>
      <div class="popup-header">{{ title }}</div>
      <div class="popup-body">
        <div :class="['msg-icon', { [popupType]: [popupType] }]"></div>
        <div class="msg-content">
          <p><span v-html="content"></span></p>
        </div>
      </div>
      <div class="popup-footer">
        <base-button
          class="popup-button"
          :type="'button-cancel'"
          tabindex="18"
          :onclick="function () {$emit('closePopup');}"
          :value="'Hủy'"
          ref="CancelBtn"
        ></base-button>

        <base-button
          class="popup-button"
          :type="'button-ok'"
          :class="{ [popupType]: [popupType] }"
          tabindex="20"
          :onclick="HandleOkClick"
          :value="ok"
        ></base-button>
      </div>
    </div>
  </div>
</template>

<script>
import BaseButton from "./BaseButtonIcon.vue";
export default {
  name: "Popup",
  components: {
    BaseButton,
  },
  props: {
    title: {
      type: String,
      require: false,
      default: "Empty Title",
    },
    content: {
      type: String,
      require: false,
      default: "Empty content",
    },
    // error, warning, info
    popupType: {
      type: String,
      require: true,
    },
    ok: {
      type: String,
      require: true,
    },
    isHide: {
      type: Boolean,
      require: true,
    },
    callback: {
      type: Function,
      require: false,
    },
  },
  mounted() {},
  methods: {
    HandleOkClick() {
      if (this.callback) {
        this.callback();
      } else {
        console.log("Callback function not found!");
      }
    },
  },
  watch: {
    isHide: function (val) {
      this.$nextTick(() => {
        if (!val) this.$refs.CancelBtn.$el.focus();
      });
    },
  },
};
</script>

<style scoped>
@import '../../css/components/popup.css';
</style>