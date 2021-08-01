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
              {{ FormatMoneyString(e[cell]) }}</span
            >
            <span v-else-if="cell == 'DateOfBirth'">
              {{ DateFormat(e[cell], false) }}</span
            >
            <span v-else> {{ e[cell] }} </span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import ultis from "../../mixins/ultis";

export default {
  name: "BaseTable",
  components: {},
  mixins: [ultis],
  props: {
    type: {
      type: String, // ex: type="Employee"
      required: true,
    },
    thead: {
      type: Array,
      required: true,
    },
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
      employees: [],
      isSelectAll: false,
      // test: 0
    };
  },
  updated: function () {
    this.$nextTick(function () {
      // Code that will run only after the
      // entire view has been re-rendered
      if (this.employees) {
        for (let e of this.employees) {
          let stored = localStorage.getItem("select");
          if(stored) stored = JSON.parse(stored);
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
  mounted() {
    // console.log("t",this.test++);
    if (this.api)
      this.axios
        .get(this.api)
        .then((res) => {
          this.employees = res.data.Data;
          if (this.employees) {
            this.$emit("dataLoaded", this.employees.length);
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
            this.$emit('showToast', 'error', this.type, 'Not found !')
          }

          localStorage.setItem('select', JSON.stringify([]));
        })
        .catch((err) => {
          console.log(err);
        });
  },
  computed: {},
  watch: {},
  methods: {
    rowClickHandle(e, type) {
      this.$emit("rowClick", e[type + "Id"], e[type + "Code"], e["FullName"]);
      e.isSelected = !e.isSelected;
      // localStorage.setItem('select', JSON.stringify([]));
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
      console.log(localStorage.getItem("select"));

      // if (e.isSelected) {
      // } else {
      // }
    },
    checkSelected(e) {
      let code = e[this.type + "Code"];
      let stored = localStorage["select"];
      if (stored) {
        e.isSelected = JSON.parse(stored).includes(code);
        return e.isSelected;
      }
    },
    selectAll() {
      this.isSelectAll = !this.isSelectAll;
      let list = localStorage.getItem("select");
      try {
        list = JSON.parse(list);
      } catch (err) {
        list = [];
        localStorage.setItem("select", JSON.stringify([]));
      }
      // localStorage.setItem('select', JSON.stringify([]));

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