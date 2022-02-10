using Sitecore.Devex.Client.Cli.Extensibility.Subcommands;
using Sitecore.DevEx.Extensibility.Cache.Tasks;

namespace Sitecore.DevEx.Extensibility.Cache.Commands
{
    public class CacheClearSchemaArgs : CacheClearTaskOptions, IVerbosityArgs
    {
        public bool Verbose { get; set; }
        
        public bool Trace { get; set; }
    }
}