<template>
  <div id="app">
    <div v-wechat-title="$route.meta.title"></div>
    <router-view></router-view>
    <el-backtop>
      <div
        style="{
        height: 100%;
        width: 100%;
        background-color: #f2f5f6;
        box-shadow: 0 0 6px rgba(0,0,0, .12);
        text-align: center;
        line-height: 40px;
        color: #1989fa;
      }"
      >UP</div>
    </el-backtop>
    <div class="bgimg"></div>
    <footer>免责声明：数据均来源自于互联网,如有侵权请联系<a href="mailto:2270969436@qq.com">管理员</a>删除。</footer>
  </div>
</template>

<script>
export default {
  name: "app"
};
</script>

<style lang="less">
* {
  margin: 0px;
  padding: 0px;
  text-decoration: none;
}
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-family: "Helvetica Neue";
  color: #2c3e50;
}
body {
  height: 100%;
  // background: url('./assets/bg.jpg') no-repeat;
  overflow: hidden;
}
.bgimg{
    position:fixed;
    top: 0;
    left: 0;
    width:100%;
    height:100%;
    min-width: 700px;
    z-index:-10;
    background: url(https://bing.ioliu.cn/v1?d=0&h=1080&w=1920) no-repeat;
    background-size: cover;
    background-position: center;
}
footer {
  width: 100%;
  height: 35px;
  line-height: 35px;
  text-align: center;
  position: fixed;
  left: 0;
  bottom: 0;
  // font-size: 0.7em;
  color: #666;
  background-color: #333333;
  font-size: small;
  a {
    color: #666;
  }
  a:hover {
    color: #a9a9a9;
  }
  z-index: 999;
}
</style>
