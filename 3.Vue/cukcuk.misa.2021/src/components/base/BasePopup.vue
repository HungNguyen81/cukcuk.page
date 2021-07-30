<template>
  <div :class="['popup-wrapper', {open: !isHide, close: isHide}]">
    <div :class="['popup-container', {open: !isHide, close: isHide}]">
      <div class="close-button" @click="$emit('closePopup')"></div>
      <div class="popup-header">{{title}}</div>
      <div class="popup-body">
        <div :class="['msg-icon', {[popupType]: [popupType]}]"></div>
        <div class="msg-content">
          <p><span v-html="content"></span></p>
        </div>
      </div>
      <div class="popup-footer">
        <div class="button button-cancel popup-button" tabindex="1" @click="$emit('closePopup')">Hủy</div>
        <div :class="['button', 'button-ok', 'popup-button', {[popupType] : [popupType]}]" @click="HandleOkClick" tabindex="2">
          {{ok}}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
    name: "Popup",
    components: {},
    props: {
        title:{
            type: String,
            require: false,
            default: 'Empty Title'
        },
        content: {
            type: String,
            require: false,
            default: 'Empty content'
        },
        // error, warning, info
        popupType: {
            type: String,
            require: true,
        },
        ok: {
            type: String,
            require: true
        },
        isHide: {
            type: Boolean,
            require: true
        },
        callback:{
            type: Function,
            require: false
        }
    },
    mounted(){        
    },
    methods:{
        HandleOkClick(){
            if(this.callback) {
                this.callback();
            } else {
                console.log("Callback function not found!");
            }
        }
    }
}
</script>