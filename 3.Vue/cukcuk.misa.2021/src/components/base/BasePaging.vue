<template>
  <div class="content-page-navigator">
    <div class="navigator-left" id="current-pagesize">
      Hiển thị <b>{{ `${startRow}-${endRow}` }}/{{ totalRecord }}</b> nhân viên
    </div>
    <div class="navigator-center">
      <div class="button-firstpage button-navigator" @click="first"></div>
      <div class="button-prev-page button-navigator" @click="prev"></div>
      <div class="page-buttons">
        <div
          class="button-page-number"
          :class="{ 'button-current-page': current == item + offset - 1 }"
          v-for="(item, index) in Math.min(5, allPage)"
          :key="index"
          @click="$emit('pageNumChange', item + offset - 2)"
        >
          {{ item + offset - 1 }}
        </div>
      </div>
      <div class="button-next-page button-navigator" @click="next"></div>
      <div class="button-lastpage button-navigator" @click="last"></div>
    </div>
    <div class="navigator-right">
      <Dropdown
        :direction="'up'"
        :data="this.pageSizeDropData"
        :type="'drop-number-of-row'"
        :displayId="'number-of-rows'"
        :typeData="'Name'"
        :value="this.pageSizeDropData[1].Name"
        @itemChange="pageSizeChange"
      ></Dropdown>
    </div>
  </div>
</template>

<script>
import Dropdown from "./BaseDropdown.vue";

export default {
  name: "PagingBar",
  components: {
    Dropdown,
  },
  props: {
    pageNumber: {
      type: Number,
      require: true,
    },
    pageSize: {
      type: Number,
      require: true,
    },
    totalRecord: {
      type: Number,
      require: true,
    },
    totalPage: {
      type: Number,
      require: true,
    },
    items: {
      type: Array,
    },
    entityName: {
      type: String,
      require: true,
      default() {
        return "Employee";
      },
    },
  },
  data() {
    return {
      entityNameMap: {
        Employee: "Nhân viên",
        Customer: "Khách hàng",
      },
      pageSizeDropData: [],
      page: {
        start: 1,
        end: 20,
      },
      pSize: 20,
      current: 1,
      allPage: 1,
    };
  },
  mounted() {
    this.pSize = this.pageSize;
    this.current = this.pageNumber + 1;
    this.allPage = this.totalPage;
  },
  created() {
    // Khởi tạo data cho dropdown chọn kích thước trang
    this.pageSizeDropData = [
      { Name: ` 10 ${this.entityNameMap[this.entityName]}/trang`, Size: 10 },
      { Name: ` 20 ${this.entityNameMap[this.entityName]}/trang`, Size: 20 },
      { Name: ` 50 ${this.entityNameMap[this.entityName]}/trang`, Size: 50 },
      { Name: `100 ${this.entityNameMap[this.entityName]}/trang`, Size: 100 },
    ];
  },
  watch: {
    totalPage: function (tp) {
      this.allPage = tp;
    },
    pageNumber: function (c) {
      this.current = c + 1;
    },
  },
  computed: {
    startRow: function () {
      return Math.min(
        (this.current - 1) * this.pSize + 1,
        Number(this.totalRecord)
      );
    },

    endRow: function () {
      return Math.min(this.current * this.pSize, Number(this.totalRecord));
    },

    offset: function () {
      return this.current < 3
        ? 1
        : this.current - 2 - Math.max(0, 2 - this.allPage + this.current);
    },
  },
  methods: {
    /**
     * Handle khi có thay đổi trong dropdown chọn kích thước trang 
     */
    // CreatedBy: HungNguyen81 (18-08-2021)
    // ModifiedBy: HungNguyen81 (18-08-2021)
    pageSizeChange(type, data) {
      this.pSize = data.Size;
      this.$emit("pageSizeChange", this.pSize);
    },

    /**
     * Chuyển sang trang kế tiếp
     * CreatedBy: HungNguyen81 (18-08-2021)
     */
    next() {
      this.current = this.current < this.totalPage ? this.current + 1 : 1;
      console.log("next", this.current + "/" + this.totalPage);
      this.$emit("pageNumChange", this.current - 1);
    },

    /**
     * Chuyển về trang ngay trước
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    prev() {
      this.current = this.current > 1 ? this.current - 1 : this.totalPage;
      console.log("prev", this.current + "/" + this.totalPage);
      this.$emit("pageNumChange", this.current - 1);
    },
    
    /**
     * Chuyển về trang đầu tiên
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    first() {
      this.current = 1;
      this.$emit("pageNumChange", 0);
    },

    /**
     * Chuyển tới trang cuối cùng
     * CreatedBy: HungNguyen81 (18-08-2021)
     */
    last() {
      this.current = this.totalPage;
      this.$emit("pageNumChange", this.current - 1);
    },
  },
};
</script>
<style scoped>
@import "../../css/base/paging-bar.css";
</style>