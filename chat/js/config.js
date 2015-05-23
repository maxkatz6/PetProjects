var sConfig={
    // The channelID of the channel to enter on login (the loginChannelName is used if set to null):
    loginChannelID:null,
    // The channelName of the channel to enter on login (the default channel is used if set to null):
    loginChannelName:null,
    // The time in ms between update calls to retrieve new chat messages:
    timerRate:3000,
    // The URL to retrieve the XML chat messages (must at least contain one parameter):
    ajaxURL:'./?ajax=true',
    // The base URL of the chat directory, used to retrieve media files (images, sound files, etc.):
    baseURL:'./',
    // Defines the IDs of DOM nodes accessed by the chat:
    domIDs:{
        chatList:'chatList',
        onlineList:'onlineList',
        inputField:'inputField',
        messageLengthCounter:'messageLengthCounter',
        channelSelection:'channelSelection',
        styleSelection:'styleSelection',
        emoticonsContainer:'emoticonsContainer',
        flashInterfaceContainer:'flashInterfaceContainer',
        statusIcon:'statusIconContainer'
    },
    // Defines the settings which can be modified by users:
    settings:{
        saveAddressee:true,
        bbCode:true,
        msgColors:true,
        nickColors:true,
        gradiens:true,
        hyperLinks:true,
        emoticons:true,
        autoScroll:true,
        fontColor:'#000000',
        nickColor:'#000000',
        audio:true,
        audioVolume:1.0,
        soundReceive:'sound_1',
        soundSend:'sound_2',
        soundEnter:'sound_3',
        soundLeave:'sound_4',
        soundChatBot:'sound_5',
        soundError:'sound_6',
        soundPrivate:'sound_7',
        myVideo:true,
        myAudio:true,
        vkPosts:true
    },
    // Defines the list of allowed BBCodes:
    bbCodeTags:['b', 'i', 's', 'quote', 'tgw', 'q'],
    // Defines the list of allowed color codes:
    colorCodes:[
        '000000', '494949', '717171', '41166f', '53377a', '4b0082',
        '6835ba', '9739b7', '810aa9', 'a70ddb', '735184', 'a60073',
        'ba0671', 'bb438a', '71001e', 'a5003c', 'b80642', 'c32025',
        'fc2847', 'dc143c', 'cc3737', 'e3691e', 'c06411', 'eb9d58',
        'c4b122', 'a6b929', '929e32', '609c0c', '67b64b', '00af4c',
        '009d7a', '03c89c', '00aab7', '017e90', '0687c1', '01adeb',
        '007fff', '0078b7', '0558a6', '1065dc', '0d14e5', '00416a'
    ],
    // Defines the list of allowed emoticon codes:
    emoticonCodes:[
        ':acute', ':aggressive', ':bad', ':biggrin', ':blum1', ':blush', ':bomb', ':boredom', ':bye', ':clapping', ':congratulate', ':cool',
        ':cray', ':dance', ':diablo', ':drinks', ':empathy', ':flag_of_truce', ':fool', ':good', ':greeting', ':help', ':hi', ':hmm', ':so_happy', ':lol',
        ':mad', ':mocking', ':music', ':nea', ':negative', ':new_russian', ':nyam1', ':ok', ':pardon', ':playboy', ':pleasantry', ':praising', ':rofl',
        ':rolleyes', ':sad', ':scare', ':scratch_one-s_head', ':secret', ':shok', ':shout', ':smile', ':sorry', ':stop', ':timeout', ':unknown', ':wink', ':yahoo',
        ':c_01', ':c_02', ':c_03',
        ':c_04', ':c_05', ':c_06',
        ':c_07', ':c_08', ':c_09',
        ':c_10', ':c_11', ':c_12',
        ':c_13', ':c_14', ':c_15',
        ':p_01', ':p_02', ':p_03',
        ':p_04', ':p_05', ':p_06',
        ':p_07', ':p_08', ':p_09',
        ':p_10', ':p_11', ':p_12',
        ':p_13', ':p_14', ':p_15',
        ':p_16', ':p_17', ':p_18',
        ':p_19', ':p_20', ':p_21',
        ':p_22', ':p_23', ':p_24'
    ],
    // Defines the list of emoticon files associated with the emoticon codes:
    emoticonFiles:[
        'acute.gif ', 'aggressive.gif', 'bad.gif', 'biggrin.gif', 'blum1.gif', 'blush.gif', 'bomb.gif', 'boredom.gif', 'bye.gif', 'clapping.gif', 'congratulate.gif', 'cool.gif',
        'cray.gif', 'dance.gif', 'diablo.gif', 'drinks.gif', 'empathy.gif', 'flag_of_truce.gif', 'fool.gif', 'good.gif', 'greeting.gif', 'help.gif', 'hi.gif', 'hmm.gif', 'so_happy.gif', 'lol.gif',
        'mad.gif', 'mocking.gif', 'music.gif', 'nea.gif', 'negative.gif', 'new_russian.gif', 'nyam1.gif', 'ok.gif', 'pardon.gif', 'playboy.gif', 'pleasantry.gif', 'praising.gif', 'rofl.gif',
        'rolleyes.gif', 'sad.gif', 'scare.gif', 'scratch_one-s_head.gif', 'secret.gif', 'shok.gif', 'shout.gif', 'smile.gif', 'sorry.gif', 'stop.gif', 'timeout.gif', 'unknown.gif', 'wink.gif', 'yahoo.gif',
        'c_01.png', 'c_02.png', 'c_03.png',
        'c_04.png', 'c_05.png', 'c_06.png',
        'c_07.png', 'c_08.png', 'c_09.png',
        'c_10.png', 'c_11.png', 'c_12.png',
        'c_13.png', 'c_14.png', 'c_15.png',
        'p_01.png', 'p_02.png', 'p_03.png',
        'p_04.png', 'p_05.png', 'p_06.png',
        'p_07.png', 'p_08.png', 'p_09.png',
        'p_10.png', 'p_11.png', 'p_12.png',
        'p_13.png', 'p_14.png', 'p_15.png',
        'p_16.png', 'p_17.png', 'p_18.png',
        'p_19.png', 'p_20.png', 'p_21.png',
        'p_22.png', 'p_23.png', 'p_24.png'
    ],
    // Defines the available sounds loaded on chat start:
    soundFiles:{
        sound_1:'sound_1.mp3',
        sound_2:'sound_2.mp3',
        sound_3:'sound_3.mp3',
        sound_4:'sound_4.mp3',
        sound_5:'sound_5.mp3',
        sound_6:'sound_6.mp3',
        sound_7:'sound_7.mp3',
        sound_8:'sound_8.mp3'
    },
    // Once users have been logged in, the following values are overridden by those in config.php.
    // You should set these to be the same as the ones in config.php to avoid confusion.
    // Session identification, used for style and setting cookies:
    sessionName:'s_chat',
    // The time in days until the style and setting cookies expire:
    cookieExpiration:365,
    // The path of the cookies, '/' allows to read the cookies from all directories:
    cookiePath:'/',
    // The domain of the cookies, defaults to the hostname of the server if set to null:
    cookieDomain:null,
    // If enabled, cookies must be sent over secure (SSL/TLS encrypted) connections:
    cookieSecure:null,
    // The name of the chat bot:
    chatBotName:'Сервер',
    // The userID of the chat bot:            
    chatBotID:2147483647,
    // Allow/Disallow registered users to delete their own messages:
    allowUserMessageDelete:true,
    // Minutes until a user is declared inactive (last status update) - the minimum is 2 minutes:
    inactiveTimeout:2,
    // UserID plus this value are private channels (this is also the max userID and max channelID):
    privateChannelDiff:500000000,
    // UserID plus this value are used for private messages:
    privateMessageDiff:1000000000,
    // Defines if login/logout and channel enter/leave are displayed:
    showChannelMessages:true,
    // Max messageText length:
    messageTextMaxLength:1040,
    // Debug allows console logging or alerts on caught errors - false/0 = no debug, true/1/2 = console log, 2 = alerts
    debug:true,
    ///videochat
    videoChat:true,
    defCamS:0.15,
    maxVidW:320,
    maxVidH:240,
    vidFPS:10,
    iceServers:[
        { url:'turn:numb.viagenie.ca', credential:'wanderer12', username:'maxim1296@yandex.ru' },
        { url:'stun:numb.viagenie.ca', credential:'wanderer12', username:'maxim1296@yandex.ru' },
        { url:'stun:stun01.sipphone.com' },
        { url:'stun:stun.ekiga.net' },
        { url:'stun:stun.fwdnet.net' },
        { url:'stun:stun.ideasip.com' },
        { url:'stun:stun.iptel.org' },
        { url:'stun:stun.rixtelecom.se' },
        { url:'stun:stun.schlund.de' },
        { url:'stun:stun.l.google.com:19302' },
        { url:'stun:stun1.l.google.com:19302' },
        { url:'stun:stun2.l.google.com:19302' },
        { url:'stun:stun3.l.google.com:19302' },
        { url:'stun:stun4.l.google.com:19302' },
        { url:'stun:stunserver.org' },
        { url:'stun:stun.softjoys.com' },
        { url:'stun:stun.voiparound.com' },
        { url:'stun:stun.voipbuster.com' },
        { url:'stun:stun.voipstunt.com' },
        { url:'stun:stun.voxgratia.org' },
        { url:'stun:stun.xten.com' },
        { url:'stun:numb.viagenie.ca', credential:'muazkh', username:'webrtc@live.com' },
        { url:'turn:numb.viagenie.ca', credential:'muazkh', username:'webrtc@live.com' },
        { url:'turn:192.158.29.39:3478?transport=udp', credential:'JZEOEt2V3Qb0y27GRntt2u2PAYA=', username:'28224511:1379330808' },
        { url:'turn:192.158.29.39:3478?transport=tcp', credential:'JZEOEt2V3Qb0y27GRntt2u2PAYA=', username:'28224511:1379330808' }
    ],
    //status
    statImg:[
        'na_meste.png', 'menya_net.png', 'skoro_budu.png', 'ne_bespokoit.png', 'otoshel_pokurit.png',
        'granit.png', 'igrayu.png', 'smotry_kino.png', 'sply.png', 'rab.png', 'party_hard!.png',
        'ne_v_nastroyeniji.png', 'v_yarosti.png', 'bolen.png', 'em.png', 'pyan.png', 'utochka.png', 'webcam.png','utochka.png'
    ],
    statText:[
        'На месте', 'Нет на месте', 'Скоро буду', 'Не беспокоить', 'Отошел покурить',
        'Грызу гранит науки', 'Играю', 'Смотрю кино', 'Сплю', 'Работаю', 'Party Hard!',
        'Не в настроении', 'В ярости', 'Болен', 'Кушаю', 'Пьян', 'Я - уточко', 'Подключен к видеоканалу','--'
    ]
};