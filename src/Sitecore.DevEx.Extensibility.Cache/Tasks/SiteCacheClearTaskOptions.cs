using Sitecore.DevEx.Client.Tasks;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public class SiteCacheClearTaskOptions : TaskOptionsBase
    {
        public string SiteName { get; set; }
        
        public bool CleanData { get; set; }
        
        public bool CleanHtml { get; set; }

        public bool CleanItem { get; set; }

        public bool CleanPath { get; set; }
        
        public string Config { get; set; }
        
        public string EnvironmentName { get; set; }

        public override void Validate()
        {
            Require(nameof(Config));
            Default(nameof(EnvironmentName), "default");
            Default(nameof(SiteName), "website");
        }
    }
}