<template>
  <div class="content-table">
    <table :class="'table-' + [type.toLowerCase()]">
      <thead>
        <tr>
          <th>
            <span
              class="checkbox-container"
              @dblclick.stop=""
              @click="selectAll"
            >
              <input type="checkbox" name="delete" v-model="isSelectAll" />
              <span class="checkmark"><i class="fas fa-check check"></i></span>
            </span>
          </th>
          <th v-for="(header, i) in thead" :key="i">{{ header }}</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(e, index) in employees"
          :key="index"
          @dblclick="
            $emit(
              'rowDblClick',
              e[type + 'Id'],
              e.PositionName,
              e.DepartmentName
            )
          "
          @click="rowClickHandle(e, type)"
          :class="{ selected: checkSelected(e, type) }"
        >
          <td>
            <span class="checkbox-container" @dblclick.stop="">
              <input type="checkbox" name="delete" v-model="e.isSelected" />
              <span class="checkmark"><i class="fas fa-check check"></i></span>
            </span>
          </td>
          <td :title="e[cell]" v-for="(cell, k) in dataMap" :key="k">
            <span v-if="cell == 'Salary'">
              {{ formatMoneyString(e[cell]) }}</span
            >
            <span v-else-if="cell == 'DateOfBirth'">
              {{ dateFormatVer2(e[cell], "dd/mm/yyyy") }}</span
            >
            <span v-else-if="cell == 'WorkStatus'">
              {{ workStatusCode2Text(e[cell]) }}</span
            >
            <span v-else> {{ e[cell] }} </span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
// import config from '../../../config/dev.env'
import axios from 'axios';
import ultis from "../../mixins/ultis";

export default {
  name: "BaseTable",
  components: {},
  mixins: [ultis],
  props: {
    tableKey: {},
    // Kiểu entity: employee hoặc customer, ...
    type: {
      type: String, // ex: type="Employee"
      required: true,
    },
    //Mảng tên header <th>
    thead: {
      type: Array,
      required: true,
    },
    //Mảng key tương ứng các cột trong thead
    dataMap: {
      type: Array,
      required: true,
    },
    api: {
      type: String,
      require: false,
    },
  },
  data() {
    return {
      employees: null,
      isSelectAll: false,
      //map từ tên type với cách gọi tên TViet
      typeMap: {
        Employee: "Nhân Viên",
        Customer: "Khách Hàng",
      },
    };
  },
  updated: function () {
    /**
     * Giữ trạng thái selected của những dòng đã click chọn khi refresh table
     * @ CreatedBy: HungNguyen81 (18-08-2021)
     * @ ModifiedBy: HungNguyen81 (18-08-2021)
     */
    this.$nextTick(function () {
      // Code that will run only after the
      // entire view has been re-rendered
      if (this.employees) {
        for (let e of this.employees) {
          let stored = localStorage.getItem("select");
          if (stored) stored = JSON.parse(stored);
          else return;
          if (!stored.includes(e[this.type + "Code"])) {
            this.isSelectAll = false;
            return;
          }
        }
        this.isSelectAll = true;
      }
    });
  },
  mounted() {},
  computed: {},
  watch: {
    tableKey: function () {
      this.buildTableContent();
    },
  },
  methods: {
    /**
     * Lấy dữ liệu từ API và đổ vào table
     * @ CreatedBy: HungNguyen81 (18-08-2021)
     */
    buildTableContent(){
      // console.log("api", this.$config.BASE_API);      
      if (this.api) {
        axios
          .get(this.api)
          .then((res) => {
            this.employees = res.data.Data;
            if (this.employees) {
              this.$emit("dataLoaded");
              this.$emit(
                "getPagingInfo",
                res.data.TotalPage,
                res.data.TotalRecord
              );
              this.employees = this.employees.map((e) => ({
                ...e,
                isSelected: false,
              }));
            } else {
              this.$emit(
                "showToast",
                "warning",
                this.typeMap[this.type],
                "Không có dữ liệu phản hồi !"
              );
              this.$emit("dataLoaded");
            }
          })
          .catch((err) => {
            console.log(err);
            this.$emit("dataLoaded");
            this.$emit(
              "showToast",
              "error",
              "LỖI SERVER",
              `Không nhận được dữ liệu ${this.typeMap[this.type]} !`
            );
          });
      }
    },

    /**
     * Handle khi click chuột vào table row
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    rowClickHandle(e, type) {
      this.$emit("rowClick", e[type + "Id"], e[type + "Code"], e["FullName"]);
      e.isSelected = !e.isSelected;
      let stored = localStorage.getItem("select");

      if (stored) {
        stored = JSON.parse(stored);
        if (e.isSelected) {
          stored.push(e[type + "Code"]);
        } else {
          let code = e[type + "Code"];
          let index = stored.indexOf(code);
          stored.splice(index, 1);
        }
        localStorage.setItem("select", JSON.stringify(stored));
      } else {
        localStorage.setItem(
          "select",
          JSON.stringify([].push(e[type + "Code"]))
        );
      }
    },

    /**
     * Check các dòng được chọn (selected)
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    checkSelected(e) {
      let code = e[this.type + "Code"];
      let stored = localStorage["select"];

      if (stored) {
        e.isSelected = JSON.parse(stored).includes(code);
        return e.isSelected;
      }
    },

    /**
     * Chọn tất cả dòng trong 1 trang của bảng <click checkbox chọn tất cả>
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    selectAll() {
      this.isSelectAll = !this.isSelectAll;
      let list = localStorage.getItem("select");
      try {
        list = JSON.parse(list);
      } catch (err) {
        list = [];
        localStorage.setItem("select", JSON.stringify([]));
      }

      for (let e of this.employees) {
        let code = e[this.type + "Code"];
        if (this.isSelectAll) {
          if (!list.includes(code)) {
            list.push(code);
            this.$emit("rowClick", e[this.type + "Id"], code, e["FullName"]);
          }
        } else {
          let index = list.indexOf(code);
          list.splice(index, 1);
          this.$emit("rowClick", e[this.type + "Id"], code, e["FullName"]);
        }
      }
      localStorage.setItem("select", JSON.stringify(list));
    },
  },
};
</script>

<style scoped>
@import "../../css/base/table.css";
</style>