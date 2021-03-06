<?php
class SChat {
    var $db;
    var $_requestVars;
    var $_infoMessages;
    var $_channels;
    var $_allChannels;
    var $_view;
    var $_lang;
    var $_invitations;
    var $_customVars;
    var $_sessionNew;
    var $_onlineUsersData;
    var $_bannedUsersData;

    function __construct($cron = false) {
        $this->initDataBaseConnection();
        if ($cron){
            return;
        }

        $this->initRequestVars();
        $this->initSession();
        $this->handleRequest();
    }

    function initRequestVars() {
        $this->_requestVars = [
            'ajax'          => isset($_REQUEST['ajax']),
	        'logout'	    => isset($_REQUEST['logout']),
	        'userID'	    => isset($_REQUEST['userID'])		? (int)$_REQUEST['userID']	  : null,
	        'userName'	    => isset($_REQUEST['userName'])		? $_REQUEST['userName']		  : null,
	        'channelID' 	=> isset($_REQUEST['channelID'])	? (int)$_REQUEST['channelID'] : null,
	        'channelName'	=> isset($_REQUEST['channelName'])	? $_REQUEST['channelName']	  : null,
	        'text'	    	=> isset($_POST   ['text'])		    ? $_POST['text']		      : null,
	        'lastID'    	=> isset($_REQUEST['lastID'])		? (int)$_REQUEST['lastID']	  : 0,
	        'view'	    	=> isset($_REQUEST['view'])	    	? $_REQUEST['view']	    	  : null,
	        'year'	    	=> isset($_REQUEST['year'])	    	? (int)$_REQUEST['year']	  : null,
	        'month'	    	=> isset($_REQUEST['month'])		? (int)$_REQUEST['month']	  : null,
	        'day'	    	=> isset($_REQUEST['day'])	    	? (int)$_REQUEST['day']		  : null,
	        'hour'	    	=> isset($_REQUEST['hour'])	    	? (int)$_REQUEST['hour']	  : null,
	        'search'	    => isset($_REQUEST['search'])		? $_REQUEST['search']		  : null,
	        'getInfos'	    => isset($_REQUEST['getInfos'])		? $_REQUEST['getInfos']		  : null,
	        'lang'          => isset($_REQUEST['lang'])	    	? $_REQUEST['lang']	    	  : null,
	        'delete'	    => isset($_REQUEST['delete'])		? (int)$_REQUEST['delete']	  : null,
	        'tmc'	    	=> isset($_REQUEST['tmc'])	    	? (int)$_REQUEST['tmc']		  : 10,
            'radio'         => isset($_REQUEST['radio'])        ? $_REQUEST['radio']          : false,
            'sortBy'        => isset($_REQUEST['sortBy'])       ? $_REQUEST['sortBy']         : Config::usersTableSortByDefault,
            'sortType'      => isset($_REQUEST['asc'])          ? "ASC"                       : "DESC",
	        'skip'    	    => isset($_REQUEST['skip'])		    ? (int)$_REQUEST['skip']	  : 0,
	        'count'    	    => isset($_REQUEST['count'])		? (int)$_REQUEST['count']	  : Config::usersTablePageSize,
            'msgColor'  	=> isset($_REQUEST['msgColor'])		? $_REQUEST['msgColor']	      : null,
            'nickColor'    	=> isset($_REQUEST['nickColor'])	? $_REQUEST['nickColor']	  : null
        ];

        // Remove slashes which have been added to user input strings if magic_quotes_gpc is On:
        if(get_magic_quotes_gpc()) {
            // It is safe to remove the slashes as we escape user data ourself
            array_walk(	$this->_requestVars,create_function('&$value, $key','if(is_string($value)) $value = stripslashes($value);') );
        }
    }

    function initDataBaseConnection() {
        // Create a new database object:
        switch(Config::$dbConnection['type']) {
			case 'mysqli':
				$this->db = new SChatDataBaseMySQLi();
				break;
			case 'mysql':
				$this->db = new SChatDataBaseMySQL();
				break;
			default:
				// Use MySQLi if available, else MySQL (and check the type of a given database connection object):
				if(function_exists('mysqli_connect') && (!Config::$dbConnection['link'] || is_object(Config::$dbConnection['link']))) {
					$this->db = new SChatDataBaseMySQLi();
				} else {
					$this->db = new SChatDataBaseMySQL();
				}
		}
        // Use a new database connection if no existing is given:
        if(!Config::$dbConnection['link']) {
            // Connect to the database server:
            $this->db->connect();
            if($this->db->error()) {
                echo $this->db->getError();
                die();
            }
            // Select the database:
            $this->db->select(Config::$dbConnection['name']);
            if($this->db->error()) {
                echo $this->db->getError();
                die();
            }
        }
        // Unset the dbConnection array for safety purposes:
        Config::$dbConnection = null;
    }

    function getDataBaseTable($table) {
        return ($this->db->getName() ? '`'.$this->db->getName().'`.ajax_chat_'.$table : 'ajax_chat_'.$table);
    }

    function initSession() {
        // Start the PHP session (if not already started):
        if(!session_id()) {
            session_name(Config::sessionName);
            session_set_cookie_params(
            0, // The session is destroyed on logout anyway, so no use to set this
            Config::sessionCookiePath,
            Config::sessionCookieDomain,
            Config::$sessionCookieSecure
            );
            session_start();
            $this->_sessionNew = true;
        }

        if($this->isLoggedIn()) {
            // Logout if we receive a logout request, the chat has been closed or the userID could not be revalidated:
            if($this->getRequestVar('logout') || !$this->isChatOpen()) {
                $this->logout();
                return;
            }
        } else {
            $this->login();
        }

        // Initialize the view:
        $this->initView();

        if($this->getView() == 'chat') {
            $this->initChatViewSession();
        }
        else {
            $this->removeInactive();
        }

        if(!$this->getRequestVar('ajax') && !headers_sent()) {
            // Set langCode cookie:
            $this->setLangCodeCookie();
        }
    }

    function initChatViewSession() {
        // If channel is not null we are logged in to the chat view:
        if($this->getChannel() !== null) {
            // Check if the current user has been logged out due to inactivity:
            if(!$this->isUserOnline()) {
                $this->logout();
                return;
            }
            if($this->getRequestVar('ajax')) {
                $this->initChannel();
                $this->updateOnlineStatus();
                $this->checkAndRemoveInactive();
            }
        } else {
            if($this->getRequestVar('ajax')) {
                // Set channel, insert login messages and add to online list on first ajax request in chat view:
                $this->chatViewLogin();
            }
        }
    }

    function isChatOpen() {
        return ($this->getUserRole() >= SCHAT_MODERATOR) || !Config::$chatClosed;
    }

    function handleRequest() {
        if($this->getRequestVar('ajax')) {
            if($this->isLoggedIn()) {
                // Parse info requests (for current userName, etc.):
                $this->parseInfoRequests();

                // Parse command requests (e.g. message deletion):
                $this->parseCommandRequests();

                // Parse message requests:
                $this->initMessageHandling();
            }
            // Send chat messages and online user list:
            $this->sendMessages();
        } else {
            if ($this->isLoggedIn()
                || $this->getRequestVar('view') == 'users'){
                // Display XHTML content for non-ajax requests:
                $this->sendXHTMLContent();
            }
            else {
                header('Location: ../');
                die();
            }
        }
    }

    function parseCommandRequests() {
        if($this->getRequestVar('delete') !== null) {
            $this->deleteMessage($this->getRequestVar('delete'));
        }
        if($this->getRequestVar('nickColor') !== null) {
            $color = ['#'.$this->getRequestVar('nickColor')];
            $this->setUserNickColor($color);
        }
        if($this->getRequestVar('msgColor') !== null) {
            $color = ['#'.$this->getRequestVar('msgColor')];
            $this->setUserMsgColor($color);
        }
    }

    function parseInfoRequests() {
        if($this->getRequestVar('getInfos')) {
            $infoRequests = explode(',', $this->getRequestVar('getInfos'));
            foreach($infoRequests as $infoRequest) {
                $this->parseInfoRequest($infoRequest);
            }
        }
    }

    function parseInfoRequest($infoRequest) {
        switch($infoRequest) {
            case 'userID':
                $this->addInfoMessage($this->getUserID(), 'userID');
                break;
            case 'userName':
                $this->addInfoMessage($this->getUserName(), 'userName');
                break;
            case 'userRole':
                $this->addInfoMessage($this->getUserRole(), 'userRole');
                break;
            case 'channelID':
                $this->addInfoMessage($this->getChannel(), 'channelID');
                break;
            case 'channelName':
                $this->addInfoMessage($this->getChannelName(), 'channelName');
                break;
            case 'userInfo':
                $this->addInfoMessage($this->getUserInfo(), 'userInfo');
                break;
        }
    }

    function sendXHTMLContent() {
        $httpHeader = new SChatHTTPHeader();

        $template = new SChatTemplate($this, $this->getTemplateFileName(), $httpHeader->getContentType());

        // Send HTTP header:
        $httpHeader->send();

        // Send parsed template content:
        echo $template->getParsedContent();
    }

    function getTemplateFileName() {
        switch($this->getView()) {
            case 'users':
                return SCHAT_PATH.DS.'lib'.DS.'template'.DS.'users.html';
            case 'logs':
                return SCHAT_PATH.DS.'lib'.DS.'template'.DS.'logs.html';
            default:
                return SCHAT_PATH.DS.($this->getSessionVar('mob')
                    ? 'lib'.DS.'template'.DS.'mob.html'
                    : 'lib'.DS.'template'.DS.'chat.html');
        }
    }

    function initView() {
        $this->_view = null;
        // "chat" is the default view:
        $view = ($this->getRequestVar('view') === null) ? 'chat' : $this->getRequestVar('view');
        if($this->hasAccessTo($view)) {
            $this->_view = $view;
        }
    }

    function getView() {
	    return $this->_view;
    }

    function hasAccessTo($view) {
        switch($view) {
            case 'teaser':
            case 'users':
                return true;
            case 'chat':
                return $this->isLoggedIn();
            case 'logs':
                return $this->isLoggedIn() && ($this->getUserRole() >= SCHAT_MODERATOR);
            default:
                return false;
        }
    }

    function login() {
        // Retrieve valid login user data (from request variables or session data):
        $userData = $this->getValidLoginUserData();

        if(!$userData) {
            $this->addInfoMessage('errorInvalidUser');
            return false;
        }

        // If the chat is closed, only the admin may login:
        if(!$this->isChatOpen() && $userData['userRole'] < SCHAT_MODERATOR) {
            $this->addInfoMessage('errorChatClosed');
            return false;
        }

        // Check if userID or userName are already listed online:
        if($this->isUserOnline($userData['userID']) || $this->isUserNameInUse($userData['userName'])) {
            // Set the registered user inactive and remove the inactive users so the user can be logged in again:
            $this->setInactive($userData['userID'], $userData['userName']);
            $this->removeInactive();
        }

        // Check if user is banned or in a user group not permitted in chat:
        if(($userData['userRole'] != SCHAT_ADMIN && $this->isUserBanned($userData['userName'], $userData['userID'], $_SERVER['REMOTE_ADDR'])) || $userData['userRole'] == SCHAT_BANNED) {
            $this->addInfoMessage('errorBanned');
            return false;
        }

        // Check if the max number of users is logged in (not affecting moderators or admins):
        if($userData['userRole'] <= SCHAT_MODERATOR && count($this->getOnlineUsersData()) >= Config::maxUsersLoggedIn) {
            $this->addInfoMessage('errorMaxUsersLoggedIn');
            return false;
        }

        // Use a new session id (if session has been started by the chat):
        $this->regenerateSessionID();

        // Log in:
        $this->setUserID($userData['userID']);
        $this->setUserName($userData['userName']);
        $this->setLoginUserName($userData['userName']);
        $this->setUserRole($userData['userRole']);
        $this->setUserInfo($userData['userInfo']);
        $this->setSessionVar('mob', $userData['mob']);

        $this->setLoggedIn(true);
        $this->setLoginTimeStamp(time());

        // IP Security check variable:
        $this->setSessionIP($_SERVER['REMOTE_ADDR']);

        // Add userID, userName and userRole to info messages:
        $this->addInfoMessage($this->getUserID(), 'userID');
        $this->addInfoMessage($this->getUserName(), 'userName');
        $this->addInfoMessage($this->getUserRole(), 'userRole');

        // Purge logs:
        if(Config::logsPurgeLogs) {
            $this->purgeLogs();
        }

        return true;
    }

    function chatViewLogin() {
        $this->setChannel($this->getValidRequestChannelID());
        $this->addToOnlineList();

        // Add channelID and channelName to info messages:
        $this->addInfoMessage($this->getChannel(), 'channelID');
        $this->addInfoMessage($this->getChannelName(), 'channelName');

        // Login message:
        $text = '/login '.$this->getUserName();
        $this->insertChatBotMessage(
            $this->getChannel(),
            $text,
            []
        );
    }

    function getValidRequestChannelID() {
        $channelID = $this->getRequestVar('channelID');
        $channelName = $this->getRequestVar('channelName');
        // Check the given channelID, or get channelID from channelName:
        if($channelID === null) {
            if($channelName !== null) {
                $channelID = $this->getChannelIDFromChannelName($channelName);
                // channelName might need encoding conversion:
                if($channelID === null) {
                    $channelID = $this->getChannelIDFromChannelName($this->trimChannelName($channelName));
                }
            }
        }
        // Validate the resulting channelID:
        if(!$this->validateChannel($channelID)) {
            if($this->getChannel() !== null) {
                return $this->getChannel();
            }
            return Config::defaultChannelID;
        }
        return $channelID;
    }

    function initChannel() {
        $channelID = $this->getRequestVar('channelID');
        $channelName = $this->getRequestVar('channelName');
        if($channelID !== null) {
            $this->switchChannel($this->getChannelNameFromChannelID($channelID));
        } else if($channelName !== null) {
            if($this->getChannelIDFromChannelName($channelName) === null) {
                // channelName might need encoding conversion:
                $channelName = $this->trimChannelName($channelName);
            }
            $this->switchChannel($channelName);
        }
    }

    function logout($type=null) {
        if($this->isUserOnline()) {
            $this->chatViewLogout($type);
        }
        $this->setLoggedIn(false);
        $this->destroySession();

        // Re-initialize the view:
        $this->initView();
    }

    function chatViewLogout($type) {
        $this->removeFromOnlineList();
        if($type !== null) {
            $type = ' '.$type;
        }
        // Logout message
        $text = '/logout '.$this->getUserName().$type;
        $this->insertChatBotMessage(
            $this->getChannel(),
            $text
        );
    }

    function switchChannel($channelName) {
        $channelID = $this->getChannelIDFromChannelName($channelName);
        $oldChannelName = $this->getChannelName();

        if($channelID !== null && $this->getChannel() == $channelID) {
            // User is already in the given channel, return:
            return;
        }

        // Check if we have a valid channel:
        if(!$this->validateChannel($channelID)) {
            // Invalid channel:
            $text = '/error InvalidChannelName '.$channelName;
            $this->insertChatBotMessage(
                $this->getPrivateMessageID(),
                $text
            );
            return;
        }

        $oldChannel = $this->getChannel();

        $this->setChannel($channelID);
        $this->updateOnlineList();

        // Channel leave message
        $text = '/channelLeave '.$this->getUserName().' '.$channelName;
        $this->insertChatBotMessage(
            $oldChannel,
            $text
        );

        // Channel enter message
        $text = '/channelEnter '.$this->getUserName().' '.$oldChannelName;
        $this->insertChatBotMessage(
            $this->getChannel(),
            $text
        );

        $this->addInfoMessage($channelName, 'channelSwitch');
        $this->addInfoMessage($channelID, 'channelID');
        $this->_requestVars['lastID'] = 0;
    }

    function addToOnlineList() {
        $sql = 'INSERT INTO '.$this->getDataBaseTable('online').'(
			    userID,
			    userName,
			    userRole,
			    channel,
			    userInfo,
			    dateTime,
			    ip,
			    mob
		    )
		    VALUES (
			    '.$this->db->makeSafe($this->getUserID()).',
			    '.$this->db->makeSafe($this->getUserName()).',
			    '.$this->db->makeSafe($this->getUserRole()).',
			    '.$this->db->makeSafe($this->getChannel()).',
			    '.$this->db->makeSafe(json_encode($this->getUserInfo(),JSON_UNESCAPED_UNICODE)).',
			    NOW(),
			    '.$this->db->makeSafe($this->ipToStorageFormat($_SERVER['REMOTE_ADDR'])).',
			    '.$this->db->makeSafe($this->getSessionVar('mob')).'
		    );';

        // Create a new SQL query:
        $result = $this->db->sqlQuery($sql);

        // Stop if an error occurs:
        if($result->error()) {
            echo $result->getError();
            die();
        }

        $this->resetOnlineUsersData();
    }

    function removeFromOnlineList() {
	    $sql = 'DELETE FROM
				    '.$this->getDataBaseTable('online').'
			    WHERE
				    userID = '.$this->db->makeSafe($this->getUserID()).';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $this->removeUserFromOnlineUsersData();
    }

    function updateOnlineList() {
	    $sql = 'UPDATE
				    '.$this->getDataBaseTable('online').'
			    SET
				    userName 	= '.$this->db->makeSafe($this->getUserName()).',
				    channel 	= '.$this->db->makeSafe($this->getChannel()).',
				    dateTime 	= NOW(),
				    ip			= '.$this->db->makeSafe($this->ipToStorageFormat($_SERVER['REMOTE_ADDR'])).'
			    WHERE
				    userID = '.$this->db->makeSafe($this->getUserID()).';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $this->resetOnlineUsersData();
    }

    function initMessageHandling() {
	    // Don't handle messages if we are not in chat view:
	    if($this->getView() != 'chat') {
		    return;
	    }

	    // Check if we have been uninvited from a private or restricted channel:
	    if(!$this->validateChannel($this->getChannel())) {
		    // Switch to the default channel:
		    $this->switchChannel($this->getChannelNameFromChannelID(Config::defaultChannelID));
		    return;
	    }

	    if($this->getRequestVar('text') !== null) {
		    $this->insertMessage($this->getRequestVar('text'));
	    }
    }

    function parseReactionMessage($textParts){
        if (count($textParts) <= 3)
            return;
        $action = $textParts[1];
        $msgId = $textParts[2];
        $reactionType = $textParts[3];

        if ($action == "add")
            $this->addReaction($msgId , $reactionType);
        else if ($action == "remove")
            $this->removeReaction($msgId , $reactionType);
    }

    function insertParsedMessage($text, $msgInfo) {
        // If a queryUserName is set, sent all messages as private messages to this userName:
        if($this->getQueryUserName() !== null && strpos($text, '/') !== 0) {
            $text = '/msg '.$this->getQueryUserName().' '.$text;
        }
        // Parse IRC-style commands:
        if(strpos($text, '/') === 0) {
            $textParts = explode(' ', $text);

            switch($textParts[0]) {
                // Channel switch:
                case '/join':
                    $this->insertParsedMessageJoin($textParts);
                    break;
                case '/reaction':
                    $this->parseReactionMessage($textParts);
                    $this->insertCustomMessage($this->getUserID(),$this->getUserName(),$this->getUserRole(),$this->getChannel(),$text, $msgInfo);
                    break;
                // Logout:
                case '/quit':
                    $this->logout();
                    break;
                // Private message:
                case '/msg':
                case '/describe':
                    $this->insertParsedMessagePrivMsg($textParts, $msgInfo);
                    break;
                // Invitation:
                case '/invite':
                    $this->insertParsedMessageInvite($textParts);
                    break;
                // Uninvitation:
                case '/uninvite':
                    $this->insertParsedMessageUninvite($textParts);
                    break;
                // Private messaging:
                case '/query':
                    $this->insertParsedMessageQuery($textParts);
                    break;
                // Kicking offending users from the chat:
                case '/kick':
                    $this->insertParsedMessageKick($textParts);
                    break;
                // Listing banned users:
                case '/bans':
                    $this->insertParsedMessageBans($textParts);
                    break;
                // Unban user (remove from ban list):
                case '/unban':
                    $this->insertParsedMessageUnban($textParts);
                    break;
                // Describing actions:
                case '/me':
                case '/action':
                    $this->insertParsedMessageAction($textParts, $msgInfo);
                    break;
                // Listing online Users:
                case '/who':
                    $this->insertParsedMessageWho($textParts);
                    break;
                // Listing available channels:
                case '/list':
                    $this->insertParsedMessageList($textParts);
                    break;
                // Retrieving the channel of a User:
                case '/whereis':
                    $this->insertParsedMessageWhereis($textParts);
                    break;
                case '/whois':
                    $this->insertParsedMessageWhois($textParts);
                    break;
                case '/roll':
                    $this->roll($textParts);
                    break;
                case '/nick':
                    $this->changeNick($textParts);
                    break;
                case '/opVideo':
                    $this->insertParsedMessageOpenVideo($textParts);
                    break;
                case '/inviteVideo':
                    $this->insertParsedMessageInviteVideo($textParts);
                    break;
                case '/setStatus':
                    $this->setStatus($textParts);
                    break;
                case '/call':
                    $this->insertParsedMessageCall($textParts);
                    break;
                case '/server':
                    if ($this->getUserRole() >= SCHAT_MODERATOR){
                        $this->insertChatBotMessage($this->getChannel(), substr($text, 8));
                    }
                    else{
                        $this->insertCustomMessage($this->getUserID(),$this->getUserName(),$this->getUserRole(),$this->getChannel(),$text, $msgInfo);
                    }
                    break;
                default:
                    $this->insertCustomMessage($this->getUserID(),$this->getUserName(),$this->getUserRole(),$this->getChannel(),$text, $msgInfo);
            }
        } else {
            // No command found, just insert the plain message:
            $this->insertCustomMessage(
                $this->getUserID(),
                $this->getUserName(),
                $this->getUserRole(),
                $this->getChannel(),
                $text,$msgInfo
                );
            $this->incrementMessageCount();
        }
    }

    function setStatus($textParts){
        $info = $this->getUserInfo();
        $info['s'] = intval($textParts[1]);
        $key = '';
        if (count($textParts) < 3){
            if (array_key_exists('vKey', $info)){unset($info['vKey']);}
        }else{
            $textParts[2] = $this->trimString(implode(' ',array_slice($textParts,2)),20,$info['s']===17,true);
            if (array_key_exists('vKey', $info) && $info['vKey'] === $textParts[2]){return;}
            $key = $info['vKey'] = $textParts[2];
        }
        $this->setUserInfo($info);
	    $result = $this->db->sqlQuery('UPDATE '.$this->getDataBaseTable('online').' SET userInfo = '.$this->db->makeSafe(json_encode($this->getUserInfo(),JSON_UNESCAPED_UNICODE)).' WHERE userID = '.$this->getUserID());
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
	    $this->insertChatBotMessage($this->getChannel(),'/setStatus '.$this->getUserName().' '.$textParts[1].' '.$key);
    }

    function insertParsedMessageJoin($textParts) {
        if(count($textParts) == 1) {
            $this->switchChannel($this->getChannelNameFromChannelID($this->getPrivateChannelID()));
        } else {
            $this->switchChannel($textParts[1]);
        }
    }
    function insertParsedMessagePrivMsg($textParts, $msgInfo) {
        if(count($textParts) < 3) {
            if(count($textParts) == 2) {
                $this->insertChatBotMessage(
                    $this->getPrivateMessageID(),
                    '/error MissingText'
                );
            } else {
                $this->insertChatBotMessage(
                    $this->getPrivateMessageID(),
                    '/error MissingUserName'
                );
            }
        } else {
            // Get UserID from UserName:
            $toUserID = $this->getIDFromName($textParts[1]);
            if($toUserID === null) {
                if($this->getQueryUserName() !== null) {
                    // Close the current query:
                    $this->insertMessage('/query');
                } else {
                    $this->insertChatBotMessage(
                        $this->getPrivateMessageID(),
                        '/error UserNameNotFound '.$textParts[1]
                    );
                }
            } else {
                // Insert /privaction command if /describe is used:
                $command = ($textParts[0] == '/describe') ? '/privaction' : '/privmsg';
                // Copy of private message to current User:
                $this->insertCustomMessage(
                    $this->getUserID(),
                    $this->getUserName(),
                    $this->getUserRole(),
                    $this->getPrivateMessageID(),
                    $command.'to '.$textParts[1].' '.implode(' ', array_slice($textParts, 2)),
                    $msgInfo
                );
                // Private message to requested User:
                $this->insertCustomMessage(
                    $this->getUserID(),
                    $this->getUserName(),
                    $this->getUserRole(),
                    $this->getPrivateMessageID($toUserID),
                    $command.' '.implode(' ', array_slice($textParts, 2)),
                    $msgInfo
                );
            }
        }
    }
    function insertParsedMessageOpenVideo($textParts)
    {
        if (count($textParts) < 2){
            $this->insertChatBotMessage($this->getPrivateMessageID(),'/error MissingKey');
        }
        else{
            $this->insertChatBotMessage($this->getChannel(),$textParts[0].' '.$this->getUserName().' '.$textParts[1]);
        }
    }
    function insertParsedMessageInviteVideo($textParts) {
        if(count($textParts) < 4) {
            if(count($textParts) == 3) {
                $this->insertChatBotMessage($this->getPrivateMessageID(),'/error MissingKey');
            } else {
                $this->insertChatBotMessage($this->getPrivateMessageID(),'/error MissingUserName');
            }
        } else {
            // Get UserID from UserName:
            $toUserID = $this->getIDFromName($textParts[1]);
            if($toUserID === null) {
                $this->insertChatBotMessage($this->getPrivateMessageID(),'/error UserNameNotFound '.$textParts[1]);
            } else {
                $this->insertChatBotMessage($this->getPrivateMessageID(), 'Приглашение отправленно');
                $this->insertChatBotMessage($this->getPrivateMessageID($toUserID), $textParts[0].' '.$textParts[2].' '.$textParts[3].' '.$textParts[4]);
            }
        }
    }
    function insertParsedMessageCall($textParts) {
        if(count($textParts) < 2) {
            $this->insertChatBotMessage($this->getPrivateMessageID(),'/error MissingUserName');
        } else {
            // Get UserID from UserName:
            $toUserID = $this->getIDFromName($textParts[1]);
            if($toUserID === null) {
                $this->insertChatBotMessage($this->getPrivateMessageID(),'/error UserNameNotFound '.$textParts[1]);
            } else {
                $this->insertChatBotMessage($this->getPrivateMessageID(), 'Вы отправили вызов пользователю '.$textParts[1]);
                $this->insertChatBotMessage($this->getPrivateMessageID($toUserID), $textParts[0].' '.$this->getUserName());
            }
        }
    }
    function insertParsedMessageInvite($textParts) {
	    if($this->getChannel() == $this->getPrivateChannelID() || in_array($this->getChannel(), $this->getChannels())) {
		    if(count($textParts) == 1) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error MissingUserName'
			    );
		    } else {
			    $toUserID = $this->getIDFromName($textParts[1]);
			    if($toUserID === null) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/error UserNameNotFound '.$textParts[1]
				    );
			    } else {
				    // Add the invitation to the database:
				    $this->addInvitation($toUserID);
				    $invitationChannelName = $this->getChannelNameFromChannelID($this->getChannel());
				    // Copy of invitation to current User:
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/inviteto '.$textParts[1].' '.$invitationChannelName
				    );
				    // Invitation to requested User:
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID($toUserID),
					    '/invite '.$this->getUserName().' '.$invitationChannelName
				    );
			    }
		    }
	    } else {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error InviteNotAllowed'
		    );
	    }
    }
    function insertParsedMessageUninvite($textParts) {
	    if($this->getChannel() == $this->getPrivateChannelID() || in_array($this->getChannel(), $this->getChannels())) {
		    if(count($textParts) == 1) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error MissingUserName'
			    );
		    } else {
			    $toUserID = $this->getIDFromName($textParts[1]);
			    if($toUserID === null) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/error UserNameNotFound '.$textParts[1]
				    );
			    } else {
				    // Remove the invitation from the database:
				    $this->removeInvitation($toUserID);
				    $invitationChannelName = $this->getChannelNameFromChannelID($this->getChannel());
				    // Copy of uninvitation to current User:
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/uninviteto '.$textParts[1].' '.$invitationChannelName
				    );
				    // Uninvitation to requested User:
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID($toUserID),
					    '/uninvite '.$this->getUserName().' '.$invitationChannelName
				    );
			    }
		    }
	    } else {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error UninviteNotAllowed'
		    );
	    }
    }
    function insertParsedMessageQuery($textParts) {
        if(count($textParts) == 1) {
            if($this->getQueryUserName() !== null) {
                $this->insertChatBotMessage(
                    $this->getPrivateMessageID(),
                    '/queryClose '.$this->getQueryUserName()
                );
                // Close the current query:
                $this->setQueryUserName(null);
            } else {
                $this->insertChatBotMessage(
                    $this->getPrivateMessageID(),
                    '/error NoOpenQuery'
                );
            }
        } else {
            if($this->getIDFromName($textParts[1]) === null) {
                $this->insertChatBotMessage(
                    $this->getPrivateMessageID(),
                    '/error UserNameNotFound '.$textParts[1]
                );
            } else {
                if($this->getQueryUserName() !== null) {
                    // Close the current query:
                    $this->insertMessage('/query');
                }
                // Open a query to the requested user:
                $this->setQueryUserName($textParts[1]);
                $this->insertChatBotMessage(
                    $this->getPrivateMessageID(),
                    '/queryOpen '.$textParts[1]
                );
            }
        }
    }
    function insertParsedMessageKick($textParts) {
	    // Only moderators/admins may kick users:
	    if($this->getUserRole() >= SCHAT_MODERATOR) {
		    if(count($textParts) == 1) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error MissingUserName'
			    );
		    } else {
			    // Get UserID from UserName:
			    $kickUserID = $this->getIDFromName($textParts[1]);
			    if($kickUserID === null) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/error UserNameNotFound '.$textParts[1]
				    );
			    } else {
				    // Check the role of the user to kick:
				    $kickUserRole = $this->getRoleFromID($kickUserID);
				    if($kickUserRole == SCHAT_ADMIN || ($kickUserRole == SCHAT_MODERATOR && $this->getUserRole() != SCHAT_ADMIN)) {
					    // Admins and moderators may not be kicked:
					    $this->insertChatBotMessage(
						    $this->getPrivateMessageID(),
						    '/error KickNotAllowed '.$textParts[1]
					    );
				    } else {
					    // Kick user and insert message:
					    $channel = $this->getChannelFromID($kickUserID);
					    $banMinutes = (count($textParts) > 2) ? $textParts[2] : null;
					    $this->kickUser($textParts[1], $banMinutes, $kickUserID);
					    // If no channel found, user logged out before he could be kicked
					    if($channel !== null) {
						    $this->insertChatBotMessage(
							    $channel,
							    '/kick '.$textParts[1]
						    );
						    // Send a copy of the message to the current user, if not in the channel:
						    if($channel != $this->getChannel()) {
							    $this->insertChatBotMessage(
								    $this->getPrivateMessageID(),
								    '/kick '.$textParts[1]
							    );
						    }
					    }
				    }
			    }
		    }
	    } else {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error CommandNotAllowed '.$textParts[0]
		    );
	    }
    }
    function insertParsedMessageBans($textParts) {
	    // Only moderators/admins may see the list of banned users:
	    if($this->getUserRole() >= SCHAT_MODERATOR) {
		    $this->removeExpiredBans();
		    $bannedUsers = $this->getBannedUsers();
		    if(count($bannedUsers) > 0) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/bans '.implode(' ', $bannedUsers)
			    );
		    } else {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/bansEmpty -'
			    );
		    }
	    } else {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error CommandNotAllowed '.$textParts[0]
		    );
	    }
    }
    function insertParsedMessageUnban($textParts) {
	    // Only moderators/admins may unban users:
	    if($this->getUserRole() >= SCHAT_MODERATOR) {
		    $this->removeExpiredBans();
		    if(count($textParts) == 1) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error MissingUserName'
			    );
		    } else {
			    if(!in_array($textParts[1], $this->getBannedUsers())) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/error UserNameNotFound '.$textParts[1]
				    );
			    } else {
				    // Unban user and insert message:
				    $this->unbanUser($textParts[1]);
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/unban '.$textParts[1]
				    );
			    }
		    }
	    } else {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error CommandNotAllowed '.$textParts[0]
		    );
	    }
    }
    function insertParsedMessageAction($textParts, $msgInfo) {
	    if(count($textParts) == 1) {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error MissingText'
		    );
	    } else {
		    if($this->getQueryUserName() !== null) {
			    // If we are in query mode, sent the action to the query user:
			    $this->insertMessage('/describe '.$this->getQueryUserName().' '.implode(' ', array_slice($textParts, 1)));
		    } else {
			    $this->insertCustomMessage(
				    $this->getUserID(),
				    $this->getUserName(),
				    $this->getUserRole(),
				    $this->getChannel(),
				    implode(' ', $textParts),
				    $msgInfo
			    );
		    }
	    }
    }
    function insertParsedMessageWho($textParts) {
	    if(count($textParts) == 1) {
		    if($this->getUserRole() >= SCHAT_MODERATOR) {
			    // List online users from any channel:
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/who '.implode(' ', $this->getOnlineUsers())
			    );
		    } else {
			    // Get online users for all accessible channels:
			    $channels = $this->getChannels();
			    array_push($channels, $this->getPrivateChannelID());
			    // Add the invitation channels:
			    foreach($this->getInvitations() as $channelID) {
				    if(!in_array($channelID, $channels)) {
					    array_push($channels, $channelID);
				    }
			    }
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/who '.implode(' ', $this->getOnlineUsers($channels))
			    );
		    }
	    } else {
		    $channelName = $textParts[1];
		    $channelID = $this->getChannelIDFromChannelName($channelName);
		    if(!$this->validateChannel($channelID)) {
			    // Invalid channel:
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error InvalidChannelName '.$channelName
			    );
		    } else {
			    // Get online users for the given channel:
			    $onlineUsers = $this->getOnlineUsers([$channelID]);
			    if(count($onlineUsers) > 0) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/whoChannel '.$channelName.' '.implode(' ', $onlineUsers)
				    );
			    } else {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/whoEmpty -'
				    );
			    }
		    }
	    }
    }
    function insertParsedMessageList($textParts) {
        // Get the names of all accessible channels:
        $channelNames = $this->getChannelNames();
        // Add the own private channel:
        array_push($channelNames, $this->getPrivateChannelName());
        // Add the invitation channels:
        foreach($this->getInvitations() as $channelID) {
            $channelName = $this->getChannelNameFromChannelID($channelID);
            if($channelName !== null && !in_array($channelName, $channelNames)) {
                array_push($channelNames, $channelName);
            }
        }
        $this->insertChatBotMessage(
            $this->getPrivateMessageID(),
            '/list '.implode(' ', $channelNames)
        );
    }
    function insertParsedMessageWhereis($textParts) {
	    if(count($textParts) == 1) {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error MissingUserName'
		    );
	    } else {
		    // Get UserID from UserName:
		    $whereisUserID = $this->getIDFromName($textParts[1]);
		    if($whereisUserID === null) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error UserNameNotFound '.$textParts[1]
			    );
		    } else {
			    $channelID = $this->getChannelFromID($whereisUserID);
			    if($this->validateChannel($channelID)) {
				    $channelName = $this->getChannelNameFromChannelID($channelID);
			    } else {
				    $channelName = null;
			    }
			    if($channelName === null) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/error UserNameNotFound '.$textParts[1]
				    );
			    } else {
				    // List user information:
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/whereis '.$textParts[1].' '.$channelName
				    );
			    }
		    }
	    }
    }
    function insertParsedMessageWhois($textParts) {
	    // Only moderators/admins:
	    if($this->getUserRole() >= SCHAT_MODERATOR) {
		    if(count($textParts) == 1) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error MissingUserName'
			    );
		    } else {
			    // Get UserID from UserName:
			    $whoisUserID = $this->getIDFromName($textParts[1]);
			    if($whoisUserID === null) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/error UserNameNotFound '.$textParts[1]
				    );
			    } else {
				    // List user information:
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/whois '.$textParts[1].' '.$this->getIPFromID($whoisUserID)
				    );
			    }
		    }
	    } else {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error CommandNotAllowed '.$textParts[0]
		    );
	    }
    }
    function roll($textParts) {
        mt_srand((double)microtime()*1000000);
	    if(count($textParts) == 1) {
		    // default is one d6:
		    $text = '/roll '.$this->getUserName().' 1d6 '.mt_rand(1, 6);
	    } else {
		    $diceParts = explode('d', $textParts[1]);
		    if (count($diceParts) == 1)
			    $diceParts[1] = '6';
		    if(count($diceParts) == 2) {
			    $number = (int)$diceParts[0];
			    $sides = (int)$diceParts[1];

			    // Dice number must be an integer between 1 and 100, else roll only one:
			    $number = ($number > 0 && $number <= 100) ?  $number : 1;

			    // Sides must be an integer between 1 and 100, else take 6:
			    $sides = ($sides > 0 && $sides <= 100) ?  $sides : 6;

			    $text = '/roll '.$this->getUserName().' '.$number.'d'.$sides.' ';
			    for($i=0; $i<$number; $i++)
				    $text .= ($i != 0 ? ',' : '').mt_rand(1, $sides);
		    } else {
			    // if dice syntax is invalid, roll one d6:
			    $text = '/roll '.$this->getUserName().' 1d6 '.mt_rand(1, 6);
		    }
	    }
	    $this->insertChatBotMessage(
		    $this->getChannel(),
		    $text
	    );
    }
    function changeNick($textParts) {
	    if(!Config::allowNickChange) {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error CommandNotAllowed '.$textParts[0]
		    );
	    } else if(count($textParts) == 1) {
		    $this->insertChatBotMessage(
			    $this->getPrivateMessageID(),
			    '/error MissingUserName'
		    );
	    } else {
		    $newUserName = implode(' ', array_slice($textParts, 1));
		    if($newUserName == $this->getLoginUserName()) {
			    // Allow the user to regain the original login userName:
			    $prefix = '';
			    $suffix = '';
		    } else {
			    $prefix = Config::changedNickPrefix;
			    $suffix = Config::changedNickSuffix;
		    }
		    $maxLength = Config::userNameMaxLength - SChatString::stringLength($prefix) - SChatString::stringLength($suffix);
		    $newUserName = $this->trimString($newUserName, $maxLength, true);
		    if(!$newUserName) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error InvalidUserName'
			    );
		    } else {
			    $newUserName = $prefix.$newUserName.$suffix;
			    if($this->isUserNameInUse($newUserName)) {
				    $this->insertChatBotMessage(
					    $this->getPrivateMessageID(),
					    '/error UserNameInUse'
				    );
			    } else {
				    $oldUserName = $this->getUserName();
				    $this->setUserName($newUserName);
				    $this->updateOnlineList();
				    // Add info message to update the client-side stored userName:
				    $this->addInfoMessage($this->getUserName(), 'userName');
				    $this->insertChatBotMessage(
					    $this->getChannel(),
					    '/nick '.$oldUserName.' '.$newUserName
				    );
			    }
		    }
	    }
    }

    function insertMessage($text) {
        if(!$this->floodControl())
            return;

        $text = $this->trimMessageText($text);

        if($text == '')
            return;

        $msgInfo = [];

        $msgInfo['ncol'] = $this->getUserNickColor();
        $msgInfo['mcol'] = $this->getUserMsgColor();
        $msgInfo['avatar'] = $this->getUserInfo()['avatar'];

        $this->insertParsedMessage($text, $msgInfo);
    }
    function deleteMessage($messageID) {
	    // Retrieve the channel of the given message:
	    $sql = 'SELECT
				    channel
			    FROM
				    '.$this->getDataBaseTable('messages').'
			    WHERE
				    id='.$this->db->makeSafe($messageID).';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $row = $result->fetch();

	    if($row['channel'] !== null) {
		    $channel = $row['channel'];

		    if($this->getUserRole() == SCHAT_ADMIN) {
			    $condition = '';
		    } else if($this->getUserRole() == SCHAT_MODERATOR) {
			    $condition = '	AND
								    NOT (userRole='.SCHAT_ADMIN.')
							    AND
								    NOT (userRole='.SCHAT_CHATBOT.')';
		    } else if($this->getUserRole() == SCHAT_USER) {
			    $condition = 'AND
							    (
							    userID='.$this->db->makeSafe($this->getUserID()).'
							    OR
								    (
								    channel = '.$this->db->makeSafe($this->getPrivateMessageID()).'
								    OR
								    channel = '.$this->db->makeSafe($this->getPrivateChannelID()).'
								    )
								    AND
									    NOT (userRole='.$this->db->makeSafe(SCHAT_ADMIN).')
								    AND
									    NOT (userRole='.$this->db->makeSafe(SCHAT_CHATBOT).')
							    )';
		    } else {
			    return false;
		    }

		    // Remove given message from the database:
		    $sql = 'DELETE FROM
					    '.$this->getDataBaseTable('messages').'
				    WHERE
					    id='.$this->db->makeSafe($messageID).'
					    '.$condition.';';

		    // Create a new SQL query:
		    $result = $this->db->sqlQuery($sql);

		    // Stop if an error occurs:
		    if($result->error()) {
			    echo $result->getError();
			    die();
		    }

		    if($result->affectedRows() == 1) {
			    // Insert a deletion command to remove the message from the clients chatlists:
			    $this->insertChatBotMessage($channel, '/delete '.$messageID);
			    return true;
		    }
	    }
	    return false;
    }

    function floodControl() {
	    // Moderators and Admins need no flood control:
	    if($this->getUserRole() >= SCHAT_MODERATOR ) {
		    return true;
	    }
	    $time = time();
	    // Check the time of the last inserted message:
	    if($this->getInsertedMessagesRateTimeStamp()+60 < $time) {
		    $this->setInsertedMessagesRateTimeStamp($time);
		    $this->setInsertedMessagesRate(1);
	    } else {
		    // Increase the inserted messages rate:
		    $rate = $this->getInsertedMessagesRate()+1;
		    $this->setInsertedMessagesRate($rate);
		    // Check if message rate is too high:
		    if($rate > Config::maxMessageRate) {
			    $this->insertChatBotMessage(
				    $this->getPrivateMessageID(),
				    '/error MaxMessageRate'
			    );
			    // Return false so the message is not inserted:
			    return false;
		    }
	    }
	    return true;
    }

    function insertChatBotMessage($channelID, $messageText, $msgInfo = null) {
        if (!$msgInfo)
            $msgInfo = [];
        if (!isset($msgInfo['avatar']))
            $msgInfo['avatar'] = Config::chatBotAvatar;

	    $this->insertCustomMessage(
		    Config::chatBotID,
		    Config::chatBotName,
		    SCHAT_CHATBOT,
		    $channelID,
		    $messageText,
		    $msgInfo
	    );
    }

    function insertCustomMessage($userID, $userName, $userRole, $channelID, $text, $msgInfo = null) {
        $ip = isset($_SERVER['REMOTE_ADDR']) ? $_SERVER['REMOTE_ADDR'] : "0:0:0:0";

	    $sql = 'INSERT INTO '.$this->getDataBaseTable('messages').'(
							    userID,
							    userName,
							    userRole,
							    channel,
							    dateTime,
							    ip,
							    text,
							    msgInfo
						    )
			    VALUES (
				    '.$this->db->makeSafe($userID).',
				    '.$this->db->makeSafe($userName).',
				    '.$this->db->makeSafe($userRole).',
				    '.$this->db->makeSafe($channelID).',
				    NOW(),
				    '.$this->db->makeSafe($this->ipToStorageFormat($ip)).',
				    '.$this->db->makeSafe($text).',
				    '.$this->db->makeSafe((!is_null($msgInfo) ? json_encode($msgInfo,JSON_UNESCAPED_UNICODE):'{}')).'
			    );';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function kickUser($userName, $banMinutes=null, $userID=null) {
	    if($userID === null) {
		    $userID = $this->getIDFromName($userName);
	    }
	    if($userID === null) {
		    return;
	    }

	    $banMinutes = ($banMinutes !== null) ? $banMinutes : Config::defaultBanTime;

	    if($banMinutes) {
		    // Ban User for the given time in minutes:
		    $this->banUser($userName, $banMinutes, $userID);
	    }

	    // Remove given User from online list:
	    $sql = 'DELETE FROM
				    '.$this->getDataBaseTable('online').'
			    WHERE
				    userID = '.$this->db->makeSafe($userID).';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $this->removeUserFromOnlineUsersData($userID);
    }

    function getBannedUsersData($key=null, $value=null) {
	    if($this->_bannedUsersData === null) {
		    $this->_bannedUsersData = [];

		    $sql = 'SELECT
					    userID,
					    userName,
					    ip
				    FROM
					    '.$this->getDataBaseTable('bans').'
				    WHERE
					    NOW() < dateTime;';

		    // Create a new SQL query:
		    $result = $this->db->sqlQuery($sql);

		    // Stop if an error occurs:
		    if($result->error()) {
			    echo $result->getError();
			    die();
		    }

		    while($row = $result->fetch()) {
			    $row['ip'] = $this->ipFromStorageFormat($row['ip']);
			    array_push($this->_bannedUsersData, $row);
		    }

		    $result->free();
	    }

	    if($key) {
		    $bannedUsersData = [];
		    foreach($this->_bannedUsersData as $bannedUserData) {
			    if(!isset($bannedUserData[$key])) {
				    return $bannedUsersData;
			    }
			    if($value) {
				    if($bannedUserData[$key] == $value) {
					    array_push($bannedUsersData, $bannedUserData);
				    } else {
					    continue;
				    }
			    } else {
				    array_push($bannedUsersData, $bannedUserData[$key]);
			    }
		    }
		    return $bannedUsersData;
	    }

	    return $this->_bannedUsersData;
    }

    function getBannedUsers() {
	    return $this->getBannedUsersData('userName');
    }

    function banUser($userName, $banMinutes=null, $userID=null) {
	    if($userID === null) {
		    $userID = $this->getIDFromName($userName);
	    }
	    $ip = $this->getIPFromID($userID);
	    if(!$ip || $userID === null) {
		    return;
	    }

	    // Remove expired bans:
	    $this->removeExpiredBans();

	    $banMinutes = (int)$banMinutes;
	    if(!$banMinutes) {
		    // If banMinutes is not a valid integer, use the defaultBanTime:
		    $banMinutes = Config::defaultBanTime;
	    }

	    $sql = 'INSERT INTO '.$this->getDataBaseTable('bans').'(
				    userID,
				    userName,
				    dateTime,
				    ip
			    )
			    VALUES (
				    '.$this->db->makeSafe($userID).',
				    '.$this->db->makeSafe($userName).',
				    DATE_ADD(NOW(), interval '.$this->db->makeSafe($banMinutes).' MINUTE),
				    '.$this->db->makeSafe($this->ipToStorageFormat($ip)).'
			    );';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function unbanUser($userName) {
	    $sql = 'DELETE FROM
				    '.$this->getDataBaseTable('bans').'
			    WHERE
				    userName = '.$this->db->makeSafe($userName).';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function removeExpiredBans() {
	    $sql = 'DELETE FROM
				    '.$this->getDataBaseTable('bans').'
			    WHERE
				    dateTime < NOW();';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function setInactive($userID, $userName=null) {
	    $condition = 'userID='.$this->db->makeSafe($userID);
	    if($userName !== null) {
		    $condition .= ' OR userName='.$this->db->makeSafe($userName);
	    }
	    $sql = 'UPDATE
				    '.$this->getDataBaseTable('online').'
			    SET
				    dateTime = DATE_SUB(NOW(), interval '.(intval(Config::inactiveTimeoutMobile)+1).' MINUTE)
			    WHERE
				    '.$condition.';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $this->resetOnlineUsersData();
    }

    function removeInactive() {
	    $sql = 'SELECT
				    userID,
				    userName,
				    channel,
				    userInfo
			    FROM
				    '.$this->getDataBaseTable('online').'
			    WHERE
				    NOW() > DATE_ADD(dateTime, interval IF (mob = 0, '.Config::inactiveTimeout.', '.Config::inactiveTimeoutMobile.') MINUTE);';
	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    if($result->numRows() > 0) {
		    $condition = '';
		    while($row = $result->fetch()) {
			    if(!empty($condition))
				    $condition .= ' OR ';
			    // Add userID to condition for removal:
			    $condition .= 'userID='.$this->db->makeSafe($row['userID']);

			    $this->removeUserFromOnlineUsersData($row['userID']);

                // Insert logout timeout message:
			    $this->insertChatBotMessage(
				    $row['channel'],
				    '/logout '.$row['userName'].' Timeout'
			    );
		    }

		    $result->free();

		    $sql = 'DELETE FROM
					    '.$this->getDataBaseTable('online').'
				    WHERE
					    '.$condition.';';

		    // Create a new SQL query:
		    $result = $this->db->sqlQuery($sql);

		    // Stop if an error occurs:
		    if($result->error()) {
			    echo $result->getError();
			    die();
		    }
	    }
    }

    function updateOnlineStatus() {
	    // Update online status every 60 seconds (this allows update requests to be in time):
	    if(!$this->getStatusUpdateTimeStamp() || ((time() - $this->getStatusUpdateTimeStamp()) > 60)) {
		    $this->updateOnlineList();
		    $this->setStatusUpdateTimeStamp(time());
            $this->appendMinutesToUserTimeInChat(1);
	    }
    }

    function checkAndRemoveInactive() {
	    // Remove inactive users every inactiveCheckInterval:
	    if(!$this->getInactiveCheckTimeStamp() || ((time() - $this->getInactiveCheckTimeStamp()) > Config::inactiveCheckInterval*60)) {
		    $this->removeInactive();
		    $this->setInactiveCheckTimeStamp(time());
	    }
    }

    function sendMessages() {
	    $httpHeader = new SChatHTTPHeader('application/json'); //or text/json

	    // Send HTTP header:
	    $httpHeader->send();
	    echo json_encode($this->getMessages(),JSON_UNESCAPED_UNICODE);
    }

    function getMessages() {
        try {
            switch($this->getView()) {
                case 'users':
                    return $this->getUsersTopJSONTable();
                case 'chat':
                    return $this->getChatViewJSONMessages();
                case 'teaser':
                    return $this->getTeaserViewJSONMessages();
                case 'logs':
                    return $this->getLogsViewJSONMessages();
                default:
                    return ["infos" => ["logout" => SChatEncoding::encodeSpecialChars(Config::logoutData)]];
            }
        }
        catch (Exception $ex) {
            return ["error" => $ex->getMessage()];
        }
    }

    function getMessageCondition() {
        $lastID = $this->db->makeSafe($this->getRequestVar('lastID'));
	    $condition = 	'msgs.id > '.$lastID.'
                        AND ('.$lastID.' > 0
                            OR
                            NOT (msgs.text like \'/reaction%\')
                        )
					    AND (
						    msgs.channel = '.$this->db->makeSafe($this->getChannel()).'
						    OR
						    msgs.channel = '.$this->db->makeSafe($this->getPrivateMessageID()).'
					    )
					    AND
					    ';
	    if(Config::requestMessagesPriorChannelEnter ||
		    (Config::requestMessagesPriorChannelEnterList && in_array($this->getChannel(), Config::requestMessagesPriorChannelEnterList))) {
		    $condition .= 'NOW() < DATE_ADD(dateTime, interval '.Config::requestMessagesTimeDiff.' HOUR)';
	    } else {
		    $condition .= 'dateTime >= FROM_UNIXTIME(' . $this->getChannelEnterTimeStamp() . ')';
	    }
	    return $condition;
    }

    function getChatViewMessages() {

	    // Get the last messages in descending order (this optimises the LIMIT usage):
	    $sql = 'SELECT
				    msgs.id,
				    msgs.userID,
				    msgs.userName,
				    msgs.userRole,
				    msgs.channel AS channelID,
				    UNIX_TIMESTAMP(msgs.dateTime) AS timeStamp,
				    msgs.text,
				    msgs.msgInfo
			    FROM
				    '.$this->getDataBaseTable('messages').' msgs
			    WHERE
				    '.$this->getMessageCondition().'
                GROUP BY msgs.id
                ORDER BY msgs.id DESC
			    LIMIT '.($this->getRequestVar('lastID') == 0?Config::requestMessagesLimit:100).';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $rows = [];
	    // Add the messages in reverse order so it is ascending again:
	    while($row = $result->fetch()) {
            $rows[] = [
                'id' => (int)$row['id'],
                'uID' => (int)$row['userID'],
                'role' => (int)$row['userRole'],
                'cID' => (int)$row['channelID'],
                'name' => SChatEncoding::encodeSpecialChars($row['userName']),
                'text' => SChatEncoding::encodeSpecialChars($row['text']),
                'time' => $row['timeStamp'],
                'info' => json_decode($row['msgInfo'], true),
                'reactions' => $this->getReactions($row['id'])
            ];
	    }
	    $result->free();

	    return array_reverse($rows);
    }

    function getChatViewJSONMessages() {
        $json = [];
        $inf = $this->getInfoMessages();
        $msg = $this->getChatViewMessages();

        if ($inf != null){
            $json['infos'] = $inf;
        }
        if ($msg != null){
            $json['msgs'] = $msg;
        }
        $json['users'] = $this->getOnlineUsersData([$this->getChannel()]);

        if ($this->getRequestVar('radio') && Config::radioServer){
            $json['radio'] = Config::radioInfo ? json_decode(file_get_contents(Config::radioServer."/info.xsl")) : [['mount' => '/radio']];
        }
        return $json;
    }

    function getTeaserMessageCondition() {
	    $channelID = $this->getValidRequestChannelID();
	    $condition = '';
	    if(Config::requestMessagesPriorChannelEnter ||
		    (Config::requestMessagesPriorChannelEnterList && in_array($channelID, Config::requestMessagesPriorChannelEnterList))) {
		    $condition .= 'NOW() < DATE_ADD(dateTime, interval '.Config::requestMessagesTimeDiff.' HOUR)';
	    } else {
		    // Teaser content may not be shown for this channel:
		    $condition .= '0 = 1';
	    }
	    return $condition;
    }

    function getTeaserViewMessages() {
	    // Get the last messages in descending order (this optimises the LIMIT usage):
	    $sql = 'SELECT
				    id,
				    userID,
				    userName,
				    userRole,
				    channel AS channelID,
				    UNIX_TIMESTAMP(dateTime) AS timeStamp,
				    text,
				    msgInfo
			    FROM
				    '.$this->getDataBaseTable('messages').'
			    WHERE
				    '.$this->getTeaserMessageCondition().'
			    ORDER BY
				    id
				    DESC
			    LIMIT '.$this->getRequestVar('tmc').';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $rows = [];
	    // Add the messages in reverse order so it is ascending again:
	    while($row = $result->fetch()) {
            $rows[] = [
                'channelID' =>  (int)$row['channelID'],
                'userName' =>  SChatEncoding::encodeSpecialChars($row['userName']),
                'text' =>  SChatEncoding::encodeSpecialChars($row['text']),
                'dateTime' =>  date('r', $row['timeStamp'])
            ];
	    }
	    $result->free();

	    return array_reverse($rows);
    }

    function getTeaserViewJSONMessages() {
        $json = [];
        if ($this->getUserRole() >= SCHAT_USER){
            $json['infos'] = $this->getInfoMessages();
        }
        if ($this->getUserRole() >= SCHAT_MODERATOR && $this->getRequestVar('tmc') != 0){
            $json['msgs'] = $this->getTeaserViewMessages();
        }
        $json['users'] = $this->getOnlineUsersData();
        return $json;
    }

    function getLogsViewCondition() {
	    $condition = 'id > '.$this->db->makeSafe($this->getRequestVar('lastID'));

	    // Check the channel condition:
	    switch($this->getRequestVar('channelID')) {
		    case '-3':
			    break;
		    case '-2':
			    $condition .= ' AND channel > '.(Config::privateMessageDiff-1);
			    break;
		    case '-1':
			    $condition .= ' AND (channel > '.(Config::privateChannelDiff-1).' AND channel < '.(Config::privateMessageDiff).')';
			    break;
		    default:
			    if($this->validateChannel($this->getRequestVar('channelID'))) {
				    $condition .= ' AND channel = '.$this->db->makeSafe($this->getRequestVar('channelID'));
			    } else {
				    // No valid channel:
				    $condition .= ' AND 0 = 1';
			    }
	    }

	    // Check the period condition:
	    $hour	= ($this->getRequestVar('hour') === null || $this->getRequestVar('hour') > 23 || $this->getRequestVar('hour') < 0) ? null : $this->getRequestVar('hour');
	    $day	= ($this->getRequestVar('day') === null || $this->getRequestVar('day') > 31 || $this->getRequestVar('day') < 1) ? null : $this->getRequestVar('day');
	    $month	= ($this->getRequestVar('month') === null || $this->getRequestVar('month') > 12 || $this->getRequestVar('month') < 1) ? null : $this->getRequestVar('month');
	    $year	= ($this->getRequestVar('year') === null || $this->getRequestVar('year') > date('Y') || $this->getRequestVar('year') < Config::logsFirstYear) ? null : $this->getRequestVar('year');

	    // If a time (hour) is given but no date (year, month, day), use the current date:
	    if($hour !== null) {
		    if($day === null)
			    $day = date('j');
		    if($month === null)
			    $month = date('n');
		    if($year === null)
			    $year = date('Y');
	    }

	    if($year === null) {
		    // No year given, so no period condition
	    } else if($month === null) {
		    // Define the given year as period:
		    $periodStart = mktime(0, 0, 0, 1, 1, $year);
		    // The last day in a month can be expressed by using 0 for the day of the next month:
		    $periodEnd = mktime(23, 59, 59, 13, 0, $year);
	    } else if($day === null) {
		    // Define the given month as period:
		    $periodStart = mktime(0, 0, 0, $month, 1, $year);
		    // The last day in a month can be expressed by using 0 for the day of the next month:
		    $periodEnd = mktime(23, 59, 59, $month+1, 0, $year);
	    } else if($hour === null){
		    // Define the given day as period:
		    $periodStart = mktime(0, 0, 0, $month, $day, $year);
		    $periodEnd = mktime(23, 59, 59, $month, $day, $year);
	    } else {
		    // Define the given hour as period:
		    $periodStart = mktime($hour, 0, 0, $month, $day, $year);
		    $periodEnd = mktime($hour, 59, 59, $month, $day, $year);
	    }

	    if(isset($periodStart))
		    $condition .= ' AND dateTime > \''.date('Y-m-d H:i:s', $periodStart).'\' AND dateTime <= \''.date('Y-m-d H:i:s', $periodEnd).'\'';

	    // Check the search condition:
	    if($this->getRequestVar('search')) {
		    if(strpos($this->getRequestVar('search'), 'ip=') === 0) {
			    // Search for messages with the given IP:
			    $ip = substr($this->getRequestVar('search'), 3);
			    $condition .= ' AND (ip = '.$this->db->makeSafe($this->ipToStorageFormat($ip)).')';
		    } else if(strpos($this->getRequestVar('search'), 'userID=') === 0) {
			    // Search for messages with the given userID:
			    $userID = substr($this->getRequestVar('search'), 7);
			    $condition .= ' AND (userID = '.$this->db->makeSafe($userID).')';
		    } else {
			    // Use the search value as regular expression on message text and username:
			    $condition .= ' AND (userName REGEXP '.$this->db->makeSafe($this->getRequestVar('search')).' OR text REGEXP '.$this->db->makeSafe($this->getRequestVar('search')).')';
		    }
	    }

	    // If no period or search condition is given, just monitor the last messages on the given channel:
	    if(!isset($periodStart) && !$this->getRequestVar('search')) {
		    $condition .= ' AND NOW() < DATE_ADD(dateTime, interval '.Config::logsRequestMessagesTimeDiff.' HOUR)';
	    }

	    return $condition;
    }

    function getLogsViewMessages() {
	    $sql = 'SELECT
				    id,
				    userID,
				    userName,
				    userRole,
				    channel AS channelID,
				    UNIX_TIMESTAMP(dateTime) AS timeStamp,
				    ip,
				    text,
				    msgInfo
			    FROM
				    '.$this->getDataBaseTable('messages').'
			    WHERE
				    '.$this->getLogsViewCondition().'
			    ORDER BY
				    id
                    DESC
			    LIMIT '.$this->getRequestVar('tmc').';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $rows = [];
	    // Add the messages in reverse order so it is ascending again:
	    while($row = $result->fetch()) {
            $rows[] = [
                'id' => (int)$row['id'],
                'uID' => (int)$row['userID'],
                'role' => (int)$row['userRole'],
                'cID' => (int)$row['channelID'],
                'name' => SChatEncoding::encodeSpecialChars($row['userName']),
                'text' => SChatEncoding::encodeSpecialChars($row['text']),
                'time' => $row['timeStamp'],
                'info' => json_decode($row['msgInfo'], true),
                'ip' => $this->ipFromStorageFormat($row['ip'])
            ];
	    }
	    $result->free();

	    return array_reverse($rows);
    }

    function getLogsViewJSONMessages() {
        $json = [];
        $json['infos'] = $this->getInfoMessages();
        $json['msgs'] = $this->getLogsViewMessages();
        return $json;
    }

    function purgeLogs() {
	    $sql = 'DELETE FROM
				    '.$this->getDataBaseTable('messages').'
			    WHERE
				    dateTime < DATE_SUB(NOW(), interval '.Config::logsPurgeTimeDiff.' DAY);';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function getInfoMessages($type=null) {
        if(!isset($this->_infoMessages)) {
            $this->_infoMessages = [];
        }
        if($type) {
            if(!isset($this->_infoMessages[$type])) {
                $this->_infoMessages[$type] = [];
            }
            return $this->_infoMessages[$type];
        } else {
            return $this->_infoMessages;
        }
    }

    function addInfoMessage($info, $type='error') {
	    if(!isset($this->_infoMessages)) {
		    $this->_infoMessages = [];
	    }
	    if(!isset($this->_infoMessages[$type])) {
		    $this->_infoMessages[$type] = [];
	    }
	    if(!in_array($info, $this->_infoMessages[$type])) {
		    array_push($this->_infoMessages[$type], $info);
	    }
    }

    function getRequestVars() {
	    return $this->_requestVars;
    }

    function getRequestVar($key) {
	    if($this->_requestVars && isset($this->_requestVars[$key])) {
		    return $this->_requestVars[$key];
	    }
	    return null;
    }

    function setRequestVar($key, $value) {
	    if(!$this->_requestVars) {
		    $this->_requestVars = [];
	    }
	    $this->_requestVars[$key] = $value;
    }

    function getOnlineUsersData($channelIDs=null, $key=null, $value=null) {
	    if($this->_onlineUsersData === null) {
		    $this->_onlineUsersData = [];

		    $sql = 'SELECT
					    userID,
					    userName,
					    userRole,
					    channel,
					    userInfo,
					    UNIX_TIMESTAMP(dateTime) AS timeStamp,
					    ip
				    FROM
					    '.$this->getDataBaseTable('online').'
				    ORDER BY
					    userName;';

		    // Create a new SQL query:
		    $result = $this->db->sqlQuery($sql);

		    // Stop if an error occurs:
		    if($result->error()) {
			    echo $result->getError();
			    die();
		    }

		    while($row = $result->fetch()) {
                $user = [
                    'id' => (int)$row['userID'],
                    'role' => (int)$row['userRole'],
                    'room' => (int)$row['channel'],
                    'time' => (int)$row['timeStamp'],
                    'name' => SChatEncoding::encodeSpecialChars($row['userName']),
                    'info' => json_decode($row['userInfo'], true),
                ];
                if ($this->getUserRole() >= SCHAT_MODERATOR){
                    $user['ip'] = $this->ipFromStorageFormat($row['ip']);
                }
                if ($this->_view == 'teaser'){
                    $user['roomName'] = array_key_exists((int)$row['channel'], Config::$channels) ? Config::$channels[(int)$row['channel']] : "Приватные комнаты";
                }
                $this->_onlineUsersData[] = $user;
		    }

		    $result->free();
	    }

	    if($channelIDs || $key) {
		    $onlineUsersData = [];
		    foreach($this->_onlineUsersData as $userData) {
			    if($channelIDs && !in_array($userData['room'], $channelIDs)) {
				    continue;
			    }
			    if($key) {
				    if(!isset($userData[$key])) {
					    return $onlineUsersData;
				    }
				    if($value !== null) {
					    if($userData[$key] == $value) {
						    array_push($onlineUsersData, $userData);
					    } else {
						    continue;
					    }
				    } else {
					    array_push($onlineUsersData, $userData[$key]);
				    }
			    } else {
				    array_push($onlineUsersData, $userData);
			    }
		    }
		    return $onlineUsersData;
	    }

	    return $this->_onlineUsersData;
    }

    function removeUserFromOnlineUsersData($userID=null) {
	    if(!$this->_onlineUsersData) {
		    return;
	    }
	    $userID = ($userID === null) ? $this->getUserID() : $userID;
	    for($i=0; $i<count($this->_onlineUsersData); $i++) {
		    if($this->_onlineUsersData[$i]['id'] == $userID) {
			    array_splice($this->_onlineUsersData, $i, 1);
			    break;
		    }
	    }
    }

    function resetOnlineUsersData() {
	    $this->_onlineUsersData = null;
    }

    function getOnlineUsers($channelIDs=null) {
	    return $this->getOnlineUsersData($channelIDs, 'name');
    }

    function getOnlineUserIDs($channelIDs=null) {
	    return $this->getOnlineUsersData($channelIDs, 'id');
    }

    function destroySession() {
	    if($this->_sessionNew) {
		    // Delete all session variables:
		    $_SESSION = [];

		    // Delete the session cookie:
		    if (isset($_COOKIE[session_name()])) {
			    setcookie(
				    session_name(),
				    '',
				    time()-42000,
				    Config::sessionCookiePath,
				    Config::sessionCookieDomain,
				    Config::$sessionCookieSecure
			    );
		    }

		    // Destroy the session:
		    session_destroy();
	    } else {
		    // Unset all session variables starting with the sessionKeyPrefix:
		    foreach($_SESSION as $key=>$value) {
			    if(strpos($key, Config::sessionKeyPrefix) === 0) {
				    unset($_SESSION[$key]);
			    }
		    }
	    }
    }

    function regenerateSessionID() {
	    if($this->_sessionNew) {
		    // Regenerate session id:
		    @session_regenerate_id(true);
	    }
    }

    function getSessionVar($key, $prefix=null) {
	    if($prefix === null)
		    $prefix = Config::sessionKeyPrefix;

	    // Return the session value if existing:
	    if(isset($_SESSION[$prefix.$key]))
		    return $_SESSION[$prefix.$key];
	    else
		    return null;
    }

    function setSessionVar($key, $value, $prefix=null) {
	    if($prefix === null)
		    $prefix = Config::sessionKeyPrefix;

	    // Set the session value:
	    $_SESSION[$prefix.$key] = $value;
    }

    function getSessionIP() {
	    return $this->getSessionVar('IP');
    }

    function setSessionIP($ip) {
	    $this->setSessionVar('IP', $ip);
    }

    function getQueryUserName() {
	    return $this->getSessionVar('QueryUserName');
    }

    function setQueryUserName($userName) {
	    $this->setSessionVar('QueryUserName', $userName);
    }

    function getInvitations() {
	    if($this->_invitations === null) {
		    $this->_invitations = [];

		    $sql = 'SELECT
					    channel
				    FROM
					    '.$this->getDataBaseTable('invitations').'
				    WHERE
					    userID='.$this->db->makeSafe($this->getUserID()).'
					    AND
					    DATE_SUB(NOW(), interval 1 DAY) < dateTime;';

		    // Create a new SQL query:
		    $result = $this->db->sqlQuery($sql);

		    // Stop if an error occurs:
		    if($result->error()) {
			    echo $result->getError();
			    die();
		    }

		    while($row = $result->fetch()) {
			    array_push($this->_invitations, $row['channel']);
		    }

		    $result->free();
	    }
	    return $this->_invitations;
    }

    function removeExpiredInvitations() {
	    $sql = 'DELETE FROM
				    '.$this->getDataBaseTable('invitations').'
			    WHERE
				    DATE_SUB(NOW(), interval 1 DAY) > dateTime;';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function addInvitation($userID, $channelID=null) {
	    $this->removeExpiredInvitations();

	    $channelID = ($channelID === null) ? $this->getChannel() : $channelID;

	    $sql = 'INSERT INTO '.$this->getDataBaseTable('invitations').'(
				    userID,
				    channel,
				    dateTime
			    )
			    VALUES (
				    '.$this->db->makeSafe($userID).',
				    '.$this->db->makeSafe($channelID).',
				    NOW()
			    );';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function removeInvitation($userID, $channelID=null) {
	    $channelID = ($channelID === null) ? $this->getChannel() : $channelID;

	    $sql = 'DELETE FROM
				    '.$this->getDataBaseTable('invitations').'
			    WHERE
				    userID='.$this->db->makeSafe($userID).'
				    AND
				    channel='.$this->db->makeSafe($channelID).';';

	    // Create a new SQL query:
	    $result = $this->db->sqlQuery($sql);

	    // Stop if an error occurs:
	    if($result->error()) {
		    echo $result->getError();
		    die();
	    }
    }

    function getReactions($msgId){
        $userId = $this->getUserID();
        $reactions = [];

        foreach (Config::$reactionTypes as $reactionType)
        {
            $reactions[$reactionType] = ["count" => 0, "users" => []];
        }

        if (!is_null($userId)){
            $sql = 'SELECT * FROM '.$this->getDataBaseTable('reactions').'
                    WHERE msgId = '.$msgId;

            $result = $this->db->sqlQuery($sql);

		    if($result->error()) {
                continue;
		    }

		    while($row = $result->fetch()) {
                $reaction = $reactions[$row["reactionType"]];
                $reaction["count"] = $reaction["count"] + 1;
                $reaction["users"][] = (int)$row["userId"];
                $reactions[$row["reactionType"]] = $reaction;
		    }

		    $result->free();
        }
        return $reactions;
    }

    function addReaction($msgId, $reactionType){
        if (!in_array($reactionType, Config::$reactionTypes))
            return;

        $this->removeReactions($msgId, Config::$conflictedReactions[$reactionType]);

        $userId = $this->getUserID();
        if (!is_null($reactionType)
            && !is_null($userId)){
            $sql = 'INSERT IGNORE INTO '.$this->getDataBaseTable('reactions').'(
				    msgId,
				    userId,
				    reactionType
			    )
			    VALUES (
				    '.$this->db->makeSafe($msgId).',
				    '.$this->db->makeSafe($userId).',
				    '.$this->db->makeSafe($reactionType).'
			    );';

            $result = $this->db->sqlQuery($sql);

            if($result->error()) {
                echo $result->getError();
                die();
            }
        }
    }

    function removeReaction($msgId, $reactionType){
        $this->removeReactions($msgId, [$reactionType]);
    }

    function removeReactions($msgId, $reactionTypes){
        if (count($reactionTypes) == 0)
            return;

        $reactionCond = "(";
        foreach ($reactionTypes as $reactionType){
            if (in_array($reactionType, Config::$reactionTypes))
            {
                if ($reactionCond != "(")
                    $reactionCond .= " OR ";
                $reactionCond .= "reactionType=".$this->db->makeSafe($reactionType);
            }
        }
        $reactionCond .= ")";

        $userId = $this->getUserID();
        if (!is_null($userId)){
            $sql = 'DELETE IGNORE FROM '.$this->getDataBaseTable('reactions').'
			        WHERE msgId='.$this->db->makeSafe($msgId).'
				        AND userId='.$this->db->makeSafe($userId).'
				        AND '.$reactionCond.';';

            $result = $this->db->sqlQuery($sql);

            if($result->error()) {
                echo $result->getError();
                die();
            }
        }
    }

    function getUserID() {
	    return $this->getSessionVar('UserID');
    }

    function setUserID($id) {
	    $this->setSessionVar('UserID', $id);
    }

    function getUserName() {
	    return $this->getSessionVar('UserName');
    }

    function setUserName($name) {
	    $this->setSessionVar('UserName', $name);
    }

    function getLoginUserName() {
	    return $this->getSessionVar('LoginUserName');
    }

    function setLoginUserName($name) {
	    $this->setSessionVar('LoginUserName', $name);
    }

    function getUserRole() {
	    return $this->getSessionVar('UserRole');
    }

    function setUserRole($role) {
	    $this->setSessionVar('UserRole', $role);
    }

    function getChannel() {
	    return $this->getSessionVar('Channel');
    }

    function getUserInfo() {
        return $this->getSessionVar('userInfo');
    }

    function setUserInfo($id) {
        $this->setSessionVar('userInfo', $id);
    }

    function setChannel($channel) {
	    $this->setSessionVar('Channel', $channel);

	    // Save the channel enter timestamp:
	    $this->setChannelEnterTimeStamp(time());
    }

    function isLoggedIn() {
	    return (bool)$this->getSessionVar('LoggedIn');
    }

    function setLoggedIn($bool) {
	    $this->setSessionVar('LoggedIn', $bool);
    }

    function getLoginTimeStamp() {
	    return $this->getSessionVar('LoginTimeStamp');
    }

    function setLoginTimeStamp($time) {
	    $this->setSessionVar('LoginTimeStamp', $time);
    }

    function getChannelEnterTimeStamp() {
	    return $this->getSessionVar('ChannelEnterTimeStamp');
    }

    function setChannelEnterTimeStamp($time) {
	    $this->setSessionVar('ChannelEnterTimeStamp', $time);
    }

    function getStatusUpdateTimeStamp() {
	    return $this->getSessionVar('StatusUpdateTimeStamp');
    }

    function setStatusUpdateTimeStamp($time) {
	    $this->setSessionVar('StatusUpdateTimeStamp', $time);
    }

    function getInactiveCheckTimeStamp() {
	    return $this->getSessionVar('InactiveCheckTimeStamp');
    }

    function setInactiveCheckTimeStamp($time) {
	    $this->setSessionVar('InactiveCheckTimeStamp', $time);
    }

    function getInsertedMessagesRate() {
	    return $this->getSessionVar('InsertedMessagesRate');
    }

    function setInsertedMessagesRate($rate) {
	    $this->setSessionVar('InsertedMessagesRate', $rate);
    }

    function getInsertedMessagesRateTimeStamp() {
	    return $this->getSessionVar('InsertedMessagesRateTimeStamp');
    }

    function setInsertedMessagesRateTimeStamp($time) {
	    $this->setSessionVar('InsertedMessagesRateTimeStamp', $time);
    }

    function getLangCode() {
	    // Get the langCode from request or cookie:
	    $langCodeCookie = isset($_COOKIE[Config::sessionName.'_lang']) ? $_COOKIE[Config::sessionName.'_lang'] : null;
	    $langCode = $this->getRequestVar('lang') ? $this->getRequestVar('lang') : $langCodeCookie;
	    // Check if the langCode is valid:
	    if(!in_array($langCode, Config::$langAvailable)) {
		    // Determine the user language:
		    $language = new SChatLanguage(Config::$langAvailable, Config::langDefault);
		    $langCode = $language->getLangCode();
	    }
	    return $langCode;
    }

    function setLangCodeCookie() {
	    setcookie(
		    Config::sessionName.'_lang',
		    $this->getLangCode(),
		    time()+60*60*24*Config::sessionCookieLifeTime,
		    Config::sessionCookiePath,
		    Config::sessionCookieDomain,
		    Config::$sessionCookieSecure
	    );
    }

    function removeUnsafeCharacters($str) {
	    // Remove NO-WS-CTL, non-whitespace control characters (RFC 2822), decimal 1–8, 11–12, 14–31, and 127:
	    return SChatEncoding::removeUnsafeCharacters($str);
    }

    function trimMessageText($text) {
	    return $this->trimString($text,  Config::messageTextMaxLength);
    }

    function trimUserName($userName) {
	    return $this->trimString($userName,  Config::userNameMaxLength, true, true);
    }

    function trimChannelName($channelName) {
	    return $this->trimString($channelName,  null, true, true);
    }

    function trimString($str, $maxLength=null, $replaceWhitespace=false, $decodeEntities=false, $htmlEntitiesMap=null) {
	    $str = trim($this->removeUnsafeCharacters($str));

	    if($replaceWhitespace) {
            // Replace any whitespace in the userName with the underscore "_":
            $str = preg_replace('/\s/u',  /*html_entity_decode('&nbsp;')*/ '_', $str);
	    }

	    if($decodeEntities) {
		    // Decode entities:
		    $str = SChatEncoding::decodeEntities($str, 'UTF-8', $htmlEntitiesMap);
	    }

	    if($maxLength) {
		    // Cut the string to the allowed length:
		    $str = SChatString::subString($str, 0, $maxLength);
	    }

	    return $str;
    }

    function ipToStorageFormat($ip) {
	    if(function_exists('inet_pton')) {
		    // ipv4 & ipv6:
		    return @inet_pton($ip);
	    }
	    // Only ipv4:
	    return @pack('N',@ip2long($ip));
    }

    function ipFromStorageFormat($ip) {
	    if(function_exists('inet_ntop')) {
		    // ipv4 & ipv6:
		    return @inet_ntop($ip);
	    }
	    // Only ipv4:
	    $unpacked = @unpack('Nlong',$ip);
	    if(isset($unpacked['long'])) {
		    return @long2ip($unpacked['long']);
	    }
	    return null;
    }

    function getLang($key=null) {
	    if(!$this->_lang) {
		    // Include the language file:
		    $lang = null;
		    require(SCHAT_PATH.DS.'lib'.DS.'lang'.DS.$this->getLangCode().'.php');
		    $this->_lang = &$lang;
	    }
	    if($key === null)
		    return $this->_lang;
	    if(isset($this->_lang[$key]))
		    return $this->_lang[$key];
	    return null;
    }

    function getChatURL() {
	    if(defined('SCHAT_URL')) {
		    return SCHAT_URL;
	    }

	    return
		    (isset($_SERVER['HTTPS']) ? 'https://' : 'http://').
		    (isset($_SERVER['REMOTE_USER']) ? $_SERVER['REMOTE_USER'].'@' : '').
		    (isset($_SERVER['HTTP_HOST']) ? $_SERVER['HTTP_HOST'] : ($_SERVER['SERVER_NAME'].
		    (isset($_SERVER['HTTPS']) && $_SERVER['SERVER_PORT'] == 443 || $_SERVER['SERVER_PORT'] == 80 ? '' : ':'.$_SERVER['SERVER_PORT']))).
		    substr($_SERVER['SCRIPT_NAME'],0, strrpos($_SERVER['SCRIPT_NAME'], '/')+1);
    }

    function getIDFromName($userName) {
	    $userDataArray = $this->getOnlineUsersData(null,'name',$userName);
	    if($userDataArray && isset($userDataArray[0])) {
		    return $userDataArray[0]['id'];
	    }
	    return null;
    }

    function getNameFromID($userID) {
	    $userDataArray = $this->getOnlineUsersData(null,'id',$userID);
	    if($userDataArray && isset($userDataArray[0])) {
		    return $userDataArray[0]['name'];
	    }
	    return null;
    }

    function getChannelFromID($userID) {
	    $userDataArray = $this->getOnlineUsersData(null,'id',$userID);
	    if($userDataArray && isset($userDataArray[0])) {
		    return $userDataArray[0]['room'];
	    }
	    return null;
    }

    function getIPFromID($userID) {
	    $userDataArray = $this->getOnlineUsersData(null,'id',$userID);
	    if($userDataArray && isset($userDataArray[0])) {
		    return $userDataArray[0]['ip'];
	    }
	    return null;
    }

    function getRoleFromID($userID) {
	    $userDataArray = $this->getOnlineUsersData(null,'id',$userID);
	    if($userDataArray && isset($userDataArray[0])) {
		    return $userDataArray[0]['role'];
	    }
	    return null;
    }

    function getChannelNames() {
	    return array_flip($this->getChannels());
    }

    function getChannelIDFromChannelName($channelName) {
	    if(!$channelName)
		    return null;
	    $channels = $this->getAllChannels();
	    if(array_key_exists($channelName,$channels)) {
		    return $channels[$channelName];
	    }
	    $channelID = null;
	    // Check if the requested channel is the own private channel:
	    if($channelName == $this->getPrivateChannelName()) {
		    return $this->getPrivateChannelID();
	    }
	    // Try to retrieve a private room ID:
	    $strlenChannelName = SChatString::stringLength($channelName);
	    $strlenPrefix = SChatString::stringLength(Config::privateChannelPrefix);
	    $strlenSuffix = SChatString::stringLength(Config::privateChannelSuffix);
	    if(SChatString::subString($channelName,0,$strlenPrefix) == Config::privateChannelPrefix
		    && SChatString::subString($channelName,$strlenChannelName-$strlenSuffix) == Config::privateChannelSuffix) {
		    $userName = SChatString::subString(
						    $channelName,
						    $strlenPrefix,
						    $strlenChannelName-($strlenPrefix+$strlenSuffix)
					    );
		    $userID = $this->getIDFromName($userName);
		    if($userID !== null) {
			    $channelID = $this->getPrivateChannelID($userID);
		    }
	    }
	    return $channelID;
    }

    function getChannelNameFromChannelID($channelID) {
	    foreach($this->getAllChannels() as $key=>$value) {
		    if($value == $channelID) {
			    return $key;
		    }
	    }
	    // Try to retrieve a private room name:
	    if($channelID == $this->getPrivateChannelID()) {
		    return $this->getPrivateChannelName();
	    }
	    $userName = $this->getNameFromID($channelID-Config::privateChannelDiff);
	    if($userName === null) {
		    return null;
	    }
	    return $this->getPrivateChannelName($userName);
    }

    function getChannelName() {
	    return $this->getChannelNameFromChannelID($this->getChannel());
    }

    function getPrivateChannelName($userName=null) {
	    if($userName === null) {
		    $userName = $this->getUserName();
	    }
	    return Config::privateChannelPrefix.$userName.Config::privateChannelSuffix;
    }

    function getPrivateChannelID($userID=null) {
	    if($userID === null) {
		    $userID = $this->getUserID();
	    }
	    return $userID + Config::privateChannelDiff;
    }

    function getPrivateMessageID($userID=null) {
	    if($userID === null) {
		    $userID = $this->getUserID();
	    }
	    return $userID + Config::privateMessageDiff;
    }

    function isUserOnline($userID=null) {
	    $userID = ($userID === null) ? $this->getUserID() : $userID;
	    $userDataArray = $this->getOnlineUsersData(null,'id',$userID);
	    return $userDataArray && count($userDataArray) > 0;
    }

    function isUserNameInUse($userName=null) {
	    $userName = ($userName === null) ? $this->getUserName() : $userName;
	    $userDataArray = $this->getOnlineUsersData(null,'name',$userName);
	    return $userDataArray && count($userDataArray) > 0;
    }

    function isUserBanned($userName, $userID=null, $ip=null) {
        if($userID !== null) {
            $bannedUserDataArray = $this->getBannedUsersData('userID',$userID);
            if($bannedUserDataArray && isset($bannedUserDataArray[0])) {
                return true;
            }
        }
        if($ip !== null) {
            $bannedUserDataArray = $this->getBannedUsersData('ip',$ip);
            if($bannedUserDataArray && isset($bannedUserDataArray[0])) {
                return true;
            }
        }
        $bannedUserDataArray = $this->getBannedUsersData('userName',$userName);
        return $bannedUserDataArray && isset($bannedUserDataArray[0]);
    }

    function validateChannel($channelID) {
        return ($channelID !== null && (in_array($channelID, $this->getChannels())
            || $channelID == $this->getPrivateChannelID()
            || in_array($channelID, $this->getInvitations())));
    }

    // Override:
    // Returns an associative array containing userName, userID and userRole
    // Returns null if login is invalid
    function getValidLoginUserData() {
        // Check if we have a valid registered user:
        if(false) {
            // Here is the place to check user authentication
        } else {
            $this->logout();
        }
    }

    // Override:
    // Store the channels the current user has access to
    // Make sure channel names don't contain any whitespace
    function &getChannels() {
        if($this->_channels === null) {
            $this->_channels = $this->getAllChannels();
        }
        return $this->_channels;
    }

    // Override:
    // Store all existing channels
    // Make sure channel names don't contain any whitespace
    function &getAllChannels() {
        if($this->_allChannels === null) {
            $this->_allChannels = array_flip(Config::$channels);
        }
        return $this->_allChannels;
    }

    function getUserMsgColor(){
        $color = $this->getSessionVar("msgColor");
        if ($color == null){
            $color = $this->loadUserMsgColor();
            if ($color == null || count($color) == 0)
                $color = ["#000000"];

            $this->setSessionVar("msgColor", $color);
        }
        return $color;
    }

    function setUserMsgColor($color){
        $this->saveUserMsgColor($color);
        $this->setSessionVar('msgColor', $color);
    }

    function getUserNickColor(){
        $color = $this->getSessionVar("nickColor");
        if ($color == null){
            $color = $this->loadUserNickColor();
            if ($color == null || count($color) == 0)
                $color = ["#000000"];

            $this->setSessionVar("nickColor", $color);
        }
        if (count($color) == 0)
            return ["#000000"];
        return $color;
    }

    function setUserNickColor($color){
        $this->saveUserNickColor($color);
        $this->setSessionVar('nickColor', $color);
    }

    function loadUserNickColor(){
        return ["#000000"];
    }

    function saveUserNickColor($color){

    }

    function loadUserMsgColor(){
        return ["#000000"];
    }

    function saveUserMsgColor($color){

    }

    function incrementMessageCount(){

    }

    function appendMinutesToUserTimeInChat($minutes){

    }

    function getUsersCount() {

    }

    /*
     *  must return array of users
     *  user: {
     *      "id"            : number,
     *      "username"      : string,
     *      "gender"        : "m", "f", "n",
     *      "tim"           : string,
     *      "registerDate"  : milliseconds,
     *      "lastvisitDate" : milliseconds,
     *      "msgCount"      : number,
     *      "minutesInChat" : minutes
     *  }
     */
    function getUsersTopTable($skip, $count, $sortBy, $asc){
        return [];
    }

    function getUsersTopJSONTable(){
        $skip = $this->getRequestVar('skip');
        $count = $this->getRequestVar('count');
        $sortBy = $this->getRequestVar('sortBy');
        $sortType = $this->getRequestVar('sortType');
        $totalCount = $this->getUsersCount();
        return [
            "skip" => $skip,
            "coung" => $count,
            "sortBy" => $count,
            "sortType" => $sortType,
            "totalCount" => $totalCount,
            "users" => $this->getUsersTopTable($skip, $count, $sortBy, $sortType)
        ];
    }
}
?>