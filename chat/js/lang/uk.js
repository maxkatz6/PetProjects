/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @author Yuriy Smetana (yura@stryi.com.ua, http://joomla.org.ua)
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

// Socio!PARTY language Object:
var sChatLang = {
    adminRole: "Адмін",
    guestRole: "Гість",
    userRole: "Користувач",
    moderatorRole: "Модератор",
    chatBotRole: "Бот",
	login: '%s заходить до Чату.',
	logout: '%s виходить з Чату.',
	logoutTimeout: '%s залишає чат (був неактивний).',
	logoutIP: '%s залишає чат (неправильна IP адреса).',
	logoutKicked: '%s залишає чат (примусово).',
    channelEnter: '%s прийшов з кімнати \"%channel\".',
    channelLeave: '%s перейшов до кімнати \"%channel\".',
	privmsg: '(пошепки)',
	privmsgto: '(пошепки до %s)',
	invite: '%s запрошує відвідати кімнату %s.',
	inviteto: 'Запрошення до %s відвідати кімнату %s надіслано.',
	uninvite: '%s відмовив в запрошенні до кімнати %s.',
	uninviteto: 'Відміну запрошення %s до кімнати %s було надіслано.',
	queryOpen: 'Створено приватну кімнату з %s.',
	queryClose: 'Приватну кімнату з %s зачинено.',
	ignoreAdded: '%s додано до переліку ігнорованих осіб.',
	ignoreRemoved: '%s вилучено з переліку ігнорованих осіб.',
	ignoreList: 'Ігроновані особи:',
	ignoreListEmpty: 'Ігнорованих осіб немає.',
	who: 'Зараз в Чаті:',
	whoChannel: 'Зараз в кімнаті %s такі користувачі:',
	whoEmpty: 'Ця кімната порожня.',
	list: 'Інші кімнати:',
	bans: 'Заблоковані користувачі:',
	bansEmpty: 'Немає заблокованих користувачів.',
	unban: 'Блокування користувача %s відмінено.',
	whois: 'Користувач %s - IP адреса:',
	whereis: 'Користувач %s в кімнаті %s.',
	roll: 'Користувачем %s кинуто %s, випало %s.',
	nick: 'Користувач %s відтепер буде називатись %s.',
	toggleUserMenu: 'Меню користувача %s',
	userMenuLogout: 'Вийти',
	userMenuWho: 'Показати присутніх',
	userMenuList: 'Показати кімнати',
	userMenuAction: 'Описати чим зараз займаєтесь',
	userMenuRoll: 'Кинути кості',
	userMenuNick: 'Змінити собі ім\'я',
	userMenuEnterPrivateRoom: 'Увійти до приватної кімнати',
	userMenuSendPrivateMessage: 'Надіслати особисте повідомлення',
	userMenuDescribe: 'Описати чим займаєтесь (приватно)',
	userMenuOpenPrivateChannel: 'Створити приватну кімнату',
	userMenuClosePrivateChannel: 'Закрити приватну кімнату',
	userMenuInvite: 'Запросити',
	userMenuUninvite: 'Відмовити',
	userMenuIgnore: 'Ігнорувати/Приймати',
	userMenuIgnoreList: 'Перелік ігнорованих',
	userMenuWhereis: 'Показати кімнати',
	userMenuKick: 'Викинути/Заблокувати',
	userMenuBans: 'Перелік заблокованих користувачів',
	userMenuWhois: 'Показати IP',
	unbanUser: 'Відмінити блокування користувача %s',
	joinChannel: 'Зайти в кімнату %s',
	cite: '%s пише:',
	urlDialog: 'Будь-ласка, введіть адресу веб-сторінки:',
	deleteMessage: 'Видалити це повідомлення',
	deleteMessageConfirm: 'Справді видалити це повідомлення?',
	errorCookiesRequired: 'Для Чату необхідно дозволити Cookies.',
	errorUserNameNotFound: 'Помилка: користувач %s не існує.',
	errorMissingText: 'Помилка: повідомлення відсутнє.',
	errorMissingUserName: 'Помилка: відсутнє ім\'я користувача.',
	errorInvalidUserName: 'Error: Invalid username.',
	errorUserNameInUse: 'Error: Username already in use.',
	errorMissingChannelName: 'Помилка: відсутня назва кімнати.',
	errorInvalidChannelName: 'Помилка: хибна назва кімнати: %s',
	errorPrivateMessageNotAllowed: 'Помилка: приватні повідомлення заборонені.',
	errorInviteNotAllowed: 'Помилка: Вам заборонено запрошувати до цієї кімнати.',
	errorUninviteNotAllowed: 'Помилка: Вам заборонено відмовляти в запрошенні до цієї кімнати.',
	errorNoOpenQuery: 'Помилка: не існує приватних кімнат.',
	errorKickNotAllowed: 'Помилка: Вам недозволено викидати користувачів %s.',
	errorCommandNotAllowed: 'Помилка: недозволена команда: %s',
	errorUnknownCommand: 'Помилка: хибна команда: %s',
	errorMaxMessageRate: 'Помилка: Ви досягли максимальної кількості повідомлень за хвилину.',
	errorConnectionTimeout: 'Помилка: час очікування минув. Будь-ласка, спробуйте знову.',
	errorConnectionStatus: 'Помилка: стан з\'єднання: %s',
	errorSoundIO: 'Помилка: Неможливо відкрити звуковий файл (Flash IO Error).',
	errorSocketIO: 'Помилка: З\'єднання з сервером невдалося (Flash IO Error).',
	errorSocketSecurity: 'Помилка: З\'єднання з сервером невдалося (Flash Security Error).',
	errorDOMSyntax: 'Помилка: Invalid DOM Syntax (DOM ID: %s).'
	
}