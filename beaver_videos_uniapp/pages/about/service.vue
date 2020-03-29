<template>
	<web-view :webview-styles="webviewStyles" src="https://cschat-ccs.aliyun.com/index.htm?tntInstId=_19cYQHA&scene=SCE00005212"></web-view>
</template>

<script>
	export default {
		data() {
			return {
				webviewStyles: {
					progress: {
						color: '#2da0f4'
					}
				}
			}
		}
	}
</script>

<style>
</style>
