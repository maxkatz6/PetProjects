<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

$lang = [
'title' => 'AJAX Brbljanje',
'userName' => 'Korisničko ime',
'password' => 'Lozinka',
'login' => 'Prijava',
'logout' => 'Odjava',
'channel' => 'Kanal',
'style' => 'Stil',
'language' => 'Jezik',
'inputLineBreak' => 'Za unos novog retka pritisnite SHIFT+ENTER',
'messageSubmit' => 'Pošalji',
'registeredUsers' => 'Registrirani korisnici',
'onlineUsers' => 'Prisutni korisnici',
'toggleAutoScroll' => 'Automatsko klizanje - uklj/isklj ',
'toggleAudio' => 'Zvuk - uklj/isklj',
'toggleHelp' => 'Pomoć - prikaži/sakrij',
'toggleSettings' => 'Postavke - prikaži/sakrij',
'toggleOnlineList' => 'Popisa prisutnih - prikaži/sakrij',
'bbCodeLabelBold' => 'b',
'bbCodeLabelItalic' => 'i',
'bbCodeLabelUnderline' => 'u',
'bbCodeLabelQuote' => 'Citat',
'bbCodeLabelCode' => 'Kôd',
'bbCodeLabelURL' => 'URL',
'bbCodeLabelImg' => 'Slika',
'bbCodeLabelColor' => 'Boja',
'bbCodeLabelEmoticon' => 'Emotikone',
'bbCodeTitleBold' => 'Snažan tekst: [b]tekst[/b]',
'bbCodeTitleItalic' => 'Kurziv: [i]tekst[/i]',
'bbCodeTitleUnderline' => 'Podvučeni tekst: [u]tekst[/u]',
'bbCodeTitleQuote' => 'Citirani tekst: [quote]tekst[/quote] ili [quote=autor]tekst[/quote]',
'bbCodeTitleCode' => 'Prikaz kôda: [code]kôd[/code]',
'bbCodeTitleURL' => 'Umetanje URL: [url]http://primjer.org[/url] ili [url=http://primjer.org]tekst[/url]',
'bbCodeTitleImg' => 'Umetanje slike: [img]http://primjer.org/slika.jpg[/img]',
'bbCodeTitleColor' => 'Boja fonta: [color=red]tekst[/color]',
'bbCodeTitleEmoticon' => 'Emotikone popis',
'help' => 'Pomoć',
'helpItemDescJoin' => 'Pridruživanje kanalu:',
'helpItemCodeJoin' => '/join kanal',
'helpItemDescJoinCreate' => 'Izrada privatne sobe (samo za registrirane korisnike):',
'helpItemCodeJoinCreate' => '/join',
'helpItemDescInvite' => 'Pozivanje korisnika (npr. u privatnu sobu):',
'helpItemCodeInvite' => '/invite korisnik',
'helpItemDescUninvite' => 'Opoziv:',
'helpItemCodeUninvite' => '/uninvite korisnik',
'helpItemDescLogout' => 'Odjava iz brbljanja:',
'helpItemCodeLogout' => '/quit',
'helpItemDescPrivateMessage' => 'Privatna poruka:',
'helpItemCodePrivateMessage' => '/msg korisnik tekst',
'helpItemDescQueryOpen' => 'Otvaranje privatnog kanala:',
'helpItemCodeQueryOpen' => '/query korisnik',
'helpItemDescQueryClose' => 'Zatvaranje privatnog kanala:',
'helpItemCodeQueryClose' => '/query',
'helpItemDescAction' => 'Aktivnost:',
'helpItemCodeAction' => '/action tekst',
'helpItemDescDescribe' => 'Aktivnost u privatnoj poruci:',
'helpItemCodeDescribe' => '/describe korisnik tekst',
'helpItemDescIgnore' => 'Ignoriranje/prihvaćanje poruka od korisnika:',
'helpItemCodeIgnore' => '/ignore korisnik',
'helpItemDescIgnoreList' => 'Ispisivanje ignoriranih korisnika:',
'helpItemCodeIgnoreList' => '/ignore',
'helpItemDescWhereis' => 'Prikazivanje korisničkog kanala:',
'helpItemCodeWhereis' => '/whereis korisnik',
'helpItemDescKick' => 'Progon korisnika (samo za moderatore):',
'helpItemCodeKick' => '/kick korisnik [minute]',
'helpItemDescUnban' => 'Uklanjanje zabrane korisnika (samo za moderatore):',
'helpItemCodeUnban' => '/unban korisnik',
'helpItemDescBans' => 'Ispisivanje zabranjenih korisnika (samo za moderatore):',
'helpItemCodeBans' => '/bans',
'helpItemDescWhois' => 'Prikazivanje IP korisnika (samo za moderatore):',
'helpItemCodeWhois' => '/whois korisnik',
'helpItemDescWho' => 'Ispisivanje prisutnih korisnika:',
'helpItemCodeWho' => '/who [kanal]',
'helpItemDescList' => 'Ispisivanje dostupnih kanala:',
'helpItemCodeList' => '/list',
'helpItemDescRoll' => 'Bacanje kocke:',
'helpItemCodeRoll' => '/roll [broj]d[strane]',
'helpItemDescNick' => 'Promjena korisničkog imena:',
'helpItemCodeNick' => '/nick korisnik',
'settings' => 'Postavke',
'settingsBBCode' => 'Omogući BB kôd:',
'settingsBBCodeImages' => 'Omogući BB kôd za slike:',
'settingsBBCodeColors' => 'Omogući BB kôd za boju fonta:',
'settingsHyperLinks' => 'Omogući hiperveze:',
'settingsLineBreaks' => 'Omogući nove retke:',
'settingsEmoticons' => 'Omogući emotikone:',
'settingsAutoFocus' => 'Automatski postavi fokus u polje unosa:',
'settingsMaxMessages' => 'Najveći broj poruka na popisu:',
'settingsWordWrap' => 'Omogući obmatanje dugih riječi:',
'settingsMaxWordLength' => 'Najveća duljina riječi prije njezinog obmatanja:',
'settingsDateFormat' => 'Oblikovanje datuma i vremena:',
'settingsPersistFontColor' => 'Dosljedna boja fonta:',
'settingsAudioVolume' => 'Glasnoća zvuka:',
'settingsSoundReceive' => 'Zvuk za dolazne poruke:',
'settingsSoundSend' => 'Zvuk za odlazne poruke:',
'settingsSoundEnter' => 'Zvuk za prijavljivanje i poruke pristupanja kanalu:',
'settingsSoundLeave' => 'Zvuk za odjavljivanje i poruke napuštanja kanala:',
'settingsSoundChatBot' => 'Zvuk za poruke brbljobota:',
'settingsSoundError' => 'Zvuk za poruke pogreške:',
'settingsSoundPrivate' => 'Zvuk za poruke privatni:',
'settingsBlink' => 'Treptanje naslova prozora kod novih poruka:',
'settingsBlinkInterval' => 'Trajanje treptanja (u milisekundama):',
'settingsBlinkIntervalNumber' => 'Broj treptanja:',
'playSelectedSound' => 'Pokreni odabrani zvuk',
'requiresJavaScript' => 'Ovo brbljanje zahtjeva JavaScript.',
'errorInvalidUser' => 'Nepravilno korisničko ime.',
'errorUserInUse' => 'Korisničko ime već je u upotrebi.',
'errorBanned' => 'Korisnik ili IP je zabranjen.',
'errorMaxUsersLoggedIn' => 'Brbljaonica je dostigla najveći dopušteni broj prijavljenih korisnika.',
'errorChatClosed' => 'Brbljaonica je trenutno zatvorena.',
'logsTitle' => 'AJAX Brbljanje - Zapisnici',
'logsDate' => 'Datum',
'logsTime' => 'Vrijeme',
'logsSearch' => 'Traži',
'logsPrivateChannels' => 'Privatni kanali',
'logsPrivateMessages' => 'Privatne poruke'
];?>
