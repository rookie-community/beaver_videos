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
  </div>
</template>

<script>
export default {
  data() {
    return {};
  },
  mounted() {
    console.clear();
  }
};
</script>

<style lang="less">
@import url('./assets/css/bg.css');
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
</style>
