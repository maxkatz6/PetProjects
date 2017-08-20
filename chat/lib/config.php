<?php
// Define Socio!PARTY user roles:
define('SCHAT_CHATBOT',   4);
define('SCHAT_ADMIN',     3);
define('SCHAT_MODERATOR', 2);
define('SCHAT_USER',      1);
define('SCHAT_BANNED',    0);

class Config{
    public static $channels = [
    0 => 'Общая',
    1 => 'Диванная',
    2 => 'Видеочат',
    10 => 'Альфа',
    11 => 'Бета',
    12 => 'Гамма',
    13 => 'Дельта',
    14 => 'Админы'];

    // Database connection values:
    public static  $dbConnection = [
    'host' => '',
    'user' => '',
    'pass' => '',
    'name' => '',
    'type' => '',
    'link' => null];

    // Available languages:
    public static  $langAvailable = [
      'ar','bg','ca','cy','cz','da','de','el','en','es','et','fa','fi','fr','gl','he','hr','hu','in','it','ja','ka','kr','mk','nl','nl-be','no','pl','pt-br','pt-pt','ro','ru','sk','sl','sr','sv','th','tr','uk','zh','zh-tw'
    ];
    // Default language:
    const langDefault = 'ru';
    // Language names (each languge code in available languages must have a display name assigned here):
    public static $langNames = [
      'ar'=>'عربي', 'bg'=>'Български', 'ca'=>'Català', 'cy'=>'Cymraeg', 'cz'=>'Česky', 'da'=>'Dansk', 'de'=>'Deutsch', 'el'=>'Ελληνικα', 'en'=>'English',
      'es'=>'Español', 'et'=>'Eesti', 'fa'=>'فارسی', 'fi'=>'Suomi', 'fr'=>'Français', 'gl'=>'Galego', 'he'=>'עברית', 'hr' => 'Hrvatski', 'hu' => 'Magyar', 'in'=>'Bahasa Indonesia', 'it'=>'Italiano',
      'ja'=>'日本語','ka'=>'ქართული','kr'=>'한 글','mk'=>'Македонски', 'nl'=>'Nederlands', 'nl-be'=>'Nederlands (België)', 'no'=>'Norsk', 'pl'=> 'Polski', 'pt-br'=>'Português (Brasil)', 'pt-pt'=>'Português (Portugal)',
      'ro'=>'România', 'ru'=>'Русский', 'sk'=> 'Slovenčina', 'sl'=>'Slovensko', 'sr'=>'Srpski', 'sv'=> 'Svenska', 'th'=>'&#x0e20;&#x0e32;&#x0e29;&#x0e32;&#x0e44;&#x0e17;&#x0e22;',
      'tr'=>'Türkçe', 'uk'=>'Українська', 'zh'=>'中文 (简体)', 'zh-tw'=>'中文 (繁體)'
    ];

    // Available styles:
    public static $styleAvailable = ['beige','grey','Oxygen','Lithium','Sulfur','prosilver','Core','MyBB','vBulletin','XenForo'];
    // Default style:
    const styleDefault = 'prosilver';

    const maleAvatar = 'images/user-man-thumb.png';
    const femaleAvatar = 'images/user-woman-thumb.png';
    const defaultAvatar = 'images/user-man-thumb.png';

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

    // Private channels should be distinguished by either a prefix or a suffix or both (no whitespace):
    const privateChannelPrefix = '[';
    // Private channels should be distinguished by either a prefix or a suffix or both (no whitespace):
    const privateChannelSuffix = ']';

    // Allow/Disallow users to change their userName (Nickname):
    const allowNickChange = true;
    // Changed userNames should be distinguished by either a prefix or a suffix or both (no whitespace):
    const changedNickPrefix = '(';
    // Changed userNames should be distinguished by either a prefix or a suffix or both (no whitespace):
    const changedNickSuffix = ')';

    // The userID used for ChatBot messages:
    const chatBotID = 2147483647;
    // The userName used for ChatBot messages
    const chatBotName = 'Сервер';

    // Minutes until a user is declared inactive (last status update) - the minimum is 2 minutes:
    const inactiveTimeout = 2;
    // Minutes until a user is declared inactive. mobile
    const inactiveTimeoutMobile = 15;
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

    // Defines the earliest year used for the logs selection:
    const logsFirstYear = 2017;

    // Defines if old messages are purged from the database:
    const logsPurgeLogs = false;
    // Max time difference in days for old messages before they are purged from the database:
    const logsPurgeTimeDiff = 365;

    const radioInfo = true;
    const radioServer = false;

    const debug = false;

    public static $chatClosed = false;
    public static $gzipEnabled = false;

    public static  $msgGrad = [
        'NightmareWhitch'   => ['eb8100','d520a0','8d13db'],
        'Экселенс' => ['556B2F','006400','000000'],
        'Niakriss' => ['F0003C','6808EF','000000'],
        //'Эйзенхорн' => ['F70502','990200','000000'],
        'Acedia'   => ['00F5C8','00F586','0B19FE']
    ];
    public static  $nickGrad = [
        'Tirraon' => ['300082','800090'],
        'NightmareWhitch'  => ['8d13db','ff00b4', 'ff8c00'],
        'Huginn'  => ['1C1C1C','363636', '4F4F4F'],
        'Niakriss'=> ['000000','6808EF', 'F0003C'],
        'Экселенс'=> ['000000','006400', '556B2F'],
        //'Эйзенхорн' => ['000000','990200','F70502'],
        'Acedia'  => ['0B19FE','00F586','00F5C8']
    ];
}