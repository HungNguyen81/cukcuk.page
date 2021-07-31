<template>
  <div
    :class="['dropdown-container', type]"
    v-if="isDataLoaded"
    @keydown="handleKeyPress"
    @keyup.enter="toggle"
    @blur="isHide = true"
  >
    <div class="dropdown" @click="toggle">
      <div :id="displayId">
        {{ current >= 0 ? items[current][typeData] : "..." }}
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
        @click="itemSelect(item, index)"
      >
        <i class="fas fa-check item-icon"></i>
        <div class="item-text">
          {{ item[typeData] }}
        </div>
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
    // typeData: PositionName / DepartmentName / Name
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
    value: {
      type: String,
      required: false,
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
      displayItem: null,
      isDataLoaded: false,
      items: [],
    };
  },
  created() {
    if (this.api) {
      this.axios
        .get(this.api)
        .then((res) => {
          this.items = res.data;

          if (this.type == "form-dropdown") {
            this.current = -1;

            if (this.value)
              this.items.forEach((e, i) => {
                if (this.value == this.items[i][this.typeData]) {
                  this.current = i;
                }
              });

            this.isDataLoaded = true;
          }
        })
        .catch((err) => {
          console.log(err);
        });
    } else {
      this.items = this.data;
      // if (this.type == "form-dropdown") {
      this.current = -1;

      if (this.value)
        this.items.forEach((e, i) => {
          if (this.value == this.items[i][this.typeData]) {
            this.current = i;
          }
        });
      // }
      this.isDataLoaded = true;
    }
  },
  computed: {},
  methods: {
    toggle() {
      this.isHide = !this.isHide;
    },
    itemSelect(item, index) {
      this.current = index;
      this.toggle();
      this.$emit("itemChange", this.typeData, item);
    },
    handleKeyPress(event) {
      if (event.code == "ArrowDown") {
        event.preventDefault();
        this.current = this.current < 0 ? 0 : this.current;
        this.current = (this.current + 1) % this.items.length;
      } else if (event.code == "ArrowUp") {
        event.preventDefault();
        this.current = this.current < 0 ? 0 : this.current;
        this.current =
          this.current == 0 ? this.items.length - 1 : this.current - 1;
      }
    },
  },
  watch: {
    value: function (val) {
      if (!val || val == "undefined") {
        console.log("value undef");
        this.isDataLoaded = false;
        this.current = -1;
        this.isDataLoaded = true;
      }
      console.log("val change", this.displayId, val);
    },
  },
};
</script>

<style scoped>
@import "../../css/base/dropdown.css";
</style>