using System;
using System.Threading.Tasks;
using Sitecore.Devex.Client.Cli.Extensibility.Subcommands;
using Sitecore.DevEx.Extensibility.Cache.Tasks;
using Sitecore.DevEx.Extensibility.Publishing;

namespace Sitecore.DevEx.Extensibility.Cache.Commands
{
    public class SiteCacheClearCommand : SubcommandBase<SiteCacheClearTask, SiteCacheClearSchemaArgs>
    {
        public SiteCacheClearCommand(IServiceProvider container) : base("site", "Cache management for a specific Sitecore site.", container)
        {
            AddOption(ArgOptions.Config);
            AddOption(ArgOptions.EnvironmentName);
            AddOption(ArgOptions.SiteName);
            AddOption(ArgOptions.ClearData);
            AddOption(ArgOptions.ClearHtml);
            AddOption(ArgOptions.ClearItem);
            AddOption(ArgOptions.ClearPath);
            AddOption(ArgOptions.ClearItemPaths);
            AddOption(ArgOptions.ClearStandardValues);
            AddOption(ArgOptions.ClearFallback);
            AddOption(ArgOptions.ClearRegistry);
            AddOption(ArgOptions.ClearXsl);
            AddOption(ArgOptions.ClearFilteredItems);
            AddOption(ArgOptions.ClearRenderingParams);
            AddOption(ArgOptions.ClearViewState);
            AddOption(ArgOptions.Trace);
            AddOption(ArgOptions.Verbose);
        }

        protected override async Task<int> Handle(SiteCacheClearTask task, SiteCacheClearSchemaArgs args)
        {
            await task.Execute(args).ConfigureAwait(false);
            
            return 0;
        }
    }
}