import Vue from 'vue'
import App from './App'

//引入全局变量数据
/* import VideoList from '@/static/VideoJson/VideoList.json' */
// import VideoInfo from '@/static/VideoJson/VideoInfo.json'
//挂载 Vue.prototype
/* Vue.prototype.$VideoList=VideoList; */
// Vue.prototype.$VideoInfo=VideoInfo;

Vue.config.productionTip = false

App.mpType = 'app'

const app = new Vue({
	...App
})
app.$mount()
