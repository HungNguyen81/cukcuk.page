<template>
  <div :class="['dropdown-container', type]">
    <div class="dropdown" @click="toggle" v-if="isDataLoaded">
      <div :id="displayId">
        {{
          current >= 0 ? items[current][typeData ? typeData : "" + "Name"] : ""
        }}
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
        <div class="item-text">
          {{ item[typeData ? typeData : "" + "Name"] }}
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
    // typeData: Position/ Department
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
    currProp: {
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
    // this.typeData = (this.typeData)? this.typeData:'';
    // this.displayItem = this.currProp;
    console.log("currprop",this.currProp);
    if (this.api) {
      this.axios
        .get(this.api)
        .then((res) => {
          this.items = res.data;
          
          console.log("data", this.items);
          this.checkSelect();
          this.isDataLoaded = true;
        })
        .catch((err) => {
          console.log(err);
        });
    } else {
      
      this.items = this.data;
      // console.log(this.items);
      this.checkSelect();
      this.isDataLoaded = true;
    }
  },
  computed: {},
  methods: {
    toggle() {
      this.isHide = !this.isHide;
    },
    itemSelect(index) {
      this.current = index;
      this.toggle();
    },
    checkSelect() {
      if (this.currProp) {
        for (let i = 0; i < this.items.length; i++) {
          if (
            this.items[i][this.typeData ? this.typeData : "" + "Name"] == this.currProp
          ) {
            this.current = i;
          }
        }
      }
    },
  },
  watch: {},
};
</script>

<style scoped>
</style>