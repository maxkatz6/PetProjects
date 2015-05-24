<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 * @Translate by Charge01 @ http://www.thaira2lovers.co.cc
 */

$lang = [
'title' => 'Socio!PARTY',
'userName' => 'ชื่อผู้ใช้',
'password' => 'รหัสผ่าน',
'login' => 'เข้าสู่ระบบ',
'logout' => 'ออกจากระบบ',
'channel' => 'ห้องแชท',
'style' => 'รูปแบบ',
'language' => 'ภาษา',
'inputLineBreak' => 'กด SHIFT+ENTER เพื่อเว้นบรรทัด',
'messageSubmit' => 'ส่ง',
'registeredUsers' => 'ผู้ใช้ที่ลงทะเบียนแล้ว',
'onlineUsers' => 'ผู้ใช้ออนไลน์อยู่',
'toggleAutoScroll' => 'เลื่อนอัตโนมัติ เปิด/ปิด',
'toggleAudio' => 'เสียง เปิด/ปิด',
'toggleHelp' => 'ช่วยเหลือ แสดง/ซ่อน',
'toggleSettings' => 'ตั้งค่า แสดง/ซ่อน',
'toggleOnlineList' => 'ผู้ใช้ออนไลน์ แสดง/ซ่อน',
'bbCodeLabelBold' => 'b',
'bbCodeLabelItalic' => 'i',
'bbCodeLabelUnderline' => 'u',
'bbCodeLabelQuote' => 'อ้างอิง',
'bbCodeLabelCode' => 'โค๊ด',
'bbCodeLabelURL' => 'ลิงค์',
'bbCodeLabelImg' => 'ภาพ',
'bbCodeLabelColor' => 'สีอักษร',
'bbCodeLabelEmoticon' => 'Smilies',
'bbCodeTitleBold' => 'อักษรตัวหน้า: [b]ข้อความ[/b]',
'bbCodeTitleItalic' => 'อักษรตัวเอียง: [i]ข้อความ[/i]',
'bbCodeTitleUnderline' => 'อักษรขีดเส้นใต้: [u]ข้อความ[/u]',
'bbCodeTitleQuote' => 'อ้างอิงข้อความ: [quote]ข้อความ[/quote] หรือ [quote=เจ้าของข้อความ]ข้อความ[/quote]',
'bbCodeTitleCode' => 'แสดงโค๊ด: [code]โค๊ด[/code]',
'bbCodeTitleURL' => 'แทรกลิงค์: [url]http://example.org[/url] or [url=http://example.org]ข้อความ[/url]',
'bbCodeTitleImg' => 'แทรกภาพ: [img]http://example.org/image.jpg[/img]',
'bbCodeTitleColor' => 'ตัวอักษรสี: [color=red]ข้อความ[/color]',
'bbCodeTitleEmoticon' => 'Smilies list',
'help' => 'ช่วยเหลือ',
'helpItemDescJoin' => 'ร่วมห้องแชท:',
'helpItemCodeJoin' => '/join ชื่อห้องแชท',
'helpItemDescJoinCreate' => 'สร้างห้องส่วนตัว (สำหรับสมาชิกเท่านั้น):',
'helpItemCodeJoinCreate' => '/join',
'helpItemDescInvite' => 'เชิญใครสักคน (เช่น คุยในห้องส่วนตัว):',
'helpItemCodeInvite' => '/invite ชื่อผู้ใช้',
'helpItemDescUninvite' => 'ถอนคำเชิญ:',
'helpItemCodeUninvite' => '/uninvite ชื่อผู้ใช้',
'helpItemDescLogout' => 'ออกจากระบบห้องแชท:',
'helpItemCodeLogout' => '/quit',
'helpItemDescPrivateMessage' => 'ข้อความส่วนตัว:',
'helpItemCodePrivateMessage' => '/msg ชื่อผู้ใช้ ข้อความ',
'helpItemDescQueryOpen' => 'เปิดห้องส่วนตัว:',
'helpItemCodeQueryOpen' => '/query ชื่อผู้ใช้',
'helpItemDescQueryClose' => 'ปิดห้องส่วนตัว:',
'helpItemCodeQueryClose' => '/query',
'helpItemDescAction' => 'บอกสิ่งกระทำ:',
'helpItemCodeAction' => '/action สิ่งที่ำกำลังทำ',
'helpItemDescDescribe' => 'บอกสิ่งที่กระทำในข้อความส่วนตัว:',
'helpItemCodeDescribe' => '/describe ชื่อผู้ใช้ ข้อความ',
'helpItemDescIgnore' => 'ไม่สนใจ/ยอมรับ ข้อความจากผู้ใช้:',
'helpItemCodeIgnore' => '/ignore ชื่อผู้ใช้',
'helpItemDescIgnoreList' => 'รายการผู้ใช้ที่ไม่สนใจ:',
'helpItemCodeIgnoreList' => '/ignore',
'helpItemDescWhereis' => 'แสดงผู้ใช้อยู่ในห้อง:',
'helpItemCodeWhereis' => '/whereis ชื่อผู้ใช้',
'helpItemDescKick' => 'ไล่ผู้ใช้ (ผู้ดูแลเท่านั้น):',
'helpItemCodeKick' => '/kick ผู้ใช้ [แบนในหนึ่งนาที]',
'helpItemDescUnban' => 'ยกเลิกแบนผู้ใช้ (ผู้ดูแลเท่านั้น):',
'helpItemCodeUnban' => '/unban ชื่อผู้ใช้',
'helpItemDescBans' => 'รายการผู้ใช้ถูกแบน (ผู้ดูแลเท่านั้น):',
'helpItemCodeBans' => '/bans',
'helpItemDescWhois' => 'แสดง IP ของผู้ใช้ (ผู้ดูแลเท่านั้น):',
'helpItemCodeWhois' => '/whois ชื่อผู้ใช้',
'helpItemDescWho' => 'รายชื่อผู้ใช้ที่ออนไลน์:',
'helpItemCodeWho' => '/who [ชื่อห้อง]',
'helpItemDescList' => 'รายชื่อห้องที่มีอยู่:',
'helpItemCodeList' => '/list',
'helpItemDescRoll' => 'ทอยลูกเต๋า:',
'helpItemCodeRoll' => '/roll [เลข]d[ด้าน]',
'helpItemDescNick' => 'เปลี่ยนชื่อผู้ใช้:',
'helpItemCodeNick' => '/nick ชื่อผู้ใช้ใหม่',
'settings' => 'ตั้งค่า',
'settingsBBCode' => 'เปิดใช้งาน BBCode:',
'settingsBBCodeImages' => 'เปิดใช้งาน BBCode ภาพ:',
'settingsBBCodeColors' => 'เปิดใช้งาน อักษรสี BBCode:',
'settingsHyperLinks' => 'เปิดใช้งานลิงค์:',
'settingsLineBreaks' => 'เปิดใช้งานการเว้นบรรทัด:',
'settingsEmoticons' => 'เปิดใช้งาน รูปแสดงอารมณ์:',
'settingsAutoFocus' => 'ตั้งโฟกัสในช่องพิมพ์อัตโนมัติ:',
'settingsMaxMessages' => 'อักษรที่สามารถให้พิมพ์ได้มากสุดในข้อความ:',
'settingsWordWrap' => 'เปิดใช้งานการเว้นบรรทัดข้อความที่ยาว:',
'settingsMaxWordLength' => 'ความาวของคำก่อนที่จะเว้นบรรทัดใหม่ให้:',
'settingsDateFormat' => 'รูปแบบวันที่และเวลา:',
'settingsPersistFontColor' => 'สีอักษรทั้งข้อความ:',
'settingsAudioVolume' => 'ระดับเสียง:',
'settingsSoundReceive' => 'เสียงสำหรับข้อความเข้า:',
'settingsSoundSend' => 'เสียงสำหรับข้อความออกไป:',
'settingsSoundEnter' => 'เสียงสำหรับเข้าระบบและเข้าห้อง:',
'settingsSoundLeave' => 'เสียงสำหรับออกจากระบบและออกจากห้อง:',
'settingsSoundChatBot' => 'เสียงสำหรับข้อความจาำกระบบ:',
'settingsSoundError' => 'เสียงสำหรับข้อความผิดพลาด:',
'settingsSoundPrivate' => 'Sound for private messages:',
'settingsBlink' => 'มีสัญญาณ กระพริบบน Title Bar เมื่อมีข้อความใหม่:',
'settingsBlinkInterval' => 'Blink interval in milliseconds:',
'settingsBlinkIntervalNumber' => 'Number of blink intervals:',
'playSelectedSound' => 'เล่นเสียงที่เลือก',
'requiresJavaScript' => 'ต้องการจาวาสคริปสำหรับห้องแชท',
'errorInvalidUser' => 'ชื่อผู้ใช้ผิดพลาด',
'errorUserInUse' => 'ชื่อผู้ใช้นี้กำลังใช้งานอยู่',
'errorBanned' => 'ผู้ใช้นี้หรือ IP นี้ถูกแบน',
'errorMaxUsersLoggedIn' => 'ห้องแชทเต็ม',
'errorChatClosed' => 'ห้องแชทถูกปิดชั่วคราว',
'logsTitle' => 'Socio!PARTY - บันทึกการใช้งาน',
'logsDate' => 'วันที่',
'logsTime' => 'เวลา',
'logsSearch' => 'ค้นหา',
'logsPrivateChannels' => 'ห้องส่วนตัว',
'logsPrivateMessages' => 'ข้อความส่วนตัว',
];?>
