var U1 = 'http://jx.598110.com/?url=';
var U2 = 'https://vip.mpos.ren/v/?url=';
var U3 = 'http://jx.618ge.com/jx/1.php?url=';
var U4 = 'https://api.sigujx.com/?url=';
var U5 = 'https://www.kkflv.com/index.php?url=';

/* 播放 */
function Play() {
    if ($("#Url").val() == "") {
        toastr.warning('视频播放路径不能为空！');
    }
    else {
        $("#test").attr("src", U4 + $("#Url").val());
    }
}
/* 切换线路 */
function switchUrl(ID) {
    if ($("#Url").val() == "") {
        toastr.warning('视频播放路径不能为空！');
    } else {
        console.info(ID);
        if (ID == 1) {
            $('#ty').text('线路一');
            $("#test").attr("src", U1 + $("#Url").val());
            toastr.info('视频播放线路已切换成线路'+ID+'！');
        } else if (ID == 2) {
            $('#ty').text('线路二');
            $("#test").attr("src", U2 + $("#Url").val());
            toastr.info('视频播放线路已切换成线路' + ID + '！');

        } else if (ID == 3) {
            $('#ty').text('线路三');
            $("#test").attr("src", U3 + $("#Url").val());
            toastr.info('视频播放线路已切换成线路' + ID + '！');

        } else if (ID == 4) {
            $('#ty').text('线路四');
            $("#test").attr("src", U4 + $("#Url").val());
            toastr.info('视频播放线路已切换成线路' + ID + '！');

        } else {
            $('#ty').text('默认线路');
            $("#test").attr("src", U5 + $("#Url").val());
            toastr.info('视频播放线路已切换成线路默认线路！');
        }
    }
}
