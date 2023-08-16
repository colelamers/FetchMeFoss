using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Models
{
    public interface FossInterface
    {
        public SoftwareInfo SoftwareItem { get; set; }
        public string ParseHtmlForDownloadLink();
    }
}
