using System.CommandLine;
using Sitecore.Devex.Client.Cli.Extensibility.Subcommands;

namespace Sitecore.DevEx.Extensibility.Cache.Commands
{
    public class CacheCommand : Command, ISubcommand
    {
        public CacheCommand(string name, string description = null) : base(name, description)
        {
        }
    }
}