<template>
	<view>
		<view class="uni-margin-wrap">
			<swiper class="swiper" circular :indicator-dots="indicatorDots" :autoplay="autoplay" :interval="interval" :duration="duration">
				<swiper-item v-for="(item,index) in backgrounds" :key="index">
					<image class="img" :src="item.PicUrl"></image>
				</swiper-item>

			</swiper>
		</view>
		<view class="example-title">电影推荐</view>
		<view class="example-body">
			<view v-for="(item,index) in Movies" v-show="index<Top" :key="index" class="example-item" :data-id="index" @tap="ToInfo">
				<image :src="item.PicUrl||defaultSrc" @error="imageError(e)"></image>
				<view>{{item.VideoName}}</view>
			</view>
		</view>
		<view class="loading">
			<text>
				{{loadingType === 0 ? contentText.contentdown : (loadingType === 1 ? contentText.contentrefresh : contentText.contentnomore)}}
			</text>
		</view>
	</view>
</template>

<script>
	import uniLoadMore from '@/components/uni-ui/uni-load-more/uni-load-more.vue';
	import Movie from "../../static/Json/VideoInfo.json";
	import uniCard from '@/components/uni-ui/uni-card/uni-card.vue';
	export default {
		components: { //注册组件
			uniLoadMore
		},
		data() {
			return {
				defaultSrc:'',//默认图片路径
				//轮播图
				backgrounds: [{
						PicUrl: '../../static/Carousel/1.jpg',
						MoviesName: '119',
					},
					{
						PicUrl: '../../static/Carousel/2.jpg',
						MoviesName: '111',
					},
					{
						PicUrl: '../../static/Carousel/3.jpg',
						MoviesName: '11-1',
					}
				],
				indicatorDots: true,
				autoplay: true,
				interval: 3000,
				duration: 500,

				Top: 4,
				Movies: [], //默认显示4条数据

				loadingText: '加载中...',
				loadingType: 0, //定义加载方式 0---contentdown  1---contentrefresh 2---contentnomore
				contentText: {
					contentdown: '上拉显示更多',
					contentrefresh: '正在加载...',
					contentnomore: '没有更多数据了'
				}
			}
		},
		onLoad() {
			this.Movies = Movie;
		},
		//下拉刷新
		onPullDownRefresh() {
			setTimeout(function() {
				uni.stopPullDownRefresh();
			}, 1000);
		},
		//触底的时候请求数据，即为上拉加载更多
		onReachBottom() {
			if (this.Top>=this.Movies.length) {
				this.loadingType=2;
			} else{
				this.Top+=4;
				this.loadingType=1;
				uni.showNavigationBarLoading(); //显示加载动画
				setTimeout(function() {
					uni.hideNavigationBarLoading(); //关闭加载动画
					this.loadingType=0;
				}, 500);
			}
		},

		//影视搜索
		onNavigationBarSearchInputConfirmed(e) {
			uni.navigateTo({
				url: '../../pages/index/Search?SeacheName=' + e.text
			})
		},
		methods: {
			ToInfo(e) {
				uni.navigateTo({
					url: "../../pages/index/info?id=" + e.currentTarget.dataset.id
				})
			},
			imageError(e){
				this.defaultSrc='https://p.ssl.qhimg.com/t0171e8c76999826ddc.jpg';
			},
		}
	}
</script>

<style>
	.loading {
		text-align: center;
		line-height: 80upx;
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

	.example-body {
		padding: 30upx;
		background: #fff
	}

	.example-item {
		width: 300upx;
		margin-top: 10upx;
		display: inline-block;
		margin-left: 30upx;

		text-align: center;

	}

	.example-item image {
		border-radius: 15upx;
	}

	.example-item view {
		font-size: 1.2em;
		padding: 6upx 0upx;
		overflow: hidden;
		text-overflow: ellipsis;
		white-space: nowrap;
	}

	.swiper {
		height: 400upx;
	}

	.swiper-item {
		display: block;
		height: 400upx;
		line-height: 400upx;
		text-align: center;
	}

	.swiper .img {
		width: 100%;
		height: 400upx;
	}
</style>
