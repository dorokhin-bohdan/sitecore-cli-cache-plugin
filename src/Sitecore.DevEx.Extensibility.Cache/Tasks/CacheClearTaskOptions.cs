using Sitecore.DevEx.Client.Tasks;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public class CacheClearTaskOptions : TaskOptionsBase
    {
        public string Config { get; set; }
        
        public string EnvironmentName { get; set; }
        
        public override void Validate()
        {
            Require(nameof(Config));
            Default(nameof(EnvironmentName), "default");
        }
    }
}