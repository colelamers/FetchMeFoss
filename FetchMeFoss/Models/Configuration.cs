using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Models
{
    // todo 3;
    public class Configuration
    {
        public List<SoftwareConfigInfo> FossDownloadData { get; set; }
        public string DownloadPath { get; set; }

        // todo 3;
        public Configuration()
        {
            FossDownloadData = new List<SoftwareConfigInfo>();
            // Quick way to rebuild xml file with generic data
            // FossDownloadData.Add(RebuildConfigTemplate());
            DownloadPath = string.Empty;
        }

        // todo 3;
        // this is for debugging when the xml serializer fails with no errors
        private SoftwareConfigInfo RebuildConfigTemplate()
        {
            SoftwareConfigInfo sci = new SoftwareConfigInfo();
            sci.ApplicationTitle = "title";
            sci.LinkToDownloadPage = "link";
            sci.CdnDownloadLink = "cdn";
            sci.UniqueDownloadLink = "unique";
            sci.ExecutableLinks = new List<string> { "exec1", "exec2" };
            return sci;
        }
    }
}
