import Vue from 'vue'
import './plugins/axios'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import './plugins/element.js'
import VueDPlayer from 'vue-dplayer'
import 'vue-dplayer/dist/vue-dplayer.css'
Vue.use(require('vue-wechat-title'));
Vue.component('d-player', VueDPlayer)
window.Hls = require('hls.js');
import VueClipboard from 'vue-clipboard2'//剪切板
Vue.use(VueClipboard)
Vue.config.productionTip = false
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
