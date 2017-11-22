var sPlayer = {
    soundPlayer: null,
    initRadio: function () {
        if (helper.isMobile() || !sChat.getSetting('radio') || !sConfig.radioServer) return;
        this.appendPlayer('#onlineListContainer', 'jp_radio');
        var stream = { title: 'SocioFM', mp3: sConfig.radioServer + '/radio' }, ready = false;
        $("#jp_radio").jPlayer({
            ready: function (event) {
                ready = true;
                $(this).jPlayer("setMedia", stream);
            },
            pause: function () {
                $(this).jPlayer("clearMedia");
            },
            error: function (event) {
                if (ready) {
                    switch (event.jPlayer.error.type) {
                        case $.jPlayer.error.URL_NOT_SET:
                            $(this).jPlayer("setMedia", stream).jPlayer("play");
                            break;
                        case $.jPlayer.error.URL:
                            $(this).jPlayer("stop").jPlayer("clearMedia");
                            break;
                    }
                }
            },
            errorAlerts: true,
            solution: "html",
            supplied: "mp3",
            preload: "none",
            wmode: "window",
            useStateClassSkin: true,
            autoBlur: false,
            keyEnabled: true,
            cssSelectorAncestor: '#jp_radio_cont'
        });
        setInterval('sChat.paramString+="&radio=true";', 20000);
    },
    appendPlayer: function (sel, id) {
        switch (id) {
            case 'jp_radio':
                {
                    $(document.body).append('<div id="jp_radio" class="jp-jplayer"></div>');
                    $(sel).append('<div id="jp_radio_cont" style="position: absolute;padding: 5px;background-color: #eee;bottom: 0px;" class="jp-audio-stream" role="application" aria-label="media player"><div class="jp-type-single"><div class="jp-gui jp-interface"><div class="jp-controls"><button class="jp-play" role="button" tabindex="0">play</button></div><div class="jp-volume-controls"><button class="jp-mute" role="button" tabindex="0">mute</button><button class="jp-volume-max" role="button" tabindex="0">max volume</button><div class="jp-volume-bar"><div class="jp-volume-bar-value"></div></div></div></div><div class="jp-details"><div class="jp-title" aria-label="title">&nbsp;</div></div><div class="jp-no-solution"><span>Update Required</span>To play the media you will need to either update your browser to a recent version or update your <a href="http://get.adobe.com/flashplayer/" target="_blank">Flash plugin</a>.</div></div></div>');
                    break;
                }
            default: break;
        }
    },
    handleRadio: function (json) {
        if (!json || !json[0]) return;
        $('#jp_radio_cont .jp-title').text(json[0].title || json[0].name);
    },
    initSounds: function () {
        $('<div id="jp_sounds" class="jp-jplayer"></div>').appendTo(document.body).jPlayer({
            ready: function () { sPlayer.soundPlayer = $(this); },
            errorAlerts: true, swfPath: "flash/", solution: "html, flash", supplied: "mp3"
        });
    },
    playSound: function (s) {
        if (this.soundPlayer && sChat.getSetting('audio')) this.soundPlayer.jPlayer("setMedia", { mp3: 'sounds/' + s + '.mp3' }).jPlayer('play');
    },
    setAudioVolume: function (volume) {
        volume = parseFloat(volume);
        if (this.soundPlayer && !isNaN(volume)) {
            if (volume < 0) volume = 0.0;
            else if (volume > 1) volume = 1.0;
            sChat.setSetting('audioVolume', volume);
            this.soundPlayer.jPlayer("volume", volume);
        }
    },
}