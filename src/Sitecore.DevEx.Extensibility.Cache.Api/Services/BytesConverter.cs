using System;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class BytesConverter : IBytesConverter
    {
        private static readonly string[] _sizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public string ToReadable(long value, int decimals = 2)
        {
            if (value == 0)
            {
                return string.Format("{0:n" + decimals + "} bytes", 0);
            }

            var sign = Math.Sign(value);
            value = Math.Abs(value);
            decimals = Math.Abs(decimals);

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            var mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimals) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimals + "} {1}",
                sign * adjustedSize,
                _sizeSuffixes[mag]);
        }
    }
}