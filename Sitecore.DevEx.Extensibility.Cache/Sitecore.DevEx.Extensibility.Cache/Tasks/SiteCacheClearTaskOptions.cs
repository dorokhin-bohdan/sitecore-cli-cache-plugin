using Sitecore.DevEx.Client.Tasks;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public class SiteCacheClearTaskOptions : TaskOptionsBase
    {
        public string SiteName { get; set; }
        
        public bool Data { get; set; }
        
        public bool Html { get; set; }

        public bool Item { get; set; }

        public bool Path { get; set; }
        
        public string Config { get; set; }
        
        public string EnvironmentName { get; set; }

        public override void Validate()
        {
            Require(nameof(Config));
            Default(nameof(EnvironmentName), "default");
        }
    }
}