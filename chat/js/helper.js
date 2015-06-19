var helper={
    makeKey:function(){
        var t="", p="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        for(var i=0; i<10; i++) t+=p.charAt(Math.floor(Math.random()*p.length));
        return t;
    },
    isMobile:function(){
        var a=navigator.userAgent||navigator.vendor||window.opera;
        return /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4));
    },
    truncate:function(s, m){ return (s.length>m?s.slice(0, m-3)+'...':s); },
    getTIM:function(t){
        switch(t){
        case "1ile":return "Дон Кихот";
        case "1sei":return "Дюма";
        case "1lii":return "Робеспьер";
        case "1ese":return "Гюго";
        case "2eie":return "Гамлет";
        case "2lsi":return "Максим Горький";
        case "2sle":return "Жуков";
        case "2iei":return "Есенин";
        case "3ili":return "Бальзак";
        case "3see":return "Наполеон";
        case "3esi":return "Драйзер";
        case "3lie":return "Джек Лондон";
        case "4lse":return "Штирлиц";
        case "4eii":return "Достоевский";
        case "4iee":return "Гексли";
        case "4sli":return "Габен";
        default:return "none";
        }
    },
    grad:function(Str, arr){ //from mp chat
        C1=arr[0];
        C2=arr[1];
        C3=arr[2];
        function c(R, G, B, Str){ return "<span style=\"color:RGB("+Math.floor(R)+','+Math.floor(G)+','+Math.floor(B)+")\">"+Str+"</span>"; }
        function l(Str){
            while(re=/[^]+/.test(Str)) Str=Str.replace(/[^]+/g, "");
            return Str.replace(/<[^>]+>?/g, "").replace(/&([a-z]{2,}|#\d+|#x[\da-f]{2});/gi, " ").length;
        }
        function g(C1, C2, l, s){
            C1=parseInt("0x"+C1);
            C2=parseInt("0x"+C2);
            var R1=C1>>16, G1=(C1>>8)&0xFF, B1=C1&0xFF;
            var R2=C2>>16, G2=(C2>>8)&0xFF, B2=C2&0xFF;
            var Res="";
            var d=l-s;
            R2-=R1;
            G2-=G1;
            B2-=B1;
            for(var i=0; iS<Str.length;)
                if(Str.charAt(iS)==='<'){
                    for(var t=0; iS<Str.length; iS++){
                        Res+=Str.charAt(iS);
                        if(Str.charAt(iS)==='') t++;
                        else if(Str.charAt(iS)==='') t--;
                        else if(Str.charAt(iS)==='>'&&!t) break;
                    }
                    iS++;
                } else if(i!==l){
                    var S=(Str.charAt(iS)=='&'&&/^(&([a-z]{2,}|#\d+|#x[\da-f]{2});)/i.test(Str.substr(iS)))?RegExp.$1:Str.charAt(iS);
                    Res+=d?c(R1+i*R2/d, G1+i*G2/d, B1+i*B2/d, S):c(R1, G1, B1, S);
                    iS+=S.length;
                    i++;
                } else break;
            return Res;
        }
        var iS=0;
        var Len=l(Str);
        return C3?g(C1, C2, Math.floor(Len/2), 0)+g(C2, C3, Math.round(Len/2), 1):g(C1, C2, Len, 1);
    },
    ytSTime:function(t){
        var h=t.indexOf('h')>0, m=t.indexOf('m')>0, s=t.indexOf('s')>0;
        t=t.split(/[hms]/g);
        return "?start="+(h||m||s?(h?t[0]*3600:0)+(m?t[h?1:0]*60:0)+(s?t[h?(m?2:1):(m?1:0)]*1:0):t);
    },
    getWebRTCInfo: function () {
        var e, o;
        window.mozRTCPeerConnection || navigator.mozGetUserMedia ? (e = "moz", o = parseInt(navigator.userAgent.match(/Firefox\/([0-9]+)\./)[1], 10)) : (window.webkitRTCPeerConnection || navigator.webkitGetUserMedia) && (e = "webkit", o = navigator.userAgent.match(/Chrom(e|ium)/) && parseInt(navigator.userAgent.match(/Chrom(e|ium)\/([0-9]+)\./)[2], 10));
        var t = window.mozRTCPeerConnection || window.webkitRTCPeerConnection,
            i = window.mozRTCIceCandidate || window.RTCIceCandidate,
            n = window.mozRTCSessionDescription || window.RTCSessionDescription,
            r = window.webkitMediaStream || window.MediaStream,
            a = "https:" === window.location.protocol && ("webkit" === e && o >= 26 || "moz" === e && o >= 33),
            d = window.AudioContext || window.webkitAudioContext,
            s = document.createElement("video"),
            p = s && s.canPlayType && "probably" === s.canPlayType('video/webm; codecs="vp8", vorbis'),
            w = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.msGetUserMedia || navigator.mozGetUserMedia;
        return !!t && p && !!w && {
            prefix: e,
            browserVersion: o,
            supportRTCPeerConnection: !!t,
            supportVp8: p,
            supportGetUserMedia: !!w,
            supportDataChannel: !!(t && t.prototype && t.prototype.createDataChannel),
            supportWebAudio: !(!d || !d.prototype.createMediaStreamSource),
            supportMediaStream: !(!r || !r.prototype.removeTrack),
            supportScreenSharing: !!a,
            audioContext: d,
            peerConnection: t,
            sessionDescription: n,
            iceCandidate: i,
            mediaStream: r,
            getUserMedia: w
        }
    },
    popupCenter:function(url, title, w, h){
        var dualScreenLeft=window.screenLeft!=undefined?window.screenLeft:screen.left;
        var dualScreenTop=window.screenTop!=undefined?window.screenTop:screen.top;
        var width=window.innerWidth?window.innerWidth:document.documentElement.clientWidth?document.documentElement.clientWidth:screen.width;
        var height=window.innerHeight?window.innerHeight:document.documentElement.clientHeight?document.documentElement.clientHeight:screen.height;
        var left=((width/2)-(w/2))+dualScreenLeft;
        var top=((height/2)-(h/2))+dualScreenTop;
        window.open(url, title, 'scrollbars=yes, width='+w+', height='+h+', top='+top+', left='+left);
    },
    colorPicker: function(el, val, ev){
        var p = document.createElement('div');
        p.className='colorPicker-palette';
        p.style.display='none';
        for(var i=0; i<sConfig.colorCodes.length; i++){
            var s = document.createElement('div'), c='#' + sConfig.colorCodes[i];
            s.innerHTML='&nbsp;';
            s.className = 'colorPicker-swatch';
            s.col=c;
            sChat.addEvent(s, 'click', function () {
                p.style.display = 'none';
                el.col = el.style.backgroundColor = this.col;
                ev(this.col);
            });
            sChat.addEvent(s, 'mouseover', function () {
                this.style.borderColor = "#598FEF";
                el.style.backgroundColor = this.col;
            });
            sChat.addEvent(s, 'mouseout', function () {
                this.style.borderColor = "#000";
                el.style.backgroundColor = el.col;
            });
            s.style.backgroundColor = c;
            p.appendChild(s);
        }
        sChat.addEvent(window, 'click', function (e) {
            var t = e.target || e.srcElement;
            if (p !== t && t.className !== 'colorPicker-picker') p.style.display = 'none';
        });
        document.body.appendChild(p);

        el.className = 'colorPicker-picker';
        el.innerHTML = '&nbsp;';
        el.style.backgroundColor=val;
        sChat.addEvent(el, 'click', function(){
            var o=el.getBoundingClientRect();
            p.style.left=o.left+'px';
            p.style.top=o.top+16+'px';
            p.style.display=p.style.display==='none'?'block':'none';
        });
    }
};
if(!Array.prototype.indexOf)
    Array.prototype.indexOf=function(obj, start){
        for(var i=(start||0), j=this.length; i<j; i++) if(this[i]===obj) return i;
        return -1;
    };