using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Models
{
    // todo 4; potential for an abstract class here. unknown if necessary yet.
    // abstract because interface was too simple for the needs of this
    public abstract class FossActions
    {
        public SoftwareInfo SoftwareItem { get; protected set; }

        /**
         * virtual because sometimes html parsing can vary, but not always
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

        public virtual async Task<string> ParseForCurrentVersion()
        {
            // todo 1;
            return "";
        }
    }
}
