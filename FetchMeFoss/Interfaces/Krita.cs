using FetchMeFoss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Interfaces
{
    public class Krita : object, FossInterface
    {
        public SoftwareInfo SoftwareItem { get; set; }
        public Krita() 
        { 
            // todo 1; assign software info
        }

        public string ParseHtmlForDownloadLink()
        {
            // todo 1;
        }
    }
}
