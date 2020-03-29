<template>
	<view>
		<view class="content">
			<image src="../../static/logo.png"></image>
		</view>
		<view class="text-area">
			<text>小狸视频</text>
		</view>
		<view class="text-area">
			<text>Version：{{Version}}</text>
		</view>
		<view style="margin-top:80upx;">
			<uni-list>
				<uni-list-item :show-extra-icon="true" :extra-icon="extraIcon1" title="获取最新版本" @tap="NavToNew(1)" />
				<uni-list-item :show-extra-icon="true" :extra-icon="extraIcon2" title="问题反馈" @tap="NavToNew(2)" />
				<uni-list-item :show-extra-icon="true" :extra-icon="extraIcon3" title="联系作者" @tap="NavToNew(3)" />
				<uni-list-item :show-extra-icon="true" :extra-icon="extraIcon4" title="智能客服" @tap="NavToNew(4)" />
				<uni-list-item :show-extra-icon="true" :extra-icon="extraIcon5" title="分享给好友" @tap="NavToNew(5)" />
			</uni-list>
		</view>
	</view>
</template>

<script>
	import uniList from '@/components/uni-ui/uni-list/uni-list.vue'
	import uniListItem from '@/components/uni-ui/uni-list-item/uni-list-item.vue'
	export default {
		components: {
			uniList,
			uniListItem
		},
		data() {
			return {
				Version: '',
				extraIcon1: {
					color: '#007aff',
					size: '22',
					type: 'spinner'
				},
				extraIcon2: {
					color: '#007aff',
					size: '22',
					type: 'help'
				},
				extraIcon3: {
					color: '#0099ff',
					size: '22',
					type: 'qq'
				},
				extraIcon4: {
					color: '#0099ff',
					size: '22',
					type: 'chat'
				},
				extraIcon5: {
					color: '#0099ff',
					size: '22',
					type: 'paperplane'
				}
			}
		},
		onLoad() {
			this.Version = plus.runtime.version;
		},
		methods: {
			NavToNew(i) {
				switch (i) {
					case 1: //获取最新版本
						plus.runtime.openURL('http://m3w.cn/__uni__674665a');
						break;
					case 2: //问题反馈
					uni.navigateTo({
						url:"../../pages/about/feedback"
					})
						break;
					case 3: //联系作者
						plus.runtime.openURL('mqqwpa://im/chat?chat_type=wpa&uin=2270969436',
							function(res) {
								plus.nativeUI.alert("本机没有安装QQ，无法启动");
							});
						break;
					case 4: //智能客服
						uni.navigateTo({
							url: "../../pages/about/service"
						})
						break;
					case 5: //分享给好友
					    uni.share({
					        provider: "weixin",
					        scene: "WXSceneSession",
					        type: 0,
					        href: "http://m3w.cn/__uni__674665a",
					        title: "小狸视频",
					        summary: "我正在使用小狸视频，可以免费看收费影视，赶紧跟我一起来体验！",
					        imageUrl: "https://img.cdn.aliyun.dcloud.net.cn/stream/icon/__UNI__674665A.png",
					        success: function (res) {
								uni.showToast({
									title:'分享成功！'
								})
					            console.log("success:" + JSON.stringify(res));
					        },
					        fail: function (err) {
								uni.showToast({
									title:'暂未开放',
									icon:'none'
								})
					            console.log("fail:" + JSON.stringify(err));
					        }
					    });
						break;
					default:
						break;
				}
			}
		}
	}
</script>

<style>
	body {
		background: #FFFFFF;
	}

	.content {

		text-align: center;
	}

	.text-area {
		padding: 10upx 0upx;
		display: flex;
		justify-content: center;
	}

	.content image {
		margin-top: 150upx;
		width: 200upx;
		height: 200upx;
		border: 1px solid #D0DEE5;
		border-radius: 15upx;
	}
</style>
