var sWebCam={
    username:"",
    room:null,
    webrtc:null,
    modal:false,
    ready:false,
    pause:false,
    mute:false,
    stopped:false,
    owner:false,
    priv:null,
    mc:0,
    dcReady:false,
    init:function(){
        if(!helper.getWebRTCInfo().supportDataChannel) sChat.addChatBotMessageToChatList('Внимание! У вас не поддерживается передача данных по каналам. Некоторые функции не будут работать.');
        $.ajax({
            type:"POST",
            url:"https://api.xirsys.com/getIceServers",
            data:{
                ident:"tirraon",
                secret:"2d07f191-e21b-45d2-b5fc-3bd7ce702714",
                domain:"www.socio-party.ru",
                application:"default",
                room:'default',
                secure:0
            },
            success:function(data){sConfig.iceServers=JSON.parse(data).d.iceServers.concat(sConfig.iceServers);},
            async:false
        });
        this.webrtc = new SimpleWebRTC({
            // url:"http://schatsignal.azurewebsites.net:8888/socket.io/",
            localVideoEl:'local',
            remoteVideosEl:'',
            media:{
                video: sChat.getSetting('myVideo') && {
                    mandatory:
                    {
                        maxFrameRate: sConfig.vidFPS,
                        maxWidth: sConfig.maxVidW,
                        maxHeight: sConfig.maxVidH
                    }
                },
                audio:  sChat.getSetting('myAudio')
            },
            autoRequestMedia:true,
            debug:true,
            detectSpeakingEvents:false,
            peerConnectionConfig:sConfig.iceServers
            //autoAdjustMic: true
        });
        this.webrtc.on('videoAdded',function(video,peer){sWebCam.modalVideo(sWebCam.webrtc.getDomId(peer),video);});
        this.webrtc.on('videoRemoved',function(video,peer){$('#'+(peer?'cont'+sWebCam.webrtc.getDomId(peer):'localContainer')).parent().remove();});
        this.webrtc.once('readyToCall',function(){
            sWebCam.ready=true;
        });
        this.webrtc.on('mute',function(data){
            sWebCam.webrtc.webrtc.getPeers(data.id).forEach(function(peer){
                var id=sWebCam.webrtc.getDomId(peer);
                if(data.name==='audio'){
                    $('.'+id+' .ui-dialog-title').attr('class','ui-dialog-title mute');
                    $('.'+id+' .muteBut').hide();
                } else
                    if(data.name==='video'){
                        $('#'+id).hide();
                        $('#cont'+id+' .pauseBut').hide();
                    }
            });
        });
        this.webrtc.on('unmute',function(data){ // hide muted symbol
            sWebCam.webrtc.webrtc.getPeers(data.id).forEach(function(peer){
                var id=sWebCam.webrtc.getDomId(peer);
                if(data.name==='audio'){
                    $('.'+id+' .ui-dialog-title').attr('class','ui-dialog-title');
                    $('.'+id+' .muteBut').show();
                } else
                    if(data.name==='video'){
                        $('#'+id).show();
                        $('#cont'+id+' .pauseBut').show();
                    }
            });
        });
        sWebCam.webrtc.on('channelMessage',sWebCam.channelMessage);
        sWebCam.webrtc.on('channelOpen',function(peer){
            sWebCam.dcReady=true;
            sWebCam.send(peer,"getUserInfo");
            if(sWebCam.owner) sWebCam.send(peer,"channelPrivate",sWebCam.priv);
        });
    },
    channelMessage:function(peer,label,data){
        var domId=sWebCam.webrtc.getDomId(peer);
        switch(data.type){
        case 'sendUserInfo':
        {
            $('.'+domId+' span.ui-dialog-title').text(data.payload.username);
            if(!data.payload.videoChat){
                $('#'+domId).hide();
                $('.'+domId+' .pauseBut').hide();
            }
            if(!data.payload.audioChat){
                $('.'+domId+' .ui-dialog-title').attr('class','ui-dialog-title mute');
                $('.'+domId+' .muteBut').hide();
            }
            break;
        }
        case 'getUserInfo':
            sWebCam.send(peer,"sendUserInfo",{username:sWebCam.username,videoChat:sChat.getSetting('myVideo'),audioChat:sChat.getSetting('myAudio')});
            break;
        case 'channelPrivate':
            sWebCam.priv=data.payload;
            break;
        case 'close':
            if(data.payload===undefined||data.payload===sWebCam.username) sWebCam.close('localContainer');
            break;
        }
    },
    send:function(peer,type,msg){
        this.sendToAll(type,msg);
    },
    sendToAll:function(type,msg){
        if(this.dcReady) this.webrtc.sendDirectlyToAll('simplewebrtc',type,msg);
        else setTimeout(function(){sWebCam.sendToAll(type,msg);},500);
    },
    joinRoom:function(room,owner,priv){
        this.owner=owner!==undefined&&owner;
        this.priv=priv!==undefined&&priv;
        if(!this.modal) this.modalVideo();
        if(!this.webrtc) this.init();
        if(this.stopped){
            this.webrtc.startLocalVideo();
            this.stopped=false;
        }
        if(this.ready){
            if(this.room!==null) sWebCam.webrtc.leaveRoom(this.room);
            sWebCam.webrtc.joinRoom(room);
            this.room=room;
            this.mc=this.modal?1:0;
            sChat.sendMessageWrapper('/setStatus 17'+(this.priv?'':' '+room));
        } else setTimeout(function(){sWebCam.joinRoom(room,owner,priv);},500);
    },
    createRoom:function(priv){
        var key=helper.makeKey();
        this.joinRoom(key,true,priv);
        return key;
    },
    close:function(domId){
        if(domId==='localContainer'){
            //if(this.owner&&this.priv) this.sendToAll('close'); i think, we don`t need it
            this.webrtc.leaveRoom(this.room);
            this.webrtc.stopLocalVideo();
            location.reload();
            /*//this.webrtc.disconnect();
            this.priv = this.room = null;
            this.dcReady = this.owner = this.pause = this.mute = this.ready = this.modal = false;
            this.stopped = true;
            this.mc = 0;
            $(".ui-dialog").remove();*/
        } else{
            var b=$('.'+domId+' .ui-dialog-titlebar-close .ui-button-text'),t=$('#cont'+domId+', .'+domId+' .ui-resizable-handle');
            if(b.html()==="+"){
                b.html("&#8211;");
                t.show();
            } else{
                b.html("+");
                t.hide();
            }
        }
    },
    modalVideo:function(domId,video){
        var me=domId===undefined;
        if(me){
            this.modal=true;
            domId='localContainer';
        }
        if(!me&&$('#'+domId).length) return;
        var w=screen.width*sConfig.defCamS,h=screen.height*sConfig.defCamS*1.5;
        var xw=$('#chatList').width()/(w+100);
        var y=10+Math.floor(sWebCam.mc/xw)*(h+20),
            x=10+Math.floor(sWebCam.mc%xw)*(w+20);
        sWebCam.mc++;
        var s={at:"left+"+x+" top+"+y,my:"left top",of:"#chatList"};
        $('<div id="'+(me?domId:'cont'+domId)+'"></div>').
            append($(me?'<video id="local"></video>':video).addClass('sVideo unmute').css('display',me&&!sChat.getSetting('myVideo')?'none':'block')).append('<div class="videoOff"></div>').
            append('<div class="sVideoToolbar">'+(me&&!sChat.getSetting('myAudio')?'':'<div onclick="sWebCam.muteUnmute(\''+domId+'\','+me+')" class="muteBut"></div>')+(me&&!sChat.getSetting('myVideo')?'':'<svg class="pauseBut" onclick="sWebCam.pauseResume(\''+domId+'\','+me+')"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="img/icons.svg#pause" x="0" y="0" width="100%" height="100%"></use></svg>')+'</div>').
            dialog({title:(me?"Моя камера":""),dialogClass:domId,height:h,width:w,position:s,close:function(){sWebCam.close(domId);}});
        if(!me){
            $('.'+domId+' .ui-dialog-titlebar-close').remove();
            $('.'+domId+' .ui-dialog-titlebar').append($("<button type='button'></button>").button({label:'&#8211;',icons:{primary:"ui-icon-closethick"},text:false}).addClass("ui-dialog-titlebar-close").click(function(){sWebCam.close(domId);}));
        } else if(!sChat.getSetting('myAudio')) $('.localContainer .ui-dialog-title').attr('class','ui-dialog-title mute');
    },
    pauseResume:function(s,me){
        var u=$('.'+s+' use');
        if(u.attr('xlink:href')==="img/icons.svg#pause") u.attr('xlink:href','img/icons.svg#play');
        else u.attr('xlink:href','img/icons.svg#pause');
        s=$('.'+s+' video');
        if(me&&sChat.getSetting('myVideo')){
            if(this.pause) this.webrtc.resume();
            else this.webrtc.pause();
            s.toggle();
            this.pause=!this.pause;
        } else{
            s.toggleClass('pause');
            if(s[0].paused) s[0].play();
            else s[0].pause();
        }
    },
    muteUnmute:function(s,me){
        $('.'+s+' .muteBut').toggleClass('mute');
        if(me){
            if(this.mute) this.webrtc.unmute();
            else this.webrtc.mute();
            this.mute=!this.mute;
        } else{
            var v=$('.'+s+' video');
            v.prop('muted',!v.prop('muted'));
        }
        $('.'+s+' .ui-dialog-title').toggleClass('mute');
    }
};