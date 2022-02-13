using System;
using System.Globalization;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class BytesConverter : IBytesConverter
    {
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public string ToReadable(long value, int decimals = 2)
        {
            if (value == 0)
            {
                return $"{0.ToString("N" + decimals, CultureInfo.InvariantCulture)} bytes";
            }

            var sign = Math.Sign(value);
            value = Math.Abs(value);
            decimals = Math.Abs(decimals);

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            var mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            var adjustedSize = (decimal)value / (1L << (mag * 10));
            var size = sign * adjustedSize;
            return $"{size.ToString("N" + decimals, CultureInfo.InvariantCulture)} {SizeSuffixes[mag]}";
        }
    }
}