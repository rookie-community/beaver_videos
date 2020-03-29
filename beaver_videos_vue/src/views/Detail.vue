<template>
  <div class="about" v-loading="loading">
    <el-row type="flex" justify="center">
      <el-col :md="20">
        <el-card>
          <el-row type="flex" justify="center" :gutter="20">
            <el-col :md="18">
              <d-player :options="options" @play="play" ref="player" v-if="PlayStatus"></d-player>
              <iframe
                v-else
                id="analysis_view"
                allowfullscreen="true"
                :src="VideoUrl"
                scrolling="no"
                frameborder="0"
              ></iframe>
              <div class="tip">《{{DataInfo[0].vod_name}}》- 影视简介</div>
              <p v-html="DataInfo[0].vod_content"></p>
            </el-col>
            <el-col :md="6">
              <div class="tip">《{{DataInfo[0].vod_name}}》- 影视详情</div>
              <el-tabs type="border-card">
                <el-tab-pane label="ckm3u8">
                  <el-button
                    type="primary"
                    :class="{'is-plain':active!=index+1}"
                    v-for="(item,index) in ckm3u8_list"
                    :key="item.name"
                    @click="switchHandle(index+1,item.name,item.url)"
                  >{{FormatNum(index)}}</el-button>
                </el-tab-pane>
                <el-tab-pane label="酷云">
                  <el-button
                    type="primary"
                    :class="{'is-plain':active!=index+1+ckm3u8_list.length}"
                    v-for="(item,index) in kuyun_list"
                    :key="item.name"
                    @click="switchHandle(index+1+ckm3u8_list.length,item.name,item.url)"
                  >{{FormatNum(index)}}</el-button>
                </el-tab-pane>
              </el-tabs>
            </el-col>
          </el-row>
          <el-row type="flex" justify="center">
            <el-col :md="24">
              <div class="tip">
                《{{DataInfo[0].vod_name}}》- 迅雷下载：
                <small>点击复制链接，然后使用迅雷打开即可下载该视频文件</small>
              </div>
              <el-table
                ref="multipleTable"
                max-height="400"
                stripe
                :data="Down_url_list"
                tooltip-effect="dark"
                style="width: 100%"
                stripe:true
              >
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column label="序号" type="index" width="50" align="center"></el-table-column>
                <el-table-column prop="name" label="剧集" width="120" align="center"></el-table-column>
                <el-table-column prop="url" label="下载地址" show-overflow-tooltip></el-table-column>
                <el-table-column prop="url" label="操作" align="center">
                  <template slot-scope="scope">
                    <el-button
                      type="success"
                      plain
                      v-clipboard:copy="scope.row.url"
                      v-clipboard:success="onCopy"
                      v-clipboard:error="onError"
                      size="small"
                      icon="el-icon-document-copy"
                    >复制链接</el-button>
                  </template>
                </el-table-column>
                <div slot="empty" style="height:200px;line-height: 200px;">
                  <i class="el-icon-warning-outline">暂无数据</i>
                </div>
              </el-table>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>
<script>
export default {
  data() {
    return {
      loading: false,
      vod_id: "", //影视ID
      DataInfo: [{ vod_name: "" }], //影视数据
      kuyun_list: [], //数据源一
      ckm3u8_list: [], //数据源二
      active: 1, //当前选中播放剧集
      Down_url_list: [], //下载列表
      VideoUrl: "", //视频Url
      PlayStatus: true, //切换酷云或ckm3u8
      options: {
        video: {
          quality: [
            {
              // name: "视频一",
              // url: "http://static.smartisanos.cn/common/video/t1-ui.mp4",
              type: "auto" //视频类型
            }
          ],
          defaultQuality: 0,
          pic: "https://cbu01.alicdn.com/img/ibank/2019/694/265/11144562496.jpg"
        },
        autoplay: false
      },
      player: null
    };
  },
  mounted() {
    let id = this.$route.query.id; //获取路由参数
    if (id == null || "") {
      this.vod_error();
    } else {
      this.vod_id = id;
      this.GetData();
    }
  },
  methods: {
    GetData() {
      this.loading = true;
      this.fullscreenLoading = true;
      this.$axios
        .get("provide/vod/?ac=detail&ids=" + this.vod_id)
        .then(res => {
          if (res.data.list.length == 0) {
            this.vod_error();
          } else {
            this.$message({
              showClose: true,
              message: "数据加载成功！",
              type: "success"
            });
            this.DataInfo = res.data.list;
            let Play_url = res.data.list[0].vod_play_url
              .split("$$$")[0]
              .split("#"); //影视列表一
            this.kuyun_list = this.FormatArray(Play_url); //格式化
            let Play_url2 = res.data.list[0].vod_play_url
              .split("$$$")[1]
              .split("#"); //影视列表二
            this.ckm3u8_list = this.FormatArray(Play_url2); //格式化
            if (
              res.data.list[0].vod_down_url != "" &&
              res.data.list[0].vod_down_url != null
            ) {
              let Down_list = res.data.list[0].vod_down_url.split("#"); //下载列表
              this.Down_url_list = this.FormatArray(Down_list); //格式化
            }
          }
          this.player = this.$refs.player.dp;
          this.switchHandle(
            this.active,
            this.ckm3u8_list[0].name,
            this.ckm3u8_list[0].url
          ); //执行播放
          this.player.play();
          this.loading = false;
        })
        .catch(error => {
          this.loading = false;
          this.$message({
            showClose: true,
            message: error,
            type: "error"
          });
        });
    },
    //播放
    play() {
      console.log("play callback");
    },
    //切换播放源
    switchHandle(index, name, url) {
      this.active = index; //更改选中按钮
      let last = url.split(".")[url.split(".").length - 1];
      //m3u8播放源
      if ("m3u8,mp4".search(last) != -1) {
        this.PlayStatus = true;
        this.$nextTick(() => {
          this.player = this.$refs.player.dp; //获取Dplayer对象
          this.player.switchVideo({
            name: name,
            url: url,
            type: "auto"
          });
          this.player.play();
        });
      } else {
        //酷云播放源
        this.PlayStatus = false;
        this.VideoUrl = url;
        this.$nextTick(() => {});
      }
    },
    //错误跳转
    vod_error() {
      this.$message({
        showClose: true,
        message: "参数有误，正在跳转到首页！",
        type: "error"
      });
      setTimeout(() => {
        this.$router.push("/");
      }, 500);
    },
    //格式化数组
    FormatArray(val) {
      let Newarr = [];
      val.forEach(element => {
        let arr = { name: "", url: "", type: "auto" };
        arr.name = element.split("$")[0];
        arr.url = element.split("$")[1];
        Newarr.push(arr);
      });
      return Newarr;
    },
    //格式化剧集文本按钮
    FormatNum(val) {
      if (val < 9) {
        return "0" + (val + 1);
      } else {
        return val + 1;
      }
    },
    //格式化文本
    FormatTxt(val) {
      if (val == "" || val == null) {
        return "未知";
      } else {
        return val;
      }
    },
    //复制下载链接
    onCopy(e) {
      this.$message.success("内容已复制到剪切板！");
    },
    onError(e) {
      this.$message.error("抱歉，复制失败！");
    }
  }
};
</script>
<style lang="less">
.el-card {
  margin: 20px 0px 50px 0px;
  .el-button {
    margin-top: 10px;
    margin-left: 5px;
  }
}
.el-tabs {
  max-height: 600px;
  overflow: auto;
}
.el-tabs__content {
  padding: 5px !important;
}
.tip {
  padding: 8px 16px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
  margin: 10px;
}
#analysis_view {
  background: #000;
}
</style>