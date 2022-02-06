using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace Sitecore.DevEx.Extensibility.Publishing
{
    /// <summary>
    /// All possible switches to CLI commands
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class ArgOptions
    {
        // GENERIC
        internal static readonly Option<string> Config = new Option<string>(new[] { "--config", "-c" }, () => Environment.CurrentDirectory, "Path to root sitecore.json directory (default: cwd).");

        internal static readonly Option<string> EnvironmentName = new Option<string>(new[] { "--environment-name", "-n" }, "Named Sitecore environment to use. Default: 'default'.");

        internal static readonly Option<bool> Verbose = new Option<bool>(new[] { "--verbose", "-v" }, () => false, "Write some additional diagnostic and performance data.");

        internal static readonly Option<bool> Trace = new Option<bool>(new[] { "--trace", "-t" }, () => false, "Write more additional diagnostic and performance data.");
        
        internal static readonly Option<string> SiteName = new Option<string>(new[] { "--site-name", "-s" }, "Named Sitecore site to use. Default: 'website'.");

        internal static readonly Option<bool> ClearData = new Option<bool>(new[] { "--clear-data", "--cd" }, () => false, "Task that will clear Data cache.");
        
        internal static readonly Option<bool> ClearHtml = new Option<bool>(new[] { "--clear-html", "--ch" }, () => false, "Task that will clear Html cache.");
        
        internal static readonly Option<bool> ClearItem = new Option<bool>(new[] { "--clear-item", "--ci" }, () => false, "Task that will clear Item cache.");
        
        internal static readonly Option<bool> ClearPath = new Option<bool>(new[] { "--clear-path", "--cp" }, () => false, "Task that will clear Path cache.");
        
        internal static readonly Option<bool> ClearItemPaths = new Option<bool>(new[] { "--clear-item-paths", "--cip" }, () => false, "Task that will clear Item Paths cache.");
        
        internal static readonly Option<bool> ClearStandardValues = new Option<bool>(new[] { "--clear-standard-values", "--csv" }, () => false, "Task that will clear Standard Values cache.");
        
        internal static readonly Option<bool> ClearFallback = new Option<bool>(new[] { "--clear-fallback", "--cf" }, () => false, "Task that will clear Is Fallback Valid cache.");
        
        internal static readonly Option<bool> ClearRegistry = new Option<bool>(new[] { "--clear-registry", "--cr" }, () => false, "Task that will clear Registry cache.");
        
        internal static readonly Option<bool> ClearXsl = new Option<bool>(new[] { "--clear-xsl", "--cx" }, () => false, "Task that will clear Xsl cache.");
        
        internal static readonly Option<bool> ClearFilteredItems = new Option<bool>(new[] { "--clear-filtered-items", "--cfi" }, () => false, "Task that will clear Filtered Items cache.");
        
        internal static readonly Option<bool> ClearRenderingParams = new Option<bool>(new[] { "--clear-rendering-params", "--crp" }, () => false, "Task that will clear Rendering Parameters cache.");
        
        internal static readonly Option<bool> ClearViewState = new Option<bool>(new[] { "--clear-view-state", "--cvs" }, () => false, "Task that will clear View State cache.");
    }
}