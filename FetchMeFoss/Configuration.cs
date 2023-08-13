using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss
{
    public class Configuration
    {
        public List<FossInfo> FossDownloadData { get; set; } 
        public string DownloadPath { get; set; }

        public Configuration() 
        {
            FossDownloadData = new List<FossInfo>();
            DownloadPath = string.Empty;
        }
    }
}
