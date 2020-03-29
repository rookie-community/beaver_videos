<template>
	<view class="content">
		<web-view id='main' :webview-styles="webviewStyles" :src="PlayUrl"></web-view>
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
				PlayUrl: '',
				// Api: 'https://z1.m1907.cn/?jx=',
				Api: 'https://jx.688ing.com/?search=',
			}
		},
		onLoad(e) {
			this.PlayUrl = this.Api + e.SeacheName;
		},
		//页面显示
		onShow() {
			plus.screen.lockOrientation(['landscape', 'portrait-primary']);
		},
		//页面渲染完成
		onReady() {
			this.test();
		},
		//隐藏页面
		onUnload() {
			//禁止横屏
			plus.screen.lockOrientation("portrait-primary");
		},
		onBackPress(e) { //监听返回按钮事件
		uni.switchTab({
				url: '/pages/index/index'
			});
			// return true 表示禁止默认返回
			return true
		},
		methods: {
			test() {
				var i = setInterval(() => {
					var WebMain = plus.webview.currentWebview(); //获取当前窗口的对象

					//获取页面对象
					const currentWebview = this.$mp.page.$getAppWebview();
					var wx = currentWebview.children()[0];

					//脚本注入
					wx.evalJS(`
				 document.querySelector("#footer").style.display='none';
				 document.title='小狸视频';
				 `);
				 console.log('111')
				}, 100);

				setTimeout(() => {
					console.log('000')
					clearInterval(i)
				}, 5000)
			}
		}
	}
</script>

<style>

</style>
