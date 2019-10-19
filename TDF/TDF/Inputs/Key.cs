namespace TDF.Inputs
{
    public enum Key
    {
        Lbutton = 0x01, // Left mouse button
        Rbutton = 0x02, // Right mouse button
        Cancel = 0x03, // Control-break processing
        Mbutton = 0x04, // Middle mouse button (three-button mouse)
        Xbutton1 = 0x05, // X1 mouse button
        Xbutton2 = 0x06, // X2 mouse button
        Back = 0x08, // BACKSPACE key
        Tab = 0x09, // TAB key
        Clear = 0x0C, // CLEAR key
        Enter = 13, // ENTER key
        Shift = 0x10, // SHIFT key
        Control = 0x11, // CTRL key
        Alt = 0x12, // ALT key
        Pause = 0x13, // PAUSE key
        Capital = 0x14, // CAPS LOCK key
        Kana = 0x15, // IME Kana mode
        Hanguel = 0x15, // IME Hanguel mode
        Hangul = 0x15, // IME Hangul mode
        Junja = 0x17, // IME Junja mode
        Final = 0x18, // IME final mode
        Hanja = 0x19, // IME Hanja mode
        Kanji = 0x19, // IME Kanji mode

        Escape = 0x1B,
        Space = 0x20,
        Pageup = 0x21,
        Pagedown = 0x22,
        End = 0x23,
        Home = 0x24,
        Left = 0x25, // left arrow key
        Up = 0x26, // up arrow key
        Right = 0x27, // right arrow key
        Down = 0x28, // down arrow key
        Select = 0x29,
        Exe = 0x2B, // execute key
        Snapshot = 0x2C,
        Insert = 0x2D,
        Delete = 0x2E,
        Help = 0x2F,

        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,

        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        Winleft = 0x5B,
        Winright = 0x5C,
        APPS = 0x5D,

        N0 = 0x60,
        N1 = 0x61,
        N2 = 0x62,
        N3 = 0x63,
        N4 = 0x64,
        N5 = 0x65,
        N6 = 0x66,
        N7 = 0x67,
        N8 = 0x68,
        N9 = 0x69,

        Multiply = 0x6A,
        Add = 0x6B,
        Separator = 0x6C,
        Subtract = 0x6D,
        Decimal = 0x6E,
        Divide = 0x6F,

        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,

        NumLock = 0x90,
        Scroll = 0x91,

        LShift = 0xA0,
        RShift = 0xA1,
        LControl = 0xA2,
        RControl = 0xA3,
        LAlt = 0xA4,
        RAlt = 0xA5,

        Plus = 0xBB, // '+'
        Comma = 188, // ','
        Minus = 0xBD, // '-'
        Period = 190, // '.'

        Exponent = 0xDC, // '^'

        ATTN = 0xF6,
        CRSEL = 0xF7,
        EXSEL = 0xF8,
        EREOF = 0xF9,
        PLAY = 0xFA,
        ZOOM = 0xFB,
        NONAME = 0xFC,
        PA1 = 0xFD,
        OEMCLEAR = 0xFE,

        Oem1 = 186,
        Oem3 = 192,
        Oem4 = 219,
        Oem6 = 221,
        Oem7 = 222,

        MAX = 0xff
    }
}