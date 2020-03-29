<template>
  <el-container>
    <!-- 标题栏 -->
    <el-header>
      <el-row type="flex" justify="space-around" :gutter="20" >
        <el-col :span="8"></el-col>
        <el-col :span="10"></el-col>
        <el-col :span="6" style="-webkit-app-region:no-drag;">
          <button @click="Win_Type('min')">
            <i class="el-icon-minus"></i>
          </button>
          <button @click="Win_Type('max')">
            <i class="iconfont icon-zuidahua2"></i>
          </button>
          <button @click="Win_Type('close')">
            <i class="el-icon-close"></i>
          </button>
        </el-col>
      </el-row>
    </el-header>
    <el-container class="el-cont">
      <!-- 侧边栏 -->
      <el-aside :width="isCollapse == true ? '64px' : '160px'" style="transition-duration:0.2s;">
        <div id="menu_fold" type="primary" @click="isCollapse=!isCollapse">
          <i :class="[isCollapse?'el-icon-s-unfold':'el-icon-s-fold']"></i>
          <span v-show="!isCollapse" style="padding-left:15px;">收起菜单</span>
        </div>
        <el-menu
          default-active="2"
          class="el-menu-vertical-demo"
          background-color="#545c64"
          text-color="#fff"
          active-text-color="#ffd04b"
          :collapse="isCollapse"
          :collapse-transition="false"
        >
          <el-submenu index="1">
            <template slot="title">
              <i class="el-icon-location"></i>
              <span slot="title">导航一</span>
            </template>
            <el-menu-item-group>
              <span slot="title">分组一</span>
              <el-menu-item index="1-1">选项1</el-menu-item>
              <el-menu-item index="1-2">选项2</el-menu-item>
            </el-menu-item-group>
            <el-menu-item-group title="分组2">
              <el-menu-item index="1-3">选项3</el-menu-item>
            </el-menu-item-group>
            <el-submenu index="1-4">
              <span slot="title">选项4</span>
              <el-menu-item index="1-4-1">选项1</el-menu-item>
            </el-submenu>
          </el-submenu>
          <el-menu-item index="2">
            <i class="el-icon-menu"></i>
            <span slot="title">导航二</span>
          </el-menu-item>
          <el-menu-item index="3">
            <i class="el-icon-setting"></i>
            <span slot="title">导航三</span>
          </el-menu-item>
        </el-menu>
      </el-aside>
      <!-- 视频展示区域 -->
      <el-main>
        <iframe ref="iframe" src="https://v.qq.com/x/cover/vooy2m9hi5p1jqm.html" allowfullscreen="true" style="display:inline-flex;padding:0px;margin:0px; width:100%; height:calc(100vh - 54.5px);"></iframe>
        <!-- <webview
          id="foo"
          src="https://v.qq.com/"
          style="display:inline-flex;padding:0px;margin:0px; width:100%; height:calc(100vh - 54.5px);"
          allowpopups
          allowfullscreen='true'
        ></webview> -->
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
const { ipcRenderer: ipc, shell } = require("electron");
let webview;
export default {
  name: "Layout",
  data() {
    return {
      jx_url: {
        思古解析: "https://api.sigujx.com/jx/?url="
      },
      templent: `
       <iframe src="https://api.sigujx.com/jx/?url={{video_url}}" style="width:100%;height: 100%;"></iframe>
      `,
      isCollapse: false,
      web_url: ""
    };
  },
  mounted() {
    document.title = "小狸视频";

    setTimeout(() => {
    let dom=document.querySelector("#iframe").contentWindow;
    console.log("dom",dom)
    }, 3000);
    
    // this.load_webview();
  },
  methods: {
    // test() {
    //   var d = document.querySelector("webview");
    //   d.src = "https://www.baidu.com";
    //   console.log(d);
    //   this.load_webview();
    // },
    load_webview() {
      webview = document.querySelector("webview");

      const loadstart = () => {
        // let palyer = document.querySelector("#tenvideo_player");
        console.log("start");
        // indicator.innerText = "loading...";
      };

      const loadstop = () => {
        //注入css
        // indicator.innerText = "";
      };
      webview.addEventListener("new-window", e => {
        const protocol = require("url").parse(e.url).protocol;
        if (protocol === "http:" || protocol === "https:") {
          //shell.openExternal(e.url)
          // window.open(e.url);
          webview.src = e.url;
        }
      });
      webview.addEventListener("load-commit",e=>{
        console.log('load-commit');
      })
      webview.addEventListener("dom-ready", e => {
        console.log('dom-ready');
        //注入样式
        webview.insertCSS(`
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
  background-color: rgba(180, 180, 180, 0.75);
}
/*滑块效果*/
::-webkit-scrollbar-thumb:hover {
  border-radius: 5px;
  -webkit-box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.2);
  background-color: rgba(85, 85, 85, 0.4);
}
`);
        //注入js
        // webview.executeJavaScript(`alert("测试")`);
      });

      webview.addEventListener("did-start-loading", loadstart);
      webview.addEventListener("did-stop-loading", loadstop);
    },
    //窗体操作
    Win_Type(type) {
      ipc.send(type);
      // ipc.send("min");
      // ipc.send("max");
    }
  }
};
</script>

<style>
html,
body,
#app,
.el-container {
  /*设置内部填充为0，几个布局元素之间没有间距*/

  padding: 0px;
  /*外部间距也是如此设置*/
  margin: 0px;
  /*统一设置高度为100%*/
  height: 100%;
  /* 允许拖拽窗体 */
  -webkit-app-region: drag;
}

.el-header {
  background-color: #b3c0d1;
  color: #333;
  text-align: center;
  height: 50px !important;
  line-height: 50px;
}
.el-aside {
  background-color: #545c64;
  color: #333;
  text-align: center;
  border: 0px;
  /* line-height: 200px; */
  -webkit-app-region: no-drag;
}
.el-menu {
  border: 0px !important;
}
.el-main {
  padding: 0px !important;
  background-color: #e9eef3;
  color: #333;
  text-align: center;
  /* line-height: 160px; */
  -webkit-app-region: no-drag;
}

#menu_fold {
  width: 100%;
  line-height: 56px;
  color: #ffffff;
  cursor: pointer;
}
#menu_fold:hover {
  background-color: rgb(67, 74, 80);
}
</style>