<?php
/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

// Class to handle HTML templates
class SChatTemplate {

	var $sChat;
	var $_regExpTemplateTags;
	var $_templateFile;
	var $_contentType;
	var $_content;
	var $_parsedContent;

	// Constructor:
	function __construct(&$sChat, $templateFile, $contentType=null) {
		$this->sChat = $sChat;
		$this->_regExpTemplateTags = '/\[(\w+?)(?:(?:\/)|(?:\](.+?)\[\/\1))\]/s';		
		$this->_templateFile = $templateFile;
		$this->_contentType = $contentType;
	}

	function getParsedContent() {
		if(!$this->_parsedContent) {
			$this->parseContent();
		}
		return $this->_parsedContent;
	}

	function getContent() {
		if(!$this->_content) {
			$this->_content = SChatFileSystem::getFileContents($this->_templateFile);
		}
		return $this->_content;
	}

	function parseContent() {
		$this->_parsedContent = $this->getContent();
		
		// Remove the XML declaration if the content-type is not xml:		
		if($this->_contentType && (strpos($this->_contentType,'xml') === false)) {
			$doctypeStart = strpos($this->_parsedContent, '<!DOCTYPE ');
			if($doctypeStart !== false) {
				// Removing the XML declaration (in front of the document type) prevents IE<7 to go into "Quirks mode":
				$this->_parsedContent = substr($this->_parsedContent, $doctypeStart);	
			}		
		}

		// Replace template tags ([TAG/] and [TAG]content[/TAG]) and return parsed template content:
		$this->_parsedContent = preg_replace_callback($this->_regExpTemplateTags, array($this, 'replaceTemplateTags'), $this->_parsedContent);
	}

	function replaceTemplateTags($tagData) {
		switch($tagData[1]) {
                        case 'VN':
                            return VER;
			case 'SCHAT_URL':
				return $this->sChat->htmlEncode($this->sChat->getChatURL());

			case 'LANG':
				return $this->sChat->htmlEncode($this->sChat->getLang((isset($tagData[2]) ? $tagData[2] : null)));				
			case 'LANG_CODE':
				return $this->sChat->getLangCode();

			case 'BASE_DIRECTION':
				return $this->getBaseDirectionAttribute();

			case 'CONTENT_ENCODING':
				return Config::contentEncoding;
					
			case 'CONTENT_TYPE':
				return $this->_contentType;
		
			case 'LOGIN_URL':
				return ($this->sChat->getRequestVar('view') == 'logs') ? './?view=logs' : './';
				
			case 'USER_NAME_MAX_LENGTH':
				return Config::userNameMaxLength;
			case 'MESSAGE_TEXT_MAX_LENGTH':
				return Config::messageTextMaxLength;

			case 'LOGIN_CHANNEL_ID':
				return $this->sChat->getValidRequestChannelID();
				
			case 'SESSION_NAME':
				return Config::sessionName;
				
			case 'COOKIE_EXPIRATION':
				return Config::sessionCookieLifeTime;
			case 'COOKIE_PATH':
				return Config::sessionCookiePath;
			case 'COOKIE_DOMAIN':
				return Config::sessionCookieDomain;
			case 'COOKIE_SECURE':
				return Config::sessionCookieSecure;
				
			case 'CHAT_BOT_NAME':
				return rawurlencode(Config::chatBotName);
			case 'CHAT_BOT_ID':
				return Config::chatBotID;

			case 'INACTIVE_TIMEOUT':
				return Config::inactiveTimeout;

			case 'PRIVATE_CHANNEL_DIFF':
				return Config::privateChannelDiff;
			case 'PRIVATE_MESSAGE_DIFF':
				return Config::privateMessageDiff;

			case 'SOCKET_SERVER_ENABLED':
				if(Config::socketServerEnabled)
					return 1;
				else
					return 0;

			case 'SOCKET_SERVER_HOST':
				if(Config::socketServerHost) {
					$socketServerHost = Config::socketServerHost;
				} else {
					$socketServerHost = (isset($_SERVER['HTTP_HOST']) ? $_SERVER['HTTP_HOST'] : $_SERVER['SERVER_NAME']);
				}
				return rawurlencode($socketServerHost);

			case 'SOCKET_SERVER_PORT':
				return Config::socketServerPort;

			case 'SOCKET_SERVER_CHAT_ID':
				return Config::socketServerChatID;

			case 'STYLE_SHEETS':
				return $this->getStyleSheetLinkTags();
				
			case 'CHANNEL_OPTIONS':
				return $this->getChannelOptionTags();
			case 'STYLE_OPTIONS':
				return $this->getStyleOptionTags();
			case 'LANGUAGE_OPTIONS':
				return $this->getLanguageOptionTags();
			
			case 'ERROR_MESSAGES':
				return $this->getErrorMessageTags();

			case 'LOGS_CHANNEL_OPTIONS':
				return $this->getLogsChannelOptionTags();
			case 'LOGS_YEAR_OPTIONS':
				return $this->getLogsYearOptionTags();
			case 'LOGS_MONTH_OPTIONS':
				return $this->getLogsMonthOptionTags();
			case 'LOGS_DAY_OPTIONS':
				return $this->getLogsDayOptionTags();
			case 'LOGS_HOUR_OPTIONS':
				return $this->getLogsHourOptionTags();
			case 'CLASS_WRITEABLE':
                            return 'write_allowed';
                        case 'HELPLIST':
                            return $this->getHelpList();
			default:
				return $this->sChat->replaceCustomTemplateTags($tagData[1], (isset($tagData[2]) ? $tagData[2] : null));
		}
	}
        function getHelpList()
        {
            $ret = '<div id="helpList">';
            $list = array('Join', 'JoinCreate', 'Invite', 'Uninvite', 'Logout', 'PrivateMessage', 'QueryOpen', 'QueryClose', 'Action', 
                'Describe', 'Ignore', 'IgnoreList', 'Whereis', 'Kick', 'Unban', 'Bans', 'Whois', 'Who', 'List', 'Roll', 'Nick');
            foreach ($list as $item)
            {
                $ret .= '<dl><dt>'.$this->sChat->htmlEncode($this->sChat->getLang('helpItemDesc'.$item)).'</dt>'
                        .'<dd>'.$this->sChat->htmlEncode($this->sChat->getLang('helpItemCode'.$item)).'</dd></dl>';
            }
            return $ret.'</div>';
        }

        // Function to display alternating table row colors:
	function alternateRow($rowOdd='rowOdd', $rowEven='rowEven') {
		static $i;
		$i += 1;
		if($i % 2 == 0) {
			return $rowEven;
		} else {
			return $rowOdd;
		}
	}

	function getBaseDirectionAttribute() {
		$langCodeParts = explode('-', $this->sChat->getLangCode());
		switch($langCodeParts[0]) {
			case 'ar':
			case 'fa':
			case 'he':
				return 'rtl';
			default:
				return 'ltr';
		}
	}

	function getStyleSheetLinkTags() {
		$styleSheets = '';
		foreach(Config::$styleAvailable as $style) {
			$alternate = ($style == Config::styleDefault) ? '' : 'alternate ';
			$styleSheets .= '<link rel="'.$alternate.'stylesheet" type="text/css" href="css/'.rawurlencode($style).'.css?'.VER.'" title="'.$this->sChat->htmlEncode($style).'"/>';
		}
		return $styleSheets;
	}

	function getChannelOptionTags() {
		$channelOptions = '';
		$channelSelected = false;
		foreach($this->sChat->getChannels() as $name=>$id) {
			if($this->sChat->isLoggedIn() && $this->sChat->getChannel()) {
				$selected = ($id == $this->sChat->getChannel()) ? ' selected="selected"' : '';
			} else {
				$selected = ($id == Config::defaultChannelID) ? ' selected="selected"' : '';
			}
			if($selected) {
				$channelSelected = true;
			}
			$channelOptions .= '<option value="'.$this->sChat->htmlEncode($name).'"'.$selected.'>'.$this->sChat->htmlEncode($name).'</option>';
		}
		if($this->sChat->isLoggedIn() && $this->sChat->isAllowedToCreatePrivateChannel()) {
			// Add the private channel of the user to the options list:
			if(!$channelSelected && $this->sChat->getPrivateChannelID() == $this->sChat->getChannel()) {
				$selected = ' selected="selected"';
				$channelSelected = true;
			} else {
				$selected = '';
			}
			$privateChannelName = $this->sChat->getPrivateChannelName();
			$channelOptions .= '<option value="'.$this->sChat->htmlEncode($privateChannelName).'"'.$selected.'>'.$this->sChat->htmlEncode($privateChannelName).'</option>';
		}
		// If current channel is not in the list, try to retrieve the channelName:
		if(!$channelSelected) {
			$channelName = $this->sChat->getChannelName();
			if($channelName !== null) {
				$channelOptions .= '<option value="'.$this->sChat->htmlEncode($channelName).'" selected="selected">'.$this->sChat->htmlEncode($channelName).'</option>';
			} else {
				// Show an empty selection:
				$channelOptions .= '<option value="" selected="selected">---</option>';
			}
		}
		return $channelOptions;
	}

	function getStyleOptionTags() {
		$styleOptions = '';
		foreach(Config::$styleAvailable as $style) {
			$selected = ($style == Config::styleDefault) ? ' selected="selected"' : '';
			$styleOptions .= '<option value="'.$this->sChat->htmlEncode($style).'"'.$selected.'>'.$this->sChat->htmlEncode($style).'</option>';
		}
		return $styleOptions;
	}

	function getLanguageOptionTags() {
		$languageOptions = '';
		$languageNames = Config::$langNames;
		foreach(Config::$langAvailable as $langCode) {
			$selected = ($langCode == $this->sChat->getLangCode()) ? ' selected="selected"' : '';
			$languageOptions .= '<option value="'.$this->sChat->htmlEncode($langCode).'"'.$selected.'>'.$languageNames[$langCode].'</option>';
		}
		return $languageOptions;
	}

	function getErrorMessageTags() {
		$errorMessages = '';
		foreach($this->sChat->getInfoMessages('error') as $error) {
			$errorMessages .= '<div>'.$this->sChat->htmlEncode($this->sChat->getLang($error)).'</div>';
		}
		return $errorMessages;
	}

	function getLogsChannelOptionTags() {
		$channelOptions = '';
		$channelOptions .= '<option value="-3">------</option>';
		foreach($this->sChat->getChannels() as $key=>$value) {
			if($this->sChat->getUserRole() != SCHAT_ADMIN && Config::logsUserAccessChannelList && !in_array($value, Config::logsUserAccessChannelList)) {
				continue;
			}
			$channelOptions .= '<option value="'.$value.'">'.$this->sChat->htmlEncode($key).'</option>';
		}
		$channelOptions .= '<option value="-1">'.$this->sChat->htmlEncode($this->sChat->getLang('logsPrivateChannels')).'</option>';
		$channelOptions .= '<option value="-2">'.$this->sChat->htmlEncode($this->sChat->getLang('logsPrivateMessages')).'</option>';
		return $channelOptions;
	}

	function getLogsYearOptionTags() {
		$yearOptions = '';
		$yearOptions .= '<option value="-1">----</option>';
		for($year=date('Y'); $year>=Config::logsFirstYear; $year--) {
			$yearOptions .= '<option value="'.$year.'">'.$year.'</option>';
		}
		return $yearOptions;
	}
	
	function getLogsMonthOptionTags() {
		$monthOptions = '';
		$monthOptions .= '<option value="-1">--</option>';
		for($month=1; $month<=12; $month++) {
			$monthOptions .= '<option value="'.$month.'">'.sprintf("%02d", $month).'</option>';
		}
		return $monthOptions;
	}
	
	function getLogsDayOptionTags() {
		$dayOptions = '';
		$dayOptions .= '<option value="-1">--</option>';
		for($day=1; $day<=31; $day++) {
			$dayOptions .= '<option value="'.$day.'">'.sprintf("%02d", $day).'</option>';
		}
		return $dayOptions;
	}
	
	function getLogsHourOptionTags() {
		$hourOptions = '';
		$hourOptions .= '<option value="-1">-----</option>';
		for($hour=0; $hour<=23; $hour++) {
			$hourOptions .= '<option value="'.$hour.'">'.sprintf("%02d", $hour).':00</option>';
		}
		return $hourOptions;
	}

}
?>
