<template>
  <div
    :class="['combobox-container', ['dropdown-' + type.toLowerCase() + 's']]"
  >
    <div class="combobox">
      <input
        type="text"
        :class="['combobox-input', 'textbox-default']"
        :value="items[current][type + 'Name']"
        v-bind:filter="type"
        onkeyup="ComboboxInputChange(this)"
        v-if="mode == 1 && isDataLoaded"
      />
      <input
        type="text"
        :class="['combobox-input', 'textbox-default']"
        value=""
        v-bind:filter="type"
        onkeyup="ComboboxInputChange(this)"
        v-else
      />
      <div class="x-icon" hidden="true">
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
        :class="['dropdown-item', { 'item-selected': index == current }]"
        v-for="(item, index) in items"
        :key="index"
        @click="current = index; isHide=true"
      >
        <i class="fas fa-check item-icon"></i>
        <div class="item-text">{{ item[type + "Name"] }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Combobox",
  components: {},
  // type: Position, Department,
  // mode: 1- add "Tat ca ...", 0/null- normal
  props: ["type", "api", "data", "mode"],
  data() {
    return {
      value: "",
      current: 0,
      isHide: true,
      isDataLoaded: false,
      typeName: this.type + "Name",
      items: this.data,
      map: {
        Position: "vị trí",
        Department: "phòng ban",
      },
    };
  },
  created() {
    this.axios
      .get(this.api)
      .then((res) => {
        this.items = [];
        if (this.mode == 1) {
          this.items.push({
            [this.typeName]: "Tất cả " + this.map[this.type],
          });
        }
        res.data.forEach((e) => {
          this.items.push(e);
        });
        this.isDataLoaded = true;
      })
      .catch((err) => {
        console.log(err);
      });
  },
};
</script>

<style scoped>
@import '../../css/base/text-box.css';
@import '../../css/base/combobox.css';
</style>