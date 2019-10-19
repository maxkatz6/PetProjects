using System;
using System.Runtime.InteropServices;
using System.Text;
using SharpDX;

namespace TDF.Core
{
    /// <summary>
    /// WinAPI func and wrappers methods
    /// </summary>
    public static class WinAPI
    {
        private const uint CfUnicodetext = 13;

        /// <summary>
        /// Copy text from clipboard.
        /// </summary>
        /// <returns></returns>
        public static string CopyFromClipboard()
        {
            if (!OpenClipboard(IntPtr.Zero)) return string.Empty;
            if (!IsClipboardFormatAvailable(CfUnicodetext)) return string.Empty;

            var h = GetClipboardData(CfUnicodetext);
            var t = GlobalLock(h);

            GlobalUnlock(h);
            CloseClipboard();

            return Marshal.PtrToStringUni(t);
        }

        /// <summary>
        /// Copy text to clipboard
        /// </summary>
        /// <param name="text">The text.</param>
        public static void CopyToClipboard(string text)
        {
            if (!OpenClipboard(IntPtr.Zero)) return;

            EmptyClipboard();
            var hClipboardData = Marshal.StringToHGlobalUni(text);

            GlobalUnlock(hClipboardData);

            SetClipboardData(CfUnicodetext, hClipboardData);

            CloseClipboard();
        }

        public static int MessageBox(string text, string caption = "Message")
        {
            return MessageBox(0, text, caption, 0);
        }

        public static Point GetCurPos()
        {
            Point p;
            GetCursorPos(out p);
            return p;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern int MessageBox(int hWnd, String text, String caption, uint type);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EmptyClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint handle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool OpenClipboard(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetClipboardData(uint format, IntPtr textIntPtr);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point lpPoint);
    }
}