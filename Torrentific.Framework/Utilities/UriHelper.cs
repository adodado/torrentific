// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="UriHelper.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Torrentific.Framework.Utilities
{
    /// <summary>
    /// Class UriHelper.
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// The hexadecimal chars
        /// </summary>
        private static readonly char[] HexChars = "0123456789abcdef".ToCharArray();

        /// <summary>
        /// URLs the encode.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">bytes</exception>
        public static string UrlEncode(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            using (var result = new MemoryStream(bytes.Length))
            {
                foreach (var t in bytes)
                    UrlEncodeChar((char) t, result, false);

                return Encoding.ASCII.GetString(result.ToArray());
            }
        }

        /// <summary>
        /// URLs the decode.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] UrlDecode(string s)
        {
            if (null == s)
                return null;

            var e = Encoding.UTF8;
            if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
                return e.GetBytes(s);

            long len = s.Length;
            var bytes = new List<byte>();

            for (var i = 0; i < len; i++)
            {
                var ch = s[i];
                if (ch == '%' && i + 2 < len && s[i + 1] != '%')
                {
                    int xchar;
                    if (s[i + 1] == 'u' && i + 5 < len)
                    {
                        // unicode hex sequence
                        xchar = GetChar(s, i + 2, 4);
                        if (xchar != -1)
                        {
                            WriteCharBytes(bytes, (char) xchar, e);
                            i += 5;
                        }
                        else
                            WriteCharBytes(bytes, '%', e);
                    }
                    else if ((xchar = GetChar(s, i + 1, 2)) != -1)
                    {
                        WriteCharBytes(bytes, (char) xchar, e);
                        i += 2;
                    }
                    else
                    {
                        WriteCharBytes(bytes, '%', e);
                    }
                    continue;
                }

                WriteCharBytes(bytes, ch == '+' ? ' ' : ch, e);
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// URLs the encode character.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="result">The result.</param>
        /// <param name="isUnicode">if set to <c>true</c> [is unicode].</param>
        private static void UrlEncodeChar(char c, Stream result, bool isUnicode)
        {
            if (c > ' ' && NotEncoded(c))
            {
                result.WriteByte((byte) c);
                return;
            }
            if (c == ' ')
            {
                result.WriteByte((byte) '+');
                return;
            }
            if ((c < '0') ||
                (c < 'A' && c > '9') ||
                (c > 'Z' && c < 'a') ||
                (c > 'z'))
            {
                if (isUnicode && c > 127)
                {
                    result.WriteByte((byte) '%');
                    result.WriteByte((byte) 'u');
                    result.WriteByte((byte) '0');
                    result.WriteByte((byte) '0');
                }
                else
                    result.WriteByte((byte) '%');

                var idx = c >> 4;
                result.WriteByte((byte) HexChars[idx]);
                idx = c & 0x0F;
                result.WriteByte((byte) HexChars[idx]);
            }
            else
            {
                result.WriteByte((byte) c);
            }
        }

        /// <summary>
        /// Gets the character.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Int32.</returns>
        private static int GetChar(string str, int offset, int length)
        {
            var val = 0;
            var end = length + offset;
            for (var i = offset; i < end; i++)
            {
                var c = str[i];
                if (c > 127)
                    return -1;

                var current = GetInt((byte) c);
                if (current == -1)
                    return -1;
                val = (val << 4) + current;
            }

            return val;
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>System.Int32.</returns>
        private static int GetInt(byte b)
        {
            var c = (char) b;
            if (c >= '0' && c <= '9')
                return c - '0';

            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;

            return -1;
        }

        /// <summary>
        /// Nots the encoded.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private static bool NotEncoded(char c)
        {
            return c == '!' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_' || c == '\'';
        }

        /// <summary>
        /// Determines whether [is hearing impaired line] [the specified line].
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns><c>true</c> if [is hearing impaired line] [the specified line]; otherwise, <c>false</c>.</returns>
        private static bool IsHearingImpairedLine(string line)
        {
            return line.Any(c => c == '(' || c == ')' || c == '[' || c == ']' || c == '{' || c == '}');
        }

        /// <summary>
        /// Writes the character bytes.
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="ch">The ch.</param>
        /// <param name="e">The e.</param>
        private static void WriteCharBytes(List<byte> buf, char ch, Encoding e)
        {
            if (ch > 255)
                buf.AddRange(e.GetBytes(new[] {ch}));
            else
                buf.Add((byte) ch);
        }
    }
}