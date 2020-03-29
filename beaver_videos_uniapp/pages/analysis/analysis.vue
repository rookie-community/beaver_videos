<template>
	<view class="warp">
		<!-- #ifndef H5 -->
		<view class="example-body">
			<uni-notice-bar :show-icon="true" :scrollable="true" :single="true" text="免责声明:小狸视频APP不做任何数据储存服务,一切数据来自于互联网,本APP仅用作学习交流使用,若有侵权信息,请联系管理员处理!" />
		</view>
		<!-- #endif -->
		<view class="example-title">进入下方影视页面后，本APP可对影视进行解析</view>
		<view class="example-title">常用影视(推荐)</view>
		<view class="example-body">
			<uni-grid :column="3" :highlight="true" @change="change">
				<uni-grid-item v-for="(item, index) in list" :key="index">
					<image :src="item.url" class="image" mode="aspectFill" />
					<text class="text">{{ item.text }}</text>
				</uni-grid-item>
			</uni-grid>
		</view>
		<view class="example-title">其他影视</view>
		<view class="example-body">
			<uni-grid :column="4" border-color="#03a9f4" @change="changeother" class="other">
				<uni-grid-item v-for="(item, index) in list2" :key="index">
					<image :src="item.url" class="image" mode="aspectFill" />
					<text class="text">{{item.text}}</text>
				</uni-grid-item>
			</uni-grid>
		</view>
	</view>
</template>

<script>
	import uniGrid from '@/components/uni-ui/uni-grid/uni-grid.vue'
	import uniGridItem from '@/components/uni-ui/uni-grid-item/uni-grid-item.vue'
	import uniNoticeBar from '@/components/uni-ui/uni-notice-bar/uni-notice-bar.vue'
	export default {
		components: {
			uniGrid,
			uniGridItem,
			uniNoticeBar
		},
		data() {
			return {
				list: [{
						url: '/static/List/qqlogo.png',
						text: '腾讯视频',
						PlayUrl:'https://v.qq.com/'
					},
					{
						url: '/static/List/iqiyilogo.png',
						text: '爱奇艺',
						PlayUrl:'https://www.iqiyi.com/'
					},
					{
						url: '/static/List/sohulogo.png',
						text: '搜狐视频',
						PlayUrl:'https://tv.sohu.com/'
					},
					{
						url: '/static/List/hunantvlogo.png',
						text: '芒果TV',
						PlayUrl:'https://www.mgtv.com/'
					},
					{
						url: '/static/List/pptv.png',
						text: 'PPTV',
						PlayUrl:'http://www.pptv.com/'
					},
					{
						url: '/static/List/youkulogo.png',
						text: '优酷',
						PlayUrl:'https://www.youku.com/'
					},
					{
						url: '/static/List/tudoulogo.png',
						text: '土豆',
						PlayUrl:'https://www.tudou.com/'
					},
					{
						url: '/static/List/fengxing.png',
						text: '风行网',
						PlayUrl:'http://www.fun.tv/'
					},
					{
						url: '/static/List/bilibili.png',
						text: 'bilibili',
						PlayUrl:'https://www.bilibili.com/'
					}
				],
				list2:[
					{
						url: 'https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3552292615,4043486314&fm=26&gp=0.jpg',
						text: '全网免VIP',
						PlayUrl:'http://www.iuk.me/'
					},
					{
						url: 'https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=853244643,1268956070&fm=26&gp=0.jpg',
						text: '乐乐影视',
						PlayUrl:'https://www.leleysw.com/'
					},
					{
						url: 'https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1536806670,2481157260&fm=26&gp=0.jpg',
						text: 'VIP电影站1',
						PlayUrl:'http://www.vipdianying.net/'
					},
					{
						url: 'https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=3145652234,4236782052&fm=26&gp=0.jpg',
						text: 'VIP电影站2',
						PlayUrl:'http://www.vip1280.cn/'
					}
				]
			}
		},
		methods: {
			change(e) {
				let {
					index
				} = e.detail;//获取选中对象的下标
				uni.navigateTo({
					url:'../../pages/analysis/centent?url='+this.list[index].PlayUrl
				})
				// console.log(index);
			},
			changeother(e) {
				let {
					index
				} = e.detail;
				uni.navigateTo({
					url:'../../pages/analysis/other?url='+this.list2[index].PlayUrl
				});
				// console.log(index);
			}
		}
	}
</script>

<style>
	page {
		display: flex;
		flex-direction: column;
		box-sizing: border-box;
		background-color: #efeff4
	}

	view {
		font-size: 28upx;
		line-height: inherit
	}

	.example {
		padding: 0 30upx 30upx
	}

	.example-title {
		display: flex;
		justify-content: space-between;
		align-items: center;
		font-size: 32upx;
		color: #464e52;
		padding: 30upx 30upx 30upx 50upx;
		margin-top: 20upx;
		position: relative;
		background-color: #fdfdfd;
		border-bottom: 1px #f5f5f5 solid
	}

	.example-title__after {
		position: relative;
		color: #031e3c
	}

	.example-title:after {
		content: '';
		position: absolute;
		left: 30upx;
		margin: auto;
		top: 0;
		bottom: 0;
		width: 6upx;
		height: 32upx;
		background-color: #ccc
	}

	.example .example-title {
		margin: 40upx 0
	}

	.example-body {
		padding: 30upx;
		background: #fff
	}

	.example-info {
		padding: 30upx;
		color: #3b4144;
		background: #fff
	}

	.image {
		width: 150upx;
		height: 50upx;
	}

	.other uni-grid-item image {
		width: 50upx;
		height: 50upx;
	}

	.text {
		font-size: 26upx;
		margin-top: 10upx;
	}
</style>
