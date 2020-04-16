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
    <footer>免责声明：所有数据均来源于于互联网,如有侵犯版权请联系<a href="mailto:2270969436@qq.com">管理员</a>删除。</footer>
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
  background: url(https://bing.ioliu.cn/v1/blur?d=0&h=1080r%3D10&w=1920)
    no-repeat;
  // background: url('./assets/bg.jpg') no-repeat;
  background-attachment: scroll;
  background-size: auto;
  background-size: cover;
  background-attachment: fixed;
  height: 100%;
}
footer {
  width: 100%;
  height: 35px;
  line-height: 35px;
  text-align: center;
  position: fixed;
  left: 0;
  bottom: 0;
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
