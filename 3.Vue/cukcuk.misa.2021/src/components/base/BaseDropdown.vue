<template>
  <div :class="['dropdown-container', [type]]">
    <div class="dropdown" @click="toggle" v-if="isDataLoaded">
      <div :id="displayId">
        {{ this.api ? items[current][typeData + "Name"] : items[current] }}
      </div>
      <div class="dropdown-icon number-of-rows-icon">
        <i :class="['fas', 'fa-chevron-' + direction.toLowerCase()]"></i>
      </div>
    </div>
    <div :class="['dropdown-data', { hide: isHide }]">      
      <div
        :class="['dropdown-item', { 'item-selected': index == current }]"
        v-for="(item, index) in items"
        :key="index"
        @click="itemSelect(index)"
      >
        <i class="fas fa-check item-icon"></i>
        <div class="item-text">{{ api ? item[typeData + "Name"] : item }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Dropdown",
  components: {},
  props: {
    type: {
      type: String,
    },
    typeData: {
      type: String,
    },
    direction: {
      type: String,
      required: true,
    },
    data: {
      type: Array,
      require: true,
    },
    displayId: {
      type: String,
      required: false,
    },
    api: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      current: 0,
      isHide: true,
      displayItem: "",
      isDataLoaded: false,
      items: this.data ? this.data : [],
    };
  },
  created() {
    if (this.api) {
      this.axios
        .get(this.api)
        .then((res) => {
          this.items = res.data;
          this.isDataLoaded = true;
          // console.log(this.items);
        })
        .catch((err) => {
          console.log(err);
        });
    } else {
      this.isDataLoaded = true;
    }
  },
  methods: {
    toggle() {
      this.isHide = !this.isHide;
    },
    itemSelect(index) {
      this.current = index;
      this.toggle();
    },
  },
};
</script>

<style scoped>
</style>