import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
      title: '小狸视频(=￣ω￣=)免费的哟',
      auth: false
    }
  },
  {
    path: '/detail',
    name: 'Detail',
    component: () => import('../views/Detail.vue'),
    meta: {
      title: '小狸视频-影视详情',
      auth: false
    }
  }
]

const router = new VueRouter({
  // mode: 'history',
  base: process.env.BASE_URL,
  routes
})
// 加入百度统计
router.beforeEach((to, from, next) => {
  if (to.path) {
    if (window._hmt) {
      window._hmt.push(['_trackPageview', '/beaver_videos/#' + to.fullPath])
    }
  }
  if (to.meta.auth) {
    let Token = sessionStorage.getItem('Token') == 'ZEhwdE1qSTNNRGsyT1RRek5nJTNEJTNE' ? true : false;
    if (Token) {
      next();
    } else {
      next('/');
    }
  } else {
    next();
  }
})
export default router
