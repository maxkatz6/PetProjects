using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Ormeli.Core
{
    public static class Cin
    {
        static Cin()
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
        }

        public static readonly Cons Console = new Cons();

        public class Cons
        {
            public Cons Int(out int i)
            {
                i = Read(int.Parse);
                return this;
            }

            public Cons Double(out double i)
            {
                i = Read(double.Parse);
                return this;
            }

            public Cons Float(out float i)
            {
                i = Read(float.Parse);
                return this;
            }

            public Cons String(out string s)
            {
                s = ReadWord();
                return this;
            }

            public Cons An<T>(out T t, Func<string, T> convertFunc)
            {
                t = convertFunc(ReadWord());
                return this;
            }
        }

        public class FileIn
        {
            public StreamReader Sr;

            public FileIn(StreamReader sr)
            {
                Sr = sr;
            }

            public FileIn Int(out int i)
            {
                i = Sr.Read(int.Parse);
                return this;
            }


            public FileIn Double(out double i)
            {
                i = Sr.Read(double.Parse);
                return this;
            }

            public FileIn Float(out float i)
            {
                i = Sr.Read(float.Parse);
                return this;
            }

            public FileIn String(out string s)
            {
                s = Sr.ReadWord();
                return this;
            }

            public FileIn An<T>(out T t, Func<string, T> convertFunc)
            {
                t = convertFunc(Sr.ReadWord());
                return this;
            }
        }

        public static FileIn Fin(this StreamReader sr)
        {
            return new FileIn(sr);
        }

        public static T Read<T>(this StreamReader sr, Func<string, T> convertFunc)
        {
            return convertFunc(sr.ReadWord());
        }

        public static string ReadWord(this StreamReader sr)
        {
            var tokenChars = new StringBuilder();
            bool skipWhiteSpaceMode = true;

            int nextChar = 0;
            while (nextChar != -1)
            {
                nextChar = sr.Read();
                var ch = (char) nextChar;

                if (nextChar < 0 || char.IsWhiteSpace(ch))
                {
                    if (skipWhiteSpaceMode) continue;
                    break;
                }
                skipWhiteSpaceMode = false;
                tokenChars.Append(ch);
            }
            return tokenChars.ToString();
        }

        public static T Read<T>(Func<string, T> convertFunc)
        {
            return convertFunc(ReadWord());
        }

        public static string ReadWord()
        {
            var tokenChars = new StringBuilder();
            bool skipWhiteSpaceMode = true;

            int nextChar = 0;
            while (nextChar != -1)
            {
            //    nextChar = System.Console.Read();
                var ch = (char) nextChar;

                if (char.IsWhiteSpace(ch))
                {
                    // Whitespace reached (' ', '\r', '\n', '\t') -->
                    // skip it if it is a leading whitespace
                    // or stop reading anymore if it is trailing
                    if (skipWhiteSpaceMode) continue;
                    if (ch == '\r' && (Environment.NewLine == "\r\n"))
                    {
                        // Reached '\r' in Windows --> skip the next '\n'
                   //     System.Console.Read();
                    }
                    break;
                }
                // Character reached --> append it to the output
                skipWhiteSpaceMode = false;
                tokenChars.Append(ch);
            }
            return tokenChars.ToString();
        }
    }
}
