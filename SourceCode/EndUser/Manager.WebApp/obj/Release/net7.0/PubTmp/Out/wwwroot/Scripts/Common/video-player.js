$(function () {
    var player = new Plyr('#my-player');
    $('body').on('click', '.player-list-item', function () {
        src = $(this).data("src");
        type = 'video/mp4';
        //poster = $(this).data("poster") || "";

        player.source = {
            type: 'video',
            title: '',
            sources: [
                {
                    src: src,
                    type: type
                }
            ],
            //poster: poster
        };

        //ScrollToElement($(".plyr__video-wrapper"));
        $("#modalPlayVideo").modal("show");

        player.play();
    });

    $("#modalPlayVideo").on('hide.bs.modal', function () {
        if (player) {
            player.pause();
        }
    });
});