<template>
	<view>
		<web-view :webview-styles="webviewStyles" :src="url"></web-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				url: '',
				webviewStyles: {
					progress: {
						color: '#2da0f4'
					}
				}
			}
		},
		onLoad(e) {
			this.url = e.url;
			// console.log('网站', this.url)
		},
		onReady() {
			if (getApp().globalData.static == false) {
				uni.showModal({
					title: '使用教程',
					content: '在视频播放页面点击右上角的解析按钮，就可以在线观看收费视频了',
					showCancel: true,
					cancelText: '明白了', //取消按钮
					confirmText: '不再提示', //确定按钮
					success: res => {
						if (res.confirm) {
							getApp().globalData.static = true;
							// console.log('用户点击确定');
						} else if (res.cancel) {
							// console.log('用户点击取消');
						}
					},
					fail: () => {},
					complete: () => {}
				});
			}
		},
		onBackPress(e) { //监听返回按钮事件
			uni.switchTab({
				url: '/pages/analysis/analysis'
			});
			return true; // return true 表示禁止默认返回
		},
		onNavigationBarButtonTap(e) {
			// console.log(e); //获取导航栏按钮对象
			if (e.text == "解析") {
				var currentWebview = this.$mp.page.$getAppWebview(); //获取当前页面的webview对象	
				var wv = currentWebview.children()[0];
				// setTimeout(function() {
				// 	// wv.evalJS(`
				// 	//     alert(window.location.href)
				// 	// `);
				// }, 1000); //如果是页面初始化调用时，需要延时一下
				var VideoUrl = wv.getURL();//视频播放地址
				var OldUrl = this.url;//当前网站初始页面
				uni.showActionSheet({
				    itemList: ['打开新页面播放（推荐）','在当前页面播放（开发中...）'],
				    success: function (res) {
						// console.log('选中了第' + (res.tapIndex + 1) + '个按钮');
						if (res.tapIndex==0) {
							uni.navigateTo({
								url: '../../pages/analysis/Play?VideoUrl='+VideoUrl+'&OldUrl='+OldUrl
							});
						} else{
							console.log('../../pages/analysis/Play?VideoUrl='+VideoUrl+'&OldUrl='+OldUrl);
							uni.showToast({
								title: '暂未开放',
								icon: 'none'
							});
						}
				    },
				    fail: function (res) {
				        console.log(res.errMsg);
				    }
				});
			}
		},
		methods: {

		}
	}
</script>

<style>

</style>
