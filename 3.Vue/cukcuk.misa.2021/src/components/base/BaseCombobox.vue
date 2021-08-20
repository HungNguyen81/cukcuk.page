<template>
  <div
    :class="['combobox-container', ['dropdown-' + type.toLowerCase() + 's']]"
    v-if="isDataLoaded"
  >
    <div class="combobox">
      <input
        type="text"
        :class="['combobox-input', 'textbox-default']"
        :filter="type"
        @input="comboboxInput"
        @focus="comboboxInput"
        @keyup="handleKeyPress"
        v-model="value"
      />
      <div :class="['x-icon', { hide: isEmptyVal }]" @click="emptyInput">
        <i class="fas fa-times"></i>
      </div>
      <div class="combobox-icon-container" @click="isHide = !isHide">
        <div class="combobox-icon"></div>
      </div>
    </div>
    <div
      :class="['dropdown-data', [type.toLowerCase() + 's'], { hide: isHide }]"
      :id="[type.toLowerCase() + 's']"
    >
      <div
        :class="[
          'dropdown-item',
          { 'item-selected': index == current, hide: item.Hidden },
        ]"
        v-for="(item, index) in items"
        :key="index"
        @click="
          current = index;
          itemClicked();
        "
      >
        <i class="fas fa-check item-icon"></i>
        <div class="item-text">{{ item[type + "Name"] }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import utils from "../../mixins/ultis";
export default {
  name: "Combobox",
  components: {},
  // type: Position, Department,
  // mode: 1- add "Tat ca ...", 0/null- normal
  props: ["type", "api", "mode"],
  mixins: [utils],
  data() {
    return {
      value: "",
      current: 0,
      // Ẩn hiện drop list
      isHide: true,
      isDataLoaded: false,
      typeName: this.type + "Name",
      items: [],
      map: {
        Position: "vị trí",
        Department: "phòng ban",
        CustomerGroup: "nhóm khách hàng",
      },
      isEmptyVal: true,
    };
  },
  created() {
    axios
      .get(this.api)
      .then((res) => {
        this.items = [];

        if (this.mode == 1) {
          this.items.push({
            [this.typeName]: "Tất cả " + this.map[this.type],
            [this.type + "Id"]: "",
          });
        }
        res.data.forEach((e) => {
          this.items.push(e);
        });

        this.items = this.items.map((e) => ({
          ...e,
          Hidden: false,
        }));

        this.isDataLoaded = true;
        this.value = this.items[0][this.type + "Name"];
      })
      .catch((err) => {
        console.log(err);
        this.$emit(
          "showToast",
          "error",
          "SERVER ERROR",
          `Cannot load ${this.type}!`
        );
        this.isDataLoaded = true;
      });
  },

  // updated() {
  //   this.$nextTick(function () {
  //     // this.value = this.items[this.current][this.type + 'Name'];
  //   });
  // },

  watch: {
    // giá trị hiển thị trên input
    value: function () {
      this.isEmptyVal = !(this.value || this.value.trim());
    },
    // chỉ số của item hiện tại
    current: function (c) {
      this.value = this.items[c][this.type + "Name"];
    },
  },
  methods: {
    // handle khi click chọn 1 item trong list dropdown
    itemClicked() {
      this.isHide = true;
      this.$emit(
        "filterActive",
        this.type,
        this.items[this.current][this.type + "Id"]
      );
    },

    // handle khi bấm phím
    handleKeyPress(event) {
      let maxOffset = this.items.length; //0;
      // for (let item of this.items) {
      //   maxOffset = maxOffset + (item.Hidden ? 0 : 1);
      // }

      if (event.code == "ArrowDown") {
        event.preventDefault();

        do {
          this.current = this.current < 0 ? 0 : this.current;
          this.current = (this.current + 1) % maxOffset;
        } while (this.items[this.current].Hidden == true);

      } else if (event.code == "ArrowUp") {
        event.preventDefault();
        
        do {
          this.current = this.current < 0 ? 0 : this.current;
          this.current = this.current == 0 ? maxOffset - 1 : this.current - 1;
        } while (this.items[this.current].Hidden == true);
      } else if (event.code == "Enter") {
        this.isHide = true;
        this.value = this.items[this.current][this.type + "Name"];
        this.$emit(
          "filterActive",
          this.type,
          this.items[this.current][this.type + "Id"]
        );
      }
    },

    /**
     * Handle khi client nhập vào combobox
     * CreatedBy: HungNguyen81 (07-2021)
     */
    comboboxInput() {
      if (this.value) {
        this.isHide = false;
      }
      if (this.items) {
        for (let item of this.items) {
          let comparedString = this.value
            ? this.removeAccents(this.value.toUpperCase())
            : " ";
          let itemString = this.removeAccents(
            item[this.type + "Name"].toUpperCase()
          );
          if (itemString.includes(comparedString)) {
            item.Hidden = false;
          } else {
            item.Hidden = true;
          }
        }
      }
    },

    emptyInput() {
      this.value = "";
      this.$emit("filterActive", this.type, "");
    },
  },
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
@import "../../css/base/combobox.css";
</style>