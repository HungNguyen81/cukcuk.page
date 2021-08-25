import Vue from 'vue'
import App from './App.vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import Config from '../config/dev.env'
import router from './router'

Vue.config.productionTip = false  
Vue.use(VueAxios, axios)

Vue.prototype.$config = Config;

new Vue({
  router: router,
  render: h => h(App)  
}).$mount('#app')

