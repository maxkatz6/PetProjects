<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

$lang = [
'title' => 'Socio!PARTY',
'userName' => 'Kasutajanimi',
'password' => 'Parool',
'login' => 'Sisene',
'logout' => 'Välju',
'channel' => 'Kanal',
'style' => 'Stiil',
'language' => 'Keel',
'inputLineBreak' => 'Vajuta SHIFT+ENTER, et sisestada reavahe',
'messageSubmit' => 'Sisesta',
'registeredUsers' => 'Registreeritud kasutajad',
'onlineUsers' => 'Sisse loginud kasutajad',
'toggleAutoScroll' => 'Automaatne kerimine sees/väljas',
'toggleAudio' => 'Helid sees/väljas',
'toggleHelp' => 'Näita/Peida Abi',
'toggleSettings' => 'Näita/Peida seaded',
'toggleOnlineList' => 'Näita/Peida sisse loginuid',
'bbCodeLabelBold' => 'b',
'bbCodeLabelItalic' => 'i',
'bbCodeLabelUnderline' => 'u',
'bbCodeLabelQuote' => 'Tsitaat',
'bbCodeLabelCode' => 'Kood',
'bbCodeLabelURL' => 'URL',
'bbCodeLabelImg' => 'Pilt',
'bbCodeLabelColor' => 'Fondi värv',
'bbCodeLabelEmoticon' => 'Emotikonid',
'bbCodeTitleBold' => 'Rasvases kirjas tekst: [b]tekst[/b]',
'bbCodeTitleItalic' => 'Kaldkirjas tekst: [i]tekst[/i]',
'bbCodeTitleUnderline' => 'Underline text: [u]tekst[/u]',
'bbCodeTitleQuote' => 'Tsiteeritud tekst: [quote]tekst[/quote] või [quote=autor]tekst[/quote]',
'bbCodeTitleCode' => 'Koodi esitamine: [code]kood[/code]',
'bbCodeTitleURL' => 'Lisa URL: [url]http://näide.org[/url] või [url=http://näide.org]tekst[/url]',
'bbCodeTitleImg' => 'Lisa pilt: [img]http://näide.org/pilt.jpg[/img]',
'bbCodeTitleColor' => 'Fondi värv: [color=red]tekst[/color]',
'bbCodeTitleEmoticon' => 'Emotikonid nimekiri',
'help' => 'Abi',
'helpItemDescJoin' => 'Liitu kanaliga:',
'helpItemCodeJoin' => '/join Kanalinimi',
'helpItemDescJoinCreate' => 'Loo privaat-ruum (Vaid registreeritud kasutajad):',
'helpItemCodeJoinCreate' => '/join',
'helpItemDescInvite' => 'Kutsu kedagi (näiteks privaat-ruumi):',
'helpItemCodeInvite' => '/invite Kasutajanimi',
'helpItemDescUninvite' => 'Tühista kutse:',
'helpItemCodeUninvite' => '/uninvite Kasutajanimi',
'helpItemDescLogout' => 'Lahku jutukast:',
'helpItemCodeLogout' => '/quit',
'helpItemDescPrivateMessage' => 'Privaat-sõnum:',
'helpItemCodePrivateMessage' => '/msg Kasutajanimi Tekst',
'helpItemDescQueryOpen' => 'Ava privaat-kanal:',
'helpItemCodeQueryOpen' => '/query Kasutajanimi',
'helpItemDescQueryClose' => 'Sulge privaat-kanal:',
'helpItemCodeQueryClose' => '/query',
'helpItemDescAction' => 'Kirjelda tegevust:',
'helpItemCodeAction' => '/action Tekst',
'helpItemDescDescribe' => 'Kirjelda tegevust privaat-sõnumis:',
'helpItemCodeDescribe' => '/describe Kasutajanimi Tekst',
'helpItemDescIgnore' => 'Ignoreeri/Tunnusta sõnumeid kindlalt kasutajalt:',
'helpItemCodeIgnore' => '/ignore Kasutajanimi',
'helpItemDescIgnoreList' => 'Ignoreeritud kasutajate nimekiri:',
'helpItemCodeIgnoreList' => '/ignore',
'helpItemDescWhereis' => 'Mis kanalis asub kasutaja?:',
'helpItemCodeWhereis' => '/whereis Kasutajanimi',
'helpItemDescKick' => 'Viska kasutaja välja (Ainult moderaatorid):',
'helpItemCodeKick' => '/kick Kasutajanimi [minutite hulk mustas nimekirjas]',
'helpItemDescUnban' => 'Eemalda kasutaja mustast nimekirjast (Ainult moderaatorid):',
'helpItemCodeUnban' => '/unban Kasutajanimi',
'helpItemDescBans' => 'Must nimekiri (Ainult moderaatorid):',
'helpItemCodeBans' => '/bans',
'helpItemDescWhois' => 'Näita kasutaja IP aadressit (Ainult moderaatorid):',
'helpItemCodeWhois' => '/whois Kasutajanimi',
'helpItemDescWho' => 'Jutukasse sisenenud kasutajad:',
'helpItemCodeWho' => '/who [Kanalinimi]',
'helpItemDescList' => 'Vabade kanalite nimekiri:',
'helpItemCodeList' => '/list',
'helpItemDescRoll' => 'Veereta täringut:',
'helpItemCodeRoll' => '/roll [number]d[küljed]',
'helpItemDescNick' => 'Muuda kasutajanimi:',
'helpItemCodeNick' => '/nick Kasutajanimi',
'settings' => 'Seaded',
'settingsBBCode' => 'Luba BBCode:',
'settingsBBCodeImages' => 'Luba pildi BBCode:',
'settingsBBCodeColors' => 'Luba fondi värvi BBCode:',
'settingsHyperLinks' => 'Luba hüpperlingid:',
'settingsLineBreaks' => 'Luba reavahed:',
'settingsEmoticons' => 'Luba emotikonid:',
'settingsAutoFocus' => 'Sea fookus automaatselt sisestusväljale:',
'settingsMaxMessages' => 'Maksimum sõnumite hulk ühes nimekirjas:',
'settingsWordWrap' => 'Luba pikkade sõnade murdmine:',
'settingsMaxWordLength' => 'Maksimum sõna pikkus enne kui seda murtakse:',
'settingsDateFormat' => 'Kuupäeva ja kellaaja formaat:',
'settingsPersistFontColor' => 'Püsiv fondi värv',
'settingsAudioVolume' => 'Heli tugevus:',
'settingsSoundReceive' => 'Heli sisenemisel:',
'settingsSoundSend' => 'Heli väljumisel:',
'settingsSoundEnter' => 'Heli sisenemisel ja kanalisse sisenemise sõnumitel:',
'settingsSoundLeave' => 'Heli väljumise ja kanalitest lahkumise sõnumitel:',
'settingsSoundChatBot' => 'Chatboti sõnumite heli:',
'settingsSoundError' => 'Veateate heli:',
'settingsSoundPrivate' => 'Privaatsõnum sõnumite heli:',
'settingsBlink' => 'Vilguta akna tiitlit uute sõnumite saabumisel:',
'settingsBlinkInterval' => 'Vilgutamise intervall millisekundites:',
'settingsBlinkIntervalNumber' => 'Vilksatuste arv intervallis:',
'playSelectedSound' => 'Mängi valitud heli',
'requiresJavaScript' => 'Vajalik on JavaScripti olemas olu jutuka kasutamiseks.',
'errorInvalidUser' => 'Vigane kasutajanimi.',
'errorUserInUse' => 'Kasutajanimi on juba võetud.',
'errorBanned' => 'Kasutaja või IP on lisatud musta nimekirja.',
'errorMaxUsersLoggedIn' => 'Jutukas on jõudnud maksimum kasutajate hulgani.',
'errorChatClosed' => 'Jutukas on hetkel suletud.',
'logsTitle' => 'ComicLand - jutukas - Logiraamat',
'logsDate' => 'Kuupäev',
'logsTime' => 'Kellaaeg',
'logsSearch' => 'Otsi',
'logsPrivateChannels' => 'Privaat-kanalid',
'logsPrivateMessages' => 'Privaat-sõnumid'
];?>
