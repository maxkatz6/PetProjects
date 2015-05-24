sChat.logsMonitorMode = null;
sChat.logsLastID = null;
sChat.logsCommand = null;

sChat.startChatUpdate = function () {
    var infos = 'userID,userName,userRole';
    this.updateChat('&getInfos=' + this.encodeText(infos));
};

sChat.updateChat = function (paramString) {
    // Only update if we have parameters, are in monitor mode or the lastID has changed since the last update:
    if (paramString || this.logsMonitorMode || !this.logsLastID || this.lastID !== this.logsLastID) {
        // Update the logsLastID for the lastID check:
        this.logsLastID = this.lastID;

        var requestUrl = sConfig.ajaxURL
                        + '&lastID='
                        + this.lastID;
        if (paramString) {
            requestUrl += paramString;
        }
        requestUrl += '&' + this.getLogsCommand();
        this.makeRequest(requestUrl, 'GET', null);
    } else {
        this.logsLastID = null;
    }
};

sChat.getLogs = function () {
    clearTimeout(this.timer);
    this.clearChatList();
    this.lastID = 0;
    this.logsCommand = null;
    this.makeRequest(sConfig.ajaxURL, 'POST', this.getLogsCommand());
};
sChat.getLogsCommand = function () {
    if (!this.logsCommand) {
        if (!this.dom['inputField'].value &&
            parseInt(this.dom['yearSelection'].value) <= 0 &&
            parseInt(this.dom['hourSelection'].value) <= 0) {
            this.logsMonitorMode = true;
        } else {
            this.logsMonitorMode = false;
        }
        this.logsCommand = 'command=getLogs'
            + '&channelID=' + this.dom['channelSelection'].value
            + '&year=' + this.dom['yearSelection'].value
            + '&month=' + this.dom['monthSelection'].value
            + '&day=' + this.dom['daySelection'].value
            + '&hour=' + this.dom['hourSelection'].value
            + '&tmc=' + this.dom['tmcCount'].value
            + '&search=' + this.encodeText(this.dom['inputField'].value);
    }
    return this.logsCommand;
};

sChat.onNewMessage = function (dateObject, userID, userName, userRoleClass, messageID, messageText, channelID, ip) {
    if (messageText.indexOf('/delete') === 0) {
        return false;
    }
    if (this.logsMonitorMode) {
        this.blinkOnNewMessage(dateObject, userID, userName, userRoleClass, messageID, messageText, channelID, ip);
        this.playSoundOnNewMessage(
            dateObject, userID, userName, userRoleClass, messageID, messageText, channelID, ip
        );
    }
    return true;
};
sChat.formatDate = function (date) {
    return '(%d.%m.%Y %H:%i:%s)'
        .replace(/%Y/g, date.getFullYear())
        .replace(/%m/g, this.addLeadingZero(date.getMonth() + 1))
        .replace(/%d/g, this.addLeadingZero(date.getDate()))
        .replace(/%H/g, this.addLeadingZero(date.getHours()))
        .replace(/%i/g, this.addLeadingZero(date.getMinutes()))
        .replace(/%s/g, this.addLeadingZero(date.getSeconds()));
},

sChat.switchLanguage = function (langCode) {
    window.location.search = '?view=logs&lang=' + langCode;
};

sChat.setChatUpdateTimer = function () {
    clearTimeout(this.timer);
    this.timer = setTimeout('sChat.updateChat();', 3000);
};

sChat.getChatListChild = function (dateObject, userID, userName, userRole, messageID, messageText, channelID, ip, msgInfo) {
    var rowClass = this.DOMbufferRowClass,
        userClass = this.getRoleClass(userRole),
        colon = ': ',
        priv = messageText.indexOf('/privmsg') === 0 || messageText.indexOf('/privmsgto') === 0 || messageText.indexOf('/privaction') === 0;
    if (messageText.indexOf('/action') === 0 || messageText.indexOf('/me') === 0 || messageText.indexOf('/privaction') === 0) {
        userClass += ' action';
        colon = ' ';
    }
    if (priv) rowClass += ' private';
    if (new RegExp('(?:^|, |])' + this.userName + ',', 'gm').test(messageText)) rowClass += ' toMe';
    var text = this.replaceText(messageText);
    var newDiv = document.createElement('div');
    newDiv.className = rowClass;
    newDiv.id = this.getMessageDocumentID(messageID);
    newDiv.innerHTML = this.getDeletionLink(messageID, userID, userRole, channelID)
        + '<span>' + this.formatDate(dateObject) + ' </span><span class="' + userClass + '"'
        + (sConfig.settings['nickColors'] && msgInfo && msgInfo.ncol ? ' style="color:' + msgInfo.ncol + '" ' : '') + ">"
        + ((sConfig.settings['nickColors'] && sConfig.settings['gradiens'] && msgInfo && msgInfo.nickGrad) ? helper.grad(userName, msgInfo.nickGrad) : userName)
        + '</span>' + colon + '<span ' + ((sConfig.settings['msgColors'] && msgInfo && msgInfo.mcol) ? 'style="color:' + msgInfo.mcol + '">' : '>')
        + ((sConfig.settings['msgColors'] && sConfig.settings['gradiens'] && msgInfo && msgInfo.msgGrad) ? helper.grad(text, msgInfo.msgGrad) : text) + '</span>';
    return newDiv;
};