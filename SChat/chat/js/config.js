/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

// Ajax Chat config parameters:
var ajaxChatConfig = {

	// The channelID of the channel to enter on login (the loginChannelName is used if set to null):
	loginChannelID: null,
	// The channelName of the channel to enter on login (the default channel is used if set to null):
	loginChannelName: null,

	// The time in ms between update calls to retrieve new chat messages:
	timerRate: 2000,

	// The URL to retrieve the XML chat messages (must at least contain one parameter):
	ajaxURL: './?ajax=true',
	// The base URL of the chat directory, used to retrieve media files (images, sound files, etc.):
	baseURL: './',

	// A regular expression for allowed source URL's for media content (e.g. images displayed inline);
	regExpMediaUrl: '^((http)|(https)):\\/\\/',

	// If set to false the chat update is delayed until the event defined in ajaxChat.setStartChatHandler():
	startChatOnLoad: true,

	// Defines the IDs of DOM nodes accessed by the chat:
	domIDs: {
		// The ID of the chat messages list:
		chatList: 'chatList',
		// The ID of the online users list:
		onlineList: 'onlineList',
		// The ID of the message text input field:
		inputField: 'inputField',
		// The ID of the message text length counter:
		messageLengthCounter: 'messageLengthCounter',
		// The ID of the channel selection:
		channelSelection: 'channelSelection',
		// The ID of the style selection:
		styleSelection: 'styleSelection',
		// The ID of the emoticons container:
		emoticonsContainer: 'emoticonsContainer',
		// The ID of the flash interface container:
		flashInterfaceContainer: 'flashInterfaceContainer',
		// The ID of the status icon:
		statusIcon: 'statusIconContainer'
	},

	// Defines the settings which can be modified by users:
	settings: {
		// Defines if BBCode tags are replaced with the associated HTML code tags:
		bbCode: true,

		msgColors: true,
                nickColors: true,
                gradiens: true,
                
		// Defines if hyperlinks are made clickable:
		hyperLinks: true,
		// Defines if emoticon codes are replaced with their associated images:
		emoticons: true,

		// Defines if the chat list scrolls automatically to display the latest messages:
		autoScroll: true,

		// The default font color, uses the page default font color if set to null:
		fontColor: null,
                nickColor: null,
                
		// Defines if sounds are played:
		audio: true,
		// Defines the sound volume (0.0 = mute, 1.0 = max):
		audioVolume: 1.0,

		// Defines the sound that is played when normal messages are reveived:
		soundReceive: 'sound_1',
		// Defines the sound that is played on sending normal messages:
		soundSend:    'sound_2',
		// Defines the sound that is played on channel enter or login:
		soundEnter:   'sound_3',
		// Defines the sound that is played on channel leave or logout:
		soundLeave:   'sound_4',
		// Defines the sound that is played on chatBot messages:
		soundChatBot: 'sound_5',
		// Defines the sound that is played on error messages:
		soundError:   'sound_6',
		// Defines the sound that is played when private messages are received:
		soundPrivate: 'sound_7'
	},

	// Defines a list of settings which are not to be stored in a session cookie:
	nonPersistentSettings: [],

	// Defines the list of allowed BBCodes:
	bbCodeTags:[
		'b',
		'i',
		's',
		'quote',
                'tgw'
	],

	// Defines the list of allowed color codes:
	colorCodes: ['000000','494949','717171','41166f','53377a','4b0082',
                    '6835ba','9739b7','810aa9','a70ddb','735184','a60073',
                    'ba0671','bb438a','71001e','a5003c','b80642','c32025',
                    'fc2847', 'dc143c', 'cc3737','e3691e','c06411','eb9d58',
                    'c4b122','a6b929','929e32','609c0c','67b64b','00af4c',
                    '009d7a','03c89c','00aab7','017e90','0687c1','01adeb',
                    '007fff','0078b7','0558a6','1065dc','0d14e5','00416a'],

	// Defines the list of allowed emoticon codes:
	emoticonCodes: [
    ':acute',
    ':aggressive',
    ':bad',
    ':biggrin',
    ':blum1',
    ':blush',
    ':bomb',
    ':boredom',
    ':bye',
    ':clapping',
    ':congratulate',
    ':cool',
    ':cray',
    ':dance',
    ':diablo',
    ':drinks',
    ':empathy',
    ':flag_of_truce',
    ':fool',
    ':good',
    ':greeting',
    ':help',
    ':hi',
    ':hmm',
    ':i_am_so_happy',
    ':lol',
    ':mad',
    ':mocking',
    ':music',
    ':nea',
    ':negative',
    ':new_russian',
    ':nyam1',
    ':ok',
    ':pardon',
    ':playboy',
    ':pleasantry',
    ':praising',
    ':rofl',
    ':rolleyes',
    ':sad',
    ':scare',
    ':scratch_one-s_head',
    ':secret',
    ':shok',
    ':shout',
    ':smile',
    ':sorry',
    ':stop',
    ':timeout',
    ':unknown',
    ':wink',
    ':yahoo',
    ':c_01',
    ':c_02',
    ':c_03',
    ':c_04',
    ':c_05',
    ':c_06',
    ':c_07',
    ':c_08',
    ':c_09',
    ':c_10',
    ':c_11',
    ':c_12',
    ':c_13',
    ':c_14',
    ':c_15',
    ':p_01',
    ':p_02',
    ':p_03',
    ':p_04',
    ':p_05',
    ':p_06',
    ':p_07',
    ':p_08',
    ':p_09',
    ':p_10',
    ':p_11',
    ':p_12',
    ':p_13',
    ':p_14',
    ':p_15',
    ':p_16',
    ':p_17',
    ':p_18',
    ':p_19',
    ':p_20',
    ':p_21',
    ':p_22',
    ':p_23',
    ':p_24'
  ],

  // Defines the list of emoticon files associated with the emoticon codes:
  emoticonFiles: [
    'acute.gif ',
    'aggressive.gif',
    'bad.gif',
    'biggrin.gif',
    'blum1.gif',
    'blush.gif',
    'bomb.gif',
    'boredom.gif',
    'bye.gif',
    'clapping.gif',
    'congratulate.gif',
    'cool.gif',
    'cray.gif',
    'dance.gif',
    'diablo.gif',
    'drinks.gif',
    'empathy.gif',
    'flag_of_truce.gif',
    'fool.gif',
    'good.gif',
    'greeting.gif',
    'help.gif',
    'hi.gif',
    'hmm.gif',
    'i_am_so_happy.gif',
    'lol.gif',
    'mad.gif',
    'mocking.gif',
    'music.gif',
    'nea.gif',
    'negative.gif',
    'new_russian.gif',
    'nyam1.gif',
    'ok.gif',
    'pardon.gif',
    'playboy.gif',
    'pleasantry.gif',
    'praising.gif',
    'rofl.gif',
    'rolleyes.gif',
    'sad.gif',
    'scare.gif',
    'scratch_one-s_head.gif',
    'secret.gif',
    'shok.gif',
    'shout.gif',
    'smile.gif',
    'sorry.gif',
    'stop.gif',
    'timeout.gif',
    'unknown.gif',
    'wink.gif',
    'yahoo.gif',
    'c_01.png',
    'c_02.png',
    'c_03.png',
    'c_04.png',
    'c_05.png',
    'c_06.png',
    'c_07.png',
    'c_08.png',
    'c_09.png',
    'c_10.png',
    'c_11.png',
    'c_12.png',
    'c_13.png',
    'c_14.png',
    'c_15.png',
    'p_01.png',
    'p_02.png',
    'p_03.png',
    'p_04.png',
    'p_05.png',
    'p_06.png',
    'p_07.png',
    'p_08.png',
    'p_09.png',
    'p_10.png',
    'p_11.png',
    'p_12.png',
    'p_13.png',
    'p_14.png',
    'p_15.png',
    'p_16.png',
    'p_17.png',
    'p_18.png',
    'p_19.png',
    'p_20.png',
    'p_21.png',
    'p_22.png',
    'p_23.png',
    'p_24.png'
  ],

	// Defines the available sounds loaded on chat start:
	soundFiles: {
		sound_1: 'sound_1.mp3',
		sound_2: 'sound_2.mp3',
		sound_3: 'sound_3.mp3',
		sound_4: 'sound_4.mp3',
		sound_5: 'sound_5.mp3',
		sound_6: 'sound_6.mp3',
		sound_7: 'sound_7.mp3',
		sound_8: 'sound_8.mp3'
	},


	// Once users have been logged in, the following values are overridden by those in config.php.
	// You should set these to be the same as the ones in config.php to avoid confusion.

	// Session identification, used for style and setting cookies:
	sessionName: 'ajax_chat',

	// The time in days until the style and setting cookies expire:
	cookieExpiration: 365,
	// The path of the cookies, '/' allows to read the cookies from all directories:
	cookiePath: '/',
	// The domain of the cookies, defaults to the hostname of the server if set to null:
	cookieDomain: null,
	// If enabled, cookies must be sent over secure (SSL/TLS encrypted) connections:
	cookieSecure: null,

	// The name of the chat bot:
	chatBotName: '������',
	// The userID of the chat bot:
	chatBotID: 2147483647,

	// Allow/Disallow registered users to delete their own messages:
	allowUserMessageDelete: true,

	// Minutes until a user is declared inactive (last status update) - the minimum is 2 minutes:
	inactiveTimeout: 2,

	// UserID plus this value are private channels (this is also the max userID and max channelID):
	privateChannelDiff: 500000000,
	// UserID plus this value are used for private messages:
	privateMessageDiff: 1000000000,

	// Defines if login/logout and channel enter/leave are displayed:
	showChannelMessages: true,

	// Max messageText length:
	messageTextMaxLength: 1040,

	// Defines if the socket server is enabled:
	socketServerEnabled: false,
	// Defines the hostname of the socket server used to connect from client side:
	socketServerHost: 'localhost',
	// Defines the port of the socket server:
	socketServerPort: 1935,
	// This ID can be used to distinguish between different chat installations using the same socket server:
	socketServerChatID: 0,

	// Debug allows console logging or alerts on caught errors - false/0 = no debug, true/1/2 = console log, 2 = alerts
	debug: false

};