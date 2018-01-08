sChat.logsMonitorMode = null;
sChat.logsLastID = null;
sChat.logsCommand = null;
//sChat.userRole=3;

sChat.startChatUpdate = function () {
    var infos = 'userID,userName,userRole';
    this.paramString='&getInfos='+this.encodeText(infos);
    this.updateChat();
};

sChat.updateChat = function (pS) {
    // Only update if we have parameters, are in monitor mode or the lastID has changed since the last update:
    if (this.logsMonitorMode || !this.logsLastID || this.lastID !== this.logsLastID) {
        // Update the logsLastID for the lastID check:
        this.logsLastID = this.lastID;

        var requestUrl = sConfig.ajaxURL+ '&lastID='+ this.lastID;
        if (this.paramString) {
            requestUrl += this.paramString;
            this.paramString = '';
        }
        if (pS) requestUrl += pS;
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
        if (!document.getElementById('inputField').value &&
            parseInt(document.getElementById('yearSelection').value) <= 0 &&
            parseInt(document.getElementById('hourSelection').value) <= 0) {
            this.logsMonitorMode = true;
        } else {
            this.logsMonitorMode = false;
        }
        this.logsCommand = 'command=getLogs'
            + '&channelID=' + document.getElementById('channelSelection').value
            + '&year=' + document.getElementById('yearSelection').value
            + '&month=' + document.getElementById('monthSelection').value
            + '&day=' + document.getElementById('daySelection').value
            + '&hour=' + document.getElementById('hourSelection').value
            + '&tmc=' + document.getElementById('tmcCount').value
            + '&search=' + this.encodeText(document.getElementById('inputField').value);
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
    newDiv.style.minHeight = "20px";
    newDiv.className = rowClass;
    newDiv.id = this.getMessageDocumentID(messageID);
    newDiv.innerHTML = '<a class="delete ignoreOnMessageClick" href="javascript:sChat.deleteMessage(' + messageID + ');"> </a>'
        + '<a class="dateTime" href="#" onclick="document.getElementById('\'yearSelection\'').value =' + dateObject.getFullYear() + ';document.getElementById('\'monthSelection\'').value =' + (dateObject.getMonth()+1) + ';document.getElementById('\'daySelection\'').value =' + dateObject.getDate() + ';document.getElementById('\'hourSelection\'').value =' + dateObject.getHours() + '">' + this.formatDate(dateObject) + ' </a><span class="' + userClass + '"'
        + (sConfig.settings['nickColors'] && msgInfo && msgInfo.ncol ? ' style="color:' + msgInfo.ncol + '" ' : '') + ">"
        + ((sConfig.settings['nickColors'] && sConfig.settings['gradiens'] && msgInfo && msgInfo.nickGrad) ? helper.grad(userName, msgInfo.nickGrad) : userName)
        + '</span>' + colon + '<span ' + ((sConfig.settings['msgColors'] && msgInfo && msgInfo.mcol) ? 'style="color:' + msgInfo.mcol + '">' : '>')
        + ((sConfig.settings['msgColors'] && sConfig.settings['gradiens'] && msgInfo && msgInfo.msgGrad) ? helper.grad(text, msgInfo.msgGrad) : text) + '</span>';
    return newDiv;
};