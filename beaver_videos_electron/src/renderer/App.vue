<template>
  <div id="app">
    <router-view></router-view>
  </div>
</template>
<script>
export default {
  name: "app"
};
</script>

<style>
@import url("./assets/font/iconfont.css");
body {
  -moz-user-select: none;
  -webkit-user-select: none;
  user-select: none;
}
/*整体部分*/
::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}
/*滑动轨道*/
::-webkit-scrollbar-track {
  border-radius: 0px;
  background: none;
}
/*滑块*/
::-webkit-scrollbar-thumb {
  border-radius: 5px;
  -webkit-box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.2);
  background-color: rgba(255, 255, 255, 0.75);
}
/*滑块效果*/
::-webkit-scrollbar-thumb:hover {
  border-radius: 5px;
  -webkit-box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.2);
  background-color: rgba(85, 85, 85, 0.4);
}
</style>
