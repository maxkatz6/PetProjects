	sChat.logsMonitorMode = null;
	sChat.logsLastID = null;
	sChat.logsCommand = null;

	sChat.startChatUpdate = function() {
		var infos = 'userID,userName,userRole';
		this.updateChat('&getInfos=' + this.encodeText(infos));
	};

	sChat.updateChat = function(paramString) {
		// Only update if we have parameters, are in monitor mode or the lastID has changed since the last update:
		if(paramString || this.logsMonitorMode || !this.logsLastID || this.lastID !== this.logsLastID) {
			// Update the logsLastID for the lastID check:
			this.logsLastID = this.lastID;

			var requestUrl = sConfig.ajaxURL
							+ '&lastID='
							+ this.lastID;
			if(paramString) {
				requestUrl += paramString;
			}
			requestUrl += '&' + this.getLogsCommand();
			this.makeRequest(requestUrl,'GET',null);
		} else {
			this.logsLastID = null;
		}
	};

	sChat.sendMessage = function() {
		this.getLogs();	
	};
	
	sChat.getLogs = function() {
		clearTimeout(this.timer);
		this.clearChatList();
		this.lastID = 0;
		this.logsCommand = null;
		this.makeRequest(this.ajaxURL,'POST',this.getLogsCommand());
	};
	
	sChat.getLogsCommand = function() {
		if(!this.logsCommand) {
			if(!this.dom['inputField'].value &&
				parseInt(this.dom['yearSelection'].value) <= 0 &&
				parseInt(this.dom['hourSelection'].value) <= 0) {
				this.logsMonitorMode = true;
			} else {
				this.logsMonitorMode = false;
			}
			this.logsCommand = 'command=getLogs'
								+ '&channelID='	+ this.dom['channelSelection'].value
								+ '&year='		+ this.dom['yearSelection'].value
								+ '&month='		+ this.dom['monthSelection'].value
								+ '&day='		+ this.dom['daySelection'].value
								+ '&hour='		+ this.dom['hourSelection'].value
								+ '&search='	+ this.encodeText(this.dom['inputField'].value);
		}
		return this.logsCommand;
	};

	sChat.onNewMessage = function(dateObject, userID, userName, userRoleClass, messageID, messageText, channelID, ip) {
		if(messageText.indexOf('/delete') === 0) {
			return false;
		}
		if(this.logsMonitorMode) {
			this.blinkOnNewMessage(dateObject, userID, userName, userRoleClass, messageID, messageText, channelID, ip);
			this.playSoundOnNewMessage(
				dateObject, userID, userName, userRoleClass, messageID, messageText, channelID, ip
			);
		}
		return true;
	};
	
	sChat.logout = function() {
		clearTimeout(this.timer);
		this.makeRequest(this.ajaxURL,'POST','logout=true');
	},

	sChat.switchLanguage = function(langCode) {
		window.location.search = '?view=logs&lang='+langCode;
	};

	sChat.setChatUpdateTimer = function() {
		clearTimeout(this.timer);
		this.timer = setTimeout('sChat.updateChat(null);', 3000);
	};
	