using System;
using System.Collections.Generic;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public interface IEnumService
    {
        IEnumerable<TEnum> GetFlagValues<TEnum>(TEnum value) where TEnum : Enum;
    }
}