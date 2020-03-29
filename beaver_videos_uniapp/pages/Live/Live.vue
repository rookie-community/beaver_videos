<template>
	<view>
		<!-- <view class="example-info">以下数据均来自互联网</view> -->
		<view class="example-title">中央电视台</view>
		<uni-list>
			<uni-list-item :show-arrow="false" v-for="(item,index) in List1" :key="index" @tap="Play(item.title,item.LiveUrl)" :title="item.title" thumb="https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=1862633852,1143600141&fm=26&gp=0.jpg" />
		</uni-list>

		<view class="example-title">地方电视台</view>
		<uni-list>
			<uni-list-item :show-arrow="false" v-for="(item,index) in List2" :key="index" @tap="Play(item.title,item.LiveUrl)" :title="item.title" thumb="https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=4064750605,2764029787&fm=26&gp=0.jpg" />
		</uni-list>
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
				//中央电视台
				List1: [{
						title: 'CCTV-1',
						PicUrl: '',
						LiveUrl: 'http://cctvalih5ca.v.myalicdn.com/live/cctv1_2/index.m3u8'
					},
					{
						title: 'CCTV-2',
						PicUrl: '',
						LiveUrl: 'http://cctvalih5ca.v.myalicdn.com/live/cctv2_2/index.m3u8'
					},
					{
						title: 'CCTV-3',
						PicUrl: '',
						LiveUrl: 'http://cctvalih5ca.v.myalicdn.com/live/cctv3_2/index.m3u8'
					},
					{
						title: 'CCTV-6',
						PicUrl: '',
						LiveUrl: 'http://cctvalih5ca.v.myalicdn.com/live/cctv6_2/index.m3u8'
					},
					{
						title: 'CCTV-8',
						PicUrl: '',
						LiveUrl: 'http://cctvalih5ca.v.myalicdn.com/live/cctv8_2/index.m3u8'
					},
					{
						title: 'CCTV-10',
						PicUrl: '',
						LiveUrl: 'http://cctvalih5ca.v.myalicdn.com/live/cctv10_2/index.m3u8'
					},
					{
						title: 'CCTV-13',
						PicUrl: '',
						LiveUrl: 'http://cctvalih5ca.v.myalicdn.com/live/cctv13_2/index.m3u8'
					}
				],

				//地方电视台
				List2: [{
						title: '安徽卫视',
						PicUrl: '',
						LiveUrl: 'http://cctvtxyh5ca.liveplay.myqcloud.com/wstv/anhui_2/index.m3u8'
					},
					{
						title: '浙江卫视',
						PicUrl: '',
						LiveUrl: 'http://ali.m.l.cztv.com/channels/lantian/channel01/360p.m3u8'
					},
					{
						title: '东方卫视',
						PicUrl: '',
						LiveUrl: 'http://cctvtxyh5ca.liveplay.myqcloud.com/wstv/dongfang_2/index.m3u8'
					},
					{
						title: '北京卫视',
						PicUrl: '',
						LiveUrl: 'http://cctvtxyh5ca.liveplay.myqcloud.com/wstv/btv1_2/index.m3u8'
					}
				]

			}
		},
		methods: {
			Play(title,url){
				// uni.showModal({
				// 	content:'../../pages/Live/LivePlay?url='+url+'&title='+title
				// })
				uni.navigateTo({
					url:'../../pages/Live/LivePlay?url='+url+'&title='+title
				})
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
</style>
