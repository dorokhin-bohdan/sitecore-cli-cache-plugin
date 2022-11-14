using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace Sitecore.DevEx.Extensibility.Publishing;

/// <summary>
/// All possible switches to CLI commands
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ArgOptions
{
    // GENERIC
    internal static readonly Option<string> Config = new(new[] { "--config", "-c" }, () => Environment.CurrentDirectory, "Path to root sitecore.json directory (default: cwd).");

    internal static readonly Option<string> EnvironmentName = new(new[] { "--environment-name", "-n" }, "Named Sitecore environment to use. Default: 'default'.");

    internal static readonly Option<bool> Verbose = new(new[] { "--verbose", "-v" }, () => false, "Write some additional diagnostic and performance data.");

    internal static readonly Option<bool> Trace = new(new[] { "--trace", "-t" }, () => false, "Write more additional diagnostic and performance data.");
        
    internal static readonly Option<string> SiteName = new(new[] { "--site-name", "-s" }, "Named Sitecore site to use. Default: 'website'.");

    internal static readonly Option<bool> ClearData = new(new[] { "--clear-data", "--cd" }, () => false, "Task that will clear Data cache.");
        
    internal static readonly Option<bool> ClearHtml = new(new[] { "--clear-html", "--ch" }, () => false, "Task that will clear Html cache.");
        
    internal static readonly Option<bool> ClearItem = new(new[] { "--clear-item", "--ci" }, () => false, "Task that will clear Item cache.");
        
    internal static readonly Option<bool> ClearPath = new(new[] { "--clear-path", "--cp" }, () => false, "Task that will clear Path cache.");
        
    internal static readonly Option<bool> ClearItemPaths = new(new[] { "--clear-item-paths", "--cip" }, () => false, "Task that will clear Item Paths cache.");
        
    internal static readonly Option<bool> ClearStandardValues = new(new[] { "--clear-standard-values", "--csv" }, () => false, "Task that will clear Standard Values cache.");
        
    internal static readonly Option<bool> ClearFallback = new(new[] { "--clear-fallback", "--cf" }, () => false, "Task that will clear Is Fallback Valid cache.");
        
    internal static readonly Option<bool> ClearRegistry = new(new[] { "--clear-registry", "--cr" }, () => false, "Task that will clear Registry cache.");
        
    internal static readonly Option<bool> ClearXsl = new(new[] { "--clear-xsl", "--cx" }, () => false, "Task that will clear Xsl cache.");
        
    internal static readonly Option<bool> ClearFilteredItems = new(new[] { "--clear-filtered-items", "--cfi" }, () => false, "Task that will clear Filtered Items cache.");
        
    internal static readonly Option<bool> ClearRenderingParams = new(new[] { "--clear-rendering-params", "--crp" }, () => false, "Task that will clear Rendering Parameters cache.");
        
    internal static readonly Option<bool> ClearViewState = new(new[] { "--clear-view-state", "--cvs" }, () => false, "Task that will clear View State cache.");
}