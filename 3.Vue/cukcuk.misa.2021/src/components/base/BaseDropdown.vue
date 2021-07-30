<template>
  <div :class="['dropdown-container', type]" v-if="isDataLoaded">
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
    // console.log(this.displayId);
    // console.log("value",this.value);
    // console.log("typeData", this.typeData);

    // if(this.displayId == 'form-dropdown') this.current = -1
    if (this.api) {
      this.axios
        .get(this.api)
        .then((res) => {
          this.items = res.data;

          // console.log("data " + this.displayId , this.items);

          if (this.type == "form-dropdown") {
            this.current = -1;

            if (this.value)
              this.items.forEach((e, i) => {
                // console.log("api",e, i);
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
      if (this.type == "form-dropdown") {
        this.current = -1;

        if (this.value)
          this.items.forEach((e, i) => {
            if (this.value == this.items[i][this.typeData]) {
              this.current = i;
            }
          });
      }
      this.isDataLoaded = true;
    }
  },
  computed: {
    // dropData: function(){
    //   return this.value? this.value : (this.current >= 0 ? this.items[this.current][this.typeData] : "")
    // }
  },
  methods: {
    toggle() {
      this.isHide = !this.isHide;
    },
    itemSelect(item, index) {
      this.current = index;
      this.toggle();
      this.$emit("itemChange", this.typeData, item);
    },
  },
  watch: {
    value: function () {
      if (!this.value || this.value == "undefined") {
        console.log("value undef");
        this.isDataLoaded = false;
        this.current = -1;
        this.isDataLoaded = true;
      }
      console.log("val change", this.displayId, this.value);
    },
  },
};
</script>

<style scoped>
</style>