<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @author Nebojsa Ilijoski
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

$lang = [
'title' => 'Socio!PARTY',
'userName' => 'Име',
'password' => 'Лозинка',
'login' => 'Влези',
'logout' => 'Излези',
'channel' => 'Канал',
'style' => 'Изглед',
'language' => 'Јазик',
'inputLineBreak' => 'Притиснете SHIFT+ENTER, за да вметнете нов ред',
'messageSubmit' => 'Прашање',
'registeredUsers' => 'Регистрирани корисници',
'onlineUsers' => 'Online корисници',
'toggleAutoScroll' => 'Автопрелистување вкл/иcкл',
'toggleAudio' => 'Звук вкл/изкл',
'toggleHelp' => 'Покашување/скривање на помошта',
'toggleSettings' => 'Покажување/скривање на поставките',
'toggleOnlineList' => 'Покажување/скривање на листата со корисници',
'bbCodeLabelBold' => 'b',
'bbCodeLabelItalic' => 'i',
'bbCodeLabelUnderline' => 'u',
'bbCodeLabelQuote' => 'Цитат',
'bbCodeLabelCode' => 'Код',
'bbCodeLabelURL' => 'URL',
'bbCodeLabelImg' => 'Image',
'bbCodeLabelColor' => 'Боја на фонт',
'bbCodeLabelEmoticon' => 'емоции',
'bbCodeTitleBold' => 'Задебелен текст: [b]текст[/b]',
'bbCodeTitleItalic' => 'Закосен текст: [i]текст[/i]',
'bbCodeTitleUnderline' => 'Подцртан текст: [u]текст[/u]',
'bbCodeTitleQuote' => 'Цитиран текст: [quote]текст[/quote] или [quote=Автор]текст[/quote]',
'bbCodeTitleCode' => 'Покажување на код: [code]код[/code]',
'bbCodeTitleURL' => 'URL: [url]http://example.org[/url] или [url=http://example.org]текст[/url]',
'bbCodeTitleImg' => 'Insert image: [img]http://example.org/image.jpg[/img]',
'bbCodeTitleColor' => 'Боја на заглавие: [color=red]текст[/color]',
'bbCodeTitleEmoticon' => 'емоции листа',
'help' => 'Помош',
'helpItemDescJoin' => 'Присоединување кон канал:',
'helpItemCodeJoin' => '/join име_на_канал',
'helpItemDescJoinCreate' => 'Креирање сопствена соба (само за регистрирани корисници):',
'helpItemCodeJoinCreate' => '/join',
'helpItemDescInvite' => 'Покана на корисници (напр. во сопствена соба):',
'helpItemCodeInvite' => '/invite Корисничко_име',
'helpItemDescUninvite' => 'Откажување на покана:',
'helpItemCodeUninvite' => '/uninvite Корисничко_име',
'helpItemDescLogout' => 'Одјавување од четот:',
'helpItemCodeLogout' => '/quit',
'helpItemDescPrivateMessage' => 'Лична порака:',
'helpItemCodePrivateMessage' => '/msg Корисничко_име Текст',
'helpItemDescQueryOpen' => 'Отварање на личен канал:',
'helpItemCodeQueryOpen' => '/query Корисничко_име',
'helpItemDescQueryClose' => 'Затварање на личен канал:',
'helpItemCodeQueryClose' => '/query',
'helpItemDescAction' => 'Опиц на дејство:',
'helpItemCodeAction' => '/action Текст',
'helpItemDescDescribe' => 'Опис на дејство во приватна порака:',
'helpItemCodeDescribe' => '/describe Корисничко_име Текст',
'helpItemDescIgnore' => 'Игнорирање/примање на пораки од корисник:',
'helpItemCodeIgnore' => '/ignore Корисничко_име',
'helpItemDescIgnoreList' => 'Преглед на игнорирани корисници:',
'helpItemCodeIgnoreList' => '/ignore',
'helpItemDescWhereis' => 'Покажување на корисничкиот канал:',
'helpItemCodeWhereis' => '/whereis Корисничко_име',
'helpItemDescKick' => 'Исфрлање на корисници (само за модератори):',
'helpItemCodeKick' => '/kick Корисничко_име [број минути]',
'helpItemDescUnban' => 'Одблокирање на корисници (само за модератори):',
'helpItemCodeUnban' => '/unban Корисничко_име',
'helpItemDescBans' => 'Преглед на блокирани корисници (само за модератори):',
'helpItemCodeBans' => '/bans',
'helpItemDescWhois' => 'Преглед на IP адреса на корисник (само за модератори):',
'helpItemCodeWhois' => '/whois Корисничко_име',
'helpItemDescWho' => 'Преглед на online корисници:',
'helpItemCodeWho' => '/who [Име_на_канал]',
'helpItemDescList' => 'Преглед на приватните канали:',
'helpItemCodeList' => '/list',
'helpItemDescRoll' => 'Фрлање коцка:',
'helpItemCodeRoll' => '/roll [пати]d[страни]',
'helpItemDescNick' => 'Промена на корисничкото име:',
'helpItemCodeNick' => '/nick Корисничко_име',
'settings' => 'Поставки',
'settingsBBCode' => 'Користење на BBCode:',
'settingsBBCodeImages' => 'Enable image BBCode:',
'settingsBBCodeColors' => 'Enable font color BBCode:',
'settingsHyperLinks' => 'Користење на на хиперлинк:',
'settingsLineBreaks' => 'Користење на крај на ред:',
'settingsEmoticons' => 'Користење на емоции:',
'settingsAutoFocus' => 'Автоматско фокусирање на полето за внес:',
'settingsMaxMessages' => 'Максимален број пораки во прозорецот на четот:',
'settingsWordWrap' => 'Префрлање на текст:',
'settingsMaxWordLength' => 'Максимална должина на порака пред да биде пренесена:',
'settingsDateFormat' => 'Формат на датата и часот:',
'settingsPersistFontColor' => 'Постојана боја на фонтот:',
'settingsAudioVolume' => 'Гласност на звука:',
'settingsSoundReceive' => 'Звук за приемни пораки:',
'settingsSoundSend' => 'Звук за испратени пораки:',
'settingsSoundEnter' => 'Звук за пораки за влез на четот или на каналот:',
'settingsSoundLeave' => 'Звук за пораки за излез од четот или од каналот:',
'settingsSoundChatBot' => 'Звук за пораки на чатботот:',
'settingsSoundError' => 'Звук за пораки за грешки:',
'settingsSoundPrivate' => 'Звук за пораки за приватни:',
'settingsBlink' => 'Жмигање на заглавието на прозорецот при нови пораки:',
'settingsBlinkInterval' => 'Интервал на жмигање во милисекунди:',
'settingsBlinkIntervalNumber' => 'Број пати на жмигање:',
'playSelectedSound' => 'Преслушување на избраниот звук',
'requiresJavaScript' => 'За четот е потребна поддршка на Јаваскрипт.',
'errorInvalidUser' => 'Грешно корисничко име.',
'errorUserInUse' => 'Корисничкото име е во употреба.',
'errorBanned' => 'Корисникот или IP адресата е блокирана.',
'errorMaxUsersLoggedIn' => 'Четот го достигна максималниот број на корисници.',
'errorChatClosed' => 'Во моментов четот е затворен.',
'logsTitle' => 'Socio!PARTY — Лого',
'logsDate' => 'Дата',
'logsTime' => 'Време',
'logsSearch' => 'Пребарување',
'logsPrivateChannels' => 'Приватни канали',
'logsPrivateMessages' => 'Приватни пораки'
];?>
