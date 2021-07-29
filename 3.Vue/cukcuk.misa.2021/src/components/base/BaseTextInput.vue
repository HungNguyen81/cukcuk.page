<template>
  <input
    :type="type"
    class="textbox-default"    
    :value="$_value"
  />
</template>

<script>
export default {
    name: 'Input',
    props:{
        type: {
            type: String,
            required: true
        },
        value: {
            type: String,
            required: false
        }
    },
    data(){
        return {
            $_value: ''
        }
    },
    created(){
        this.$_value = this.value;
    }
}
</script>

<style scoped>
@import '../../css/base/text-box.css';
</style>