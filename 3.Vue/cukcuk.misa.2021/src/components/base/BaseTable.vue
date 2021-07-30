<template>
  <div class="content-table">
    <table :class="'table-' + [type.toLowerCase()]">
      <thead>
        <tr>
          <th></th>
          <th v-for="(header, i) in thead" :key="i">{{ header }}</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(e, index) in employees"
          :key="index"
          @dblclick="$emit('rowDblClick', e[type + 'Id'], e.PositionName, e.DepartmentName)"
          @click="rowClickHandle(e, type)"
          :class="{'selected' : e.isSelected}"
        >
          <td>
            <span class="checkbox-container">
              <input type="checkbox" name="delete" v-model="e.isSelected"/>
              <span class="checkmark"><i class="fas fa-check check"></i></span>
            </span>
          </td>
          <td :title="e[cell]" v-for="(cell, k) in dataMap" :key="k">
            <span v-if="cell == 'Salary'">{{ FormatMoneyString(e[cell]) }}</span>
            <span v-else-if="cell == 'DateOfBirth'">{{ DateFormat(e[cell], false) }}</span>
            <span v-else>{{ e[cell] }}</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import ultis from '../../mixins/ultis';

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
    };
  },
  mounted() {    
    if (this.api)
      this.axios
        .get(this.api)
        .then((res) => {
          this.employees = res.data;          
          this.$emit("dataLoaded", this.employees.length);
          this.employees = this.employees.map(e => ({...e, isSelected : false}));
        })
        .catch((err) => {
          console.log(err);
        });
  },
  computed: {
    
  },
  watch:{
    
  },
  methods:{
      rowClickHandle(e, type){
          this.$emit('rowClick', e[type + 'Id'], e[type+'Code']);
          e.isSelected = !e.isSelected;
      }
  }
};
</script>

<style scoped>
@import "../../css/base/table.css";
</style>