<template>
  <div class="content-table">
    <table :class="'table-' + [type.toLowerCase()]">      
      <thead>
        <tr>
          <th></th>
          <th v-for="(h, i) in thead" :key="i">{{ h }}</th>          
        </tr>
      </thead>
      <tbody>
        <tr v-for="(e, index) in employees" :key="index">
          <td>
            <span class="checkbox-container">
              <input type="checkbox" name="delete" />
              <span class="checkmark"><i class="fas fa-check check"></i></span>
            </span>
          </td>
          <td :title="e[cell]" v-for="(cell, k) in dataMap" :key="k">
            {{ e[cell] }}
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "BaseTable",
  components: {},
  props: {
    type: {
      type: String,
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
  created() {
    if (this.api)
      this.axios
        .get(this.api)
        .then((res) => {
          this.employees = res.data;          
        })
        .catch((err) => {
          console.log(err);
        });
  },
};
</script>

<style scoped>
@import '../../css/base/table.css'
</style>