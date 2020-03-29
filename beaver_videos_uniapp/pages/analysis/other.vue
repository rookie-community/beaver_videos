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
			console.log(this.url)
		},
		methods: {

		}
	}
</script>

<style>

</style>
