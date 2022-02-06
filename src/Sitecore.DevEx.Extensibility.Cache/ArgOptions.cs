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

        internal static readonly Option<bool> CleanData = new Option<bool>(new[] { "--clean-data", "--cd" }, () => false, "Task that will cleanup Data cache.");
        
        internal static readonly Option<bool> CleanHtml = new Option<bool>(new[] { "--clean-html", "--ch" }, () => false, "Task that will cleanup Html cache.");
        
        internal static readonly Option<bool> CleanItem = new Option<bool>(new[] { "--clean-item", "--ci" }, () => false, "Task that will cleanup Item cache.");
        
        internal static readonly Option<bool> CleanPath = new Option<bool>(new[] { "--clean-path", "--cp" }, () => false, "Task that will cleanup Path cache.");
    }
}