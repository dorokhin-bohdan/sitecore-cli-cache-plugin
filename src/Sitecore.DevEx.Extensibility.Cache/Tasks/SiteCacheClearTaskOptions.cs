using Sitecore.DevEx.Client.Tasks;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public class SiteCacheClearTaskOptions : TaskOptionsBase
    {
        public string SiteName { get; set; }
        
        public bool ClearData { get; set; }
        
        public bool ClearHtml { get; set; }

        public bool ClearItem { get; set; }

        public bool ClearPath { get; set; }
        
        public bool ClearItemPaths { get; set; }
        
        public bool ClearStandardValues { get; set; }
        
        public bool ClearFallback { get; set; }
        
        public bool ClearRegistry { get; set; }
        
        public bool ClearXsl { get; set; }
        
        public bool ClearFilteredItems { get; set; }
        
        public bool ClearRenderingParams { get; set; }
        
        public bool ClearViewState { get; set; }

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