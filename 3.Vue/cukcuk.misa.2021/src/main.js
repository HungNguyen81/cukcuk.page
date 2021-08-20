import Vue from 'vue'
import App from './App.vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import VueRouter from 'vue-router'
import PageEmployee from './components/pages/PageEmployee.vue'
import PageCustomer from './components/pages/PageCustomer.vue'

Vue.config.productionTip = false
Vue.use(VueRouter, VueAxios, axios)

const routes = [
  {path: '/employees', component: PageEmployee},
  {path: '/customers', component: PageCustomer}
];

const router = new VueRouter({
  routes: routes,
  mode: 'history'
}); 

new Vue({
  router,
  render: h => h(App)  
}).$mount('#app')

