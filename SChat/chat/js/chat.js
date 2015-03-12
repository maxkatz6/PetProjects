/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 *
 * The SELFHTML documentation has been used throughout this project:
 * http://selfhtml.org
 *
 * Stylesheet and cookie methods have been inspired by Paul Sowden (A List Apart):
 * http://www.alistapart.com/stories/alternate/
 */

// AJAX Chat client side logic:
var ajaxChat = {
    settingsInitiated: null,
    styleInitiated: null,
    initializeFunction: null,
    finalizeFunction: null,
    loginChannelID: null,
    loginChannelName: null,
    timerRate: null,
    timer: null,
    ajaxURL: null,
    baseURL: null,
    regExpMediaUrl: null,
    dirs: null,
    startChatOnLoad: null,
    chatStarted: null,
    domIDs: null,
    dom: null,
    settings: null,
    nonPersistentSettings: null,
    unusedSettings: null,
    bbCodeTags: null,
    colorCodes: null,
    emoticonCodes: null,
    emoticonFiles: null,
    soundFiles: null,
    sounds: null,
    soundTransform: null,
    sessionName: null,
    cookieExpiration: null,
    cookiePath: null,
    cookieDomain: null,
    cookieSecure: null,
    chatBotName: null,
    chatBotID: null,
    allowUserMessageDelete: null,
    inactiveTimeout: null,
    privateChannelDiff: null,
    privateMessageDiff: null,
    showChannelMessages: null,
    messageTextMaxLength: null,
    socketServerEnabled: null,
    socketServerHost: null,
    socketServerPort: null,
    socketServerChatID: null,
    socket: null,
    socketIsConnected: null,
    socketTimerRate: null,
    socketReconnectTimer: null,
    socketRegistrationID: null,
    userID: null,
    userName: null,
    userRole: null,
    channelID: null,
    channelName: null,
    channelSwitch: null,
    usersList: null,
    userNamesList: null,
    userMenuCounter: null,
    encodedUserName: null,
    userNodeString: null,
    ignoredUserNames: null,
    lastID: null,
    localID: null,
    lang: null,
    langCode: null,
    baseDirection: null,
    originalDocumentTitle: null,
    blinkInterval: null,
    infocus:true,
    httpRequest: null,
    retryTimer: null,
    retryTimerDelay: null,
    requestStatus: null,
    DOMbuffering: null,
    DOMbuffer: null,
    DOMbufferRowClass: null,
    debug: null,
    inUrlBBCode: null,

    audioHtml5: [],

    init: function(config, lang, initSettings, initStyle, initialize, initializeFunction, finalizeFunction) {
        this.httpRequest = {};
        this.usersList = [];
        this.userNamesList = [];
        this.userMenuCounter = 0;
        this.lastID = 0;
        this.localID = 0;
        this.lang = lang;
        this.requestStatus = 'ok';
        this.DOMbufferRowClass = 'rowOdd';
        this.inUrlBBCode = false;
        this.initConfig(config);
        this.initDirectories();
        if (initSettings) {
            this.initSettings();
        }
        if (initStyle) {
            this.initStyle();
        }
        this.initializeFunction = initializeFunction;
        this.finalizeFunction = finalizeFunction;
        if (initialize) {
            var self = this;
            this.addEvent(window, 'load', function () {
                self.initialize();
            });
        }

        var a = document.createElement('audio');
        if (!!a.canPlayType) {
            this.audioHtml5['mp3'] = !!(a.canPlayType('audio/mpeg;').replace(/no/, ''));
            this.audioHtml5['ogg'] = !!(a.canPlayType && a.canPlayType('audio/ogg; codecs="vorbis"').replace(/no/, ''));
            this.audioHtml5['wav'] = !!(a.canPlayType && a.canPlayType('audio/wav; codecs="1"').replace(/no/, ''));
        }
    },

    initConfig: function(config) {
        this.loginChannelID = config['loginChannelID'];
        this.loginChannelName = config['loginChannelName'];
        this.timerRate = config['timerRate'];
        this.ajaxURL = config['ajaxURL'];
        this.baseURL = config['baseURL'];
        this.regExpMediaUrl = config['regExpMediaUrl'];
        this.startChatOnLoad = config['startChatOnLoad'];
        this.domIDs = config['domIDs'];
        this.settings = config['settings'];
        this.nonPersistentSettings = config['nonPersistentSettings'];
        this.bbCodeTags = config['bbCodeTags'];
        this.colorCodes = config['colorCodes'];
        this.emoticonCodes = config['emoticonCodes'];
        this.emoticonFiles = config['emoticonFiles'];
        this.soundFiles = config['soundFiles'];
        this.sessionName = config['sessionName'];
        this.cookieExpiration = config['cookieExpiration'];
        this.cookiePath = config['cookiePath'];
        this.cookieDomain = config['cookieDomain'];
        this.cookieSecure = config['cookieSecure'];
        this.chatBotName = config['chatBotName'];
        this.chatBotID = config['chatBotID'];
        this.allowUserMessageDelete = config['allowUserMessageDelete'];
        this.inactiveTimeout = config['inactiveTimeout'];
        this.privateChannelDiff = config['privateChannelDiff'];
        this.privateMessageDiff = config['privateMessageDiff'];
        this.showChannelMessages = config['showChannelMessages'];
        this.messageTextMaxLength = config['messageTextMaxLength'];
        this.socketServerEnabled = config['socketServerEnabled'];
        this.socketServerHost = config['socketServerHost'];
        this.socketServerPort = config['socketServerPort'];
        this.socketServerChatID = config['socketServerChatID'];
        this.debug = config['debug'];
        this.DOMbuffering = false;
        this.DOMbuffer = "";
        this.retryTimerDelay = this.inactiveTimeout * (helper.isMobile() ? 30000 : 1500); //the best
    },

    initDirectories: function() {
        this.dirs = {};
        this.dirs['emoticons'] = this.baseURL + 'img/emoticons/';
        this.dirs['sounds'] = this.baseURL + 'sounds/';
        this.dirs['flash'] = this.baseURL + 'flash/';
    },

    initSettings: function() {
        var cookie = this.readCookie(this.sessionName + '_settings'),
            i,
            settingsArray,
            setting,
            key,
            value,
            number;
        this.settingsInitiated = true;
        this.unusedSettings = {};
        if (cookie) {
            settingsArray = cookie.split('&');
            for (i = 0; i < settingsArray.length; i++) {
                setting = settingsArray[i].split('=');
                if (setting.length === 2) {
                    key = setting[0];
                    value = this.decodeText(setting[1]);
                    switch (value) {
                    case 'true':
                        value = true;
                        break;
                    case 'false':
                        value = false;
                        break;
                    case 'null':
                        value = null;
                        break;
                    default:
                        number = parseFloat(value);
                        if (!isNaN(number)) {
                            if (parseInt(number) === number) {
                                value = parseInt(number);
                            } else {
                                value = number;
                            }
                        }
                    }
                    if (this.inArray(this.nonPersistentSettings, key)) {
                        // The setting is not used, store it for the persistSettings method:
                        this.unusedSettings[key] = value;
                    } else {
                        this.settings[key] = value;
                    }
                }
            }
        }
    },

    persistSettings: function() {
        var settingsArray;
        if (this.settingsInitiated) {
            settingsArray = [];
            for (var property in this.settings) {
                if (this.inArray(this.nonPersistentSettings, property)) {
                    if (this.unusedSettings && this.unusedSettings[property]) {
                        // Store the unusedSetting previously stored:
                        this.settings[property] = this.unusedSettings[property];
                    } else {
                        continue;
                    }
                }
                settingsArray.push(property + '=' + this.encodeText(this.settings[property]));
            }
            this.createCookie(this.sessionName + '_settings', settingsArray.join('&'), this.cookieExpiration);
        }
    },

    getSettings: function() {
        return this.settings;
    },

    getSetting: function(key) {
        // Only return null if setting is null or undefined, not if it is false:
        for (var property in this.settings) {
            if (property === key) {
                return this.settings[key];
            }
        }
        return null;
    },

    setSetting: function(key, value) {
        this.settings[key] = value;
    },

    initializeSettings: function() {
        if (this.settings['persistFontColor'] && this.settings['fontColor']) {
            // Set the inputField font color to the font color:
            if (this.dom['inputField']) {
                this.dom['inputField'].style.color = this.settings['fontColor'];
            }
        }
    },

    initialize: function() {
        this.setUnloadHandler();
        this.initializeDocumentNodes();
        this.loadPageAttributes();
        this.initEmoticons();
        this.initColorCodes();
        this.initializeSettings();
        this.setSelectedStyle();
        // preload the Alert icon (it can't display if there's no connection unless it's cached!)
        this.setStatus('retrying');
        if (typeof this.initializeFunction === 'function') {
            this.initializeFunction();
        }
        if (!this.isCookieEnabled()) {
            this.addChatBotMessageToChatList('/error CookiesRequired');
        } else {
            if (this.startChatOnLoad) {
                this.startChat();
            } else {
                this.setStartChatHandler();
                this.requestTeaserContent();
            }
        }
        if (helper.isMobile())     //на телефоне выводит список онлайн
            this.sendMessage('/who'); //хз, как оно будет, работает через раз
    },

    requestTeaserContent: function() {
        var params = '&view=teaser';
        params += '&getInfos=' + this.encodeText('userID,userName,userRole');
        if (!isNaN(parseInt(this.loginChannelID))) {
            params += '&channelID=' + this.loginChannelID;
        } else if (this.loginChannelName !== null) {
            params += '&channelName=' + this.encodeText(this.loginChannelName);
        }
        this.updateChat(params);
    },

    setStartChatHandler: function() {
        if (this.dom['inputField']) {
            this.dom['inputField'].onfocus = function() {
                ajaxChat.startChat();
                // Reset the onfocus event on first call:
                ajaxChat.dom['inputField'].onfocus = '';
            };
        }
    },

    startChat: function() {
        this.chatStarted = true;
        if (this.dom['inputField']) {
            this.dom['inputField'].focus();
        }
        this.loadFlashInterface();
        this.startChatUpdate();
    },

    loadPageAttributes: function() {
        var htmlTag = document.getElementsByTagName('html')[0];
        this.langCode = htmlTag.getAttribute('lang') ? htmlTag.getAttribute('lang') : 'en';
        this.baseDirection = htmlTag.getAttribute('dir') ? htmlTag.getAttribute('dir') : 'ltr';
    },

    setUnloadHandler: function() {
        // Make sure finalize() is called on page unload:
        var onunload = window.onunload;
        if (typeof onunload !== 'function') {
            window.onunload = function() {
                ajaxChat.finalize();
            };
        } else {
            window.onunload = function() {
                ajaxChat.finalize();
                onunload();
            };
        }
    },

    updateDOM: function(id, str, prepend, overwrite) {
        var domNode = this.dom[id] ? this.dom[id] : document.getElementById(id);
        if (!domNode) {
            return;
        }
        try {
            // Test for validity before adding the string to the DOM:
            domNode.cloneNode(false).innerHTML = str;
            if (overwrite) {
                domNode.innerHTML = str;
            } else if (prepend) {
                domNode.innerHTML = str + domNode.innerHTML;
            } else {
                domNode.innerHTML += str;
            }
        } catch (e) {
            this.addChatBotMessageToChatList('/error DOMSyntax ' + id);
            this.updateChatlistView();
        }
    },

    initializeDocumentNodes: function() {
        this.dom = {};
        for (var key in this.domIDs) {
            this.dom[key] = document.getElementById(this.domIDs[key]);
        }
    },

    initEmoticons: function() {
        this.DOMbuffer = "";
        for (var i = 0; i < this.emoticonCodes.length; i++) {
            // Replace specials characters in emoticon codes:
            this.emoticonCodes[i] = this.encodeSpecialChars(this.emoticonCodes[i]);
            this.DOMbuffer = this.DOMbuffer
                + '<a href="javascript:ajaxChat.insertText(\''
                + this.scriptLinkEncode(this.emoticonCodes[i])
                + '\');"><img src="'
                + this.dirs['emoticons']
                + this.emoticonFiles[i]
                + '" alt="'
                + this.emoticonCodes[i]
                + '" title="'
                + this.emoticonCodes[i]
                + '"/></a>';
        }
        if (this.dom['emoticonsContainer']) {
            this.updateDOM('emoticonsContainer', this.DOMbuffer);
        }
        this.DOMbuffer = "";
    },

    initColorCodes: function() {
        if (this.dom['colorCodesContainer']) {
            this.DOMbuffer = "";
            for (var i = 0; i < this.colorCodes.length; i++) {
                this.DOMbuffer = this.DOMbuffer
                    + '<a href="javascript:ajaxChat.setFontColor(\''
                    + this.colorCodes[i]
                    + '\');" style="background-color:'
                    + this.colorCodes[i]
                    + ';" title="'
                    + this.colorCodes[i]
                    + '"></a>'
                    + "\n";
            }
            this.updateDOM('colorCodesContainer', this.DOMbuffer);
            this.DOMbuffer = "";
        }
    },


    startChatUpdate: function() {
        // Start the chat update and retrieve current user and channel info and set the login channel:
        var infos = 'userID,userName,userRole,channelID,channelName';
        if (this.socketServerEnabled) {
            infos += ',socketRegistrationID';
        }
        var params = '&getInfos=' + this.encodeText(infos);
        if (!isNaN(parseInt(this.loginChannelID))) {
            params += '&channelID=' + this.loginChannelID;
        } else if (this.loginChannelName !== null) {
            params += '&channelName=' + this.encodeText(this.loginChannelName);
        }
        this.updateChat(params);
    },

    updateChat: function(paramString) {
        var requestUrl = this.ajaxURL
            + '&lastID='
            + this.lastID;
        if (paramString) {
            requestUrl += paramString;
        }
        this.makeRequest(requestUrl, 'GET', null);
    },

    loadFlashInterface: function() {
        if (this.dom['flashInterfaceContainer']) {
            this.updateDOM(
                'flashInterfaceContainer',
                '<object id="ajaxChatFlashInterface" style="position:absolute; left:-100px;" '
                + 'classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" '
                + 'codebase="'
                + window.location.protocol
                + '//download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" '
                + 'height="1" width="1">'
                + '<param name="flashvars" value="bridgeName=ajaxChat"/>'
                + '<param name="src" value="' + this.dirs['flash'] + 'FABridge.swf"/>'
                + '<embed name="ajaxChatFlashInterface" type="application/x-shockwave-flash" pluginspage="'
                + window.location.protocol
                + '//www.macromedia.com/go/getflashplayer" '
                + 'src="' + this.dirs['flash'] + 'FABridge.swf" height="1" width="1" flashvars="bridgeName=ajaxChat"/>'
                + '</object>'
            );
            FABridge.addInitializationCallback('ajaxChat', this.flashInterfaceLoadCompleteHandler);
        }
    },

    flashInterfaceLoadCompleteHandler: function() {
        ajaxChat.initializeFlashInterface();
    },

    initializeFlashInterface: function() {
        if (this.socketServerEnabled) {
            this.socketTimerRate = (this.inactiveTimeout - 1) * 60 * 1000;
            this.socketConnect();
        }
        this.loadSounds();
    },

    socketConnect: function() {
        if (!this.socketIsConnected) {
            try {
                if (!this.socket && FABridge.ajaxChat) {
                    this.socket = FABridge.ajaxChat.create('flash.net.XMLSocket');
                    this.socket.addEventListener('connect', this.socketConnectHandler);
                    this.socket.addEventListener('close', this.socketCloseHandler);
                    this.socket.addEventListener('data', this.socketDataHandler);
                    this.socket.addEventListener('ioError', this.socketIOErrorHandler);
                    this.socket.addEventListener('securityError', this.socketSecurityErrorHandler);
                }
                this.socket.connect(this.socketServerHost, this.socketServerPort);
            } catch (e) {
                this.debugMessage('socketConnect', e);
            }
        }
        clearTimeout(this.socketReconnectTimer);
        this.socketReconnectTimer = null;
    },

    socketConnectHandler: function() {
        ajaxChat.socketIsConnected = true;
        // setTimeout is needed to avoid calling the flash interface recursively:
        setTimeout(ajaxChat.socketRegister, 0);
    },

    socketCloseHandler: function() {
        ajaxChat.socketIsConnected = false;
        if (ajaxChat.socket) {
            clearTimeout(ajaxChat.timer);
            ajaxChat.updateChat(null);
        }
    },

    socketDataHandler: function(event) {
        ajaxChat.socketUpdate(event.getData());
    },

    socketIOErrorHandler: function() {
        // setTimeout is needed to avoid calling the flash interface recursively (e.g. sound on new messages):
        setTimeout(function() { ajaxChat.addChatBotMessageToChatList('/error SocketIO'); }, 0);
        setTimeout(ajaxChat.updateChatlistView, 1);
    },

    socketSecurityErrorHandler: function() {
        // setTimeout is needed to avoid calling the flash interface recursively (e.g. sound on new messages):
        setTimeout(function() { ajaxChat.addChatBotMessageToChatList('/error SocketSecurity'); }, 0);
        setTimeout(ajaxChat.updateChatlistView, 1);
    },

    socketRegister: function() {
        if (this.socket && this.socketIsConnected) {
            try {
                this.socket.send(
                    '<register chatID="'
                    + this.socketServerChatID
                    + '" userID="'
                    + this.userID
                    + '" regID="'
                    + this.socketRegistrationID
                    + '"/>'
                );
            } catch (e) {
                this.debugMessage('socketRegister', e);
            }
        }
    },

    loadXML: function(_str) {
        if (!arguments.callee.parser) {
            try {
                // DOMParser native implementation (Mozilla, Opera):
                arguments.callee.parser = new DOMParser();
            } catch (e) {
                var customDOMParser = function() {};
                if (navigator.appName === 'Microsoft Internet Explorer') {
                    // IE implementation:
                    customDOMParser.prototype.parseFromString = function(str) {
                        if (!arguments.callee.XMLDOM) {
                            arguments.callee.XMLDOM = new ActiveXObject('Microsoft.XMLDOM');
                        }
                        arguments.callee.XMLDOM.loadXML(str);
                        return arguments.callee.XMLDOM;
                    };
                } else {
                    // Safari, Konqueror:
                    customDOMParser.prototype.parseFromString = function(str) {
                        if (!arguments.callee.httpRequest) {
                            arguments.callee.httpRequest = new XMLHttpRequest();
                        }
                        arguments.callee.httpRequest.open(
                            'GET',
                            'data:text/xml;charset=utf-8,' + this.encodeText(str),
                            false
                        );
                        arguments.callee.httpRequest.send(null);
                        return arguments.callee.httpRequest.responseXML;
                    };
                }
                arguments.callee.parser = new customDOMParser();
            }
        }
        return arguments.callee.parser.parseFromString(_str, 'text/xml');
    },

    socketUpdate: function(data) {
        var xmlDoc = this.loadXML(data);
        if (xmlDoc) {
            this.handleOnlineUsers(xmlDoc.getElementsByTagName('user'));
            // If the root node has the attribute "mode" set to "1" it is a channel message:
            if ((this.showChannelMessages || xmlDoc.firstChild.getAttribute('mode') !== '1') && !this.channelSwitch) {
                var channelID = xmlDoc.firstChild.getAttribute('channelID');
                if (channelID === this.channelID ||
                        parseInt(channelID) === parseInt(this.userID) + this.privateMessageDiff
                ) {
                    this.handleChatMessages(xmlDoc.getElementsByTagName('message'));
                }
            }
        }
    },

    setAudioVolume: function(volume) {
        volume = parseFloat(volume);
        if (!isNaN(volume)) {
            if (volume < 0) {
                volume = 0.0;
            } else if (volume > 1) {
                volume = 1.0;
            }
            this.settings['audioVolume'] = volume;
            try {
                if (!this.soundTransform) {
                    this.soundTransform = FABridge.ajaxChat.create('flash.media.SoundTransform');
                }
                this.soundTransform.setVolume(volume);
            } catch (e) {
                this.debugMessage('setAudioVolume', e);
            }
        }
    },

    loadSounds: function() {
        try {
            this.setAudioVolume(this.settings['audioVolume']);
            this.sounds = {};
            var sound, urlRequest;
            for (var key in this.soundFiles) {
                sound = FABridge.ajaxChat.create('flash.media.Sound');
                sound.addEventListener('complete', this.soundLoadCompleteHandler);
                sound.addEventListener('ioError', this.soundIOErrorHandler);
                urlRequest = FABridge.ajaxChat.create('flash.net.URLRequest');
                urlRequest.setUrl(this.dirs['sounds'] + this.soundFiles[key]);
                sound.load(urlRequest);
            }
        } catch (e) {
            this.debugMessage('loadSounds', e);
        }
    },

    soundLoadCompleteHandler: function(event) {
        var sound = event.getTarget();
        for (var key in ajaxChat.soundFiles) {
            // Get the sound key by matching the sound URL with the sound filename:
            if ((new RegExp(ajaxChat.soundFiles[key])).test(sound.getUrl())) {
                // Add the loaded sound to the sounds list:
                ajaxChat.sounds[key] = sound;
            }
        }
    },

    soundIOErrorHandler: function() {
        // setTimeout is needed to avoid calling the flash interface recursively (e.g. sound on new messages):
        setTimeout(function() { ajaxChat.addChatBotMessageToChatList('/error SoundIO'); }, 0);
        setTimeout(ajaxChat.updateChatlistView, 1);
    },

    playSound: function(soundID) {
        if (this.sounds && this.sounds[soundID]) {
            try {
                // play() parameters are
                // startTime:Number (default = 0),
                // loops:int (default = 0) and
                // sndTransform:SoundTransform  (default = null)
                return this.sounds[soundID].play(0, 0, this.soundTransform);
            } catch (e) {
                this.debugMessage('playSound', e);
            }
        }
        return null;
    },

    playSoundOnNewMessage: function(dateObject, userID, userName, userRole, messageID, messageText) {
        var messageParts;
        if (this.settings['audio'] && this.sounds && this.lastID && !this.channelSwitch) {

        if (new RegExp('(?:^|, |])' + this.userName + ', ','gm').test(messageText)){
                this.playSound(this.settings['soundPrivate']);
                return;
            }

            messageParts = messageText.split(' ', 1);
            switch (userID) {
            case this.chatBotID:
                switch (messageParts[0]) {
                case '/login':
                case '/channelEnter':
                    this.playSound(this.settings['soundEnter']);
                    break;
                case '/logout':
                case '/channelLeave':
                case '/kick':
                    this.playSound(this.settings['soundLeave']);
                    break;
                case '/error':
                    this.playSound(this.settings['soundError']);
                    break;
                default:
                    this.playSound(this.settings['soundChatBot']);
                }
                break;
            case this.userID:
                switch (messageParts[0]) {
                case '/privmsgto':
                    this.playSound(this.settings['soundPrivate']);
                    break;
                default:
                    this.playSound(this.settings['soundSend']);
                }
                break;
            default:
                switch (messageParts[0]) {
                case '/privmsg':
                    this.playSound(this.settings['soundPrivate']);
                    break;
                default:
                    this.playSound(this.settings['soundReceive']);
                }
                break;
            }
        }
    },

    fillSoundSelection: function(selectionID, selectedSound) {
        var selection = document.getElementById(selectionID);
        // Skip the first, empty selection:
        var i = 1;
        for (var key in this.soundFiles) {
            selection.options[i] = new Option(key, key);
            if (key === selectedSound) {
                selection.options[i].selected = true;
            }
            i++;
        }
    },

    setStatus: function(newStatus) {
        // status options are: ok, retrying, waiting
        if (!(newStatus === 'waiting' && this.requestStatus === 'retrying')) {
            this.requestStatus = newStatus;
        }

        if (this.dom['statusIcon']) {
            this.dom['statusIcon'].className = this.requestStatus;
        }
    },

    forceNewRequest: function() {
        ajaxChat.updateChat(null);
        ajaxChat.setStatus('retrying');
    },

    getHttpRequest: function(identifier) {
        if (!this.httpRequest[identifier]) {
            if (window.XMLHttpRequest) {
                this.httpRequest[identifier] = new XMLHttpRequest();
                if (this.httpRequest[identifier].overrideMimeType) {
                    this.httpRequest[identifier].overrideMimeType('text/xml');
                }
            } else if (window.ActiveXObject) {
                try {
                    this.httpRequest[identifier] = new ActiveXObject("Msxml2.XMLHTTP");
                } catch (e) {
                    try {
                        this.httpRequest[identifier] = new ActiveXObject("Microsoft.XMLHTTP");
                    } catch (e) {
                    }
                }
            }
        }
        return this.httpRequest[identifier];
    },

    makeRequest: function(url, method, data) {
        var identifier;
        this.setStatus('waiting');

        try {
            if (data) {
                // Create up to 50 HTTPRequest objects:
                if (!arguments.callee.identifier || arguments.callee.identifier > 50) {
                    arguments.callee.identifier = 1;
                } else {
                    arguments.callee.identifier++;
                }
                identifier = arguments.callee.identifier;
            } else {
                identifier = 0;
            }
            //if the response takes longer than retryTimerDelay to give an OK status, abort the connection and start again.
            this.retryTimer = setTimeout(ajaxChat.forceNewRequest, ajaxChat.retryTimerDelay);

            this.getHttpRequest(identifier).open(method, url, true);
            this.getHttpRequest(identifier).onreadystatechange = function() {
                try {
                    ajaxChat.handleResponse(identifier);
                } catch (__e) {
                    try {
                        clearTimeout(ajaxChat.timer);
                    } catch (e) {
                        this.debugMessage('makeRequest::clearTimeout', e);
                    }
                    try {
                        if (data) {
                            ajaxChat.addChatBotMessageToChatList('/error ConnectionTimeout');
                            ajaxChat.setStatus('retrying');
                            ajaxChat.updateChatlistView();
                        }
                    } catch (e) {
                        this.debugMessage('makeRequest::logRetry', e);
                    }
                    try {
                        ajaxChat.timer = setTimeout(function() { ajaxChat.updateChat(null); }, ajaxChat.timerRate);
                    } catch (e) {
                        this.debugMessage('makeRequest::setTimeout', e);
                    }
                }
            };
            if (method === 'POST') {
                this.getHttpRequest(identifier).setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            }
            this.getHttpRequest(identifier).send(data);
        } catch (_e) {
            clearTimeout(this.timer);
            if (data) {
                this.addChatBotMessageToChatList('/error ConnectionTimeout');
                ajaxChat.setStatus('retrying');
                this.updateChatlistView();
            }
            this.timer = setTimeout(function() { ajaxChat.updateChat(null); }, this.timerRate);
        }
    },

    handleResponse: function(identifier) {
        var json;
        if (this.getHttpRequest(identifier).readyState === 4) {
            if (this.getHttpRequest(identifier).status === 200) {
                clearTimeout(ajaxChat.retryTimer);
                json = this.getHttpRequest(identifier).responseText;
                ajaxChat.setStatus('ok');
            } else {
                // Connection status 0 can be ignored.
                if (this.getHttpRequest(identifier).status === 0) {
                    this.setStatus('waiting');
                    this.updateChatlistView();
                    return false;
                } else {
                    this.addChatBotMessageToChatList('/error ConnectionStatus ' + this.getHttpRequest(identifier).status);
                    this.setStatus('retrying');
                    this.updateChatlistView();
                    return false;
                }
            }
        }
        if (!json) {
            return false;
        }
        this.handleJSON(JSON.parse(json));
        return true;
    },

    handleJSON: function(json) {
        this.handleInfoMessages(json.infos);
        this.handleOnlineUsers(json.users);
        this.handleChatMessages(json.msgs);
        this.channelSwitch = null;
        this.setChatUpdateTimer();
    },

    setChatUpdateTimer: function() {
        clearTimeout(this.timer);
        if (this.chatStarted) {
            var timeout;
            if (this.socketIsConnected) {
                timeout = this.socketTimerRate;
            } else {
                timeout = this.timerRate;
                if (this.socketServerEnabled && !this.socketReconnectTimer) {
                    // If the socket connection fails try to reconnect once in a minute:
                    this.socketReconnectTimer = setTimeout(ajaxChat.socketConnect, 60000);
                }
            }
            this.timer = setTimeout(function() { ajaxChat.updateChat(null); }, timeout);
        }
    },

    handleInfoMessages: function(infos) {
        for (var key in infos) {
            var infoData = infos[key][0];
            switch (key) {
            case 'channelSwitch':
                this.clearChatList();
                this.clearOnlineUsersList();
                this.setSelectedChannel(infoData);
                this.channelName = infoData;
                this.channelSwitch = true;
                break;
            case 'channelName':
                this.setSelectedChannel(infoData);
                this.channelName = infoData;
                break;
            case 'channelID':
                this.channelID = parseInt(infoData);
                break;
            case 'userID':
                this.userID = parseInt(infoData);
                break;
            case 'userName':
                this.userName = infoData;
                this.encodedUserName = this.scriptLinkEncode(infoData);
                this.userNodeString = null;
                break;
            case 'userRole':
                this.userRole = parseInt(infoData);
                break;
            case 'logout':
                this.handleLogout(infoData);
                return;
            case 'socketRegistrationID':
                this.socketRegistrationID = parseInt(infoData);
                this.socketRegister();
            }
        }
    },
    
    ////////////////////
    ////            ////
    //////////////////// 
    ////            ////
    ////////////////////
    handleOnlineUsers: function(json) {
        if (json.length) {
            var index,
                userID,
                userName,
                userRole,
                userInfo,
                i,
                onlineUsers = [];
            for (i = 0; i < json.length; i++) {
                userID = parseInt(json[i].userID);
                userName = json[i].userName ? json[i].userName : '';
                userRole = parseInt(json[i].userRole);
                userInfo = JSON.parse(json[i].userInfo);
                onlineUsers.push(userID);
                index = this.arraySearch(userID, this.usersList);
                if (index === -1) {
                    this.addUserToOnlineList(userID, userName, userRole, userInfo);
                } else if (this.userNamesList[index] !== userName) {
                    this.removeUserFromOnlineList(userID, index);
                    this.addUserToOnlineList(userID, userName, userRole, userInfo);
                }
            }
            // Clear the offline users from the online users list:
            for (i = 0; i < this.usersList.length; i++) {
                if (!this.inArray(onlineUsers, this.usersList[i])) {
                    this.removeUserFromOnlineList(this.usersList[i], i);
                }
            }
            this.setOnlineListRowClasses();
        }
    },
    ////////////////////
    ////            ////
    ////////////////////
    ////            ////
    ////////////////////
    handleChatMessages: function(json) {
        var userName, textNode, messageText, i;
        if (json.length) {
            for (i = 0; i < json.length; i++) {
                this.DOMbuffering = true;
                userName = json[i].userName ? json[i].userName : '';
                messageText = json[i].text ? json[i].text : '';
                if (i === (json.length - 1)) {
                    this.DOMbuffering = false;
                }
                this.addMessageToChatList(
                    new Date(json[i].dateTime.replace(/-/g,'/')),
                    parseInt(json[i].userID),
                    userName,
                    parseInt(json[i].userRole),
                    parseInt(json[i].id),
                    messageText,
                    parseInt(json[i].channelID),
                    json[i].ip
                );
            }
            this.DOMbuffering = false;
            this.updateChatlistView();
            this.lastID = parseInt(json[json.length - 1].id);
        }
    },

    setSelectedChannel: function(channel) {
        var channelSelected = false,
            i,
            option,
            text;
        if (this.dom['channelSelection']) {
            // Replace the entities in the channel name with their character equivalent:
            channel = this.decodeSpecialChars(channel);
            for (i = 0; i < this.dom['channelSelection'].options.length; i++) {
                if (this.dom['channelSelection'].options[i].value === channel) {
                    this.dom['channelSelection'].options[i].selected = true;
                    channelSelected = true;
                    break;
                }
            }
            // The given channel is not in the list, add it:
            if (!channelSelected) {
                option = document.createElement('option');
                text = document.createTextNode(channel);
                option.appendChild(text);
                option.setAttribute('value', channel);
                option.setAttribute('selected', 'selected');
                this.dom['channelSelection'].appendChild(option);
            }
        }
    },

    removeUserFromOnlineList: function(userID, index) {
        this.usersList.splice(index, 1);
        this.userNamesList.splice(index, 1);
        if (this.dom['onlineList']) {
            this.dom['onlineList'].removeChild(this.getUserNode(userID));
        }
    },

    addUserToOnlineList: function(userID, userName, userRole, userInfo) {
        this.usersList.push(userID);
        this.userNamesList.push(userName);
        if (this.dom['onlineList']) {
            this.updateDOM(
                'onlineList',
                this.getUserNodeString(userID, userName, userRole, userInfo),
                (this.userID === userID)
            );
        }
    },
    ////////////////////
    ////            ////
    ////////////////////
    toUser: function(nick) {
        if (!(new RegExp('(?:^|, )' + nick + ', ','gm').test(this.dom['inputField'].value))) {
            this.dom['inputField'].value = nick + ", " + this.dom['inputField'].value;
        }
        this.dom['inputField'].focus();
        this.dom['inputField'].selectionStart = this.dom['inputField'].selectionEnd = this.dom['inputField'].value.length; //why not
    },
    ////            ////
    ////////////////////
    getUserNodeString: function(userID, userName, userRole, userInfo) {
        var encodedUserName, str;
        if (this.userNodeString && userID === this.userID) {
            return this.userNodeString;
        } else {
            encodedUserName = this.scriptLinkEncode(userName);
            str = '<div id="' + this.getUserDocumentID(userID) + '">'
                + "<div style='display: inline-block; width: 93%;position: relative; height:30px'>"
                + "<img style=\"position: absolute;left: 2px; bottom:1px;\" src=\"" + ((userInfo.avatar && userInfo.avatar !== "anon" && userInfo.avatar !== '') ? "../" + userInfo.avatar : "img/anon.png") + "\" border=\"0\" width=\"30\" height=\"30\" ></img>"
                + "<a style='font-size: 13px; left: 38px; bottom: 6px;position: absolute;' href=\"javascript:ajaxChat.toUser('" + userName + "');\">" + userName + "</a>"
                + (userInfo.tim !== "none" ? "<img src=\"img/tim/" + userInfo.tim + ".png\" border=\"0\" style=\"position: absolute;right: 38px;bottom: 5px;\" title=\""+helper.getTIM(userInfo.tim)+"\"></img>" : "")
                + (userInfo.gender && userInfo.gender !== 'n' ? "<img src=\"img/gender/"+userInfo.gender+".png\" border=\"0\" style=\"position: absolute;right: 20px;bottom: 6px;height: 13px;\" title=\""+(userInfo.gender === 'm'? 'Мужской' : 'Женский')+"\">" : '')
                + "<a id='showMenuButton' style=' position: absolute; right: 0px; bottom: 4px; background-position: -46px 0px;' href=\"javascript:ajaxChat.toggleUserMenu(\'"
                + this.getUserMenuDocumentID(userID) + "\', \'" + encodedUserName + '\', ' + userID + " );\"></a>"
                + "</div>"
                + '<ul class="userMenu" style="display:none;" id="' + this.getUserMenuDocumentID(userID) + ((userID === this.userID) ? '">' + this.getUserNodeStringItems(encodedUserName, userID, false) : '">') + '</ul>'
                + '</div>';

            if (userID === this.userID) {
                this.userNodeString = str;
            }
            return str;
        }
    },

    toggleUserMenu: function(menuID, userName, userID) {
        // If the menu is empty, fill it with user node menu items before toggling it.
        var isInline = false;
        if (menuID.indexOf('ium') >= 0) {
            isInline = true;
        }
        if (!document.getElementById(menuID).firstChild) {
            this.updateDOM(
                menuID,
                this.getUserNodeStringItems(
                    this.encodeText(this.addSlashes(this.getScriptLinkValue(userName))),
                    userID,
                    isInline
                ),
                false,
                true
            );
        }
        this.showHide(menuID);
        this.dom['chatList'].scrollTop = this.dom['chatList'].scrollHeight;
    },

    getUserNodeStringItems: function(encodedUserName, userID, isInline) {
        var menu;
        if (encodedUserName !== this.encodedUserName) {
            menu = '<li><a target="_blank" href="/index.php/jomsocial/'
                + "/index.php?option=com_community&view=profile&userid="+userID
                + '/profile/" >Профиль</a></li>'
                + '<li><a href="javascript:ajaxChat.insertMessageWrapper(\'/msg '
                + encodedUserName
                + ' \');">'
                + this.lang['userMenuSendPrivateMessage']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.insertMessageWrapper(\'/describe '
                + encodedUserName
                + ' \');">'
                + this.lang['userMenuDescribe']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/query '
                + encodedUserName
                + '\');">'
                + this.lang['userMenuOpenPrivateChannel']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/query\');">'
                + this.lang['userMenuClosePrivateChannel']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/ignore '
                + encodedUserName
                + '\');">'
                + this.lang['userMenuIgnore']
                + '</a></li>';
            if (isInline) {
                menu += '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/invite '
                    + encodedUserName
                    + '\');">'
                    + this.lang['userMenuInvite']
                    + '</a></li>'
                    + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/uninvite '
                    + encodedUserName
                    + '\');">'
                    + this.lang['userMenuUninvite']
                    + '</a></li>'
                    + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/whereis '
                    + encodedUserName
                    + '\');">'
                    + this.lang['userMenuWhereis']
                    + '</a></li>';
            }
            if (this.userRole === 2 || this.userRole === 3) {
                menu += '<li><a href="javascript:ajaxChat.insertMessageWrapper(\'/kick '
                    + encodedUserName
                    + ' \');">'
                    + this.lang['userMenuKick']
                    + '</a></li>'
                    + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/whois '
                    + encodedUserName
                    + '\');">'
                    + this.lang['userMenuWhois']
                    + '</a></li>';
            }
        } else {
            menu = '<li><a target="_blank" href="/index.php/jomsocial/profile/">Профиль</a></li>'
                + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/quit\');">'
                + this.lang['userMenuLogout']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/who\');">'
                + this.lang['userMenuWho']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/ignore\');">'
                + this.lang['userMenuIgnoreList']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/list\');">'
                + this.lang['userMenuList']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.insertMessageWrapper(\'/action \');">'
                + this.lang['userMenuAction']
                + '</a></li>'
                + '<li><a href="javascript:ajaxChat.insertMessageWrapper(\'/roll \');">'
                + this.lang['userMenuRoll']
                + '</a></li>';
            if (this.userRole === 1 || this.userRole === 2 || this.userRole === 3) {
                menu += '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/join\');">'
                    + this.lang['userMenuEnterPrivateRoom']
                    + '</a></li>';
                if (this.userRole === 2 || this.userRole === 3) {
                    menu += '<li><a href="javascript:ajaxChat.sendMessageWrapper(\'/bans\');">'
                        + this.lang['userMenuBans']
                        + '</a></li>';
                }
            }
        }
        return menu;
    },

    setOnlineListRowClasses: function() {
        if (this.dom['onlineList']) {
            var node = this.dom['onlineList'].firstChild;
            var rowEven = false;
            while (node) {
                node.className = (rowEven ? 'rowEven' : 'rowOdd');
                node = node.nextSibling;
                rowEven = !rowEven;
            }
        }
    },

    clearChatList: function() {
        while (this.dom['chatList'].hasChildNodes()) {
            this.dom['chatList'].removeChild(this.dom['chatList'].firstChild);
        }
    },

    clearOnlineUsersList: function() {
        this.usersList = [];
        this.userNamesList = [];
        if (this.dom['onlineList']) {
            while (this.dom['onlineList'].hasChildNodes()) {
                this.dom['onlineList'].removeChild(this.dom['onlineList'].firstChild);
            }
        }
    },

    getEncodedChatBotName: function() {
        if (typeof arguments.callee.encodedChatBotName === 'undefined') {
            arguments.callee.encodedChatBotName = this.encodeSpecialChars(this.chatBotName);
        }
        return arguments.callee.encodedChatBotName;
    },

    addChatBotMessageToChatList: function(messageText) {
        this.addMessageToChatList(
            new Date(),
            this.chatBotID,
            this.getEncodedChatBotName(),
            4,
            null,
            messageText,
            null
        );
    },

    addMessageToChatList: function(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip) {
        // Prevent adding the same message twice:
        if (this.getMessageNode(messageID)) {
            return;
        }
        if (!this.onNewMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip)) {
            return;
        }
        this.DOMbufferRowClass = this.DOMbufferRowClass === 'rowEven' ? 'rowOdd' : 'rowEven';

        document.getElementById('chatList').appendChild(this.getChatListMessageString(
            dateObject, userID, userName, userRole, messageID, messageText, channelID, ip
        ));
    },

    getChatListMessageString: function(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip) {
        var rowClass = this.DOMbufferRowClass,
            userClass = this.getRoleClass(userRole),
            colon = ': ';
        if (messageText.indexOf('/action') === 0 || messageText.indexOf('/me') === 0 || messageText.indexOf('/privaction') === 0) {
            userClass += ' action';
            colon = ' ';
        }
        if (messageText.indexOf('/privmsg') === 0 || messageText.indexOf('/privmsgto') === 0 || messageText.indexOf('/privaction') === 0) {
            rowClass += ' private';
        }
        if (new RegExp('(?:^|, |])' + this.userName + ', ','gm').test(messageText)){
            rowClass += ' toMe';
        }
        var dateTime = '<span class="dateTime">' + this.formatDate('(%H:%i:%s)', dateObject) + '</span> ';

        var newDiv = document.createElement('div');
        newDiv.className = rowClass;
        newDiv.id = this.getMessageDocumentID(messageID);
        newDiv.innerHTML = this.getDeletionLink(messageID, userID, userRole, channelID)
            + /*'<a href="javascript:ajaxChat.blinkMessage('+ messageID+ ');">'+*/dateTime//+'</a>'
            + '<span class="'
            + userClass
            + '"'
            + ' dir="'
            + this.baseDirection
            + "\" onclick=\"ajaxChat.toUser('" + userName + "');\">"
            + userName
            + '</span>'
            + colon
            + this.replaceText(messageText);

        return newDiv;

    },
    blinkMessage: function (messageID)
    {
        var messageNode = this.getMessageNode(messageID);
        //messageNode.style.display = 'none';
    },
  
    getMessageDocumentID: function(messageID) {
        return ((messageID === null) ? 'ajaxChat_lm_' + (this.localID++) : 'ajaxChat_m_' + messageID);
    },

    getMessageNode: function(messageID) {
        return ((messageID === null) ? null : document.getElementById(this.getMessageDocumentID(messageID)));
    },

    getUserDocumentID: function(userID) {
        return 'ajaxChat_u_' + userID;
    },

    getUserNode: function(userID) {
        return document.getElementById(this.getUserDocumentID(userID));
    },

    getUserMenuDocumentID: function(userID) {
        return 'ajaxChat_um_' + userID;
    },

    getInlineUserMenuDocumentID: function(menuID, index) {
        return 'ajaxChat_ium_' + menuID + '_' + index;
    },

    getDeletionLink: function(messageID, userID, userRole, channelID) {
        if (messageID !== null && this.isAllowedToDeleteMessage(messageID, userID, userRole, channelID)) {
            if (!arguments.callee.deleteMessage) {
                arguments.callee.deleteMessage = this.encodeSpecialChars(this.lang['deleteMessage']);
            }
            return '<a class="delete" title="'
                + arguments.callee.deleteMessage
                + '" href="javascript:ajaxChat.deleteMessage('
                + messageID
                + ');"> </a>'; // Adding a space - without any content Opera messes up the chatlist display
        }
        return '';
    },

    isAllowedToDeleteMessage: function(messageID, userID, userRole, channelID) {
        if ((((this.userRole === 1 && this.allowUserMessageDelete && (userID === this.userID ||
                parseInt(channelID) === parseInt(this.userID) + this.privateMessageDiff ||
                parseInt(channelID) === parseInt(this.userID) + this.privateChannelDiff)) ||
            (this.userRole === 5 && this.allowUserMessageDelete && (userID === this.userID ||
                parseInt(channelID) === parseInt(this.userID) + this.privateMessageDiff ||
                parseInt(channelID) === parseInt(this.userID) + this.privateChannelDiff)) ||
            this.userRole === 2) && userRole !== 3 && userRole !== 4) || this.userRole === 3) {
            return true;
        }
        return false;
    },


    ////////////////////
    ////            ////
    ////////////////////
    ////            ////
    ////////////////////
    onNewMessage: function(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip) {
        if (this.ignoreMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip)) {
            return false;
        }
        if (this.parseDeleteMessageCommand(messageText)) {
            return false;
        }
        this.blinkOnNewMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip);
        this.playSoundOnNewMessage(dateObject, userID, userName, userRole, messageID, messageText, channelID, ip);
        return true;
    },

    parseDeleteMessageCommand: function(messageText) {
        if (messageText.indexOf('/delete') === 0) {
            var messageID = messageText.substr(8);
            var messageNode = this.getMessageNode(messageID);
            if (messageNode) {
                var nextSibling = messageNode.nextSibling;
                try {
                    this.dom['chatList'].removeChild(messageNode);
                    if (nextSibling) {
                        this.updateChatListRowClasses(nextSibling);
                    }
                } catch (e) {
                }
            }
            return true;
        }
        return false;
    },

    blinkOnNewMessage: function(dateObject, userID, userName) {
        if (!this.infocus && this.lastID && !this.channelSwitch && userID !== this.userID) {
            clearInterval(this.blinkInterval);
            this.blinkInterval = setInterval(
                'ajaxChat.blinkUpdate(\'' + this.addSlashes(this.decodeSpecialChars(userName)) + '\')', 500);
        }
    },

    blinkUpdate: function(blinkStr) {
        if (this.infocus) arguments.callee.blink = this.settings[10]+1;
        if (!this.originalDocumentTitle) {
            this.originalDocumentTitle = document.title;
        }
        if (!arguments.callee.blink) {
            document.title = '[@ ] ' + blinkStr + ' - ' + this.originalDocumentTitle;
            arguments.callee.blink = 1;
        } else if (arguments.callee.blink > this.settings[10]) {
            clearInterval(this.blinkInterval);
            document.title = (this.infocus ? '' :'[+] ') + this.originalDocumentTitle;
            arguments.callee.blink = 0;
        } else {
            if (arguments.callee.blink % 2 !== 0) {
                document.title = '[@ ] ' + blinkStr + ' - ' + this.originalDocumentTitle;
            } else {
                document.title = '[ @] ' + blinkStr + ' - ' + this.originalDocumentTitle;
            }
            arguments.callee.blink++;
        }
    },

    updateChatlistView: function() {
        if (this.dom['chatList'].childNodes && this.settings['maxMessages']) {
            while (this.dom['chatList'].childNodes.length > this.settings['maxMessages']) {
                this.dom['chatList'].removeChild(this.dom['chatList'].firstChild);
            }
        }

        if (this.settings['autoScroll']) {
            this.dom['chatList'].scrollTop = this.dom['chatList'].scrollHeight;
        }
    },

    encodeText: function(text) {
        return encodeURIComponent(text);
    },

    decodeText: function(text) {
        return decodeURIComponent(text);
    },

    utf8Encode: function(plainText) {
        var utf8Text = '';
        for (var i = 0; i < plainText.length; i++) {
            var c = plainText.charCodeAt(i);
            if (c < 128) {
                utf8Text += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utf8Text += String.fromCharCode((c >> 6) | 192);
                utf8Text += String.fromCharCode((c & 63) | 128);
            } else {
                utf8Text += String.fromCharCode((c >> 12) | 224);
                utf8Text += String.fromCharCode(((c >> 6) & 63) | 128);
                utf8Text += String.fromCharCode((c & 63) | 128);
            }
        }
        return utf8Text;
    },

    utf8Decode: function(utf8Text) {
        var plainText = '';
        var c, c2, c3;
        var i = 0;
        while (i < utf8Text.length) {
            c = utf8Text.charCodeAt(i);
            if (c < 128) {
                plainText += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utf8Text.charCodeAt(i + 1);
                plainText += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utf8Text.charCodeAt(i + 1);
                c3 = utf8Text.charCodeAt(i + 2);
                plainText += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return plainText;
    },

    encodeSpecialChars: function(text) {
        return text.replace(
            /[&<>'"]/g,
            function(str) {
                switch (str) {
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
        
    decodeSpecialChars: function(text) {
        var regExp = new RegExp('(&amp;)|(&lt;)|(&gt;)|(&#39;)|(&quot;)', 'g');

        return text.replace(
            regExp,
            function(str) {
                switch (str) {
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

    inArray: function(array, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] === value) return true;
        }
        return false;
    },

    arraySearch: function(needle, haystack) {
        if (!Array.prototype.indexOf) { // IE<9
            var i = haystack.length;
            while (i--) {
                if (haystack[i] === needle) {
                    return i;
                }
            }
            return -1;
        } else {
            return haystack.indexOf(needle);
        }
    },

    stripTags: function(str) {
        if (!arguments.callee.regExp) {
            arguments.callee.regExp = new RegExp('<\\/?[^>]+?>', 'g');
        }

        return str.replace(arguments.callee.regExp, '');
    },

    stripBBCodeTags: function(str) {
        if (!arguments.callee.regExp) {
            arguments.callee.regExp = new RegExp('\\[\\/?[^\\]]+?\\]', 'g');
        }

        return str.replace(arguments.callee.regExp, '');
    },

    escapeRegExp: function(text) {
        if (!arguments.callee.regExp) {
            var specials = new Array(
                '^', '$', '*', '+', '?', '.', '|', '/',
                '(', ')', '[', ']', '{', '}', '\\'
            );
            arguments.callee.regExp = new RegExp(
                '(\\' + specials.join('|\\') + ')', 'g'
            );
        }
        return text.replace(arguments.callee.regExp, '\\$1');
    },

    addSlashes: function(text) {
        // Adding slashes in front of apostrophs and backslashes to ensure a valid JavaScript expression:
        return text.replace(/\\/g, '\\\\').replace(/\'/g, '\\\'');
    },

    removeSlashes: function(text) {
        // Removing slashes added by calling addSlashes(text) previously:
        return text.replace(/\\\\/g, '\\').replace(/\\\'/g, '\'');
    },

    formatDate: function(format, _date) {
        _date = (_date === null) ? new date() : _date;

        return format
            .replace(/%Y/g, _date.getFullYear())
            .replace(/%m/g, this.addLeadingZero(_date.getMonth() + 1))
            .replace(/%d/g, this.addLeadingZero(_date.getDate()))
            .replace(/%H/g, this.addLeadingZero(_date.getHours()))
            .replace(/%i/g, this.addLeadingZero(_date.getMinutes()))
            .replace(/%s/g, this.addLeadingZero(_date.getSeconds()));
    },

    addLeadingZero: function(number) {
        number = number.toString();
        if (number.length < 2) {
            number = '0' + number;
        }
        return number;
    },

    getUserIDFromUserName: function(userName) {
        var index = this.arraySearch(userName, this.userNamesList);
        if (index !== -1) {
            return this.usersList[index];
        }
        return null;
    },

    getUserNameFromUserID: function(userID) {
        var index = this.arraySearch(userID, this.usersList);
        if (index !== -1) {
            return this.userNamesList[index];
        }
        return null;
    },

    getRoleClass: function(roleID) {
        switch (parseInt(roleID)) {
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

    handleInputFieldKeyDown: function(event) {
        var text, lastWord, i;

        // Enter key without shift should send messages
        if (event.keyCode === 13 && !event.shiftKey) {
            this.sendMessage();
            try {
                event.preventDefault();
            } catch (e) {
                event.returnValue = false; // IE<9
            }
            return false;
        }
        // Tab should complete usernames
        else if (event.keyCode === 9 && !event.shiftKey) {
            text = this.dom['inputField'].value;
            if (text) {
                lastWord = text.match(/\w+/g).slice(-1)[0];

                if (lastWord.length > 2) {
                    for (i = 0; i < this.userNamesList.length; i++) {
                        if (this.userNamesList[i].replace("(", "").toLowerCase().indexOf(lastWord.toLowerCase()) === 0) {
                            this.dom['inputField'].value = text.replace(new RegExp(lastWord + '$'), this.userNamesList[i]);
                            break;
                        }
                    }
                }
            }
            try {
                event.preventDefault();
            } catch (e) {
                event.returnValue = false; // IE
            }
            return false;
        }
        return true;
    },

    handleInputFieldKeyUp: function() {
        this.updateMessageLengthCounter();
    },

    updateMessageLengthCounter: function() {
        if (this.dom['messageLengthCounter']) {
            this.updateDOM(
                'messageLengthCounter',
                this.dom['inputField'].value.length + '/1000',
                false,
                true
            );
        }
    },

    sendMessage: function(text) {
        text = text ? text : this.dom['inputField'].value;
        if (!text) {
            return;
        }
        text = this.parseInputMessage(text);
        if (text) {
            clearTimeout(this.timer);
            var message = 'lastID='
                + this.lastID
                + '&text='
                + this.encodeText(text);
            this.makeRequest(this.ajaxURL, 'POST', message);
        }
        this.dom['inputField'].value = '';
        this.dom['inputField'].focus();
        this.updateMessageLengthCounter();
    },

    parseInputMessage: function(text) {
        var textParts;
        if (text.charAt(0) === '/') {
            textParts = text.split(' ');
            switch (textParts[0]) {
            case '/ignore':
                text = this.parseIgnoreInputCommand(text, textParts);
                break;
            case '/clear':
                this.clearChatList();
                return false;
            }
            if (text && this.settings['persistFontColor'] && this.settings['fontColor']) {
                text = this.assignFontColorToCommandMessage(text, textParts);
            }
        } else {
            text = this.parseToUserBold(text);
            if (text && this.settings['persistFontColor'] && this.settings['fontColor']) {
                text = '[color=' + this.settings['fontColor'] + ']' + text + '[/color]';
            }
        }
        return text;
    },
    parseToUserBold: function(text){
        var r = '(?:(';
        for (var i = 0; i < this.userNamesList.length; i++)
        {
            r += this.userNamesList[i] + '|';
        }
        r+='Сервер), )*';
        return text.replace(new RegExp(r,'gm'), function (a,b) {return (a !== '' ? '[b]'+a+'[/b]':'');});
    },
    assignFontColorToCommandMessage: function(text, textParts) {
        switch (textParts[0]) {
        case '/msg':
        case '/describe':
            if (textParts.length > 2) {
                return textParts[0] + ' ' + textParts[1] + ' '
                    + '[color=' + this.settings['fontColor'] + ']'
                    + textParts.slice(2).join(' ')
                    + '[/color]';
            }
            break;
        case '/me':
        case '/action':
            if (textParts.length > 1) {
                return textParts[0] + ' '
                    + '[color=' + this.settings['fontColor'] + ']'
                    + textParts.slice(1).join(' ')
                    + '[/color]';
            }
            break;
        }
        return text;
    },

    parseIgnoreInputCommand: function(text, textParts) {
        var userName, ignoredUserNames = this.getIgnoredUserNames(), i;
        if (textParts.length > 1) {
            userName = this.encodeSpecialChars(textParts[1]);
            // Prevent adding the chatBot or current user to the list:
            if (userName === this.userName || userName === this.getEncodedChatBotName()) {
                // Display the list of ignored users instead:
                return this.parseIgnoreInputCommand(null, new Array('/ignore'));
            }
            if (ignoredUserNames.length > 0) {
                i = ignoredUserNames.length;
                while (i--) {
                    if (ignoredUserNames[i] === userName) {
                        ignoredUserNames.splice(i, 1);
                        this.addChatBotMessageToChatList('/ignoreRemoved ' + userName);
                        this.setIgnoredUserNames(ignoredUserNames);
                        this.updateChatlistView();
                        return null;
                    }
                }
            }
            ignoredUserNames.push(userName);
            this.addChatBotMessageToChatList('/ignoreAdded ' + userName);
            this.setIgnoredUserNames(ignoredUserNames);
        } else {
            if (ignoredUserNames.length === 0) {
                this.addChatBotMessageToChatList('/ignoreListEmpty -');
            } else {
                this.addChatBotMessageToChatList('/ignoreList ' + ignoredUserNames.join(' '));
            }
        }
        this.updateChatlistView();
        return null;
    },

    getIgnoredUserNames: function() {
        var ignoredUserNamesString;
        if (!this.ignoredUserNames) {
            ignoredUserNamesString = this.getSetting('ignoredUserNames');
            if (ignoredUserNamesString) {
                this.ignoredUserNames = ignoredUserNamesString.split(' ');
            } else {
                this.ignoredUserNames = [];
            }
        }
        return this.ignoredUserNames;
    },

    setIgnoredUserNames: function(ignoredUserNames) {
        this.ignoredUserNames = ignoredUserNames;
        this.setSetting('ignoredUserNames', ignoredUserNames.join(' '));
    },

    ignoreMessage: function(dateObject, userID, userName, userRole, messageID, messageText) {
        var textParts;
        if (userID === this.chatBotID && messageText.charAt(0) === '/') {
            textParts = messageText.split(' ');
            if (textParts.length > 1) {
                switch (textParts[0]) {
                case '/invite':
                case '/uninvite':
                case '/roll':
                    userName = textParts[1];
                    break;
                }
            }
        }
        return (this.inArray(this.getIgnoredUserNames(), userName));
    },

    deleteMessage: function(messageID) {
        var messageNode = this.getMessageNode(messageID), originalClass, nextSibling;
        if (messageNode) {
            originalClass = messageNode.className;
            this.addClass(messageNode, 'deleteSelected');
            if (confirm(this.lang['deleteMessageConfirm'])) {
                nextSibling = messageNode.nextSibling;
                try {
                    this.dom['chatList'].removeChild(messageNode);
                    if (nextSibling) {
                        this.updateChatListRowClasses(nextSibling);
                    }
                    this.updateChat('&delete=' + messageID);
                } catch (e) {
                    this.setClass(messageNode, originalClass);
                }
            } else {
                messageNode.className = originalClass;
            }
        }
    },

    updateChatListRowClasses: function(node) {
        var previousNode, rowEven;
        if (!node) {
            node = this.dom['chatList'].firstChild;
        }
        if (node) {
            previousNode = node.previousSibling;
            rowEven = (previousNode && previousNode.className === 'rowOdd') ? true : false;
            while (node) {
                node.className = (rowEven ? 'rowEven' : 'rowOdd');
                node = node.nextSibling;
                rowEven = !rowEven;
            }
        }
    },

    addEvent: function(elem, type, eventHandle) {
        if (!elem)
            return;
        if (elem.addEventListener) {
            elem.addEventListener(type, eventHandle, false);
        } else if (elem.attachEvent) {
            elem.attachEvent("on" + type, eventHandle);
        } else {
            elem["on" + type] = eventHandle;
        }
    },

    addClass: function(node, theClass) {
        if (!this.hasClass(node, theClass)) {
            node.className += ' ' + theClass;
        }
    },

    removeClass: function(node, theClass) {
        node.className = node.className.replace(new RegExp('(?:^|\\s)' + theClass + '(?!\\S)', 'g'), '');
    },

    hasClass: function(node, theClass) {
        return node.className.match(new RegExp('\\b' + theClass + '\\b'));
    },

    scriptLinkEncode: function(text) {
        return this.decodeText(this.encodeText(this.addSlashes(this.decodeSpecialChars(text))));
    },

    scriptLinkDecode: function(text) {
        return this.encodeSpecialChars(this.removeSlashes(this.decodeText(text)));
    },

    getScriptLinkValue: function(value) {
        // This method returns plainText encoded values from javascript links
        // The value has to be utf8Decoded for MSIE and Opera:
        if (typeof arguments.callee.utf8Decode === 'undefined') {
            switch (navigator.appName) {
            case 'Microsoft Internet Explorer':
            case 'Opera':
                arguments.callee.utf8Decode = true;
                return this.utf8Decode(value);
            default:
                arguments.callee.utf8Decode = false;
                return value;
            }
        } else if (arguments.callee.utf8Decode) {
            return this.utf8Decode(value);
        } else {
            return value;
        }
    },

    sendMessageWrapper: function(text) {
        this.sendMessage(this.getScriptLinkValue(text));
    },

    insertMessageWrapper: function(text) {
        this.insertText(this.getScriptLinkValue(text), true);
    },

    switchChannel: function(channel) {
        if (!this.chatStarted) {
            this.clearChatList();
            this.channelSwitch = true;
            this.loginChannelID = null;
            this.loginChannelName = channel;
            this.requestTeaserContent();
            return;
        }
        clearTimeout(this.timer);
        var message = 'lastID='
            + this.lastID
            + '&channelName='
            + this.encodeText(channel);
        this.makeRequest(this.ajaxURL, 'POST', message);
        if (this.dom['inputField']) {
            this.dom['inputField'].focus();
        }
    },

    logout: function() {
        clearTimeout(this.timer);
        var message = 'logout=true';
        this.makeRequest(this.ajaxURL, 'POST', message);
    },

    handleLogout: function(url) {
        window.location.href = url;
    },

    toggleSetting: function(setting, buttonID) {
        this.setSetting(setting, !this.getSetting(setting));
        if (buttonID) {
            this.updateButton(setting, buttonID);
        }
    },

    updateButton: function(setting, buttonID) {
        var node = document.getElementById(buttonID);
        if (node) {
            node.className = (this.getSetting(setting) ? 'button' : 'button off');
        }
    },

    showHide: function(id, styleDisplay, displayInline) {
        var node = document.getElementById(id);
        var button = node.parentNode.firstChild.lastChild;
        if (node) {
            if (styleDisplay) {
                node.style.display = styleDisplay;
            } else {
                if (node.style.display === 'none') {
                    node.style.display = (displayInline ? 'inline' : 'block');
                    if (button != null)
                        button.style.backgroundPosition = "-46px -22px";
                } else {
                    node.style.display = 'none';
                    if (button != null)
                        button.style.backgroundPosition = "-46px 0px";
                }
            }
        }
    },

    setPersistFontColor: function(bool) {
        this.settings['persistFontColor'] = bool;
        if (!this.settings['persistFontColor']) {
            this.settings['fontColor'] = null;
            if (this.dom['inputField']) {
                this.dom['inputField'].style.color = '';
            }
        }
    },

    setFontColor: function(color) {
        if (this.settings['persistFontColor']) {
            this.settings['fontColor'] = color;
            if (this.dom['inputField']) {
                this.dom['inputField'].style.color = color;
            }
            if (this.dom['colorCodesContainer']) {
                this.dom['colorCodesContainer'].style.display = 'none';
                if (this.dom['inputField']) {
                    this.dom['inputField'].focus();
                }
            }
        } else {
            this.insert('[color=' + color + ']', '[/color]');
        }
    },

    insertText: function(text, clearInputField) {
        if (clearInputField) {
            this.dom['inputField'].value = '';
        }
        this.insert(text, '');
    },

    insertBBCode: function(bbCode) {
        this.insert('[' + bbCode + ']', '[/' + bbCode + ']');
    },

    insert: function(startTag, endTag) {
        this.dom['inputField'].focus();
        var insText;
        // Internet Explorer:
        if (typeof document.selection !== 'undefined') {
            // Insert the tags:
            var range = document.selection.createRange();
            insText = range.text;
            range.text = startTag + insText + endTag;
            // Adjust the cursor position:
            range = document.selection.createRange();
            if (insText.length === 0) {
                range.move('character', -endTag.length);
            } else {
                range.moveStart('character', startTag.length + insText.length + endTag.length);
            }
            range.select();
        }
        else {
            var pos;
            // Firefox, etc. (Gecko based browsers):
            if (typeof this.dom['inputField'].selectionStart !== 'undefined') {
                // Insert the tags:
                var start = this.dom['inputField'].selectionStart;
                var end = this.dom['inputField'].selectionEnd;
                insText = this.dom['inputField'].value.substring(start, end);
                this.dom['inputField'].value = this.dom['inputField'].value.substr(0, start)
                    + startTag
                    + insText
                    + endTag
                    + this.dom['inputField'].value.substr(end);
                // Adjust the cursor position:
                if (insText.length === 0) {
                    pos = start + startTag.length;
                } else {
                    pos = start + startTag.length + insText.length + endTag.length;
                }
                this.dom['inputField'].selectionStart = pos;
                this.dom['inputField'].selectionEnd = pos;
            }
            // Other browsers:
            else {
                pos = this.dom['inputField'].value.length;
                this.dom['inputField'].value = this.dom['inputField'].value.substr(0, pos)
                    + startTag
                    + endTag
                    + this.dom['inputField'].value.substr(pos);
            }
        }
    },

    replaceText: function(text) {
        try {
            
            text = text.replace(/[\u200B-\u200D\uFEFF]/g, '');
            text = text.replace(/\\n/g, '<br/>');
            if (text.charAt(0) === '/') {
                text = this.replaceCommands(text);
            } else {
                text = this.replaceBBCode(text);
                text = this.replaceHyperLinks(text);
                text = this.replaceEmoticons(text);
            }
        } catch (e) {
            this.debugMessage('replaceText', e);
        }
        return text;
    },

    replaceCommands: function(text) {
        try {
            if (text.charAt(0) !== '/') {
                return text;
            }
            var textParts = text.split(' ');
            switch (textParts[0]) {
            case '/login':
                return this.replaceCommandLogin(textParts);
            case '/logout':
                return this.replaceCommandLogout(textParts);
            case '/channelEnter':
                return this.replaceCommandChannelEnter(textParts);
            case '/channelLeave':
                return this.replaceCommandChannelLeave(textParts);
            case '/privmsg':
                return this.replaceCommandPrivMsg(textParts);
            case '/privmsgto':
                return this.replaceCommandPrivMsgTo(textParts);
            case '/privaction':
                return this.replaceCommandPrivAction(textParts);
            case '/privactionto':
                return this.replaceCommandPrivActionTo(textParts);
            case '/me':
            case '/action':
                return this.replaceCommandAction(textParts);
            case '/invite':
                return this.replaceCommandInvite(textParts);
            case '/inviteto':
                return this.replaceCommandInviteTo(textParts);
            case '/uninvite':
                return this.replaceCommandUninvite(textParts);
            case '/uninviteto':
                return this.replaceCommandUninviteTo(textParts);
            case '/queryOpen':
                return this.replaceCommandQueryOpen(textParts);
            case '/queryClose':
                return this.replaceCommandQueryClose(textParts);
            case '/ignoreAdded':
                return this.replaceCommandIgnoreAdded(textParts);
            case '/ignoreRemoved':
                return this.replaceCommandIgnoreRemoved(textParts);
            case '/ignoreList':
                return this.replaceCommandIgnoreList(textParts);
            case '/ignoreListEmpty':
                return this.replaceCommandIgnoreListEmpty(textParts);
            case '/kick':
                return this.replaceCommandKick(textParts);
            case '/who':
                return this.replaceCommandWho(textParts);
            case '/whoChannel':
                return this.replaceCommandWhoChannel(textParts);
            case '/whoEmpty':
                return this.replaceCommandWhoEmpty(textParts);
            case '/list':
                return this.replaceCommandList(textParts);
            case '/bans':
                return this.replaceCommandBans(textParts);
            case '/bansEmpty':
                return this.replaceCommandBansEmpty(textParts);
            case '/unban':
                return this.replaceCommandUnban(textParts);
            case '/whois':
                return this.replaceCommandWhois(textParts);
            case '/whereis':
                return this.replaceCommandWhereis(textParts);
            case '/roll':
                return this.replaceCommandRoll(textParts);
            case '/nick':
                return this.replaceCommandNick(textParts);
            case '/error':
                return this.replaceCommandError(textParts);
            }
        } catch (e) {
            this.debugMessage('replaceCommands', e);
        }
        return text;
    },

    replaceCommandLogin: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['login'].replace(/%s/, "<a href=\"javascript:ajaxChat.toUser('" + textParts[1] + "');\">" + textParts[1] + "</a>")
            + '</span>';
    },

    replaceCommandLogout: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['logout' + (textParts.length === 3 ? textParts[2] : '')].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandChannelEnter: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['channelEnter'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandChannelLeave: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['channelLeave'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandPrivMsg: function(textParts) {
        var privMsgText = textParts.slice(1).join(' ');
        privMsgText = this.replaceBBCode(privMsgText);
        privMsgText = this.replaceHyperLinks(privMsgText);
        privMsgText = this.replaceEmoticons(privMsgText);
        return '<span class="privmsg">'
            + this.lang['privmsg']
            + '</span> '
            + privMsgText;
    },

    replaceCommandPrivMsgTo: function(textParts) {
        var privMsgText = textParts.slice(2).join(' ');
        privMsgText = this.replaceBBCode(privMsgText);
        privMsgText = this.replaceHyperLinks(privMsgText);
        privMsgText = this.replaceEmoticons(privMsgText);
        return '<span class="privmsg">'
            + this.lang['privmsgto'].replace(/%s/, textParts[1])
            + '</span> '
            + privMsgText;
    },

    replaceCommandPrivAction: function(textParts) {
        var privActionText = textParts.slice(1).join(' ');
        privActionText = this.replaceBBCode(privActionText);
        privActionText = this.replaceHyperLinks(privActionText);
        privActionText = this.replaceEmoticons(privActionText);
        return '<span class="action">'
            + privActionText
            + '</span> <span class="privmsg">'
            + this.lang['privmsg']
            + '</span> ';
    },

    replaceCommandPrivActionTo: function(textParts) {
        var privActionText = textParts.slice(2).join(' ');
        privActionText = this.replaceBBCode(privActionText);
        privActionText = this.replaceHyperLinks(privActionText);
        privActionText = this.replaceEmoticons(privActionText);
        return '<span class="action">'
            + privActionText
            + '</span> <span class="privmsg">'
            + this.lang['privmsgto'].replace(/%s/, textParts[1])
            + '</span> ';
    },

    replaceCommandAction: function(textParts) {
        var actionText = textParts.slice(1).join(' ');
        actionText = this.replaceBBCode(actionText);
        actionText = this.replaceHyperLinks(actionText);
        actionText = this.replaceEmoticons(actionText);
        return '<span class="action">'
            + actionText
            + '</span>';
    },

    replaceCommandInvite: function(textParts) {
        var inviteText = this.lang['invite']
            .replace(/%s/, textParts[1])
            .replace(
                /%s/,
                '<a href="javascript:ajaxChat.sendMessageWrapper(\'/join '
                + this.scriptLinkEncode(textParts[2])
                + '\');" title="'
                + this.lang['joinChannel'].replace(/%s/, textParts[2])
                + '">'
                + textParts[2]
                + '</a>'
            );
        return '<span class="chatBotMessage">'
            + inviteText
            + '</span>';
    },

    replaceCommandInviteTo: function(textParts) {
        var inviteText = this.lang['inviteto']
            .replace(/%s/, textParts[1])
            .replace(/%s/, textParts[2]);
        return '<span class="chatBotMessage">'
            + inviteText
            + '</span>';
    },

    replaceCommandUninvite: function(textParts) {
        var uninviteText = this.lang['uninvite']
            .replace(/%s/, textParts[1])
            .replace(/%s/, textParts[2]);
        return '<span class="chatBotMessage">'
            + uninviteText
            + '</span>';
    },

    replaceCommandUninviteTo: function(textParts) {
        var uninviteText = this.lang['uninviteto']
            .replace(/%s/, textParts[1])
            .replace(/%s/, textParts[2]);
        return '<span class="chatBotMessage">'
            + uninviteText
            + '</span>';
    },

    replaceCommandQueryOpen: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['queryOpen'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandQueryClose: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['queryClose'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandIgnoreAdded: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['ignoreAdded'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandIgnoreRemoved: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['ignoreRemoved'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandIgnoreList: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['ignoreList'] + ' '
            + this.getInlineUserMenu(textParts.slice(1))
            + '</span>';
    },

    replaceCommandIgnoreListEmpty: function() {
        return '<span class="chatBotMessage">'
            + this.lang['ignoreListEmpty']
            + '</span>';
    },

    replaceCommandKick: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['logoutKicked'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandWho: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['who'] + ' '
            + this.getInlineUserMenu(textParts.slice(1))
            + '</span>';
    },

    replaceCommandWhoChannel: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['whoChannel'].replace(/%s/, textParts[1]) + ' '
            + this.getInlineUserMenu(textParts.slice(2))
            + '</span>';
    },

    replaceCommandWhoEmpty: function() {
        return '<span class="chatBotMessage">'
            + this.lang['whoEmpty']
            + '</span>';
    },

    replaceCommandList: function(textParts) {
        var channels = textParts.slice(1);
        var listChannels = [];
        var channelName;
        for (var i = 0; i < channels.length; i++) {
            channelName = (channels[i] === this.channelName) ? '<b>' + channels[i] + '</b>' : channels[i];
            listChannels.push(
                '<a href="javascript:ajaxChat.sendMessageWrapper(\'/join '
                + this.scriptLinkEncode(channels[i])
                + '\');" title="'
                + this.lang['joinChannel'].replace(/%s/, channels[i])
                + '">'
                + channelName
                + '</a>'
            );
        }
        return '<span class="chatBotMessage">'
            + this.lang['list'] + ' '
            + listChannels.join(', ')
            + '</span>';
    },

    replaceCommandBans: function(textParts) {
        var users = textParts.slice(1);
        var listUsers = [];
        for (var i = 0; i < users.length; i++) {
            listUsers.push(
                '<a href="javascript:ajaxChat.sendMessageWrapper(\'/unban '
                + this.scriptLinkEncode(users[i])
                + '\');" title="'
                + this.lang['unbanUser'].replace(/%s/, users[i])
                + '">'
                + users[i]
                + '</a>'
            );
        }
        return '<span class="chatBotMessage">'
            + this.lang['bans'] + ' '
            + listUsers.join(', ')
            + '</span>';
    },

    replaceCommandBansEmpty: function() {
        return '<span class="chatBotMessage">'
            + this.lang['bansEmpty']
            + '</span>';
    },

    replaceCommandUnban: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['unban'].replace(/%s/, textParts[1])
            + '</span>';
    },

    replaceCommandWhois: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['whois'].replace(/%s/, textParts[1]) + ' '
            + textParts[2]
            + '</span>';
    },

    replaceCommandWhereis: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['whereis'].replace(/%s/, textParts[1]).replace(
                /%s/,
                '<a href="javascript:ajaxChat.sendMessageWrapper(\'/join '
                + this.scriptLinkEncode(textParts[2])
                + '\');" title="'
                + this.lang['joinChannel'].replace(/%s/, textParts[2])
                + '">'
                + textParts[2]
                + '</a>'
            )
            + '</span>';
    },

    replaceCommandRoll: function(textParts) {
        var rollText = this.lang['roll'].replace(/%s/, textParts[1]);
        rollText = rollText.replace(/%s/, textParts[2]);
        rollText = rollText.replace(/%s/, textParts[3]);
        return '<span class="chatBotMessage">'
            + rollText
            + '</span>';
    },

    replaceCommandNick: function(textParts) {
        return '<span class="chatBotMessage">'
            + this.lang['nick'].replace(/%s/, textParts[1]).replace(/%s/, textParts[2])
            + '</span>';
    },

    replaceCommandError: function(textParts) {
        var errorMessage = this.lang['error' + textParts[1]];
        if (!errorMessage) {
            errorMessage = 'Error: Unknown.';
        } else if (textParts.length > 2) {
            errorMessage = errorMessage.replace(/%s/, textParts.slice(2).join(' '));
        }
        return '<span class="chatBotErrorMessage">'
            + errorMessage
            + '</span>';
    },

    getInlineUserMenu: function(users) {
        var menu = '';
        for (var i = 0; i < users.length; i++) {
            if (i > 0) {
                menu += ', ';
            }
            menu += '<a href="javascript:ajaxChat.toggleUserMenu(\''
                + this.getInlineUserMenuDocumentID(this.userMenuCounter, i)
                + '\', \''
                + this.scriptLinkEncode(users[i])
                + '\', null);" title="'
                + this.lang['toggleUserMenu'].replace(/%s/, users[i])
                + '" dir="'
                + this.baseDirection
                + '">'
                + ((users[i] === this.userName) ? '<b>' + users[i] + '</b>' : users[i])
                + '</a>'
                + '<ul class="inlineUserMenu" id="'
                + this.getInlineUserMenuDocumentID(this.userMenuCounter, i)
                + '" style="display:none;">'
                + '</ul>';
        }
        this.userMenuCounter++;
        return menu;
    },

    containsUnclosedTags: function(str) {
        var openTags = str.match(/<[^>\/]+?>/gm),
            closeTags = str.match(/<\/[^>]+?>/gm);
        // Return true if the number of tags doesn't match:
        return (!openTags && closeTags) ||
        (openTags && !closeTags) ||
        (openTags && closeTags && (openTags.length !== closeTags.length));
    },

    replaceBBCode: function(text) {
        if (!this.settings['bbCode']) {
            // If BBCode is disabled, just strip the text from BBCode tags:
            return text.replace(/(\[\/?\w+(?:=[^<>]*?)?\])/g, '');
        }
        // Remove the BBCode tags:
        return text.replace(
            /\[(\w+)(?:=([^<>]*?))?\](.+?)\[\/\1\]/gm,
            function(str, tag, attribute, content) {
                // Only replace predefined BBCode tags:
                if (!ajaxChat.inArray(ajaxChat.bbCodeTags, tag)) {
                    return str;
                }
                // Avoid invalid XHTML (unclosed tags):
                if (ajaxChat.containsUnclosedTags(content)) {
                    return str;
                }
                switch (tag) {
                    case 'color':
                        return ajaxChat.replaceBBCodeColor(content, attribute);
                    case 'quote':
                        return ajaxChat.replaceBBCodeQuote(content, attribute);
                    case 's':
                        return '<span style="text-decoration:line-through;">' + ajaxChat.replaceBBCode(content) + '</span>';
                    case 'tgw':
                        return '<span class=\"tgw\">' + ajaxChat.replaceBBCode(content) + '</span>';
                    default:
                        return '<' + tag + '>' + ajaxChat.replaceBBCode(content) + '</' + tag + '>';
                }
            }
        );
    },

    replaceBBCodeColor: function(content, attribute) {
        return (this.settings['bbCodeColors'] && attribute && this.inArray(ajaxChat.colorCodes, attribute) ? '<span style="color:'
            + attribute + ';">'
            + this.replaceBBCode(content)
            + '</span>' : content);
    },

    replaceBBCodeQuote: function(content, attribute) {
        return (attribute ? '<span class="quote"><cite>'
            + this.lang['cite'].replace(/%s/, attribute)
            + '</cite><q>'
            + this.replaceBBCode(content)
            + '</q></span>'
            : '<span class="quote"><q>'
            + this.replaceBBCode(content)
            + '</q></span>');
    },
	replaceHyperLinks: function(text) {
	    if(!this.settings['hyperLinks']) {
			return text;
	    }
	    return text.replace(/(^|\s|>)(((?:https?|ftp):\/\/)([\w_\.-]{2,256}\.[\w\.]{2,4})(:\d{1,5})?(\/([\w+,\/%$^&\*=;\-_а-яА-Я:\)\(\.]*)?((?:\?|#)[:\w%\/\)\(\-+,а-яА-Я;.?=&#]*)?)?)/gim,
	        function(str, s, a, prot, host, port, sa, sa1, sa2) {
	            var hostArr = host.split('.'),
	                fe = (sa1 ? sa1.split(/\.|\//g).pop().toLowerCase() : ''),
                    c = (ajaxChat.dom['chatList'].offsetWidth - 50);
	            switch (fe) {
	            case "mp3":
	                return s + (ajaxChat.audioHtml5['mp3'] ? "<audio style=\"height:30px; width:"+Math.min(c, 400)+"px;\" src=\"" + a + "\" controls></audio> " : '<a href="' + a + '" onclick="window.open(this.href); return false;">Скачать mp3 </a> ');
	            case "jpg":
	            case "jpeg":
	            case "png":
	            case "bmp":
	            case "gif":
	                return s + '<a href="'
	                    + a
	                    + '" onclick="window.open(this.href); return false;">'
	                    + '<img class="bbCodeImage" style="max-width:'
	                    + c
	                    + 'px; max-height:200px;" src="'
	                    + a
	                    + '"/>'
	                    + '</a>';
	            default:
	            {
                        var width = Math.min(c, 400),
                        height = width / 1.7;
	                if (sa2 && ajaxChat.inArray(hostArr, 'youtube'))
	                    return s + '<iframe width="'+width+'" height="'+height+'" src="https://www.youtube.com/embed/' + sa2.split(/[=\?&]+/)[2] + '" frameborder="0" allowfullscreen="ture"></iframe>';
	                else if (sa1){
                            if (ajaxChat.inArray(hostArr, 'youtu'))
                                return s + '<iframe width="'+width+'" height="'+height+'" src="https://www.youtube.com/embed/' + sa1 + '" frameborder="0" allowfullscreen="ture"></iframe>';
                            else if (ajaxChat.inArray(hostArr, 'coub'))
                                return s + '<iframe width="'+width+'" height="'+height+'" src="http://coub.com/embed/'+fe+'?muted=false&autostart=false&originalSize=true&hideTopBar=false&startWithHD=false" allowfullscreen="true" frameborder="0"></iframe>';
                        }
	                return s + '<a href="' + a + '" onclick="window.open(this.href); return false;">' + helper.truncate(a, 35) + '</a>';
	            }
	            }
	    });
	},

	replaceEmoticons: function(text) {
            if(!this.settings['emoticons']) {
                    return text;
            }
            if(!arguments.callee.regExp) {
                var regExpStr = '^(.*)(';
                for(var i=0; i<this.emoticonCodes.length; i++) {
                    if(i!==0)
                        regExpStr += '|';
                    regExpStr += '(?:' + this.escapeRegExp(this.emoticonCodes[i]) + ')';
                }
                regExpStr += ')(.*)$';
                arguments.callee.regExp = new RegExp(regExpStr, 'gm');
            }
            return text.replace(
                arguments.callee.regExp,
                function(str, p1, p2, p3) {
                    if (!arguments.callee.regExp) {
                        arguments.callee.regExp = new RegExp('(="[^"]*$)|(&[^;]*$)', '');
                    }
                    // Avoid replacing emoticons in tag attributes or XHTML entities:
                    if(p1.match(arguments.callee.regExp)) {
                        return str;
                    }
                    if(p2) {
                        var index = ajaxChat.arraySearch(p2, ajaxChat.emoticonCodes);
                        return 	ajaxChat.replaceEmoticons(p1)
                            +	'<img src="'
                            +	ajaxChat.dirs['emoticons']
                            +	ajaxChat.emoticonFiles[index]
                            +	'" alt="'
                            +	p2
                            +	'" />'
                            + 	ajaxChat.replaceEmoticons(p3);
                    }
                    return str;
                }
            );
	},


	getActiveStyle: function() {
		var cookie = this.readCookie(this.sessionName + '_style');
		var style = cookie ? cookie : this.getPreferredStyleSheet();
		return style;
	},

	initStyle: function() {
		this.styleInitiated = true;
		this.setActiveStyleSheet(this.getActiveStyle());
	},

	persistStyle: function() {
		if(this.styleInitiated) {
			this.createCookie(this.sessionName + '_style', this.getActiveStyleSheet(), this.cookieExpiration);
		}
	},

	setSelectedStyle: function() {
		if(this.dom['styleSelection']) {
			var style = this.getActiveStyle();
			var styleOptions = this.dom['styleSelection'].getElementsByTagName('option');
			for(var i=0; i<styleOptions.length; i++) {
				if(styleOptions[i].value === style) {
					styleOptions[i].selected = true;
					break;
				}
			}
		}
	},

	getSelectedStyle: function() {
		var styleOptions = this.dom['styleSelection'].getElementsByTagName('option');
		if(this.dom['styleSelection'].selectedIndex === -1) {
			return styleOptions[0].value;
		} else {
			return styleOptions[this.dom['styleSelection'].selectedIndex].value;
		}
	},

	setActiveStyleSheet: function(title) {
		var i, a, titleFound = false;
		for(i=0; (a = document.getElementsByTagName('link')[i]); i++) {
			if(a.getAttribute('rel').indexOf('style') !== -1 && a.getAttribute('title')) {
				a.disabled = true;
				if(a.getAttribute('title') === title) {
	                a.disabled = false;
	                titleFound = true;
				}
			}
		}
		if(!titleFound && title !== null) {
		   this.setActiveStyleSheet(this.getPreferredStyleSheet());
		}
	},

	getActiveStyleSheet: function() {
		var i, a;
		for(i=0; (a = document.getElementsByTagName('link')[i]); i++) {
			if(a.getAttribute('rel').indexOf('style') !== -1 && a.getAttribute('title') && !a.disabled) {
				return a.getAttribute('title');
			}
		}
		return null;
	},

	getPreferredStyleSheet: function() {
		var i,a;
		for(i=0; (a = document.getElementsByTagName('link')[i]); i++) {
			if(a.getAttribute('rel').indexOf('style') !== -1
				&& a.getAttribute('rel').indexOf('alt') === -1
				&& a.getAttribute('title')
				) {
				return a.getAttribute('title');
			}
		}
		return null;
	},

	switchLanguage: function(langCode) {
		window.location.search = '?lang='+langCode;
	},

	createCookie: function(name,value,days) {
		var expires = '';
		if(days) {
			var date = new Date();
			date.setTime(date.getTime()+(days*24*60*60*1000));
			expires = '; expires='+date.toGMTString();
		}
		var path = '; path='+this.cookiePath;
		var domain = this.cookieDomain ? '; domain='+this.cookieDomain : '';
		var secure = this.cookieSecure ? '; secure' : '';
		document.cookie = name+'='+this.encodeText(value)+expires+path+domain+secure;
	},

	readCookie: function(name) {
		if(!document.cookie)
		   return null;
		var nameEQ = name + '=';
		var ca = document.cookie.split(';');
		for(var i=0; i<ca.length; i++) {
			var c = ca[i];
			while(c.charAt(0) === ' ') {
				c = c.substring(1, c.length);
			}
			if(c.indexOf(nameEQ) === 0) {
				return this.decodeText(c.substring(nameEQ.length, c.length));
			}
		}
		return null;
	},

	isCookieEnabled: function() {
		this.createCookie(this.sessionName + '_cookie_test', true, 1);
		var cookie = this.readCookie(this.sessionName + '_cookie_test');
		if(cookie) {
			// Unset the test cookie:
			this.createCookie(this.sessionName + '_cookie_test', true, -1);
			// Cookie test successfull, return true:
			return true;
		}
		return false;
	},

	finalize: function() {
		if(typeof this.finalizeFunction === 'function') {
			this.finalizeFunction();
		}
		this.persistSettings();
		this.persistStyle();
	},

	debugMessage: function(msg, e) {
		if (this.debug) {
			msg = 'Ajax chat: ' + msg + ' exception: ';
			console.log(msg, e);
			if (this.debug === 2) {
				alert(msg + e);
			}
		}
	}

};

var helper = {
    isMobile: function() {
        var a = navigator.userAgent || navigator.vendor || window.opera;
        return /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4));
    },
    truncate: function(str, maxlength) {
        return (str.length > maxlength ? str.slice(0, maxlength - 3) + '...' : str);
    },
    getTIM: function(t){
        switch(t)
        {
        case "1ile":
                return "Дон Кихот";
        case "1sei":
                return "Дюма";
        case "1lii":
                return "Робеспьер";
        case "1ese":
                return "Гюго";

        case "2eie":
                return "Гамлет";
        case "2lsi":
                return "Максим Горький";
        case "2sle":
                return "Жуков";
        case "2iei":
                return "Есенин";

        case "3ili":
                return "Бальзак";
        case "3see":
                return "Наполеон";
        case "3esi":
                return "Драйзер";
        case "3lie":
                return "Джек Лондон";

        case "4lse":
                return "Штирлиц";
        case "4eii":
                return "Достоевский";
        case "4iee":
                return "Гексли";
        case "4sli":
                return "Габен";
        default:
                return "none";
        }
    }
};