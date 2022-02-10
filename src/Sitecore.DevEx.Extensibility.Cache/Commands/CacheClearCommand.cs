using System;
using System.Threading.Tasks;
using Sitecore.Devex.Client.Cli.Extensibility.Subcommands;
using Sitecore.DevEx.Extensibility.Cache.Tasks;
using Sitecore.DevEx.Extensibility.Publishing;

namespace Sitecore.DevEx.Extensibility.Cache.Commands
{
    public class CacheClearCommand : SubcommandBase<CacheClearTask, CacheClearSchemaArgs>
    {
        public CacheClearCommand(IServiceProvider container) : base("clear", "Cache management for Sitecore.", container)
        {
            AddOption(ArgOptions.Config);
            AddOption(ArgOptions.Trace);
            AddOption(ArgOptions.Verbose);
            AddOption(ArgOptions.EnvironmentName);
        }

        protected override async Task<int> Handle(CacheClearTask task, CacheClearSchemaArgs args)
        {
            await task.Execute(args).ConfigureAwait(false);
            
            return 0;
        }
    }
}