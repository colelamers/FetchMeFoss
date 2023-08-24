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
        private string _fullPath {get; set;}
        public string DownloadPath { get { return _fullPath; } 
                                     set { _fullPath = Path.GetFullPath(value); } }
        public List<SoftwareConfigInfo> FossDownloadData { get; set; }

        // todo 3;
        public Configuration()
        {
            FossDownloadData = new List<SoftwareConfigInfo>();
            // Quick way to rebuild xml file with generic data
            // FossDownloadData.Add(DefaultConfiguration());
            // DownloadPath = "C:\\Users\\Public\\Downloads";
        }

        // todo 3;
        // this is for debugging when the xml serializer fails with no errors
        private SoftwareConfigInfo DefaultConfiguration()
        {
            SoftwareConfigInfo sci = new SoftwareConfigInfo();
            sci.AppTitle = "title";
            sci.SiteDownloadPageLink = "https://www.website.com/downloads/";
            sci.BaseUri = "https://www.website.com"; 
            sci.UriPathToExec = "/path/to/exec/";
            sci.VersionNo = "2.3.1";
            sci.FileType = "msi";
            sci.FileName = "fossware-2.3.1-win32.msi";
            return sci;
        }
    }
}
