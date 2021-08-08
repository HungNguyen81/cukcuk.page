import Vue from 'vue'
import Router from 'vue-router'
import EmployeePage from '../components/pages/PageEmployee'
import CustomerPage from '../components/pages/PageCustomer'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/cukcuk/employees',
      name: 'Employees',
      component: EmployeePage
    },
    {
      path: '/cukcuk/customers',
      name: 'Customers',
      component: CustomerPage
    }
  ]
})