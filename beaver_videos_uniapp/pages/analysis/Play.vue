<template>
	<view class="content">
		<web-view :webview-styles="webviewStyles" :src="PlayUrl"></web-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				webviewStyles: {
					progress: {
						color: '#FF3333'
					}
				},
				PlayUrl: '', //完整播放链接
				Api: ['https://z1.m1907.cn/?jx=', 'http://yun.mt2t.com/lines?url=','https://jx.688ing.com/?search=', 'http://ok.jlsprh.com/g/?url=',
					'https://jiexi.380k.com/?url=','http://jx.wfxzzx.cn/?url=', 'http://jx.618ge.com/jx/3.php?url='
				],
				APIName: ['默认', '线路二', '线路三', '[含广告]线路四','[含广告]线路五','[含广告]线路六','[含广告]线路七'], //API接口对应的名称
				Active: 0, //选中的API选项
				VideoUrl: '', //视频地址
				OldUrl: '', //上一级目录地址
			}
		},
		onLoad(e) {
			this.OldUrl = e.OldUrl;
			this.VideoUrl = e.VideoUrl;
			this.PlayUrl = this.Api[0] + this.VideoUrl;
		},
		onShow() {
			//解除锁定屏幕方向
			plus.screen.lockOrientation(['landscape', 'portrait-primary']);
			// plus.screen.unlockOrientation();
		},
		//卸载当前页面
		onUnload() {
			//禁止横屏
			plus.screen.lockOrientation("portrait-primary");
		},
		onBackPress(e) { //监听返回按钮事件
			uni.redirectTo({
				url: '../../pages/analysis/centent?url=' + this.OldUrl
			});
			return true; // return true 表示禁止默认返回
		},
		//导航栏按钮单击事件
		onNavigationBarButtonTap(e) {
			this.chang();
			console.log(this.APIName);
		},
		methods: {
			chang() {
				let item = JSON.parse(JSON.stringify(this.APIName));
				item[this.Active] += "(当前使用)";
				uni.showActionSheet({
					itemList: item,
					success: res => {
						this.PlayUrl = this.Api[res.tapIndex] + this.VideoUrl;
						this.Active = res.tapIndex;
					}
				});
			}
		}
	}
</script>
<style>
</style>
