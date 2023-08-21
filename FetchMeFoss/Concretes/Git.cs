using CommonLibrary;
using FetchMeFoss.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Concretes
{
    public class Git : FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; set; }

        public Git(SoftwareConfigInfo sci, Init.Initialization<Configuration> initialization) 
        { 
            SoftwareItem = sci;
            _init = initialization;
        }

        public async Task<string> ParseHtmlForDownloadLink(HttpClient client, Uri siteLink)
        {
            if (siteLink != null)
            {
                string fullExecLink = string.Empty;
                string rawHtml = await client.GetStringAsync(siteLink);
                string[] pageExecs = rawHtml.Split(
                    new string[] { SoftwareItem.FileType }, StringSplitOptions.None);
                foreach (string unparsedExec in pageExecs)
                {
                    // todo 2; may need to revise this to pull non-https items
                    int httpsIndex = unparsedExec.LastIndexOf("https://");
                    if (httpsIndex > 0)
                    {
                        int substringLength = unparsedExec.Length - httpsIndex;
                        string executableHref = unparsedExec.Substring(httpsIndex,
                                                                        substringLength);
                        return executableHref + SoftwareItem.FileType;
                    }
                }
            }
            return string.Empty;
        }
    }
}
