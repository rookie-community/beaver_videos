<template>
	<view class="uni-padding-wrap uni-common-mt">
		<video id="myVideo" :src="url" autoplay="true" controls="false" objectFit="fill"></video>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				url: '',
				title: ''
			}
		},
		onLoad(e) {
			this.url = e.url;
			this.title = e.title;
			this.load();
		},
		onReady(res) {
			this.videoContext = uni.createVideoContext('myVideo');
			this.videoContext.requestFullScreen();
		},
		methods: {
			load() {
				uni.setNavigationBarTitle({
					title: this.title + '-直播',
				})
			}
		}
	}
</script>

<style>
	.uni-common-mt {
		height: 100%;
	}
	video{
		width: 100%;
	}
</style>
