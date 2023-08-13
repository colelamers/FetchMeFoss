using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss
{
    // todo 3;
    public class Configuration
    {
        public List<FossInfo> FossDownloadData { get; set; } 
        public string DownloadPath { get; set; }

        // todo 3;
        public Configuration() 
        {
            FossDownloadData = new List<FossInfo>();
            DownloadPath = string.Empty;
        }
    }
}
