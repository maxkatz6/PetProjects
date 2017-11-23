var sConfig = {
    // The channelID of the channel to enter on login (the loginChannelName is used if set to null):
    loginChannelID: null,
    // The channelName of the channel to enter on login (the default channel is used if set to null):
    loginChannelName: null,
    // The time in ms between update calls to retrieve new chat messages:
    timerRate: 3000,
    // 
    messageUnionTimeLimit: 1 * 60 * 1000,
    // The URL to retrieve the XML chat messages (must at least contain one parameter):
    ajaxURL: './?ajax=true',
    // Defines the IDs of DOM nodes accessed by the chat:
    domIDs: {
        chatList: 'chatList',
        onlineList: 'onlineList',
        inputField: 'inputField',
        messageLengthCounter: 'messageLengthCounter',
        channelSelection: 'channelSelection',
        styleSelection: 'styleSelection',
        emoticonsContainer: 'emoticonsContainer',
        flashInterfaceContainer: 'flashInterfaceContainer',
        statusIcon: 'statusIconContainer'
    },
    // Defines the settings which can be modified by users:
    settings: {
        saveAddressee: true,
        bbCode: true,
        msgColors: true,
        nickColors: true,
        gradiens: true,
        hyperLinks: true,
        emoticons: true,
        autoScroll: true,
        audio: true,
        audioVolume: 1.0,
        soundReceive: 'sound_1',
        soundSend: 'sound_2',
        soundEnter: 'sound_3',
        soundLeave: 'sound_4',
        soundChatBot: 'sound_5',
        soundError: 'sound_6',
        soundPrivate: 'sound_7',
        myVideo: true,
        myAudio: true,
        vkPosts: true,
        radio: true
    },
    // Defines the list of allowed BBCodes:
    bbCodeTags: ['b', 'i', 's', 'quote', 'tgw', 'q'],
    // Defines the list of allowed color codes:
    colorCodes: [
        '000000', '494949', '717171', '41166f', '53377a', '4b0082',
        '6835ba', '9739b7', '810aa9', 'a70ddb', '735184', 'a60073',
        'ba0671', 'bb438a', '71001e', 'a5003c', 'b80642', 'c32025',
        'fc2847', 'dc143c', 'cc3737', 'e3691e', 'c06411', 'eb9d58',
        'c4b122', 'a6b929', '929e32', '609c0c', '67b64b', '00af4c',
        '009d7a', '03c89c', '00aab7', '017e90', '0687c1', '01adeb',
        '007fff', '0078b7', '0558a6', '1065dc', '0d14e5', '00416a'
    ],
    // Defines the list of allowed emoticon:
    emoticons: [{
        title: "Пуш",
        id: "push",
        icon: "p_01.png",
        class: "sticker",
        codes: ['push01', 'push02', 'push03',
            'push04', 'push05', 'push06',
            'push07', 'push08', 'push09',
            'push10', 'push11', 'push12',
            'push13', 'push14', 'push15',
            'push16', 'push17', 'push18',
            'push19', 'push20', 'push21'],
        files: ['p_01.png', 'p_02.png', 'p_03.png',
            'p_04.png', 'p_05.png', 'p_06.png',
            'p_07.png', 'p_08.png', 'p_09.png',
            'p_10.png', 'p_11.png', 'p_12.png',
            'p_13.png', 'p_14.png', 'p_15.png',
            'p_16.png', 'p_17.png', 'p_18.png',
            'p_19.png', 'p_20.png', 'p_21.png']
    }, {
        title: "Сова",
        id: "owl",
        icon: "s_01.png",
        class: "sticker",
        codes: ['s_01', 's_02', 's_03',
            's_04', 's_05', 's_06',
            's_07', 's_08', 's_09',
            's_10', 's_11', 's_12',
            's_13', 's_14', 's_15',
            's_16', 's_17', 's_18',
            's_19', 's_20', 's_21',
            's_22', 's_23', 's_24',
            's_25', 's_26', 's_27',
            's_28',],
        files: ['s_01.png', 's_02.png', 's_03.png',
            's_04.png', 's_05.png', 's_06.png',
            's_07.png', 's_08.png', 's_09.png',
            's_10.png', 's_11.png', 's_12.png',
            's_13.png', 's_14.png', 's_15.png',
            's_16.png', 's_17.png', 's_18.png',
            's_19.png', 's_20.png', 's_21.png',
            's_22.png', 's_23.png', 's_24.png',
            's_25.png', 's_26.png', 's_27.png',
            's_28.png',]
    }, {
        title: "Игра престолов",
        id: "got",
        icon: "i_01.png",
        class: "sticker",
        codes: ['i_01', 'i_02', 'i_03',
            'i_04', 'i_05', 'i_06',
            'i_07', 'i_08', 'i_09',
            'i_10', 'i_11', 'i_12',
            'i_13', 'i_14', 'i_15',
            'i_16', 'i_17', 'i_18',
            'i_19', 'i_20', 'i_21',
            'i_22', 'i_23', 'i_24',],
        files: ['i_01.png', 'i_02.png', 'i_03.png',
            'i_04.png', 'i_05.png', 'i_06.png',
            'i_07.png', 'i_08.png', 'i_09.png',
            'i_10.png', 'i_11.png', 'i_12.png',
            'i_13.png', 'i_14.png', 'i_15.png',
            'i_16.png', 'i_17.png', 'i_18.png',
            'i_19.png', 'i_20.png', 'i_21.png',
            'i_22.png', 'i_23.png', 'i_24.png',]
    }, {
        title: "Лис",
        id: "fox",
        icon: "l_01.png",
        class: "sticker",
        codes: ['l_01', 'l_02', 'l_03',
            'l_04', 'l_05', 'l_06',
            'l_07', 'l_08', 'l_09',
            'l_10', 'l_11', 'l_12',
            'l_13', 'l_14', 'l_15'],
        files: ['l_01.png', 'l_02.png', 'l_03.png',
            'l_04.png', 'l_05.png', 'l_06.png',
            'l_07.png', 'l_08.png', 'l_09.png',
            'l_10.png', 'l_11.png', 'l_12.png',
            'l_13.png', 'l_14.png', 'l_15.png']
    }],
    // Defines the available sounds loaded on chat start:
    soundFiles: {
        sound_1: 'sound_1.mp3',
        sound_2: 'sound_2.mp3',
        sound_3: 'sound_3.mp3',
        sound_4: 'sound_4.mp3',
        sound_5: 'sound_5.mp3',
        sound_6: 'sound_6.mp3',
        sound_7: 'sound_7.mp3',
        sound_8: 'sound_8.mp3'
    },
    // Once users have been logged in, the following values are overridden by those in config.php.
    // You should set these to be the same as the ones in config.php to avoid confusion.
    // Session identification, used for style and setting cookies:
    sessionName: 's_chat',
    // The time in days until the style and setting cookies expire:
    cookieExpiration: 365,
    // The path of the cookies, '/' allows to read the cookies from all directories:
    cookiePath: '/',
    // The domain of the cookies, defaults to the hostname of the server if set to null:
    cookieDomain: null,
    // If enabled, cookies must be sent over secure (SSL/TLS encrypted) connections:
    cookieSecure: null,
    // The name of the chat bot:
    chatBotName: 'Сервер',
    // The userID of the chat bot:            
    chatBotID: 2147483647,
    // Allow/Disallow registered users to delete their own messages:
    allowUserMessageDelete: true,
    // Minutes until a user is declared inactive (last status update) - the minimum is 2 minutes:
    inactiveTimeout: 2,
    // UserID plus this value are private channels (this is also the max userID and max channelID):
    privateChannelDiff: 500000000,
    // UserID plus this value are used for private messages:
    privateMessageDiff: 1000000000,
    // Defines if login/logout and channel enter/leave are displayed:
    showChannelMessages: true,
    // Max messageText length:
    messageTextMaxLength: 1040,
    // Debug allows console logging or alerts on caught errors - false/0 = no debug, true/1/2 = console log, 2 = alerts
    debug: true,
    radioServer: false,
    mobile: false,
    // file upload in kb
    maxFileSize: 10*1024*1024,
    ///videochat
    videoChat: true,
    defCamS: 0.15,
    maxVidW: 320,
    maxVidH: 240,
    vidFPS: 10,
    webRtcSignalServer: "https://109.87.163.34:8888/socket.io/",
    iceServers: [
        { url: 'stun:stun01.sipphone.com' },
        { url: 'stun:stun.ekiga.net' },
        { url: 'stun:stun.fwdnet.net' },
        { url: 'stun:stun.ideasip.com' },
        { url: 'stun:stun.iptel.org' },
        { url: 'stun:stun.rixtelecom.se' },
        { url: 'stun:stun.schlund.de' },
        { url: 'stun:stun.l.google.com:19302' },
        { url: 'stun:stun1.l.google.com:19302' },
        { url: 'stun:stun2.l.google.com:19302' },
        { url: 'stun:stun3.l.google.com:19302' },
        { url: 'stun:stun4.l.google.com:19302' },
        { url: 'stun:stunserver.org' },
        { url: 'stun:stun.softjoys.com' },
        { url: 'stun:stun.voiparound.com' },
        { url: 'stun:stun.voipbuster.com' },
        { url: 'stun:stun.voipstunt.com' },
        { url: 'stun:stun.voxgratia.org' },
        { url: 'stun:stun.xten.com' },
        {
            url: 'turn:numb.viagenie.ca',
            credential: 'muazkh',
            username: 'webrtc@live.com'
        },
        {
            url: 'turn:192.158.29.39:3478?transport=udp',
            credential: 'JZEOEt2V3Qb0y27GRntt2u2PAYA=',
            username: '28224511:1379330808'
        },
        {
            url: 'turn:192.158.29.39:3478?transport=tcp',
            credential: 'JZEOEt2V3Qb0y27GRntt2u2PAYA=',
            username: '28224511:1379330808'
        }
    ],
    //status
    statImg: [
        '1.png', '2.png', '3.png', '4.png', '5.png',
        '6.png', '7.png', '8.png', '9.png', '10.png', '11.png',
        '12.png', '13.png', '14.png', '15.png', '16.png', '17.png',
        '18.png', '19.png', '20.png', '21.png'
    ],
    statText: [
        'На месте', 'Нет на месте', 'Скоро буду', 'На работе', 'На учёбе',
        'Болею', 'Играю', 'Смотрю кино', 'Слушаю музыку',
        'Читаю', 'Пьяненький', 'Перекур', 'Кушаю', 'Сплю',
        'Творю', 'Отдыхаю', 'Веселюсь', 'Хочу убивать',
        'Безудержное веселье', 'Превед, медвед!', 'На проводе'
    ],
    webCamStatId: -2,
    webCamStatImg: 'webcam.png',
    webCamStatText: 'Подключен к видеоканалу',
    specialStatId: -1,
    specialStatImg: 'special.png',
    specialStatText: '--Свой статус--'
};