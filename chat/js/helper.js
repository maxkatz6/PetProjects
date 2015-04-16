var helper = {
    makeKey: function () {
        var text = "",possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        for (var i = 0; i < 11; i++)text += possible.charAt(Math.floor(Math.random() * possible.length));
        return text;
    },
    isMobile: function () {
        var a = navigator.userAgent || navigator.vendor || window.opera;
        return /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4));
    },
    truncate: function (str, maxlength) {
        return (str.length > maxlength ? str.slice(0, maxlength - 3) + '...' : str);
    },
    getTIM: function (t) {
        switch (t) {
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
            default:return "none";}
    },
    grad: function (str, arr) {
        var c1 = arr[0], c2 = arr[1], c3 = arr[2];
        function c(r, g, b, s) { return "<span style=\"color:RGB(" + Math.floor(r) + ',' + Math.floor(g) + ',' + Math.floor(b) + ")\">" + s + "</span>"; }
        function l(s) {
            while (re = /[^]+/.test(s)) s = s.replace(/[^]+/g, "");
            return s.replace(/<[^>]+>?/g, "").replace(/&([a-z]{2,}|#\d+|#x[\da-f]{2});/gi, " ").length;
        }
        function g(C1, C2, l, s) {
            C1 = parseInt("0x" + C1);
            C2 = parseInt("0x" + C2);
            var R1 = C1 >> 16, G1 = (C1 >> 8) & 0xFF, B1 = C1 & 0xFF,
                R2 = C2 >> 16, G2 = (C2 >> 8) & 0xFF, B2 = C2 & 0xFF,
                Res = "",d = l - s,iS=0;
            R2 -= R1; G2 -= G1; B2 -= B1;
            for (var i = 0; iS < str.length;) {
                if (str.charAt(iS) === '<') {
                    for (var t = 0; iS < str.length; iS++) {
                        Res += str.charAt(iS);
                        if (str.charAt(iS) === '') t++;
                        else if (str.charAt(iS) === '') t--;
                        else if (str.charAt(iS) === '>' && !t) break;
                    }iS++;
                } else if (i !== l) {
                    var S = (str.charAt(iS) == '&' && /^(&([a-z]{2,}|#\d+|#x[\da-f]{2});)/i.test(str.substr(iS))) ? RegExp.$1 : str.charAt(iS);
                    Res += d ? c(R1 + i * R2 / d, G1 + i * G2 / d, B1 + i * B2 / d, S) : c(R1, G1, B1, S);
                    iS += S.length;
                    i++;
                } else break;
            }
            return Res;
        }
        var len = l(str);
        return c3 ? g(c1, c2, Math.floor(len / 2), 0) + g(c2, c3, Math.round(len / 2), 1) : g(c1, c2, len, 1);
    },
    ytSTime: function (t) {
        var h = t.indexOf('h') > 0, m = t.indexOf('m') > 0, s = t.indexOf('s') > 0;t = t.split(/[hms]/g);
        return "?start=" + (h ? t[0] * 3600 : 0)+(m ? t[h ? 1 : 0] * 60 : 0)+(s ? t[h ? (m ? 2 : 1) : (m ? 1 : 0)] * 1 : 0);
    },
    getWebRTCInfo: function() {
        var prefix,version;
        if (window.mozRTCPeerConnection || navigator.mozGetUserMedia) {
            prefix = 'moz';version = parseInt(navigator.userAgent.match(/Firefox\/([0-9]+)\./)[1], 10);
        } else if (window.webkitRTCPeerConnection || navigator.webkitGetUserMedia) {
            prefix = 'webkit';version = navigator.userAgent.match(/Chrom(e|ium)/) && parseInt(navigator.userAgent.match(/Chrom(e|ium)\/([0-9]+)\./)[2], 10);
        }
        var pc = window.mozRTCPeerConnection || window.webkitRTCPeerConnection;
        var iceCandidate = window.mozRTCIceCandidate || window.RTCIceCandidate;
        var sessionDescription = window.mozRTCSessionDescription || window.RTCSessionDescription;
        var mediaStream = window.webkitMediaStream || window.MediaStream;
        var screenSharing = window.location.protocol === 'https:' &&((prefix === 'webkit' && version >= 26) ||(prefix === 'moz' && version >= 33));
        var audioContext = window.AudioContext || window.webkitAudioContext;
        var videoEl = document.createElement('video');
        var supportVp8 = videoEl && videoEl.canPlayType && videoEl.canPlayType('video/webm; codecs="vp8", vorbis') === "probably";
        var getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.msGetUserMedia || navigator.mozGetUserMedia;
        return {
            prefix: prefix,
            browserVersion: version,
            support: !!pc && supportVp8 && !!getUserMedia,
            // new support style
            supportRTCPeerConnection: !!pc,
            supportVp8: supportVp8,
            supportGetUserMedia: !!getUserMedia,
            supportDataChannel: !!(pc && pc.prototype && pc.prototype.createDataChannel),
            supportWebAudio: !!(audioContext && audioContext.prototype.createMediaStreamSource),
            supportMediaStream: !!(mediaStream && mediaStream.prototype.removeTrack),
            supportScreenSharing: !!screenSharing,
            // constructors
            audioContext: audioContext,
            peerConnection: pc,
            sessionDescription: sessionDescription,
            iceCandidate: iceCandidate,
            mediaStream: mediaStream,
            getUserMedia: getUserMedia
        };
    }
};