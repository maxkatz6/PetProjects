var sWebCam = {
    username: "",
    room: null,
    webrtc : null,
    modal: false,
    ready: false,
    pause: false,
    stopped: false,
    owner: false,
    priv: null,
    mc:0,
    dcReady: false,
    init: function()
    {
        if (!helper.getWebRTCInfo().supportDataChannel)
            sChat.addChatBotMessageToChatList('Внимание! У вас не поддерживается передача данных по каналам. Некоторые функции не будут работать. Если кто-то пнет кодера, то испралю быстрее :\\');
        this.webrtc = new SimpleWebRTC({
            localVideoEl: 'local',
            remoteVideosEl: '',
            media: {video: (sChat.getSetting('myVideo')?{mandatory: {
                            maxFrameRate: sConfig.vidFPS,
                            maxWidth: sConfig.maxVidW,
                            maxHeight: sConfig.maxVidH}}:false),
                    audio: true},
            autoRequestMedia: true,
            debug: true,
            detectSpeakingEvents: false,
            signalingOptions: {'forceNew' : true}
            //autoAdjustMic: true
        });
        this.webrtc.on('videoAdded', function (video, peer) {sWebCam.modalVideo(sWebCam.webrtc.getDomId(peer), video);});
        this.webrtc.on('videoRemoved', function (video, peer) {$('#' +(peer ? 'cont' + sWebCam.webrtc.getDomId(peer):'localContainer')).parent().remove();});
        this.webrtc.once('readyToCall', function () {sWebCam.ready = true;});
        this.webrtc.on('mute', function (data) {
        sWebCam.webrtc.webrtc.getPeers(data.id).forEach(function (peer) {
                var id = sWebCam.webrtc.getDomId(peer);
                if (data.name === 'audio') {
                    $('#'+id).toggleClass('unmute mute');
                    id = '#cont'+id;
                    $(id+' div.mute, '+ id +' div.unmute').css('display', 'none');
                } else if (data.name === 'video') {
                    $('#'+id).hide();
                    $('#cont'+id+' .tkIcon').hide();
                } 
            });
        });
        this.webrtc.on('unmute', function (data) { // hide muted symbol
            sWebCam.webrtc.webrtc.getPeers(data.id).forEach(function (peer) {
                var id = sWebCam.webrtc.getDomId(peer);
                if (data.name === 'audio') {
                    $('#'+id).toggleClass('unmute mute');
                    id = '#cont'+id;
                    $(id+' div.mute, '+ id +' div.unmute').css('display', 'block');
                } else if (data.name === 'video') {
                    $('#'+id).show();
                    $('#cont'+id+' .tkIcon').show();
                }
            });
        });
        sWebCam.webrtc.on('channelMessage', function (peer, label, data) {
            switch (data.type)
            {
                case 'sendUserInfo':
                    $('#cont'+sWebCam.webrtc.getDomId(peer)).parent().find('span.ui-dialog-title').text(data.payload.username);
                    if (!data.payload.videoChat)
                        $('#'+sWebCam.webrtc.getDomId(peer)).hide();
                    break
                case 'getUserInfo':
                    sWebCam.send(peer, "sendUserInfo",{username:sWebCam.username, videoChat: sChat.getSetting('myVideo')});
                    break;
                case 'channelPrivate':
                    sWebCam.priv = data.payload;
                    break;
                case 'close':
                    if (data.payload === undefined || data.payload === sWebCam.username) 
                        sWebCam.close();
                    break;
                case 'reconnect':
                    if (data.payload === undefined) 
                        sWebCam.joinRoom(sWebCam.room, sWebCam.owner);
                    else sWebCam.joinRoom(data.payload.room);
            }
        });
        sWebCam.webrtc.on('channelOpen', function (peer) {
            sWebCam.dcReady = true;
            sWebCam.send(peer, "getUserInfo");
            if (sWebCam.owner)
                sWebCam.send(peer, "channelPrivate", sWebCam.priv);
        });
    },
    send: function(peer, type, msg){   
        this.sendToAll(type, msg);
        //sWebCam.webrtc.sendDirectlyToAll(peer.id, label, msg);
    },
    sendToAll: function(type, msg){
        if (this.dcReady)
            this.webrtc.sendDirectlyToAll('simplewebrtc', type, msg);
        else setTimeout(function(){sWebCam.sendToAll(type,msg);},500);
    },
    joinRoom: function(room, owner){
        this.owner = owner !== undefined && owner;
        if (!this.modal)  this.modalVideo();
        if (!this.webrtc) this.init();
        if (this.stopped) {this.webrtc.startLocalVideo(); this.stopped = false;}
        if (this.ready){
            if (this.room !== null) 
                sWebCam.webrtc.leaveRoom(this.room);
            sWebCam.webrtc.joinRoom(room);
            this.room = room;
            this.mc = this.modal ? 1 : 0;
        }
        else setTimeout(function(){sWebCam.joinRoom(room,owner);},500);
    },
    createRoom: function(priv){
        this.priv = priv;
        var key = helper.makeKey();
        this.joinRoom(key, true);
        return key;
    },
    close: function(domId){
        if (domId === undefined){
            if (this.owner && this.priv)
                this.sendToAll('close');
            this.webrtc.leaveRoom(this.room);
            this.webrtc.stopLocalVideo();
            location.reload();
            /*//this.webrtc.disconnect();
            this.priv = this.room = null;
            this.dcReady = this.owner = this.pause = this.ready = this.modal = false;
            this.stopped = true;
            this.mc = 0;
            $(".ui-dialog").remove();*/
        }
    },
    modalVideo: function(domId, video){
        var me = domId === undefined;
        if (me) 
            this.modal = true;
        
        if (!me && $('#'+domId).length)
            return;
        var f = function() {
            var xw = $('#chatList').width()/(sConfig.defCamW+100);
            var y = 10+Math.floor(sWebCam.mc/xw)*(sConfig.defCamH+20),
            x = 10+Math.floor(sWebCam.mc%xw)*(sConfig.defCamW+20);
            sWebCam.mc++;
            return {at:"left+"+x + " top+"+y,my: "left top", of:"#chatList"};
        };
   
        $('<div id="'+(me?'localContainer':'cont'+domId)+'"></div>').
            append($(me? '<video id="local"></video>' : video).addClass('sVideo unmute').css('display',me && !sChat.getSetting('myVideo')? 'none':'block')).append('<div style="background: url(img/no_video.png) no-repeat center;height: inherit;opacity: 0.3;"></div>').
            append('<div class="sVideoToolbar"><svg class="tkIcon" onclick="sWebCam.pauseResume(this,'+me+')"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="img/icons.svg#pause" x="0" y="0" width="36" height="36"></use></svg></div>').
            dialog({title:(me?"Моя камера":""),height:sConfig.defCamH,width:sConfig.defCamW,position:f(),close: function(){sWebCam.close(domId);}});

    },
    pauseResume: function(s,me){		
        s = $(s);
        var u = s.children();
        if (u.attr('xlink:href') === "img/icons.svg#pause")
            u.attr('xlink:href','img/icons.svg#play');
        else 
            u.attr('xlink:href','img/icons.svg#pause');


        s = s.parent().parent().children()[0];
        if (me && sChat.getSetting('myVideo')){
            if (this.pause)
                this.webrtc.resume();
            else 
                this.webrtc.pause();
            $(s).toggle();
            this.pause = !this.pause;
        }
        else {
            if (s.paused) s.play();
            else s.pause();
            $(s).toggleClass('pause');
        }
        $(s).toggleClass('unmute mute');
    }
};