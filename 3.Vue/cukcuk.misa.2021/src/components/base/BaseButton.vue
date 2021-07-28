<template>
  <div class="button" :id='type'>
      {{value}}
  </div>
</template>

<script>
export default {
  name: 'BaseButton',
  props: ['value', 'type']
}
</script>

<style>
/* @import '../../css/base/button.css' */
</style>
