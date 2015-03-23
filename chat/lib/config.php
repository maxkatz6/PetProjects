<?php
// Define AJAX Chat user roles:
define('SCHAT_BANNED',    6);
define('SCHAT_CHATBOT',   4);
define('SCHAT_ADMIN',   3);
define('SCHAT_MODERATOR', 2);
define('SCHAT_USER',    1);

class Config{
public static $channels = array(
0 => 'Общая',
9 => 'Диванная',
10 => 'Альфа',
11 => 'Бета',
12 => 'Гамма',
13 => 'Дельта',
14 => 'Админы');

// Database connection values:
public static  $dbConnection = array(
/*'host' => 'localhost', 
'user' => 'root',
'pass' => '',
'name' => 'chat',
'link' => null*/);

// Available languages:
public static  $langAvailable = array(
  'ar','bg','ca','cy','cz','da','de','el','en','es','et','fa','fi','fr','gl','he','hr','hu','in','it','ja','ka','kr','mk','nl','nl-be','no','pl','pt-br','pt-pt','ro','ru','sk','sl','sr','sv','th','tr','uk','zh','zh-tw'
);
// Default language:
const langDefault = 'ru';
// Language names (each languge code in available languages must have a display name assigned here):
public static $langNames = array(
  'ar'=>'عربي', 'bg'=>'Български', 'ca'=>'Català', 'cy'=>'Cymraeg', 'cz'=>'Česky', 'da'=>'Dansk', 'de'=>'Deutsch', 'el'=>'Ελληνικα', 'en'=>'English',
  'es'=>'Español', 'et'=>'Eesti', 'fa'=>'فارسی', 'fi'=>'Suomi', 'fr'=>'Français', 'gl'=>'Galego', 'he'=>'עברית', 'hr' => 'Hrvatski', 'hu' => 'Magyar', 'in'=>'Bahasa Indonesia', 'it'=>'Italiano',
  'ja'=>'日本語','ka'=>'ქართული','kr'=>'한 글','mk'=>'Македонски', 'nl'=>'Nederlands', 'nl-be'=>'Nederlands (België)', 'no'=>'Norsk', 'pl'=> 'Polski', 'pt-br'=>'Português (Brasil)', 'pt-pt'=>'Português (Portugal)',
  'ro'=>'România', 'ru'=>'Русский', 'sk'=> 'Slovenčina', 'sl'=>'Slovensko', 'sr'=>'Srpski', 'sv'=> 'Svenska', 'th'=>'&#x0e20;&#x0e32;&#x0e29;&#x0e32;&#x0e44;&#x0e17;&#x0e22;',
  'tr'=>'Türkçe', 'uk'=>'Українська', 'zh'=>'中文 (简体)', 'zh-tw'=>'中文 (繁體)'
);

// Available styles:
public static $styleAvailable = array('beige','grey','Oxygen','Lithium','Sulfur','prosilver','Core','MyBB','vBulletin','XenForo');
// Default style:
const styleDefault = 'prosilver';

// The encoding of the data source, like userNames and channelNames:
const sourceEncoding = 'UTF-8';

// Session name used to identify the session cookie:
const sessionName = 's_chat';
// Prefix added to every session key:
const sessionKeyPrefix = 'sChat';
// The lifetime of the language, style and setting cookies in days:
const sessionCookieLifeTime = 365;
// The path of the cookies, '/' allows to read the cookies from all directories:
const sessionCookiePath = '/';
// The domain of the cookies, defaults to the hostname of the server if set to null:
const sessionCookieDomain = null;
// If enabled, cookies must be sent over secure (SSL/TLS encrypted) connections:
const sessionCookieSecure = null;

// Default channelName used together with the defaultChannelID if no channel with this ID exists:
const defaultChannelName = 'Общая';
// ChannelID used when no channel is given:
const defaultChannelID = 0;

// UserID plus this value are private channels (this is also the max userID and max channelID):
const privateChannelDiff = 500000000;
// UserID plus this value are used for private messages:
const privateMessageDiff = 1000000000;

// Enable/Disable private Channels:
const allowPrivateChannels = true;

// Private channels should be distinguished by either a prefix or a suffix or both (no whitespace):
const privateChannelPrefix = '[';
// Private channels should be distinguished by either a prefix or a suffix or both (no whitespace):
const privateChannelSuffix = ']';

const forceAutoLogin = true;

// Defines if login/logout and channel enter/leave are displayed:
const showChannelMessages = true;

// Defines the timezone offset in seconds (-12*60*60 to 12*60*60) - if null, the server timezone is used:
const timeZoneOffset = null;
// Defines the hour of the day the chat is opened (0 - closingHour):
const openingHour = 0;
// Defines the hour of the day the chat is closed (openingHour - 24):
const closingHour = 24;

// Allow/Disallow users to change their userName (Nickname):
const allowNickChange = true;
// Changed userNames should be distinguished by either a prefix or a suffix or both (no whitespace):
const changedNickPrefix = '(';
// Changed userNames should be distinguished by either a prefix or a suffix or both (no whitespace):
const changedNickSuffix = ')';

// Allow/Disallow registered users to delete their own messages:
const allowUserMessageDelete = true;

// The userID used for ChatBot messages:
const chatBotID = 2147483647;
// The userName used for ChatBot messages
const chatBotName = 'Сервер';

// Minutes until a user is declared inactive (last status update) - the minimum is 2 minutes:
const inactiveTimeout = 2;
// Interval in minutes to check for inactive users:
const inactiveCheckInterval = 2;

// Defines if messages are shown which have been sent before the user entered the channel:
const requestMessagesPriorChannelEnter = true;
// Defines an array of channelIDs (e.g. array(0, 1)) for which the previous setting is always true (will be ignored if set to null):
const requestMessagesPriorChannelEnterList = null;
// Max time difference in hours for messages to display on each request:
const requestMessagesTimeDiff = 24;
// Max number of messages to display on each request:
const requestMessagesLimit = 10;

// Max users in chat (does not affect moderators or admins):
const maxUsersLoggedIn = 100;
// Max userName length:
const userNameMaxLength = 16;
// Max messageText length:
const messageTextMaxLength = 1040;
// Defines the max number of messages a user may send per minute:
const maxMessageRate = 20;

// Defines the default time in minutes a user gets banned if kicked from a moderator without ban minutes parameter:
const defaultBanTime = 5;

// Argument that is given to the handleLogout JavaScript method:
const logoutData = '/';

// Defines the max time difference in hours for logs when no period or search condition is given:
const logsRequestMessagesTimeDiff = 1;
// Defines how many logs are returned on each logs request:
const logsRequestMessagesLimit = 10;

// Defines the earliest year used for the logs selection:
const logsFirstYear = 2015;

// Defines if old messages are purged from the database:
const logsPurgeLogs = false;
// Max time difference in days for old messages before they are purged from the database:
const logsPurgeTimeDiff = 365;

// Defines if the socket server is enabled:
const socketServerEnabled = false;
// Defines the hostname of the socket server used to connect from client side (the server hostname is used if set to null):
const socketServerHost = null;
// Defines the IP of the socket server used to connect from server side to broadcast update messages:
const socketServerIP = '127.0.0.1';
// Defines the port of the socket server:
const socketServerPort = 1935;
// This ID can be used to distinguish between different chat installations using the same socket server:
const socketServerChatID = 0;


public static $chatClosed = false;
public static $gzipEnabled = false;
public static $debug = true;

public static  $msgGrad = array(
    'Sagita' => array('eb8100','d520a0','8d13db')
);
public static  $nickGrad = array(
    'Тирраон' => array('300082','800090'),
    'Sagita' => array('8d13db','ff00b4', 'ff8c00')
);
}