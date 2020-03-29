<script>
	export default {
		//全局变量
		globalData: {
			static:false,
		},
		onLaunch: function() {
			let that = this.$options.globalData;
			console.log('App Launch');
			//应用方向默认竖屏
			// #ifdef APP-PLUS
			plus.screen.lockOrientation("portrait-primary");
			// #endif

		},
		onShow: function() {
			console.log('App Show')
		},
		onHide: function() {
			console.log('App Hide')
		}
	}
</script>

<style>
	/*每个页面公共css */
	/*colorui*/
/* 	@import "./components/colorui/main.css";
	@import "./components/colorui/icon.css"; */
	/*uni-app*/
	@import "./common/icon.css";
	@import "./common/uni.css";
</style>
