<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @author pepotiger (www.dd4bb.com)
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

$lang = [
'title' => 'Socio!PARTY',
'userName' => 'نام عبور',
'password' => 'کلمه عبور',
'login' => 'ورود',
'logout' => 'خروج',
'channel' => 'اطاق',
'style' => 'ظاهر',
'language' => 'زبان',
'inputLineBreak' => 'کلید SHIFT+ENTER را برای ایجاد خط جدید فشار دهید',
'messageSubmit' => 'ارسل',
'registeredUsers' => 'کاربران ثبت نام شده',
'onlineUsers' => 'کاربران حاضر',
'toggleAutoScroll' => 'اسکرول اتوماتیک فعال/غیر فعال',
'toggleAudio' => 'صدا روشن/خاموش',
'toggleHelp' => 'نمایش/پنهان سازی کمک رسان',
'toggleSettings' => 'نمایش/پنهان سازی تنظیمات',
'toggleOnlineList' => 'نمایش/پنهان سازی لیست کاربران حاضر',
'bbCodeLabelBold' => 'b',
'bbCodeLabelItalic' => 'i',
'bbCodeLabelUnderline' => 'u',
'bbCodeLabelQuote' => 'اقتباس',
'bbCodeLabelCode' => 'کد',
'bbCodeLabelURL' => 'URL',
'bbCodeLabelImg' => 'تصویر',
'bbCodeLabelColor' => 'رنگ خط',
'bbCodeLabelEmoticon' => 'Smilies',
'bbCodeTitleBold' => 'متن درشت: [b]متن[/b]',
'bbCodeTitleItalic' => 'متن کج: [i]متن[/i]',
'bbCodeTitleUnderline' => 'زیر خط دار: [u]متن[/u]',
'bbCodeTitleQuote' => 'نقل قول: [quote]متن[/quote] او [quote=الكاتب]نص[/quote]',
'bbCodeTitleCode' => 'نمایش کد: [code]code[/code]',
'bbCodeTitleURL' => 'وارد کردن آدرس: [url]http://example.org[/url] یا [url=http://example.org]متن[/url]',
'bbCodeTitleImg' => 'وارد کردن تصویر: [img]http://example.org/image.jpg[/img]',
'bbCodeTitleColor' => 'رنگ متن: [color=red]متن[/color]',
'bbCodeTitleEmoticon' => 'Smilies list',
'help' => 'کمک',
'helpItemDescJoin' => 'ورود به اطاق:',
'helpItemCodeJoin' => '/join نام اطاق',
'helpItemDescJoinCreate' => 'ساخت اطاق خصوصی (تنها برای اعضا):',
'helpItemCodeJoinCreate' => '/join',
'helpItemDescInvite' => 'دعوت کاربر (به عنوان مثال برای اطاق خصوصی):',
'helpItemCodeInvite' => '/invite نام کاربری',
'helpItemDescUninvite' => 'لغو دعوت:',
'helpItemCodeUninvite' => '/uninvite نام کاربری',
'helpItemDescLogout' => 'خروج:',
'helpItemCodeLogout' => '/quit',
'helpItemDescPrivateMessage' => 'پیام خصوصی:',
'helpItemCodePrivateMessage' => '/msg نامکاربری متن',
'helpItemDescQueryOpen' => 'ساخت اطاق خصوصی:',
'helpItemCodeQueryOpen' => '/query نام کاربری',
'helpItemDescQueryClose' => 'خروج از اطاق خصوصی:',
'helpItemCodeQueryClose' => '/query',
'helpItemDescAction' => 'شرح واقعه:',
'helpItemCodeAction' => '/action متن',
'helpItemDescDescribe' => 'شرح واقعه با پیام خصوصی:',
'helpItemCodeDescribe' => '/describe نام کاربری متن',
'helpItemDescIgnore' => 'قبول/صرف نظر پیغام از کاربر:',
'helpItemCodeIgnore' => '/ignore نام کاربری',
'helpItemDescIgnoreList' => 'افراد صرف نظر شده:',
'helpItemCodeIgnoreList' => '/ignore',
'helpItemDescWhereis' => 'نمایش اطاق کاربر:',
'helpItemCodeWhereis' => '/whereis نام کاربر',
'helpItemDescKick' => 'اجراج کاربر (تنها برای مدیران):',
'helpItemCodeKick' => '/kick نام کاربری [زمان اخراج]',
'helpItemDescUnban' => 'لغو اخراج (تنها برای مدیران):',
'helpItemCodeUnban' => '/unban نام کاربری',
'helpItemDescBans' => 'لیست کاربران اخراج شده (تنها برای مدیران):',
'helpItemCodeBans' => '/bans',
'helpItemDescWhois' => 'مشاهده آیپی کاربر (تنها برای مدیران):',
'helpItemCodeWhois' => '/whois نام کاربری',
'helpItemDescWho' => 'لیست کاربران حاضر:',
'helpItemCodeWho' => '/who [نام اطاق]',
'helpItemDescList' => 'نام اطاق های موجود:',
'helpItemCodeList' => '/list',
'helpItemDescRoll' => 'Roll dice:',
'helpItemCodeRoll' => '/roll [number]d[sides]',
'helpItemDescNick' => 'تغییر نام کاربری:',
'helpItemCodeNick' => '/nick نام کاربری',
'settings' => 'تنظیمات',
'settingsBBCode' => 'فعال سازی BBCode:',
'settingsBBCodeImages' => 'فعال سازی تصاویر BBCode:',
'settingsBBCodeColors' => 'فعال سازی رنگ متن BBCode:',
'settingsHyperLinks' => 'فعال سازی لینک های اینترنتی:',
'settingsLineBreaks' => 'فعال سازی پیام چند خطی:',
'settingsEmoticons' => 'فعال سازی شکلک ها:',
'settingsAutoFocus' => 'Automatically set the focus on the input field:',
'settingsMaxMessages' => 'حد اکثر تعداد پیام در هر اطاق:',
'settingsWordWrap' => 'Enable wrapping of long words:',
'settingsMaxWordLength' => 'Maximum length of a word before it gets wrapped:',
'settingsDateFormat' => 'Format of date and time display:',
'settingsPersistFontColor' => 'Persist font color:',
'settingsAudioVolume' => 'Sound Volume:',
'settingsSoundReceive' => 'Sound for incoming messages:',
'settingsSoundSend' => 'Sound for outgoing messages:',
'settingsSoundEnter' => 'Sound for login and channel enter messages:',
'settingsSoundLeave' => 'Sound for logout and channel leave messages:',
'settingsSoundChatBot' => 'Sound for chatbot messages:',
'settingsSoundError' => 'Sound for error messages:',
'settingsSoundPrivate' => 'Sound for private messages:',
'settingsBlink' => 'Blink window title on new messages:',
'settingsBlinkInterval' => 'Blink interval in milliseconds:',
'settingsBlinkIntervalNumber' => 'Number of blink intervals:',
'playSelectedSound' => 'اجرای صدای انتخاب شده',
'requiresJavaScript' => 'جاوا اسکریپت برای این تالار نیاز است.',
'errorInvalidUser' => 'نا کاربری نامعتبر.',
'errorUserInUse' => 'نام کاربری در حال استفاده است.',
'errorBanned' => 'نام کاربری یا آیپی اخراج شده اند.',
'errorMaxUsersLoggedIn' => 'متاسفانه تالار گفتگو به حداکثر ظرفیت خود برای کاربران حاضر رسیده است.',
'errorChatClosed' => 'تالار گفتوگو هم اکنون بسته می باشد.',
'logsTitle' => 'Socio!PARTY - Logs',
'logsDate' => 'تاریخ',
'logsTime' => 'زمان',
'logsSearch' => 'جست و جو',
'logsPrivateChannels' => 'اطاق های خصوصی',
'logsPrivateMessages' => 'پیغام های خصوصی'
];?>
