using System;
using System.Collections.Generic;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class EnumService : IEnumService
    {
        public IEnumerable<TEnum> GetFlagValues<TEnum>(TEnum value) where TEnum : Enum
        {
            foreach (Enum enumValue in Enum.GetValues(typeof(TEnum)))
                if (value.HasFlag(enumValue))
                    yield return (TEnum)enumValue;
        }
    }
}