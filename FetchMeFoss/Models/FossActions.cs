using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FetchMeFoss.Models
{
    // todo 3;
    public abstract class FossActions
    {
        public SoftwareConfigInfo SoftwareItem { get; protected set; }

        /**
         * virtuals because sometimes html parsing can vary and you may
         * need to override
         * todo 3;
         */
        public virtual async Task<string> ParseHtmlForDownloadLink(Uri siteLink)
        {
            string fullExecLink = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string rawHtml = await client.GetStringAsync(siteLink);
                string[] pageExecs = rawHtml.Split(new string[] { ".exe" }, StringSplitOptions.None);
                foreach (string unparsedExec in pageExecs)
                {
                    string appendExe = ".exe";
                    int httpsIndex = unparsedExec.LastIndexOf("https://");
                    if (httpsIndex > 0)
                    {
                        int substringLength = unparsedExec.Length - httpsIndex;
                        string executableHref = unparsedExec.Substring(httpsIndex, substringLength);
                        fullExecLink = executableHref + appendExe;
                    }
                }
            }
            return fullExecLink;
        }

        public virtual async Task<string> ParseForCurrentVersion(Uri siteLink)
        {
            string fullExecLink = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string rawHtml = await client.GetStringAsync(siteLink);
                Regex rgx = new Regex("/([0-9]{1,4})+?" +     // Major
                                    "(\\.([0-9]{1,4}))+?" +   // Minor
                                    "(\\.([0-9]{1,4}))?" +    // Patch
                                    "(\\.([0-9]{1,4}))?" +    // Build
                                    "(\\.([0-9]{1,4}))?/gi"); // Etc.
                string[] lis = rgx.Split(rawHtml);

            }
            return fullExecLink;
        }
    }
}
