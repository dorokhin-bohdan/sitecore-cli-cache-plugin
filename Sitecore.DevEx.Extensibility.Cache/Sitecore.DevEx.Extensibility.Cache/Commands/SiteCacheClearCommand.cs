using System;
using System.Threading.Tasks;
using Sitecore.Devex.Client.Cli.Extensibility.Subcommands;
using Sitecore.DevEx.Extensibility.Cache.Tasks;
using Sitecore.DevEx.Extensibility.Publishing;

namespace Sitecore.DevEx.Extensibility.Cache.Commands
{
    public class SiteCacheClearCommand : SubcommandBase<SiteCacheClearTask, SiteCacheClearSchemaArgs>
    {
        public SiteCacheClearCommand(IServiceProvider container) : base("site", "Management cache for Sitecore site.", container)
        {
            AddOption(ArgOptions.Config);
            AddOption(ArgOptions.EnvironmentName);
            AddOption(ArgOptions.SiteName);
            AddOption(ArgOptions.CleanData);
            AddOption(ArgOptions.CleanHtml);
            AddOption(ArgOptions.CleanItem);
            AddOption(ArgOptions.CleanPath);
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