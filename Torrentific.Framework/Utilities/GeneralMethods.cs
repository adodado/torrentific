// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="GeneralMethods.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Globalization;

namespace Torrentific.Framework.Utilities
{
    /// <summary>
    /// Class GeneralMethods.
    /// </summary>
    public static class GeneralMethods
    {
        /// <summary>
        /// Numbers the size of to file.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string NumberToFileSize(long value)
        {
            string[] suf = {"B", "KB", "MB", "GB", "TB", "PB", "EB"};

            if (value == 0)
                return "0 " + suf[0];

            var bytes = Math.Abs(value);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes/Math.Pow(1024, place), 1);

            return string.Format(NumberFormatInfo.InvariantInfo, "{0:0.0} {1}", Math.Sign(value)*num, suf[place]);
        }

        /// <summary>
        /// Numbers to speed.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string NumberToSpeed(int value)
        {
            string[] suf = {"b/s", "kB/s", "mB/s", "gB/s", "tB/s", "pB/s", "eB/s"};

            if (value == 0)
                return "0 " + suf[0];

            var bytes = Math.Abs(value);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes/Math.Pow(1024, place), 0);

            return Math.Sign(value)*num + " " + suf[place];
        }

        /// <summary>
        /// Gets the enum value description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetEnumValueDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}