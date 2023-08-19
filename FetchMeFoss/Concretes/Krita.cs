using FetchMeFoss.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Concretes
{
    public class Krita : FossActions
    {
        // todo 4; privatize these?
        public List<Uri>? ExecutableLinks { get; private set; }
        public Uri? LinkToDownloadPage { get; private set; }
        public Uri? CdnDownloadLink { get; private set; }
        public Uri? UniqueDownloadLink { get; private set; }
        public Krita(SoftwareConfigInfo sci) 
        { 
            SoftwareItem = sci;

            if (!string.IsNullOrEmpty(sci.LinkToDownloadPage))
            {
                this.LinkToDownloadPage = new Uri(sci.LinkToDownloadPage.ToString());
            }

            if (!string.IsNullOrEmpty(sci.CdnDownloadLink))
            {
                this.CdnDownloadLink = new Uri(sci.CdnDownloadLink.ToString());
            }

            if (!string.IsNullOrEmpty(sci.UniqueDownloadLink))
            {
                this.UniqueDownloadLink = new Uri(sci.UniqueDownloadLink.ToString());
            }

            if (sci.ExecutableLinks.Count > 0)
            {
                this.ExecutableLinks = new List<Uri>();
                foreach (string url in sci.ExecutableLinks)
                {
                    this.ExecutableLinks.Add(new Uri(url));
                }
            }
        }
    }
}
