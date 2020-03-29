// 调用 ( e, 消失毫秒, 数组, 向上漂浮距离)
$(document).click(function(e) {
	var list = ['学而时习之',  '有朋自远方来, 不亦乐乎', '人不知而不愠, 不亦君子乎', '三人行 必有我师焉', '择其善者而从之', '其不善者而改之', '学而不思则罔',
		'思而不学则殆','山有木兮木有枝，心悦君兮君不知。','人生若只如初见，何事秋风悲画扇。','十年生死两茫茫，不思量，自难忘。','曾经沧海难为水，除却巫山不是云。',
		'玲珑骰子安红豆，入骨相思知不知。','只愿君心似我心，定不负相思意。','平生不会相思，才会相思，便害相思。','愿得一心人，白头不相离。','山无陵，江水为竭。冬雷震震，夏雨雪。天地合，乃敢与君绝。'
		,'入我相思门，知我相思苦。','桃之夭夭，灼灼其华。','雨打梨花深闭门，忘了青春，误了青春。','去年今日此门中，人面桃花相映红。'
	];
	textUp(e, 2000, list, 200)
})

function textUp(e, time, arr, heightUp) {
	var lists = Math.floor(Math.random() * arr.length);
	var colors = '#' + Math.floor(Math.random() * 0xffffff).toString(16);
	var $i = $('<strong />').text(arr[lists]);
	var xx = e.pageX || e.clientX + document.body.scroolLeft;
	var yy = e.pageY || e.clientY + document.body.scrollTop;

	$('body').append($i);
	$i.css({
		top: yy,
		left: xx,
		color: colors,
		transform: 'translate(-50%, -50%)',
		display: 'block',
		position: 'absolute',
		zIndex: 999999999999
	})
	$i.animate({
		top: yy - (heightUp ? heightUp : 200),
		opacity: 0
	}, time, function() {
		$i.remove();
	})
}
