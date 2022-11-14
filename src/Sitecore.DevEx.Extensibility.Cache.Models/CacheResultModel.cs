using System.Collections.Generic;
using Sitecore.DevEx.Logging;

namespace Sitecore.DevEx.Extensibility.Cache.Models;

public class CacheResultModel
{
    public bool Successful { get; set; } = true;
    public IEnumerable<OperationResult> OperationResults { get; set; } = new List<OperationResult>();
}