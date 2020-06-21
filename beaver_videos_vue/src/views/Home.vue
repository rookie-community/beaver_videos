<template>
  <div id="home">
    <div class="content">
      <el-row type="flex" justify="center">
        <el-col :xs="4" :sm="1" :md="1">
          <a src="https://www.baidu.com">
            <img class="logo" @click="ToApp" alt="logo" src="../assets/img/logo.png" />
          </a>
        </el-col>
        <el-col :xs="9" :sm="3" :md="3">
          <el-tooltip class="item" effect="dark" content="点击跳转到APP详情页" placement="right">
            <h1 class="title" @click="ToApp">小狸视频</h1>
          </el-tooltip>
        </el-col>
      </el-row>
      <el-row type="flex" justify="center">
        <el-col :xs="20" :sm="10" :md="12">
          <el-input
            placeholder="请输入您要搜索的内容"
            v-model="SearchName"
            @focus="focus=true"
            @blur="focus=false"
            @keyup.enter.native="Search()"
          >
            <el-button slot="append" type="primary" icon="el-icon-search" @click="Search()">搜索</el-button>
          </el-input>
        </el-col>
      </el-row>
    </div>
    <el-row type="flex" justify="center">
      <el-col :xs="23" :md="16">
        <el-card class="box-card" v-show="Active!=0">
          <!-- 影视搜索 -->
          <div v-if="Active==1" ref="SearchInfo">
            <div v-for="item in DataList" :key="item.vod_id" ref="Info">
              <el-row id="datalist" :gutter="20">
                <el-col :xs="12" :sm="12" :md=" {span: 4, offset: 3}">
                  <router-link
                    type="primary"
                    target="_blank"
                    :to="{path:'/detail',query:{id:item.vod_id}}"
                  >
                    <img :src="item.vod_pic" onerror="../assets/img/error.jpg" :alt="item.vod_name" />
                  </router-link>
                </el-col>
                <el-col :xs="12" :sm="12" :md="14">
                  <router-link
                    class="hidden-md-and-up"
                    type="primary"
                    target="_blank"
                    :to="{path:'/detail',query:{id:item.vod_id}}"
                  >{{item.vod_name}}</router-link>
                  <h3 class="hidden-sm-and-down">
                    <router-link
                      type="primary"
                      target="_blank"
                      :to="{path:'/detail',query:{id:item.vod_id}}"
                    >{{item.vod_name}}</router-link>
                  </h3>
                  <p style="margin-top:5px;">导演：{{FormatTxt(item.vod_director)}}</p>
                  <p>主演：{{FormatTxt(item.vod_actor)}}</p>
                  <p>类型：{{item.vod_class}}</p>
                  <p class="hidden-md-and-up">地区：{{item.vod_area}}</p>
                  <p>语言：{{item.vod_lang}}</p>
                  <p class="hidden-md-and-up">其他：{{item.vod_remarks}}</p>
                  <!-- <p>更新时间：{{item.vod_time}}</p> -->
                  <p class="hidden-sm-and-down">
                    简介：
                    <span v-html="item.vod_blurb"></span>
                    <router-link
                      type="primary"
                      target="_blank"
                      :to="{path:'/detail',query:{id:item.vod_id}}"
                    >影视详情>></router-link>
                  </p>
                </el-col>
              </el-row>
              <el-divider></el-divider>
            </div>
            <el-row type="flex" justify="center">
              <el-col :md="18">
                <!-- 分页切换 -->
                <el-pagination
                  background
                  layout="prev, pager, next"
                  :page-count="pagecount"
                  @current-change="CurrentChange"
                ></el-pagination>
              </el-col>
            </el-row>
          </div>
          <!-- 视频解析 -->
          <div ref="analysis" v-else-if="Active==2">
            <iframe
              id="analysis_view"
              allowfullscreen="true"
              :src="Play_Url+VideoUrl"
              scrolling="no"
              frameborder="0"
            ></iframe>
          </div>
          <!-- 视频Url播放 -->
          <div v-else-if="Active==3">
            <d-player :options="options" @play="play" ref="player"></d-player>
          </div>
          <!-- 无数据 -->
          <div ref="empty" v-else-if="Active==4"></div>
        </el-card>
      </el-col>
    </el-row>
    <bea-footer />
    <div :class="{'focus_ovr':focus}"></div>
  </div>
</template>

<script>
import beafooter from "../components/beaver_footer";
export default {
  name: "Home",
  data() {
    return {
      SearchName: "", //搜索内容
      Vod_Name: "",
      focus: false, //遮罩层
      DataList: "",
      pagecount: 1, //总页数
      Active: 0, //类型切换 0隐藏，1搜索数据，2解析，3播放Url
      Play_Url: "https://api.sigujx.com/?url=", //解析
      VideoUrl: "", //视频地址
      options: {
        video: {
          quality: [
            {
              // name: "视频一",
              url: "http://static.smartisanos.cn/common/video/t1-ui.mp4",
              type: "auto" //视频类型
            }
          ],
          defaultQuality: 0,
          pic: "https://cbu01.alicdn.com/img/ibank/2019/694/265/11144562496.jpg"
        },
        autoplay: false
      },
      player: null,
      auth: { name: "tzm2270969436", Token: "ZEhwdE1qSTNNRGsyT1RRek5nJTNEJTNE" }
    };
  },
  components: {
    "bea-footer": beafooter
  },
  mounted() {
    this.SearchName = this.$route.query.search;
    if (this.SearchName != "" && this.SearchName != null) {
      this.analysis();
    }
  },
  methods: {
    //搜索事件
    Search(){
      this.$router.push(`/?search=${this.SearchName}`);
    },
    //解析
    analysis() {
      let name = this.SearchName;
      this.Vod_Name = name;
      if (name == "" || name == null) {
        this.$message({
          showClose: true,
          message: "搜索内容不能为空！",
          type: "warning"
        });
      } else if (name == this.auth.name) {
        sessionStorage.setItem("Token", this.auth.Token);
        let q = this.$route.query.search;
        if (q != "" && q != null) {
          this.$router.push("/welfare");
        }
      } else {
        //播放Url
        let LowerName = name.toLowerCase(); //英文字母转小写
        let last = name.split(".")[name.split(".").length - 1];
        if ("m3u8,mp4".search(last) != -1) {
          this.Active = 3;
          this.$message({
            showClose: true,
            message: "正在播放：" + name
          });
          this.switchHandle(name);
        } else if (
          //解析
          LowerName.indexOf("https://") >= 0 ||
          LowerName.indexOf("http://") >= 0
        ) {
          this.Active = 2;
          this.$message({
            showClose: true,
            message: "正在解析：" + name
          });
          this.VideoUrl = name;
        } else {
          //根据名称查询数据
          this.GetData(1, name);
        }
      }
    },
    //查询数据
    GetData(page, name) {
      const loading = this.$loading({
        lock: true,
        text: "Loading",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)"
      });
      this.$axios
        .get(`provide/vod/?ac=detail&wd=${name}&pg=${page}`)
        .then(res => {
          if (res.data.list.length == 0) {
            this.$message({
              showClose: true,
              message: `${name}的查询数据为空，请更换关键字后重试！`,
              type: "warning"
            });
            this.Active = 0; //数据为空
          } else {
            this.DataList = res.data.list;
            this.$message({
              showClose: true,
              message: "数据加载成功！",
              type: "success"
            });
            this.Active = 1; //展示数据
            this.pagecount = res.data.pagecount;
          }
          loading.close();
        })
        .catch(error => {
          loading.close();
          this.$message({
            showClose: true,
            message: error,
            type: "error"
          });
        });
    },
    ToApp() {
      window.open("http://m3w.cn/__uni__674665a");
    },
    //分页切换
    CurrentChange(val) {
      this.GetData(val, this.Vod_Name);
    },
    play() {
      console.log("play callback");
    },
    //切换播放源
    switchHandle(url) {
      this.$nextTick(() => {
        this.player = this.$refs.player.dp; //获取Dplayer对象
        this.player.switchVideo({
          url: url,
          type: "auto"
        });
        this.player.play();
      });
    },
    //格式化文本
    FormatTxt(val) {
      if (val == "" || val == null) {
        return "未知";
      } else {
        return val;
      }
    }
  }
};
</script>

<style lang="less" scoped>
.logo {
  margin-right: 0px;
  width: 60px;
  height: 60px;
}
.title {
  text-align: left;
  line-height: 70px;
  color: aliceblue;
}
.title:hover,
.logo:hover {
  cursor: pointer;
}
#home {
  margin-bottom: 100px;
}
#datalist {
  border: 1px solid #fff;
  border-radius: 5px;
  padding: 20px 0px;
  h3,
  a {
    color: #409eff;
    margin-bottom: 10px;
  }
  p {
    font-size: small;
    line-height: 25px;
  }
  img {
    width: 100%;
    height: 220px;
    border-radius: 5px;
  }
  img:after {
    content: "图片加载失败";
    display: inline-block;
    position: relative;
    z-index: 2;
    top: 0;
    left: 0;
    width: 144px;
    height: 220px;
    background-image: url("../assets/img/error.jpg");
    background-size: cover;
    background-color: rgba(191, 191, 191, 0.18);
  }
  .el-button {
    bottom: 0px;
    margin-bottom: 0px;
  }
}
// #datalist:hover,
#datalist img:hover {
  box-shadow: 0 4px 8px rgba(40, 40, 40, 0.2);
}
.el-pagination {
  margin-top: 20px;
}
.el-row:nth-child(2),
.el-row:nth-child(3) {
  margin-top: 30px;
}
.el-row:nth-child(3) {
  position: relative;
  margin-bottom: 50px;
  z-index: 100;
}
.dplayer-menu {
  display: none !important;
}
.box-card {
  opacity: 0.95;
}
#analysis_view {
  width: 100%;
  height: calc(100vw / 3);
}
.content {
  padding-top: 100px;
  position: relative;
  z-index: 103;
}
.focus_ovr {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  background: black;
  transition: 0.5s;
  opacity: 0.6;
  z-index: 101;
}
</style>
