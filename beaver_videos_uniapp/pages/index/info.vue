<template>
	<view class="content">
		<view class="example-body">
			<view class="example-box content" style="margin-left: 25upx;">
				<view class="uni-flex uni-row">
					<view class="uni-flex" style="width: 300upx;">
						<image :src="DataInfo.PicUrl" style="height:350upx;width:250upx;margin-left: 20upx;border-radius: 10upx;"></image>
					</view>
					<view class="uni-flex uni-column">
						<view class="title">{{DataInfo.VideoName}}</view>
						<view class="text">地区:{{DataInfo.Region}}</view>
						<view class="text">时长:{{DataInfo.LongTime}}分钟</view>
						<view class="text">导演:{{DataInfo.Director}}</view>
						<view class="text">分类:{{DataInfo.ClassName}}</view>
						<view class="text">语言:{{DataInfo.Language}}</view>
					</view>
				</view>
			</view>
		</view>

		<view class="example-title">剧集：{{list[0]}}</view>
		<view class="example-body">
			<view class="example-box content" style="padding: 30upx;">
					<video :src="list[1]" style="width: 100%;"></video>
			</view>
		</view>
		<view class="example-title">简介</view>
		<view class="example-body">
			<view class="example-box" style="padding: 20upx;font-size: 1.2em;">
				{{DataInfo.Brief}}
			</view>
		</view>
	</view>
</template>

<script>
	import Movie from "../../static/Json/VideoInfo.json";
	import uniCard from '@/components/uni-ui/uni-card/uni-card.vue'
	export default {
		data() {
			return {
				DataInfo: [],
				Active: -1,
				list:[]
			}
		},
		onLoad(e) {
			this.DataInfo=Movie[e.id];
			this.list=this.DataInfo.ckm3u8.split('$');
		},
		onShow() {
		},
		methods: {
			// Play(url, i) {
			// 	this.Active = i;
			// 	uni.navigateTo({
			// 		url: '../Play/Play?url=' + url
			// 	})
			// },
			// Formaster(val, i) {
			// 	if ((val == '电视剧' || val == '动漫') && i < 9) {
			// 		return 0
			// 	} else {
			// 		return ''
			// 	}

		},
		components: {
			uniCard
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

	.example-body {
		padding: 30upx 0;
	}

	.example-box {
		margin-bottom: 30upx;
	}

	.example-box:last-child {
		margin-bottom: 0;
	}

	.image-box {
		width: 100%;
		height: 350upx;
		overflow: hidden;
	}

	.image-box .image {
		width: 100%;
		height: 100%;
	}

	.content-box {
		padding-top: 20upx;
	}

	.footer-box {
		display: flex;
		justify-content: space-between;
		width: 100%;

	}

	.footer-box__item {
		width: 100%;
		display: flex;
		align-items: center;
	}

	.footer-box__item:nth-child(2) {
		justify-content: center;
	}

	.footer-box__item:last-child {
		justify-content: flex-end;
	}

	.title {
		font-size: 1em;
		line-height: 60upx;
		font-weight: bold;
	}

	.text {
		line-height: 60upx;
		font-size: 0.8rem;
	}

	button {
		margin: 10upx;
	}
</style>
