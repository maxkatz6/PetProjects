var sChat={
    settingsInitiated:null,
    styleInitiated:null,
    timer:null,
    chatStarted:null,
    dom: {},

    userID:null,
    userName: null,
    encodedUserName: null,
    userRole: null,

    channelID:null,
    channelName:null,
    channelSwitch: null,

    usersList:[],
    userNamesList:[],
    userStatList: [],

    uniCounter: 0,

    ignoredUserNames: null,

    lastID: 0,

    localID: 0,

    originalDocumentTitle: null,

    blinkInterval: null,

    infocus: true,

    httpRequest:{},
    retryTimer: null,

    DOMbufferRowClass:'rowOdd',
    audioHtml5:[],
    selQuo:[], // selected quotes
    needScroll:true,
    selAddressee:'',
    removeOld: false,
    paramString:'',
    init: function () {
        this.initSettings();
        this.initStyle();
        this.addEvent(window, 'load', function(){
            for(var key in sConfig.domIDs) sChat.dom[key]=document.getElementById(sConfig.domIDs[key]);
            sChat.setUnloadHandler();
            setTimeout(sChat.initEmoticons,0);
            if(sConfig.settings['fontColor']) if(sChat.dom['inputField']) sChat.dom['inputField'].style.color=sConfig.settings['fontColor'];
            sChat.setSelectedStyle();
            sChat.initializeFunction();
            sChat.startChat();
            sChat.dom['chatList'].onscroll=function(){ sChat.needScroll=((sChat.dom['chatList'].scrollTop+sChat.dom['chatList'].offsetHeight)>=sChat.dom['chatList'].scrollHeight); };
        });
    },
    initializeFunction:function(){},
    initSettings:function(){
        var cookie=this.readCookie(sConfig.sessionName+'_settings'),
            i,
            settingsArray,
            setting,
            key,
            value,
            number;
        this.settingsInitiated = true;
        if(cookie){
            settingsArray=cookie.split('&');
            for(i=0; i<settingsArray.length; i++){
                setting=settingsArray[i].split('=');
                if(setting.length===2){
                    key=setting[0];
                    value=this.decodeText(setting[1]);
                    switch(value){
                    case 'true':
                        value=true;
                        break;
                    case 'false':
                        value=false;
                        break;
                    case 'null':
                        value=null;
                        break;
                    default:
                        number=parseFloat(value);
                        if(!isNaN(number))
                            if(parseInt(number)===number) value=parseInt(number);
                            else value=number;
                    }
                    sConfig.settings[key]=value;
                }
            }
        }
    },
    persistSettings:function(){
        var settingsArray;
        if(this.settingsInitiated){
            settingsArray=[];
            for(var property in sConfig.settings) settingsArray.push(property+'='+this.encodeText(sConfig.settings[property]));
            this.createCookie(sConfig.sessionName+'_settings', settingsArray.join('&'), sConfig.cookieExpiration);
        }
    },
    getSetting:function(key){
        for(var property in sConfig.settings) if(property===key) return sConfig.settings[key];
        return null;
    },
    setSetting:function(key, value){ sConfig.settings[key]=value; },
    startChat:function(){
        this.chatStarted=true;
        if(!(sChat.isMenuOpened()&&helper.isMobile())&&this.dom['inputField']) this.dom['inputField'].focus();
        var a = document.createElement('audio');
        if (a && !!a.canPlayType) {
            this.audioHtml5['mp3']=!!(a.canPlayType('audio/mpeg;').replace(/no/, ''));
            this.audioHtml5['ogg']=!!(a.canPlayType&&a.canPlayType('audio/ogg; codecs="vorbis"').replace(/no/, ''));
            this.audioHtml5['wav']=!!(a.canPlayType&&a.canPlayType('audio/wav; codecs="1"').replace(/no/, ''));
        }
        this.startChatUpdate();
    },
    setUnloadHandler:function(){
        var onunload=window.onunload;
        if(typeof onunload!=='function')
            window.onunload=function(){
                sChat.persistSettings();
                sChat.persistStyle();
            };
        else
            window.onunload=function(){
                sChat.persistSettings();
                sChat.persistStyle();
                onunload();
            };
    },
    updateDOM:function(id, str, prepend, overwrite){
        var domNode=this.dom[id]?this.dom[id]:document.getElementById(id);
        if(!domNode) return;
        try{
            // Test for validity before adding the string to the DOM:
            domNode.cloneNode(false).innerHTML=str;
            if(overwrite) domNode.innerHTML=str;
            else if(prepend) domNode.innerHTML=str+domNode.innerHTML;
            else domNode.innerHTML+=str;
        } catch(e){
            this.addChatBotMessageToChatList('/error DOMSyntax '+id);
            this.updateChatlistView();
        }
    },
    initEmoticons:function(){
        var buffer = "";
        for(var i=0; i<sConfig.emoticonCodes.length; i++){
            // Replace specials characters in emoticon codes:
            sConfig.emoticonCodes[i] = sChat.encodeSpecialChars(sConfig.emoticonCodes[i]);
            var cl=sConfig.emoticonFiles[i].indexOf('_')!==1?"smile":"sticker";
            buffer += '<a class="' + cl + '" href="javascript:sChat.insertText(\''
                + sChat.scriptLinkEncode(sConfig.emoticonCodes[i])
                + '\');"><img src="img/emoticons/' + sConfig.emoticonFiles[i]
                +'" alt="'+sConfig.emoticonCodes[i]
                +'" title="'+sConfig.emoticonCodes[i]
                +'"/></a>';
        }
        if (sChat.dom['emoticonsContainer']) sChat.updateDOM('emoticonsContainer', buffer);
    },
    startChatUpdate:function(){
        // Start the chat update and retrieve current user and channel info and set the login channel:
        this.paramString += '&getInfos=' + this.encodeText('userID,userName,userRole,channelID,channelName,userInfo');
        if (!isNaN(parseInt(sConfig.loginChannelID))) this.paramString += '&channelID=' + sConfig.loginChannelID;
        else if (sConfig.loginChannelName !== null) this.paramString += '&channelName=' + this.encodeText(sConfig.loginChannelName);
        this.updateChat();
    },
    updateChat:function(pS){
        var requestUrl=sConfig.ajaxURL+'&lastID='+this.lastID;
        if(this.paramString){
            requestUrl += this.paramString;
            this.paramString='';
        }
        if(pS) requestUrl+=pS;
        this.makeRequest(requestUrl, 'GET', null);
    },

    playSoundOnNewMessage:function(dateObject, userID, userName, userRole, messageID, messageText){
        var messageParts;
        if(sConfig.settings['audio']&&this.lastID&&!this.channelSwitch){
            if(new RegExp('(?:^|, |])'+this.userName+',', 'gm').test(messageText)){
                sPlayer.playSound(sConfig.settings['soundPrivate']);
                return;
            }
            messageParts=messageText.split(' ', 1);
            switch(userID){
            case sConfig.chatBotID:
                switch(messageParts[0]){
                case '/login':
                case '/channelEnter':
                    sPlayer.playSound(sConfig.settings['soundEnter']);
                    break;
                case '/logout':
                case '/channelLeave':
                case '/kick':
                    sPlayer.playSound(sConfig.settings['soundLeave']);
                    break;
                case '/error':
                    sPlayer.playSound(sConfig.settings['soundError']);
                    break;
                default:
                    sPlayer.playSound(sConfig.settings['soundChatBot']);
                }
                break;
            case this.userID:
                switch(messageParts[0]){
                case '/privmsgto':
                    sPlayer.playSound(sConfig.settings['soundPrivate']);
                    break;
                default:
                    sPlayer.playSound(sConfig.settings['soundSend']);
                }
                break;
            default:
                switch(messageParts[0]){
                case '/privmsg':
                    sPlayer.playSound(sConfig.settings['soundPrivate']);
                    break;
                default:
                    sPlayer.playSound(sConfig.settings['soundReceive']);
                }
                break;
            }
        }
    },
    getHttpRequest:function(identifier){
        if(!this.httpRequest[identifier])
            if(window.XMLHttpRequest){
                this.httpRequest[identifier]=new XMLHttpRequest();
                if(this.httpRequest[identifier].overrideMimeType) this.httpRequest[identifier].overrideMimeType('text/xml');
            } else if(window.ActiveXObject)
                try{
                    this.httpRequest[identifier]=new ActiveXObject("Msxml2.XMLHTTP");
                } catch(e){
                    try{
                        this.httpRequest[identifier]=new ActiveXObject("Microsoft.XMLHTTP");
                    } catch(e){
                    }
                }
        return this.httpRequest[identifier];
    },
    makeRequest:function(url, method, data){
        var identifier;
        try{
            if(data){
                // Create up to 50 HTTPRequest objects:
                if(!arguments.callee.identifier||arguments.callee.identifier>50) arguments.callee.identifier=1;
                else arguments.callee.identifier++;
                identifier=arguments.callee.identifier;
            } else identifier=0;
            //if the response takes longer than retryTimerDelay to give an OK status, abort the connection and start again.
            this.retryTimer=setTimeout(function(){ sChat.updateChat(); }, sConfig.inactiveTimeout*1500);
            this.getHttpRequest(identifier).open(method, url, true);
            this.getHttpRequest(identifier).onreadystatechange=function(){
                try{
                    sChat.handleResponse(identifier);
                } catch(__e){
                    try{
                        clearTimeout(sChat.timer);
                    } catch(e){
                        this.debugMessage('makeRequest::clearTimeout', e);
                    }
                    try{
                        if(data){
                            sChat.addChatBotMessageToChatList('/error ConnectionTimeout');
                            sChat.updateChatlistView();
                        }
                    } catch(e){
                        this.debugMessage('makeRequest::logRetry', e);
                    }
                    try{
                        sChat.timer=setTimeout(function(){ sChat.updateChat(); }, sConfig.timerRate);
                    } catch(e){
                        this.debugMessage('makeRequest::setTimeout', e);
                    }
                    sChat.debugMessage('makeRequest::catch', __e);
                }
            };
            if(method==='POST') this.getHttpRequest(identifier).setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            this.getHttpRequest(identifier).send(data);
        } catch(_e){
            clearTimeout(this.timer);
            if(data){
                this.addChatBotMessageToChatList('/error ConnectionTimeout');
                this.updateChatlistView();
            }
            this.timer=setTimeout(function(){ sChat.updateChat(); }, sConfig.timerRate);
        }
    },
    handleResponse:function(identifier){
        var json=false;
        if(this.getHttpRequest(identifier).readyState===4)
            if(this.getHttpRequest(identifier).status===200){
                clearTimeout(sChat.retryTimer);
                json=this.getHttpRequest(identifier).responseText;
            } else // Connection status 0 can be ignored.
            if(this.getHttpRequest(identifier).status===0){
                this.updateChatlistView();
                return false;
            } else{
                this.addChatBotMessageToChatList('/error ConnectionStatus '+this.getHttpRequest(identifier).status);
                this.updateChatlistView();
                return false;
            }
        return json && this.handleJSON(JSON.parse(json));
    },
    handleJSON:function(json){
        this.handleInfoMessages(json.infos);
        this.handleOnlineUsers(json.users);
        this.handleChatMessages(json.msgs);
        if (sConfig.radioServer) sPlayer.handleRadio(json.radio);
        this.channelSwitch=null;
        this.setChatUpdateTimer();
    },
    setChatUpdateTimer:function(){
        clearTimeout(this.timer);
        if(this.chatStarted){
            var timeout=sConfig.timerRate;
            this.timer=setTimeout(function(){ sChat.updateChat(); }, timeout);
        }
    },
    handleInfoMessages:function(infos){
        if(infos)
            for(var key in infos){
                var infoData=infos[key][0];
                switch(key){
                case 'channelSwitch':
                    this.clearChatList();
                    this.clearOnlineUsersList();
                    this.setSelectedChannel(infoData);
                    this.channelName=infoData;
                    this.channelSwitch=true;
                    break;
                case 'channelName':
                    this.setSelectedChannel(infoData);
                    this.channelName=infoData;
                    break;
                case 'channelID':
                    this.channelID=parseInt(infoData);
                    break;
                case 'userID':
                    this.userID=parseInt(infoData);
                    break;
                case 'userName':
                {
                    if(sConfig.videoChat) sWebCam.username=infoData;
                    this.userName=infoData;
                    this.encodedUserName=this.scriptLinkEncode(infoData);
                    break;
                }
                case 'userRole':
                    this.userRole=parseInt(infoData);
                    break;
                case 'userInfo':
                    if(infoData.s===17) sChat.sendMessageWrapper('/setStatus 0');
                    break;
                case 'logout':
                    this.handleLogout(infoData);
                    return;
                }
            }
    },
    handleOnlineUsers:function(json){
        if(json&&json.length){
            var index, userID, userName, userRole, userInfo, i, onlineUsers=[];
            for(i=0; i<json.length; i++){
                userID=json[i].id;
                userName=json[i].name?json[i].name:'';
                userRole=json[i].role;
                userInfo=json[i].info;
                onlineUsers.push(userID);
                index=this.usersList.indexOf(userID);
                if(index===-1) this.addUserToOnlineList(userID, userName, userRole, userInfo);
                else{
                    if(this.userStatList[index]!==userInfo.s) this.changeUserStatus(userID, userInfo);
                    if(this.userNamesList[index]!==userName){
                        this.removeUserFromOnlineList(userID, index);
                        this.addUserToOnlineList(userID, userName, userRole, userInfo);
                    }
                }
            }
            // Clear the offline users from the online users list:
            for(i=0; i<this.usersList.length; i++) if(!this.inArray(onlineUsers, this.usersList[i])) this.removeUserFromOnlineList(this.usersList[i], i);
            this.setOnlineListRowClasses();
        }
    },
    changeUserStatus:function(userID, userInfo){
        var s=document.getElementById('stat'+userID);
        if(s){
            s.src='img/status/'+sConfig.statImg[userInfo.s];
            s.title = userInfo.s === 18 ? userInfo.vKey : sConfig.statText[userInfo.s];
            s.style.cursor=userInfo.vKey&&userInfo.s===17?'pointer':'default';
            s.onclick = sConfig.videoChat && userInfo.vKey && userInfo.s === 17 ? function () { sWebCam.joinRoom(sChat.scriptLinkEncode(userInfo.vKey)); } : function () { return false; };
        }
        if(userID===this.userID){
            var selStat = document.getElementById('statusSelect'), userStat = document.getElementById('userStat');
            if (userInfo.s === 18) {
                if (!userStat) {
                    userStat = document.createElement('option');
                    userStat.id = 'userStat';
                }
                if(userStat.textContent) userStat.textContent=userInfo.vKey;
                else userStat.innerText= userInfo.vKey;
                userStat.value = 'userStat';
                selStat.appendChild(userStat);
                selStat.value = 'userStat';
            } else{
                if(userStat) userStat.remove();
                selStat.value = userInfo.s;
            }
        }
        this.userStatList[this.usersList.indexOf(userID)] = userInfo.s === 18 ? 'userStat' : userInfo.s;
    },
    handleChatMessages:function(json){
        var userName, messageText, i;
        if(json&&json.length){
            for(i=0; i<json.length; i++){
                userName=json[i].name?json[i].name:'';
                messageText=json[i].text?json[i].text:'';
                this.addMessageToChatList(
                    new Date(json[i].time),
                    json[i].uID,
                    userName,
                    json[i].role,
                    json[i].id,
                    messageText,
                    json[i].cID,
                    json[i].ip,
                    json[i].info
                );
            }
            this.updateChatlistView();
            this.lastID=json[json.length-1].id;
        }
    },
    setSelectedChannel:function(channel){
        var channelSelected=false,
            i,
            option,
            text;
        if(this.dom['channelSelection']){
            // Replace the entities in the channel name with their character equivalent:
            channel=this.decodeSpecialChars(channel);
            for(i=0; i<this.dom['channelSelection'].options.length; i++)
                if(this.dom['channelSelection'].options[i].value===channel){
                    this.dom['channelSelection'].options[i].selected=true;
                    channelSelected=true;
                    break;
                }
            // The given channel is not in the list, add it:
            if(!channelSelected){
                option=document.createElement('option');
                text=document.createTextNode(channel);
                option.appendChild(text);
                option.setAttribute('value', channel);
                option.setAttribute('selected', 'selected');
                this.dom['channelSelection'].appendChild(option);
            }
        }
    },
    removeUserFromOnlineList:function(userID, index){
        this.usersList.splice(index, 1);
        this.userNamesList.splice(index, 1);
        this.userStatList.splice(index, 1);
        if(this.dom['onlineList']) this.dom['onlineList'].removeChild(this.getUserNode(userID));
    },
    addUserToOnlineList:function(userID, userName, userRole, userInfo){
        this.usersList.push(userID);
        this.userNamesList.push(userName);
        this.userStatList.push(userInfo.s);
        if (this.dom['onlineList']) this.updateDOM('onlineList', this.getUserNodeString(userID, userName, userRole, userInfo), (this.userID === userID));
        this.changeUserStatus(userID, userInfo);
    },
    toUser:function(nick, priv){
        if(this.removeOld){
            this.dom['inputField'].value=this.dom['inputField'].value.substr(this.selAddressee.length);
            this.removeOld=false;
            this.selAddressee='';
        }
        if(priv){
            this.insertMessageWrapper('/msg '+nick+' ');
            this.selAddressee='/msg '+nick+' ';
        } else{
            if(!(new RegExp('(?:^|, )'+nick+', ', 'gm').test(this.dom['inputField'].value))){
                this.dom['inputField'].value=nick+", "+this.dom['inputField'].value;
                this.selAddressee=nick+", "+this.selAddressee;
            }
            if(sChat.isMenuOpened()&&helper.isMobile()) return;
            this.dom['inputField'].focus();
            this.dom['inputField'].selectionStart=this.dom['inputField'].selectionEnd=this.dom['inputField'].value.length;
        }
    },
    getUserNodeString:function(userID, userName, userRole, userInfo){
        var encodedUserName=this.scriptLinkEncode(userName);
        return '<div id="'+this.getUserDocumentID(userID)+'">'
            +'<table width="100%" style="table-layout:fixed;"><tr><td width="34"><img src="../'+userInfo.avatar+'" height="30"/></td>'
            +'<td><a href="javascript:sChat.toUser(\''+encodedUserName+'\',false);">'+userName.replace('_', ' ')+'</a></td>'
            + '<td width="14"><img width="16" id="stat' + userID + '"/></td>'
            +'<td width="30">'+(userInfo.tim!=='none'?'<img src="img/tim/'+userInfo.tim+'.png" border="0" title="'+helper.getTIM(userInfo.tim)+'"></img></td>':'</td>')
            +'<td width="10">'+(userInfo.gender&&userInfo.gender!=='n'?'<img height="13" src="img/gender/'+userInfo.gender+'.png" border="0" title="'+(userInfo.gender==='m'?'Мужской':'Женский')+'"></td>':'</td>')
            + '<td width="20"><a class="arrowBut" id="showMenu' + userID + '" href="javascript:sChat.toggleUserMenu(\'' + this.getUserMenuDocumentID(userID) + '\', \'' + encodedUserName + '\', ' + userID + ' );"></a></td></tr></table>'
            +'<ul class="userMenu" style="display:none;" id="'+this.getUserMenuDocumentID(userID)+((userID===this.userID)?'">'+this.getUserNodeStringItems(encodedUserName, userID, false):'">')+'</ul>'
            +'</div>';
    },
    toggleUserMenu:function(menuID, userName, userID){
        // If the menu is empty, fill it with user node menu items before toggling it.
        var isInline=false;
        if(menuID.indexOf('ium')>=0) isInline=true;
        this.updateDOM(
            menuID,
            this.getUserNodeStringItems(this.encodeText(this.addSlashes(this.getScriptLinkValue(userName))),userID,isInline),
            false,
            true
        );
        if (isInline) this.showHide(menuID);
        else this.toggleButton(menuID, 'showMenu' + userID);
        if(isInline&&this.needScroll) this.dom['chatList'].scrollTop=this.dom['chatList'].scrollHeight;
    },
    inviteVideo:function(encodedUserName){ this.sendMessageWrapper('/inviteVideo '+encodedUserName+' '+this.userName+' '+sWebCam.room+' '+sWebCam.priv); },
    getUserNodeStringItems:function(encodedUserName, userID, isInline){
        var menu;
        if(encodedUserName!==this.encodedUserName){
            menu='<li><a target="_blank" href="/index.php/jomsocial/'+"/index.php?option=com_community&view=profile&userid="+userID+'/profile/">Профиль</a></li>'
                +'<li><a href="javascript:sChat.toUser(\''+encodedUserName+'\',true);">'+sChatLang['userMenuSendPrivateMessage']+'</a></li>'
                +'<li><a href="javascript:sChat.insertMessageWrapper(\'/describe '+encodedUserName+' \');">'+sChatLang['userMenuDescribe']+'</a></li>'
                +'<li><a href="javascript:sChat.sendMessageWrapper(\'/ignore '+encodedUserName+'\');">'+sChatLang['userMenuIgnore']+'</a></li>'
                + (sWebCam !== undefined && sWebCam.room != null && (sWebCam.owner || !sWebCam.private) ? '<li><a href="javascript:sChat.inviteVideo(\'' + encodedUserName + '\');">Пригласить в канал</a></li>' : "")
                +'<li><a href="javascript:sChat.sendMessageWrapper(\'/call '+encodedUserName+'\');">Вызвать в чат</a></li>';
            if(isInline)
                menu+='<li><a href="javascript:sChat.sendMessageWrapper(\'/invite '+encodedUserName+'\');">'+sChatLang['userMenuInvite']+'</a></li>'
                    +'<li><a href="javascript:sChat.sendMessageWrapper(\'/uninvite '+encodedUserName+'\');">'+sChatLang['userMenuUninvite']+'</a></li>'
                    +'<li><a href="javascript:sChat.sendMessageWrapper(\'/whereis '+encodedUserName+'\');">'+sChatLang['userMenuWhereis']+'</a></li>';
            if(this.userRole===2||this.userRole===3)
                menu+='<li><a href="javascript:sChat.insertMessageWrapper(\'/kick '+encodedUserName+' \');">'+sChatLang['userMenuKick']
                    +'</a></li>'+'<li><a href="javascript:sChat.sendMessageWrapper(\'/whois '+encodedUserName+'\');">'+sChatLang['userMenuWhois']+'</a></li>';
        } else{

            menu='<li style="text-indent:-3px;"><select id="statusSelect" onchange="sChat.setStatus(this.options[this.selectedIndex].value)" style="background: transparent;border:none;outline:none;cursor:pointer;">'+sChat.getStatusUserNodeItem()+'</select></li>'
                +'<li><a target="_blank" href="/index.php/jomsocial/profile/">Профиль</a></li>'
                +'<li><a href="javascript:sChat.sendMessageWrapper(\'/quit\');">'+sChatLang['userMenuLogout']+'</a></li>'
                +'<li><a href="javascript:sChat.sendMessageWrapper(\'/who\');">'+sChatLang['userMenuWho']+'</a></li>'
                +'<li><a href="javascript:sChat.sendMessageWrapper(\'/ignore\');">'+sChatLang['userMenuIgnoreList']+'</a></li>'
                +'<li><a href="javascript:sChat.insertMessageWrapper(\'/action \');">'+sChatLang['userMenuAction']+'</a></li>'
                +(sChat.channelID===2?'<li><a href="javascript:sChat.openVideoChannel(false);">Открыть публичный канал</a></li>'
                      +'<li><a href="javascript:sChat.openVideoChannel(true);">Открыть приватный канал</a></li>':"")
                +'<li><a href="javascript:sChat.insertMessageWrapper(\'/roll \');">'+sChatLang['userMenuRoll']+'</a></li>';
            if(this.userRole===1||this.userRole===2||this.userRole===3){
                menu+='<li><a href="javascript:sChat.sendMessageWrapper(\'/join\');">'
                    +sChatLang['userMenuEnterPrivateRoom']
                    +'</a></li>';
                if(this.userRole===2||this.userRole===3)
                    menu+='<li><a href="javascript:sChat.sendMessageWrapper(\'/bans\');">'
                        +sChatLang['userMenuBans']
                        +'</a></li>';
            }
        }
        return menu;
    },
    getStatusUserNodeItem: function(){
        var statHTML='';
        for(var i=0; i<sConfig.statText.length-2; i++) // not include last - webcam
            statHTML += '<option value="' + i + '"' + (i === this.userStatList[this.usersList.indexOf(this.userID)] ? ' selected="selected">' : '>') + sConfig.statText[i] + '</option>';
        return statHTML +'<option value="18"'+('userStat'===this.userStatList[this.usersList.indexOf(this.userID)]?' selected="selected">':'>')+'--Свой статус--</option>';
    },
    setStatus: function (id){
        var stat;
        if (id == 18) stat = prompt("Введите статус");
        sChat.sendMessageWrapper('/setStatus '+id + (stat? ' '+stat:''));
    },
    openVideoChannel:function(priv){
        var key=sWebCam.createRoom(priv);
        if(!priv) this.sendMessageWrapper('/opVideo '+key);
        else this.addChatBotMessageToChatList('Приватный канал успешно создан.');
    },
    setOnlineListRowClasses:function(){
        if(this.dom['onlineList']){
            var node=this.dom['onlineList'].firstChild;
            var rowEven=false;
            while(node){
                node.className=(rowEven?'rowEven':'rowOdd');
                node=node.nextSibling;
                rowEven=!rowEven;
            }
        }
    },
    clearChatList:function(){ while(this.dom['chatList'].hasChildNodes()) this.dom['chatList'].removeChild(this.dom['chatList'].firstChild); },
    clearOnlineUsersList:function(){
        this.usersList=[];
        this.userNamesList=[];
        this.userStatList=[];
        if(this.dom['onlineList']) while(this.dom['onlineList'].hasChildNodes()) this.dom['onlineList'].removeChild(this.dom['onlineList'].firstChild);
    },
    getEncodedChatBotName:function(){
        if(typeof arguments.callee.encodedChatBotName==='undefined') arguments.callee.encodedChatBotName=this.encodeSpecialChars(sConfig.chatBotName);
        return arguments.callee.encodedChatBotName;
    },
    addChatBotMessageToChatList:function(messageText){
        this.addMessageToChatList(
            new Date(),
            sConfig.chatBotID,
            this.getEncodedChatBotName(),
            4,
            null,
            messageText,
            null,
            null
        );
    },
    addMessageToChatList:function(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip, msgInfo){
        if(this.getMessageNode(messageID)) return; // Prevent adding the same message twice:
        if(!this.onNewMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip)) return;
        this.DOMbufferRowClass=this.DOMbufferRowClass==='rowEven'?'rowOdd':'rowEven';
        this.dom['chatList'].appendChild(this.getChatListChild(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip, msgInfo));
    },
    getChatListChild:function(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip, msgInfo){
        var rowClass=this.DOMbufferRowClass,
            userClass=this.getRoleClass(userRole),
            colon=': ',
            priv=messageText.indexOf('/privmsg')===0||messageText.indexOf('/privmsgto')===0||messageText.indexOf('/privaction')===0;
        if(messageText.indexOf('/action')===0||messageText.indexOf('/me')===0||messageText.indexOf('/privaction')===0){
            userClass+=' action';
            colon=' ';
        }
        if(priv) rowClass+=' private';
        if(new RegExp('(?:^|, |])'+this.userName+',', 'gm').test(messageText)) rowClass+=' toMe';
        var text=this.replaceText(messageText);
        var newDiv=document.createElement('div');
        newDiv.className=rowClass;
        newDiv.id=this.getMessageDocumentID(messageID);
        newDiv.innerHTML=this.getDeletionLink(messageID, userID, userRole, channelID)
            +'<a  class="dateTime" href="javascript:sChat.selectQuote('+messageID+');">'+this.formatDate(dateObject)+' </a>'
            +'<span class="'
            +userClass+'"'
            +(sConfig.settings['nickColors']&&msgInfo&&msgInfo.ncol?' style="color:'+msgInfo.ncol+'" ':'')
            +" onclick=\"sChat.toUser('"+userName+"',"+priv+");\">"
            +((sConfig.settings['nickColors']&&sConfig.settings['gradiens']&&msgInfo&&msgInfo.nickGrad)?helper.grad(userName, msgInfo.nickGrad):userName)
            +'</span>'
            +colon
            +'<span '+((sConfig.settings['msgColors']&&msgInfo&&msgInfo.mcol)?'style="color:'+msgInfo.mcol+'">':'>')
            +((sConfig.settings['msgColors']&&sConfig.settings['gradiens']&&msgInfo&&msgInfo.msgGrad)?helper.grad(text, msgInfo.msgGrad):text)+'</span>';
        return newDiv;
    },
    selectQuote:function(messageID){
        var messageNode=this.getMessageNode(messageID);
        var time=(messageNode.firstChild.className==="dateTime"?messageNode.firstChild:messageNode.childNodes[1]);
        this.insertText(' см '+time.innerHTML);
        this.dom['inputField'].focus();
        this.dom['inputField'].selectionStart=this.dom['inputField'].selectionEnd=this.dom['inputField'].value.length;
        this.selQuo.push(messageID);
    },
    getMessageDocumentID:function(messageID){ return ((messageID===null)?'sChat_lm_'+(this.localID++):'sChat_m_'+messageID); },
    getMessageNode:function(messageID){ return ((messageID===null)?null:document.getElementById(this.getMessageDocumentID(messageID))); },
    getUserDocumentID:function(userID){ return 'sChat_u_'+userID; },
    getUserNode:function(userID){ return document.getElementById(this.getUserDocumentID(userID)); },
    getUserMenuDocumentID:function(userID){ return 'sChat_um_'+userID; },
    getInlineUserMenuDocumentID:function(menuID, index){ return 'sChat_ium_'+menuID+'_'+index; },
    getDeletionLink:function(messageID, userID, userRole, channelID){
        if(messageID!==null&&this.isAllowedToDeleteMessage(messageID, userID, userRole, channelID)){
            if(!arguments.callee.deleteMessage) arguments.callee.deleteMessage=this.encodeSpecialChars(sChatLang['deleteMessage']);
            return '<a class="delete" title="'+arguments.callee.deleteMessage+'" href="javascript:sChat.deleteMessage('+messageID+');"> </a>'; // Adding a space - without any content Opera messes up the chatlist display
        }
        return '';
    },
    isAllowedToDeleteMessage:function(messageID, userID, userRole, channelID){
        if((((this.userRole===1&&sConfig.allowUserMessageDelete&&(userID===this.userID||
                channelID===this.userID+sConfig.privateMessageDiff||
                channelID===this.userID+sConfig.privateChannelDiff))||
            (this.userRole===5&&sConfig.allowUserMessageDelete&&(userID===this.userID||
                channelID===this.userID+sConfig.privateMessageDiff||
                channelID===this.userID+sConfig.privateChannelDiff))||
            this.userRole===2)&&userRole!==3&&userRole!==4)||this.userRole===3) return true;
        return false;
    },
    onNewMessage:function(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip){
        if(this.ignoreMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip)
            || !this.parseCommands(dateObject, userID, userName, userRole, messageID, messageText)) return false;
        
        this.blinkOnNewMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip);
        this.playSoundOnNewMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip);
        return true;
    },
    parseCommands:function(dateObject, userID, userName, userRole, messageID, messageText){
        var messageParts=messageText.split(' ');
        switch(messageParts[0]){
        case '/delete':
        {
            var messageNode=sChat.getMessageNode(messageParts[1]);
            if(messageNode){
                var nextSibling=messageNode.nextSibling;
                try{
                    this.dom['chatList'].removeChild(messageNode);
                    if(nextSibling) sChat.updateChatListRowClasses(nextSibling);
                } catch(e){
                }
            }
            return false;
        }
        case '/call':
        {
            if(sChat.infocus) return true;
            alert(messageParts[1]+' вызывает вас в чат!');
            return true;
        }
        default:
            return true;
        }
    },

    blinkOnNewMessage:function(dateObject, userID, userName){
        if(!this.infocus&&this.lastID&&!this.channelSwitch&&userID!==this.userID){
            clearInterval(this.blinkInterval);
            this.blinkInterval=setInterval(function(){
                if(sChat.infocus) arguments.callee.blink=11;
                if(!sChat.originalDocumentTitle) sChat.originalDocumentTitle=document.title;
                if(!arguments.callee.blink){
                    document.title='[@ ] '+userName+' - '+sChat.originalDocumentTitle;
                    arguments.callee.blink=1;
                } else if(arguments.callee.blink>10){
                    clearInterval(sChat.blinkInterval);
                    document.title=(sChat.infocus?'':'[+] ')+sChat.originalDocumentTitle;
                    arguments.callee.blink=0;
                } else{
                    if(arguments.callee.blink%2!==0) document.title='[@ ] '+userName+' - '+sChat.originalDocumentTitle;
                    else document.title='[ @] '+userName+' - '+sChat.originalDocumentTitle;
                    arguments.callee.blink++;
                }
            }, 500);
        }
    },
    updateChatlistView:function(){
        if(!this.dom['chatList'].childNodes) return;
        while(this.dom['chatList'].childNodes.length>500) // 500 - max messages count
            this.dom['chatList'].removeChild(this.dom['chatList'].firstChild);
        if(sConfig.settings['autoScroll']&&this.needScroll) this.dom['chatList'].scrollTop=this.dom['chatList'].scrollHeight;
        //this.updateChatListRowClasses();
    },
    encodeText:function(text){ return encodeURIComponent(text); },
    decodeText:function(text){ return decodeURIComponent(text); },
    utf8Encode:function(plainText){
        var utf8Text='';
        for(var i=0; i<plainText.length; i++){
            var c=plainText.charCodeAt(i);
            if(c<128) utf8Text+=String.fromCharCode(c);
            else if((c>127)&&(c<2048)){
                utf8Text+=String.fromCharCode((c>>6)|192);
                utf8Text+=String.fromCharCode((c&63)|128);
            } else{
                utf8Text+=String.fromCharCode((c>>12)|224);
                utf8Text+=String.fromCharCode(((c>>6)&63)|128);
                utf8Text+=String.fromCharCode((c&63)|128);
            }
        }
        return utf8Text;
    },
    utf8Decode:function(utf8Text){
        var plainText='';
        var c, c2, c3;
        var i=0;
        while(i<utf8Text.length){
            c=utf8Text.charCodeAt(i);
            if(c<128){
                plainText+=String.fromCharCode(c);
                i++;
            } else if((c>191)&&(c<224)){
                c2=utf8Text.charCodeAt(i+1);
                plainText+=String.fromCharCode(((c&31)<<6)|(c2&63));
                i+=2;
            } else{
                c2=utf8Text.charCodeAt(i+1);
                c3=utf8Text.charCodeAt(i+2);
                plainText+=String.fromCharCode(((c&15)<<12)|((c2&63)<<6)|(c3&63));
                i+=3;
            }
        }
        return plainText;
    },
    encodeSpecialChars:function(text){
        return text.replace(
            /[&<>'"]/g,
            function(str){
                switch(str){
                case '&':
                    return '&amp;';
                case '<':
                    return '&lt;';
                case '>':
                    return '&gt;';
                case '\'':
                    return '&#39;';
                case '"':
                    return '&quot;';
                default:
                    return str;
                }
            });
    },
    decodeSpecialChars:function(text){
        var regExp=new RegExp('(&amp;)|(&lt;)|(&gt;)|(&#39;)|(&quot;)', 'g');
        return text.replace(
            regExp,
            function(str){
                switch(str){
                case '&amp;':
                    return '&';
                case '&lt;':
                    return '<';
                case '&gt;':
                    return '>';
                case '&#39;':
                    return '\'';
                case '&quot;':
                    return '"';
                default:
                    return str;
                }
            }
        );
    },
    inArray:function(array, value){
        for(var i=0; i<array.length; i++) if(array[i]===value) return true;
        return false;
    },
    stripTags:function(str){
        if(!arguments.callee.regExp) arguments.callee.regExp=new RegExp('<\\/?[^>]+?>', 'g');
        return str.replace(arguments.callee.regExp, '');
    },
    stripBBCodeTags:function(str){
        if(!arguments.callee.regExp) arguments.callee.regExp=new RegExp('\\[\\/?[^\\]]+?\\]', 'g');
        return str.replace(arguments.callee.regExp, '');
    },
    escapeRegExp:function(text){
        if(!arguments.callee.regExp) arguments.callee.regExp=new RegExp('(\\'+['^', '$', '*', '+', '?', '.', '|', '/', '(', ')', '[', ']', '{', '}', '\\'].join('|\\')+')', 'g');
        return text.replace(arguments.callee.regExp, '\\$1');
    },
    formatDate:function(date){
        return '(%H:%i:%s)'
            .replace(/%H/g, this.addLeadingZero(date.getHours()))
            .replace(/%i/g, this.addLeadingZero(date.getMinutes()))
            .replace(/%s/g, this.addLeadingZero(date.getSeconds()));
    },
    addLeadingZero:function(number){
        number=number.toString();
        if(number.length<2) number='0'+number;
        return number;
    },
    getUserIDFromUserName:function(userName){
        var index=this.userNamesList.indexOf(userName);
        if(index!==-1) return this.usersList[index];
        return null;
    },
    getRoleClass:function(roleID){
        switch(roleID){
        case 0:
            return 'guest';
        case 1:
            return 'user';
        case 2:
            return 'moderator';
        case 3:
            return 'admin';
        case 4:
            return 'chatBot';
        case 5:
            return 'customUser';
        default:
            return 'default';
        }
    },
    handleInputFieldKeyDown:function(event){
        // Enter key without shift should send messages
        if(event.keyCode===13&&!event.shiftKey){
            this.sendMessage();
            try{
                event.preventDefault();
            } catch(e){
                event.returnValue=false; // IE<9
            }
            return false;
        }
        return true;
    },
    updateMessageLengthCounter:function(){ if(this.dom['messageLengthCounter']) this.updateDOM('messageLengthCounter', this.dom['inputField'].value.length+'/1000', false, true); },
    sendMessage:function(txt){
        txt=txt?txt:this.dom['inputField'].value;
        if(!txt) return;
        txt=this.parseInputMessage(txt);
        if(txt){
            var msg={
                text:(txt)
            };
            if(this.getSetting('nickColor')!=null) msg.ncol=this.getSetting('nickColor');
            if(this.getSetting('fontColor')!=null) msg.mcol=this.getSetting('fontColor');
            clearTimeout(this.timer);
            var message='lastID='
                +this.lastID
                +'&text='
                +this.encodeText(JSON.stringify(msg));
            this.makeRequest(sConfig.ajaxURL, 'POST', message);
        }
        if(this.dom['inputField'].value.indexOf(this.selAddressee)!==0) this.selAddressee='';
        this.removeOld=true;
        this.dom['inputField'].value=this.getSetting('saveAddressee')?this.selAddressee:'';
        if(!(sChat.isMenuOpened()&&helper.isMobile())) this.dom['inputField'].focus();
        this.updateMessageLengthCounter();
    },
    parseInputMessage:function(text){
        var textParts;
        if(text.charAt(0)==='/'){
            textParts=text.split(' ');
            switch(textParts[0]){
            case '/ignore':
                text=sChat.parseIgnoreInputCommand(text, textParts);
                break;
            case '/clear':
                sChat.clearChatList();
                return false;
            case '/closeVid':
                sWebCam.close();
                return false;
            case '/setStatus':
                setTimeout(function(){ sChat.changeUserStatus(sChat.userID, { s:parseInt(textParts[1]), vKey:(textParts[2]?textParts.slice(2).join(' '):false) }); }, 500);
                break;
            case '/afk':
                sChat.setStatus(1);
                return false;
            }
        } else{
            text=this.parseToUserBold(text);
            text=this.parseUserQuetes(text);
        }
        return text;
    },
    parseToUserBold:function(text){ return text.replace(new RegExp('(?:('+this.userNamesList.join('|')+'|Сервер),(?: |))*', 'gm'), function(a){ return (a!==''?'[b]'+a+'[/b]':''); }); },
    parseUserQuetes:function(text){
        return text.replace(/\(\d\d:\d\d:\d\d\)/g, function(a){
            for(var i=0; i<sChat.selQuo.length; i++){
                var q=sChat.selQuo[i], n=sChat.getMessageNode(q);;
                if(n&&n.innerHTML.indexOf(a)>-1) return '[q]'+q+'[/q]';
            }
            return a;
        });
    },
    parseIgnoreInputCommand:function(text, textParts){
        var userName, ignoredUserNames=this.getIgnoredUserNames(), i;
        if(textParts.length>1){
            userName=this.encodeSpecialChars(textParts[1]);
            // Prevent adding the chatBot or current user to the list:
            if(userName==='Sagita'||userName===this.userName||userName===this.getEncodedChatBotName()) // Display the list of ignored users instead:
                return this.parseIgnoreInputCommand(null, new Array('/ignore'));
            if(ignoredUserNames.length>0){
                i=ignoredUserNames.length;
                while(i--)
                    if(ignoredUserNames[i]===userName){
                        ignoredUserNames.splice(i, 1);
                        this.addChatBotMessageToChatList('/ignoreRemoved '+userName);
                        this.setIgnoredUserNames(ignoredUserNames);
                        this.updateChatlistView();
                        return null;
                    }
            }
            ignoredUserNames.push(userName);
            this.addChatBotMessageToChatList('/ignoreAdded '+userName);
            this.setIgnoredUserNames(ignoredUserNames);
        } else if(ignoredUserNames.length===0) this.addChatBotMessageToChatList('/ignoreListEmpty -');
        else this.addChatBotMessageToChatList('/ignoreList '+ignoredUserNames.join(' '));
        this.updateChatlistView();
        return null;
    },
    getIgnoredUserNames:function(){
        var ignoredUserNamesString;
        if(!this.ignoredUserNames){
            ignoredUserNamesString=this.getSetting('ignoredUserNames');
            if(ignoredUserNamesString) this.ignoredUserNames=ignoredUserNamesString.split(' ');
            else this.ignoredUserNames=[];
        }
        return this.ignoredUserNames;
    },
    setIgnoredUserNames:function(ignoredUserNames){
        this.ignoredUserNames=ignoredUserNames;
        this.setSetting('ignoredUserNames', ignoredUserNames.join(' '));
    },
    ignoreMessage:function(dateObject, userID, userName, userRole, messageID, messageText){
        var textParts;
        if(userID===sConfig.chatBotID&&messageText.charAt(0)==='/'){
            textParts=messageText.split(' ');
            if(textParts.length>1)
                switch(textParts[0]){
                case '/invite':
                case '/uninvite':
                case '/roll':
                    userName=textParts[1]; 
                    break;
                }
        }
        return (this.inArray(this.getIgnoredUserNames(), userName));
    },
    deleteMessage:function(messageID){
        var messageNode=this.getMessageNode(messageID), originalClass, nextSibling;
        if(messageNode){
            originalClass=messageNode.className;
            this.addClass(messageNode, 'deleteSelected');
            if(confirm(sChatLang['deleteMessageConfirm'])){
                nextSibling=messageNode.nextSibling;
                try{
                    this.dom['chatList'].removeChild(messageNode);
                    if(nextSibling) this.updateChatListRowClasses(nextSibling);
                    this.updateChat('&delete=' + messageID);
                } catch(e){
                    this.setClass(messageNode, originalClass);
                }
            } else messageNode.className=originalClass;
        }
    },
    updateChatListRowClasses:function(node){
        var previousNode, rowEven;
        if(!node) node=this.dom['chatList'].firstChild;
        if(node){
            previousNode=node.previousSibling;
            rowEven=(previousNode&&previousNode.className==='rowOdd')?true:false;
            while(node){
                node.className=(rowEven?'rowEven':'rowOdd');
                node=node.nextSibling;
                rowEven=!rowEven;
            }
        }
    },
    addEvent:function(elem, type, eventHandle){
        if(elem.addEventListener) elem.addEventListener(type, eventHandle, false);
        else if(elem.attachEvent) elem.attachEvent("on"+type, eventHandle);
        else elem["on"+type]=eventHandle;
    },
    addClass:function(node, theClass){ if(!this.hasClass(node, theClass)) node.className+=' '+theClass; },
    removeClass:function(node, theClass){ node.className=node.className.replace(new RegExp('(?:^|\\s)'+theClass+'(?!\\S)', 'g'), ''); },
    hasClass:function(node, theClass){ return node.className.match(new RegExp('\\b'+theClass+'\\b')); },
    scriptLinkEncode:function(text){ return this.encodeText(this.addSlashes(this.decodeSpecialChars(text))); },
    scriptLinkDecode:function(text){ return this.encodeSpecialChars(this.removeSlashes(this.decodeText(text))); },
    addSlashes:function(text){ return text.replace(/\\/g, '\\\\').replace(/\'/g, '\\\''); },
    removeSlashes:function(text){ return text.replace(/\\\\/g, '\\').replace(/\\\'/g, '\''); },
    getScriptLinkValue:function(value){
        // This method returns plainText encoded values from javascript links
        // The value has to be utf8Decoded for MSIE and Opera:
        if(typeof arguments.callee.utf8Decode==='undefined')
            switch(navigator.appName){
            case 'Microsoft Internet Explorer':
            case 'Opera':
                arguments.callee.utf8Decode=true;
                return this.utf8Decode(value);
            default:
                arguments.callee.utf8Decode=false;
                return value;
            }
        else if(arguments.callee.utf8Decode) return this.utf8Decode(value);
        else return value;
    },
    sendMessageWrapper:function(text){ this.sendMessage(this.getScriptLinkValue(text)); },
    insertMessageWrapper:function(text){ this.insertText(this.getScriptLinkValue(text), true); },
    switchChannel:function(channel){
        if(!this.chatStarted){
            this.clearChatList();
            this.channelSwitch=true;
            sConfig.loginChannelID=null;
            sConfig.loginChannelName=channel;
            return;
        }
        clearTimeout(this.timer);
        var message='lastID='+this.lastID+'&channelName='+this.encodeText(channel);
        this.makeRequest(sConfig.ajaxURL, 'POST', message);
        // this.clearOnlineUsersList();
        if(this.dom['inputField']) this.dom['inputField'].focus();
    },
    logout:function(){
        clearTimeout(this.timer);
        this.makeRequest(sConfig.ajaxURL, 'POST', 'logout=true');
    },
    handleLogout:function(url){ window.location.href=url; },
    toggleSetting:function(setting, buttonID){
        this.setSetting(setting, !this.getSetting(setting));
        if(buttonID) this.updateButton(setting, buttonID);
    },
    updateButton:function(setting, buttonID){
        var node=document.getElementById(buttonID);
        if(node) node.className=(this.getSetting(setting)?'button':'button off');
    },
    toggleButton:function(idShowHide, idBut){
        if (idBut) {
            var button = document.getElementById(idBut);
            button.className = button.className.indexOf(' off') !== -1 ?
                button.className = button.className.replace(' off', '') :
                button.className + ' off';
        }
        this.showHide(idShowHide);
    },
    toggleContainer:function(containerID,hideContainerIDs){
        if (hideContainerIDs) 
            sChat.showHide(hideContainerIDs, 'none');
        sChat.showHide(containerID);
    },
    showHide:function(id, styleDisplay, displayInline){
        if (id instanceof Array) {
            for (var i = 0; i < id.length; i++) this.showHide(id[i], styleDisplay, displayInline);
            return;
        }
        var node=document.getElementById(id);
        if(node)
            if(styleDisplay) node.style.display=styleDisplay;
            else if(node.style.display===''||node.style.display==='none') node.style.display=(displayInline?'inline':'block');
            else node.style.display='none';
    },
    setFontColor:function(color){
        this.setSetting('fontColor', color);
        if(this.dom['inputField']) this.dom['inputField'].style.color=color;
    },
    insertText:function(text, clearInputField){
        if(clearInputField) this.dom['inputField'].value='';
        this.insert(text, '');
    },
    insertBBCode:function(bbCode){ this.insert('['+bbCode+']', '[/'+bbCode+']'); },
    insert:function(startTag, endTag){
        if(!(sChat.isMenuOpened()&&helper.isMobile())) this.dom['inputField'].focus();
        var insText;
        // Internet Explorer:
        if(typeof document.selection!=='undefined'){
            // Insert the tags:
            var range=document.selection.createRange();
            insText=range.text;
            range.text=startTag+insText+endTag;
            // Adjust the cursor position:
            range=document.selection.createRange();
            if(insText.length===0) range.move('character', -endTag.length);
            else range.moveStart('character', startTag.length+insText.length+endTag.length);
            range.select();
        } else{
            var pos;
            // Firefox, etc. (Gecko based browsers):
            if(typeof this.dom['inputField'].selectionStart!=='undefined'){
                // Insert the tags:
                var start=this.dom['inputField'].selectionStart;
                var end=this.dom['inputField'].selectionEnd;
                insText=this.dom['inputField'].value.substring(start, end);
                this.dom['inputField'].value=this.dom['inputField'].value.substr(0, start)
                    +startTag
                    +insText
                    +endTag
                    +this.dom['inputField'].value.substr(end);
                // Adjust the cursor position:
                if(insText.length===0) pos=start+startTag.length;
                else pos=start+startTag.length+insText.length+endTag.length;
                this.dom['inputField'].selectionStart=pos;
                this.dom['inputField'].selectionEnd=pos;
            }
            // Other browsers:
            else{
                pos=this.dom['inputField'].value.length;
                this.dom['inputField'].value=this.dom['inputField'].value.substr(0, pos)
                    +startTag
                    +endTag
                    +this.dom['inputField'].value.substr(pos);
            }
        }
    },
    replaceText:function(text){
        try{
            text=text.replace(/[\u200B-\u200D\uFEFF]/g, '');
            text=text.replace(/\n/g, '<br/>');
            if(text.charAt(0)==='/') text=this.replaceCommands(text);
            else{
                text=this.replaceBBCode(text);
                text=this.replaceHyperLinks(text);
                text=this.replaceEmoticons(text);
            }
        } catch(e){
            this.debugMessage('replaceText', e);
        }
        return text;
    },
    replaceCommands:function(text){
        try{
            if(text.charAt(0)!=='/') return text;
            var i,textParts=text.split(' ');
            switch(textParts[0]){
            case '/login':
                return '<span class="chatBotMessage">'+sChatLang['login'].replace(/%s/, "<a href=\"javascript:sChat.toUser('"+textParts[1]+"');\">"+textParts[1]+"</a>")+'</span>';
            case '/logout':
                return '<span class="chatBotMessage">' + sChatLang['logout' + (textParts.length === 3 ? textParts[2] : '')].replace(/%s/, textParts[1]) + '</span>';
            case '/call':
                return '<span class="chatBotMessage">' + textParts[1]+ ' вызывает вас в чат.</span>';
            case '/channelEnter':
                return '<span class="chatBotMessage">'+sChatLang['channelEnter'].replace(/%s/, textParts[1])+'</span>';
            case '/channelLeave':
                return '<span class="chatBotMessage">'+sChatLang['channelLeave'].replace(/%s/, textParts[1])+'</span>';
            case '/invite':
                return '<span class="chatBotMessage">'+sChatLang['invite'].replace(/%s/, textParts[1]).replace(/%s/, '<a href="javascript:sChat.sendMessageWrapper(\'/join '+sChat.scriptLinkEncode(textParts[2])+'\');" title="'+sChatLang['joinChannel'].replace(/%s/, textParts[2])+'">'+textParts[2]+'</a>')+'</span>';
            case '/inviteto':
                return '<span class="chatBotMessage">'+sChatLang['inviteto'].replace(/%s/, textParts[1]).replace(/%s/, textParts[2])+'</span>';
            case '/uninvite':
                return '<span class="chatBotMessage">'+sChatLang['uninvite'].replace(/%s/, textParts[1]).replace(/%s/, textParts[2])+'</span>';
            case '/uninviteto':
                return '<span class="chatBotMessage">'+sChatLang['uninviteto'].replace(/%s/, textParts[1]).replace(/%s/, textParts[2])+'</span>';
            case '/queryOpen':
                return '<span class="chatBotMessage">'+sChatLang['queryOpen'].replace(/%s/, textParts[1])+'</span>';
            case '/queryClose':
                return '<span class="chatBotMessage">'+sChatLang['queryClose'].replace(/%s/, textParts[1])+'</span>';
            case '/ignoreAdded':
                return '<span class="chatBotMessage">'+sChatLang['ignoreAdded'].replace(/%s/, textParts[1])+'</span>';
            case '/ignoreRemoved':
                return '<span class="chatBotMessage">'+sChatLang['ignoreRemoved'].replace(/%s/, textParts[1])+'</span>';
            case '/ignoreList':
                return '<span class="chatBotMessage">'+sChatLang['ignoreList']+' '+sChat.getInlineUserMenu(textParts.slice(1))+'</span>';
            case '/ignoreListEmpty':
                return '<span class="chatBotMessage">'+sChatLang['ignoreListEmpty']+'</span>';
            case '/kick':
                return '<span class="chatBotMessage">'+sChatLang['logoutKicked'].replace(/%s/, textParts[1])+'</span>';
            case '/who':
                return '<span class="chatBotMessage">'+sChatLang['who']+' '+sChat.getInlineUserMenu(textParts.slice(1))+'</span>';
            case '/whoChannel':
                return '<span class="chatBotMessage">'+sChatLang['whoChannel'].replace(/%s/, textParts[1])+' '+sChat.getInlineUserMenu(textParts.slice(2))+'</span>';
            case '/whoEmpty':
                return '<span class="chatBotMessage">'+sChatLang['whoEmpty']+'</span>';
            case '/bansEmpty':
                return '<span class="chatBotMessage">'+sChatLang['bansEmpty']+'</span>';
            case '/unban':
                return '<span class="chatBotMessage">'+sChatLang['unban'].replace(/%s/, textParts[1])+'</span>';
            case '/whois':
                return '<span class="chatBotMessage">'+sChatLang['whois'].replace(/%s/, textParts[1])+' '+textParts[2]+'</span>';
            case '/whereis':
                return '<span class="chatBotMessage">'+sChatLang['whereis'].replace(/%s/, textParts[1]).replace(/%s/, '<a href="javascript:sChat.sendMessageWrapper(\'/join '+sChat.scriptLinkEncode(textParts[2])+'\');" title="'+sChatLang['joinChannel'].replace(/%s/, textParts[2])+'">'+textParts[2]+'</a>')+'</span>';
            case '/roll':
                return '<span class="chatBotMessage">'+sChatLang['roll'].replace(/%s/, textParts[1]).replace(/%s/, textParts[2]).replace(/%s/, textParts[3])+'</span>';
            case '/nick':
                return '<span class="chatBotMessage">'+sChatLang['nick'].replace(/%s/, textParts[1]).replace(/%s/, textParts[2])+'</span>';
            case '/setStatus':
                return '<span class="chatBotMessage">' + textParts[1] + " сменил статус на \"" + (textParts[2] == 18 ? textParts.slice(3).join(' ') : sConfig.statText[parseInt(textParts[2])]) + "\".</span>";
            case '/opVideo':
                return '<span class="chatBotMessage">Публичный канал был создан пользователем '+textParts[1]+". <a href=\"javascript:sWebCam.joinRoom('"+textParts[2]+"');\">Подключиться</a>.</span>";
            case '/inviteVideo':
                return '<span class="chatBotMessage">'+textParts[1]+" приглашает вас в канал. <a href=\"javascript:sWebCam.joinRoom('"+textParts[2]+"',false,"+textParts[3]+");\">Подключиться</a>.</span>";
            case '/privmsg':
                return '<span class="privmsg">'+sChatLang['privmsg']+'</span> '+sChat.replaceBBCodeHLEmot(textParts.slice(1).join(' '));
            case '/privmsgto':
                return '<span class="privmsg">'+sChatLang['privmsgto'].replace(/%s/, textParts[1])+'</span> '+sChat.replaceBBCodeHLEmot(textParts.slice(2).join(' '));
            case '/privaction':
                return '<span class="action">'+sChat.replaceBBCodeHLEmot(textParts.slice(1).join(' '))+'</span> <span class="privmsg">'+sChatLang['privmsg']+'</span> ';
            case '/privactionto':
                return '<span class="action">'+sChat.replaceBBCodeHLEmot(textParts.slice(2).join(' '))+'</span> <span class="privmsg">'+sChatLang['privmsgto'].replace(/%s/, textParts[1])+'</span> ';
            case '/me':
            case '/action':
                return '<span class="action">'+sChat.replaceBBCodeHLEmot(textParts.slice(1).join(' '))+'</span>';
            case '/error':
            {
                var errorMessage=sChatLang['error'+textParts[1]]||'Error: Unknown.';
                if(textParts.length>2) errorMessage=errorMessage.replace(/%s/, textParts.slice(2).join(' '));
                return '<span class="chatBotErrorMessage">'+errorMessage+'</span>';
            }
            case '/list':
            {
                var channels=textParts.slice(1), listChannels=[], channelName;
                for(i=0; i<channels.length; i++){
                    channelName=(channels[i]===this.channelName)?'<b>'+channels[i]+'</b>':channels[i];
                    listChannels.push(
                        '<a href="javascript:sChat.sendMessageWrapper(\'/join '
                        +this.scriptLinkEncode(channels[i])
                        +'\');" title="'
                        +sChatLang['joinChannel'].replace(/%s/, channels[i])
                        +'">'
                        +channelName
                        +'</a>'
                    );
                }
                return '<span class="chatBotMessage">'+sChatLang['list']+' '+listChannels.join(', ')+'</span>';
            }
            case '/bans':
            {
                var users=textParts.slice(1), listUsers=[];
                for(i=0; i<users.length; i++)
                    listUsers.push(
                        '<a href="javascript:sChat.sendMessageWrapper(\'/unban '
                        +this.scriptLinkEncode(users[i])
                        +'\');" title="'
                        +sChatLang['unbanUser'].replace(/%s/, users[i])
                        +'">'
                        +users[i]
                        +'</a>'
                    );
                return '<span class="chatBotMessage">'+sChatLang['bans']+' '+listUsers.join(', ')+'</span>';
            }
            }
        } catch(e){
            this.debugMessage('replaceCommands', e);
        }
        return text;
    },
    replaceBBCodeHLEmot:function(text){ return sChat.replaceEmoticons(sChat.replaceHyperLinks(sChat.replaceBBCode(text))); },
    getInlineUserMenu:function(users){
        var menu='';
        for(var i=0; i<users.length; i++){
            if(i>0) menu+=', ';
            menu+='<a href="javascript:sChat.toggleUserMenu(\''
                + this.getInlineUserMenuDocumentID(this.uniCounter, i)
                +'\', \''
                +this.scriptLinkEncode(users[i])
                +'\', null);" title="'
                +sChatLang['toggleUserMenu'].replace(/%s/, users[i])+'" >'
                +((users[i]===this.userName)?'<b>'+users[i]+'</b>':users[i])
                +'</a>'
                +'<ul class="inlineUserMenu" id="'
                + this.getInlineUserMenuDocumentID(this.uniCounter, i)
                +'" style="display:none;">'
                +'</ul>';
        }
        this.uniCounter++;
        return menu;
    },
    containsUnclosedTags:function(str){
        var openTags=str.match(/<[^>\/]+?>/gm),
            closeTags=str.match(/<\/[^>]+?>/gm);
        // Return true if the number of tags doesn't match:
        return (!openTags&&closeTags)||
        (openTags&&!closeTags)||
        (openTags&&closeTags&&(openTags.length!==closeTags.length));
    },
    replaceBBCode:function(text){
        if(!sConfig.settings['bbCode']) // If BBCode is disabled, just strip the text from BBCode tags:
            return text.replace(/(\[\/?\w+(?:=[^<>]*?)?\])/g, '');
        // Remove the BBCode tags:
        return text.replace(
            /\[(\w+)(?:=([^<>]*?))?\](.+?)\[\/\1\]/gm,
            function(str, tag, attribute, content){
                // Only replace predefined BBCode tags:
                if(!sChat.inArray(sConfig.bbCodeTags, tag)||
                    sChat.containsUnclosedTags(content)) return str;
                if(!content||content.length===0) return '';
                switch(tag){
                case 'q':
                    var m = sChat.getMessageNode(content);
                    return '<a class="dateTime" href="javascript:sChat.blinkMessage(' + content + ');">' + (m ? (m.firstChild.className === "dateTime" ? m.firstChild : m.childNodes[1]).innerHTML : "(00:00:00)") + '</a>';
                case 'quote':
                    return '<span class="quote"><q>'+sChat.replaceBBCode(content)+'</q></span>';
                case 's':
                    return '<span style="text-decoration:line-through;">'+sChat.replaceBBCode(content)+'</span>';
                case 'tgw':
                    return '<span class=\"tgw\">'+sChat.replaceBBCode(content)+'</span>';
                default:
                    return '<'+tag+'>'+sChat.replaceBBCode(content)+'</'+tag+'>';
                }
            }
        );
    },
    blinkMessage:function(messageID){
        var messageNode=this.getMessageNode(messageID);
        if(!messageNode) return;
        if (messageNode.scrollIntoView) 
            messageNode.scrollIntoView({ block: "end", behavior: "smooth" });
        else window.location.hash = messageNode.id;
        var k=0.1, c=0;
        messageNode.style.opacity=1;
        var b=setInterval(function(){
            var o=parseFloat(messageNode.style.opacity);
            if(o>=1||o<=0.3){
                k=-k;
                c++;
            }
            messageNode.style.opacity=o+k;
            if(c>=14){
                messageNode.style.opacity=1;
                clearInterval(b);
            }
        }, 40);
    },
    replaceHyperLinks:function(text, vk){
        if(!sConfig.settings['hyperLinks']) return text;
        return text.replace(/(^|\s|>)(((?:https?|ftp):\/\/)([\w_\.-]{2,256}\.[\w\.]{2,4})(:\d{1,5})?(\/([\w+,\/%$^&\*=;\-_а-яА-Я:\)\(\.]*)?((?:\?|#)[:\w%\/\)\(\-+,а-яА-Я;.?=&#]*)?)?)/gim,
            function(str, s, a, prot, host, port, sa, sa1, sa2){
                var hostArr=host.split('.'),
                    fe=(sa1?sa1.split(/\.|\//g).pop().toLowerCase():'');
                switch(fe){
                case "mp3":
                case "ogg":
                case "wav":
                    return s+(sChat.audioHtml5[fe]?'<audio style="height:30px; max-width:100%;" src="'+a+'" controls></audio> ':'<a href="'+a+'" onclick="window.open(this.href); return false;">Скачать '+fe+' </a> ');
                case"jpg":
                case"jpeg":
                case"png":
                case"bmp":
                case"ico":
                case"svg":
                    return s + '<a href="' + a + '" onclick="window.open(this.href); return false;"><img onload="sChat.updateChatlistView();" class="bbCodeImage" style="max-width:90%; max-height:' + (vk ? 300 : 200) + 'px;" src="' + a + '"/></a>';
                case "gif":
                    return s + sChat.replaceGif(a);
                default:
                {
                    var req=sa2&&sa2.split(/&amp;|[=\?&\#]+/), height=400/1.7, t=req&&req.indexOf('t')!==-1?helper.ytSTime(req[req.indexOf('t')+1]):'';
                    if(sa2&&sChat.inArray(hostArr, 'youtube')) return s+'<iframe onload="sChat.updateChatlistView();" style="height:'+height+'px;width:400px;max-width:100%;" src="https://www.youtube.com/embed/'+req[req.indexOf('v')+1]+t+'" frameborder="0" allowfullscreen="true"></iframe>';
                    else if(sa1)
                        if(sChat.inArray(hostArr, 'youtu')) return s+'<iframe onload="sChat.updateChatlistView();" style="height:'+height+'px;width:400px;max-width:100%;" src="https://www.youtube.com/embed/'+sa1+t+'" frameborder="0" allowfullscreen="true"></iframe>';
                        else if(sChat.getSetting('vkPosts')&&sChat.inArray(hostArr, 'vk')&&!vk) return s+sChat.replaceVK(a, sa);
                        else if(sChat.inArray(hostArr, 'coub')) return s+'<iframe onload="sChat.updateChatlistView();" style="height:'+height+'px;width:400px;max-width:100%;" src="http://coub.com/embed/'+fe+'?muted=false&autostart=false&originalSize=true&hideTopBar=false&startWithHD=false" allowfullscreen="true" frameborder="0"></iframe>';
                        else if(sChat.inArray(hostArr, 'soundcloud')) return s+'<br/><iframe  onload="sChat.updateChatlistView();" style="height:120;width:80%;" scrolling="no" frameborder="0" src="https://w.soundcloud.com/player/?url='+a+'"></iframe>';
                    return s+'<a href="'+a+'" onclick="window.open(this.href); return false;">'+helper.truncate(a, 35)+'</a>';
                }
                }
            });
    },
    replaceVK:function(str, t){
        var method, args, id = 'vk' + this.uniCounter++, imC = 0;
        var p=t.split(/&amp;|[=\?&/\\#]+/);
        if(t.indexOf('/wall')===0){
            method='wall.getById';
            args='posts='+t.slice(5);
        } else if(p.indexOf('w')>0){
            method='wall.getById';
            args='posts='+p[p.indexOf('w')+1].slice(4);
        } else return sChat.replaceHyperLinks(str, true);
        $.ajax({
            url:'https://api.vk.com/method/'+method+'?'+args,
            jsonp:"callback",
            dataType:"jsonp",
            data:null,
            async:false,
            success:function(r){
                var inner='<a href="'+str+'" onclick="window.open(this.href); return false;"><img src="img/vk.png" style="width:32px;float:right;"></a>'+r.response[0].text;
                if(r.response[0].attachments){
                    inner+=r.response[0].text.length?'<br><br>':'';
                    for(var i=0; i<r.response[0].attachments.length; i++){
                        var at=r.response[0].attachments[i];
                        switch(at.type){
                        case 'photo':
                            inner+=(at.photo.src_big||at.photo.src)+' ';
                            imC++;
                            break;
                        case 'audio':
                            inner+='<br><span><b>'+at.audio.artist+'</b> - '+at.audio.title+'</span><br>'+at.audio.url+' ';
                            break;
                        case 'link':
                            inner+=at.link.url+' ';
                            break;
                        case 'doc':
                            inner += at.doc.ext === 'gif' ? sChat.replaceGif(at.doc.url) + ' ' : '<a href="' + at.doc.url + '" onclick="window.open(this.href); return false;"><span style=\'border-radius:15%;background-color:#6BA0D0;\'>&nbsp;' + at.doc.ext + '&nbsp;</span></a>';
                            break;
                        default:
                            inner+='<span style=\'border-radius:15%;background-color:#6BA0D0;\'>&nbsp;'+at.type+'&nbsp;</span>';
                            break;
                        }
                    }
                }
                document.getElementById(id).innerHTML=sChat.replaceHyperLinks(inner, imC<4);
                document.getElementById(id).className='vkPost';
                sChat.updateChatlistView();
            }
        });
        return '<div id=\''+id+'\' onclick="this.style.maxHeight=\'100%\';this.style.cursor=\'default\';sChat.updateChatlistView();"><a href="'+str+'" onclick="window.open(this.href); return false;">'+helper.truncate(str, 35)+'</a></div>';
    },
    replaceGif: function(url){ //сделай же что-то с этим
        var str = '<a href="' + url + '" onclick="window.open(this.href); return false;"><img onload="sChat.updateChatlistView();" class="bbCodeImage" style="max-width:90%; max-height:200px;" src="' + url + '"/></a>';
        return str;
    },
    replaceEmoticons:function(text){
        if(!sConfig.settings['emoticons']) return text;
        if(!arguments.callee.regExp){
            var regExpStr='^(.*)(';
            for(var i=0; i<sConfig.emoticonCodes.length; i++){
                if(i!==0) regExpStr+='|';
                regExpStr+='(?:'+this.escapeRegExp(sConfig.emoticonCodes[i])+')';
            }
            regExpStr+=')(.*)$';
            arguments.callee.regExp=new RegExp(regExpStr, 'gm');
        }
        return text.replace(arguments.callee.regExp, function(str, p1, p2, p3){
                if(!arguments.callee.regExp) arguments.callee.regExp=new RegExp('(="[^"]*$)|(&[^;]*$)', '');
                // Avoid replacing emoticons in tag attributes or XHTML entities:
                if(p1.match(arguments.callee.regExp)) return str;
                if(p2) return sChat.replaceEmoticons(p1)+'<img src="img/emoticons/'+sConfig.emoticonFiles[sConfig.emoticonCodes.indexOf(p2)]+'" alt="'+p2+'" />'+sChat.replaceEmoticons(p3);
                return str;
            }
        );
    },
    getActiveStyle:function(){
        var cookie=this.readCookie(sConfig.sessionName+'_style');
        var style=cookie?cookie:this.getPreferredStyleSheet();
        return style;
    },
    initStyle:function(){
        this.styleInitiated=true;
        this.setActiveStyleSheet(this.getActiveStyle());
    },
    persistStyle:function(){ if(this.styleInitiated) this.createCookie(sConfig.sessionName+'_style', this.getActiveStyleSheet(), sConfig.cookieExpiration); },
    setSelectedStyle:function(){
        if(this.dom['styleSelection']){
            var style=this.getActiveStyle();
            var styleOptions=this.dom['styleSelection'].getElementsByTagName('option');
            for(var i=0; i<styleOptions.length; i++)
                if(styleOptions[i].value===style){
                    styleOptions[i].selected=true;
                    break;
                }
        }
    },
    getSelectedStyle:function(){
        var styleOptions=this.dom['styleSelection'].getElementsByTagName('option');
        if(this.dom['styleSelection'].selectedIndex===-1) return styleOptions[0].value;
        else return styleOptions[this.dom['styleSelection'].selectedIndex].value;
    },
    setActiveStyleSheet:function(title){
        var i, a, titleFound=false;
        for(i=0; (a=document.getElementsByTagName('link')[i]); i++)
            if(a.getAttribute('rel').indexOf('style')!==-1&&a.getAttribute('title')){
                a.disabled=true;
                if(a.getAttribute('title')===title){
                    a.disabled=false;
                    titleFound=true;
                }
            }
        if(!titleFound&&title!==null) this.setActiveStyleSheet(this.getPreferredStyleSheet());
    },
    getActiveStyleSheet:function(){
        var i, a;
        for(i=0; (a=document.getElementsByTagName('link')[i]); i++) if(a.getAttribute('rel').indexOf('style')!==-1&&a.getAttribute('title')&&!a.disabled) return a.getAttribute('title');
        return null;
    },
    getPreferredStyleSheet:function(){
        var i, a;
        for(i=0; (a=document.getElementsByTagName('link')[i]); i++) if(a.getAttribute('rel').indexOf('style')!==-1&&a.getAttribute('rel').indexOf('alt')===-1&&a.getAttribute('title')) return a.getAttribute('title');
        return null;
    },
    switchLanguage:function(langCode){ window.location.search='?lang='+langCode; },
    createCookie:function(name, value, days){
        var expires='';
        if(days){
            var date=new Date();
            date.setTime(date.getTime()+(days*24*60*60*1000));
            expires='; expires='+date.toGMTString();
        }
        var path='; path='+sConfig.cookiePath;
        var domain=sConfig.cookieDomain?'; domain='+sConfig.cookieDomain:'';
        var secure=sConfig.cookieSecure?'; secure':'';
        document.cookie=name+'='+this.encodeText(value)+expires+path+domain+secure;
    },
    readCookie:function(name){
        if(!document.cookie) return null;
        var nameEQ=name+'=';
        var ca=document.cookie.split(';');
        for(var i=0; i<ca.length; i++){
            var c=ca[i];
            while(c.charAt(0)===' ') c=c.substring(1, c.length);
            if(c.indexOf(nameEQ)===0) return this.decodeText(c.substring(nameEQ.length, c.length));
        }
        return null;
    },
    debugMessage:function(msg, e){ if(sConfig.debug) console.log('Socio!Chat: '+msg+' exception: ', e); },
    isMenuOpened:function(){ return document.getElementById('onlineListContainer')&&document.getElementById('onlineListContainer').style.display==='block'; },
    fillSoundSelection:function(selectionID, selectedSound){
        var selection=document.getElementById(selectionID);
        // Skip the first, empty selection:
        var i=1;
        for(var key in sConfig.soundFiles){
            selection.options[i]=new Option(key, key);
            if(key===selectedSound) selection.options[i].selected=true;
            i++;
        }
    }
};