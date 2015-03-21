/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @author Cydonian
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

// Ajax Chat language Object:
var sChatLang = {
	
	login: '%s sohbet odasına girdi.',
	logout: '%s sohbet odasından çıktı.',
	logoutTimeout: '%s sohbetten çıkarıldı (Bağlantı Gecikmesi).',
	logoutIP: '%s sohbetten çıkarıldı (Geçersiz IP adresi).',
	logoutKicked: '%s sohbetten çıkarıldı (Atıldı).',
	channelEnter: '%s kanala girdi.',
	channelLeave: '%s kanaldan çıktı.',
	privmsg: '(fısıldıyor)',
	privmsgto: '(%s size fısıldıyor)',
	invite: '%s sizi %s odasına davet ediyor.',
	inviteto: '%s kanalı için davetiniz %s e gönderildi.',
	uninvite: '%s sizi %s kanalından çıkmaya çağırıyor.',
	uninviteto: '%s kanalından çıkma davetiniz %s e gönderildi',
	queryOpen: '%s e özel kanal açıldı.',
	queryClose: '%s e özel kanal kapatıldı.',
	ignoreAdded: '%s blok listesine eklendi.',
	ignoreRemoved: '%s blok listesinden çıkarıldı.',
	ignoreList: 'Blok edilen üyeler:',
	ignoreListEmpty: 'Blok Listesi boş.',
	who: 'Çevrimiçi üyeler:',
	whoChannel: '%s kanalındaki çevrimiçi üyeler:',
	whoEmpty: 'Kanalda çevrimiçi üye yoktur.',
	list: 'Açık Odalar:',
	bans: 'Yasaklanan Üyeler:',
	bansEmpty: 'Yasaklı Listesi boş.',
	unban: '%s adlı üyenin yasağı kaldırıldı.',
	whois: 'Üye %s - IP adresi:',
	whereis: '%s adlı üye %s kanalında.',
	roll: '%s sallar %s ve alır %s.',
	nick: '%s rumuzunu %s yaptı.',
	toggleUserMenu: 'Toggle user menu for %s',
	userMenuLogout: 'Çıkış',
	userMenuWho: 'Çevrimiçi üyeleri göster',
	userMenuList: 'Uygun odaları göster',
	userMenuAction: 'Aksiyon:',
	userMenuRoll: 'Zarları at',
	userMenuNick: 'Rumuz değiştir',
	userMenuEnterPrivateRoom: 'Özel odaya gir',
	userMenuSendPrivateMessage: 'Özel mesaj gönder',
	userMenuDescribe: 'Özel aksiyon gönder',
	userMenuOpenPrivateChannel: 'Özel kanal aç',
	userMenuClosePrivateChannel: 'Özel kanalı kapat',
	userMenuInvite: 'Davet et',
	userMenuUninvite: 'Davet etme',
	userMenuIgnore: 'İptal/Kabul',
	userMenuIgnoreList: 'Bloklanmış üyeleri göster',
	userMenuWhereis: 'Kanalı göster',
	userMenuKick: 'At/Yasakla',
	userMenuBans: 'Yasaklanmış üyeleri göster',
	userMenuWhois: 'IP göster',
	unbanUser: 'Üye %s nin yasağını kaldır',
	joinChannel: '%s kanalına Gir',
	cite: '%s :',
	urlDialog: 'Web sayfasının adresini (URL) giriniz:',
	deleteMessage: 'Bu mesajı sil',
	deleteMessageConfirm: 'İşaretli mesajı gerçekten silmek istiyor musunuz?',
	errorCookiesRequired: 'Bu sohbet için Cookies açık olmalıdır.',
	errorUserNameNotFound: 'Hata: %s adlı üye bulunamadı.',
	errorMissingText: 'Hata: Mesaj yazısı yok.',
	errorMissingUserName: 'Hata: Üye adı yok.',
	errorInvalidUserName: 'Hata: Geçersiz üye adı.',
	errorUserNameInUse: 'Hata: Üye adı kullanımda.',
	errorMissingChannelName: 'Hata: Kanal adı yok.',
	errorInvalidChannelName: 'Hata: Geçersiz kanal adı: %s',
	errorPrivateMessageNotAllowed: 'Hata: Özel mesajlara izin verilmiyor.',
	errorInviteNotAllowed: 'Hata: Başka bir üyeyi bu kanala davet etme izniniz yok.',
	errorUninviteNotAllowed: 'Hata: Başka bir üyeyi bu kanaldan dışarı davete izniniz yok.',
	errorNoOpenQuery: 'Hata: Açık özel kanal yok.',
	errorKickNotAllowed: 'Hata: %s adlı üyeyi atma yetkiniz yok.',
	errorCommandNotAllowed: 'Hata: Bu komuta izin yok: %s',
	errorUnknownCommand: 'Hata: Bilinmeyen komut: %s',
	errorMaxMessageRate: 'Hata: Bir dakika içinde atılabilecek maksimum mesaj sayısına ulaştınız.',
	errorConnectionTimeout: 'Hata: Bağlantı süresi aşımı. Lütfen tekrar deneyin.',
	errorConnectionStatus: 'Hata: Bağlantı durumu: %s',
	errorSoundIO: 'Hata: Ses dosyası yüklenemedi (Flash IO Hatası).',
	errorSocketIO: 'Hata: Socket server bağlantısı yapılamadı (Flash IO Hatası).',
	errorSocketSecurity: 'Hata: Socket server bağlantısı yapılamadı(Flash Güvenlik Hatası).',
	errorDOMSyntax: 'Hata: Geçersiz DOM Sözdizimi (DOM ID: %s).'
	
}