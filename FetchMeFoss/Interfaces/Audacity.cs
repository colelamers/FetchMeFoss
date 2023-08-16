using FetchMeFoss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Interfaces
{
    public class Audacity : object, FossInterface
    {
        public SoftwareInfo SoftwareItem { get; set; }
        public Audacity() 
        { 
            // todo 1; assign software info
        }

        public string ParseHtmlForDownloadLink()
        {
            // todo 1;
        }
    }
}
