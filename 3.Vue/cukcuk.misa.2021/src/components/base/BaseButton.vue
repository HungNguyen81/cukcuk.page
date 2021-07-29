<template>
  <div class="button" :class='type' @click="onclick">
      {{value}}
  </div>
</template>

<script>
export default {
  name: 'BaseButton',
  props: ['value', 'type', 'onclick']
}
</script>

<style>
/* @import '../../css/base/button.css' */
</style>
