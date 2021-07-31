<template>
  <div class="content-page-navigator">
    <div class="navigator-left" id="current-pagesize">
      Hiển thị <b>{{ `${startRow}-${endRow}` }}/{{ totalRecord }}</b> nhân
      viên
    </div>
    <div class="navigator-center">
      <div class="button-firstpage button-navigator" @click="first"></div>
      <div class="button-prev-page button-navigator" @click="prev"></div>
      <div class="page-buttons">
        <div
          class="button-page-number"
          :class="{ 'button-current-page': current == index + 1 }"
          v-for="(item, index) in items"
          :key="index"
          @click="$emit('pageNumChange', index)"
        >
          {{ index + 1 }}
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
    totalRecord:{
        type: Number,
        require: true
    },
    totalPage: {
        type: Number,
        require: true
    }
  },
  mounted(){
      console.log("MOUNTED");
      this.pSize = this.pageSize;
    //   this.$on('pagingInfo', this.UpdatePagingInfo)
    console.log("reset paging bar at currpage:", this.pageNumber, this.pSize);
    this.current = this.pageNumber + 1;
    // this.pSize = 
  },
  data() {
    return {
      pageSizeDropData: [
        { Name: " 10 nhân viên/trang", Size: 10 },
        { Name: " 20 nhân viên/trang", Size: 20 },
        { Name: " 50 nhân viên/trang", Size: 50 },
        { Name: "100 nhân viên/trang", Size: 100 },
      ],
      page: {
        start: 1,
        end: 20,
      },
      pSize: 20,
      current: 1,            
      items: [1,2,3],
    };
  },
  watch:{
      pageSize: function(p){
          console.log("page size", p);
      },
      
      totalPage: function(tp){
          console.log('tp', tp);
      },
      totalRecord: function(tr){
          console.log('tr', tr);
      },
      pageNumber: function(c){
          console.log("pg-num", c);
          this.current = c+1;
      }
  },
  computed:{
      startRow: function(){
          return Math.min((this.current-1) * this.pSize, Number(this.totalRecord));
      },
      endRow: function(){
          return Math.min(this.current * this.pSize, Number(this.totalRecord));
      },      
  },
  methods:{
      pageSizeChange(type, data){
          this.pSize = data.Size;
          console.log("M: page size changed to", data.Size);
          this.$emit("pageSizeChange", this.pSize);
      },
      next(){
          this.current = (this.current < this.totalPage)? this.current+1 : 1;
          console.log('next', this.current +'/'+ this.totalPage, this.totalRecord);
      },
      prev(){
          this.current = (this.current > 1)? this.current-1: this.totalPage;
          console.log('prev', this.current +'/'+ this.totalPage);
      },
      first(){

      },
      last(){

      }
  }
};
</script>
<style scoped>
@import "../../css/base/paging-bar.css";
</style>