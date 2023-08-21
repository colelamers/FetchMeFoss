using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FetchMeFoss.Models
{
    // todo 3;
    public interface FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; protected set;}
        // todo 1; implement a configuration update function after url fetch
        // todo 3; 
        public async void PerformDownload()
        {
            // Grab download page url info first, else swap out version in direct link
            Uri currentUrl = new Uri(this.SoftwareItem.SiteDownloadPageLink);

            if (currentUrl != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    string directDownloadLink = await ParseHtmlForDownloadLink(client, currentUrl);
                    string fileName = Path.GetFileNameWithoutExtension(directDownloadLink);
                    string extension = Path.GetExtension(directDownloadLink);
                    string downloadPath = _init.Configuration.DownloadPath + fileName + extension;

                    // todo 1; when you get the stream, it preemptively downloads the file.
                    using (Stream? fileDownload = await client.GetStreamAsync(directDownloadLink))
                    {
                        using (Stream fs = new FileStream(downloadPath, FileMode.CreateNew))
                        {
                            if (fileDownload.ToString().Length > 0)
                            {
                                // todo 1; working on this part.
                                string versionNo = await ParseForCurrentVersion(currentUrl);
                            }
                            // fileDownload.CopyTo(fs);         // synchronous downloads
                            await fileDownload.CopyToAsync(fs); // asyncronous downloads
                            fs.Flush();
                        }
                        fileDownload.Flush();
                    }
                }
            }
            else
            {
                // todo 1; throw error if uri is null?
            }
        }
        /**
         * default download and return very first executable file found
         * todo 3;
         */
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

        public async Task<string> ParseForCurrentVersion(Uri siteLink)
        {
            if (siteLink != null)
            {
                string fullExecLink = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    string rawHtml = await client.GetStringAsync(siteLink);
                    /*Regex rgx = new Regex("/([0-9]{1,4})+?(\.([0-9]{1,4}))+?(\.([0-9]{1,4}))?(\.([0-9]{1,4}))?(\.([0-9]{1,4}))?/gi"); */
                    // todo 1;, neet to work on / prioritize this.
                    Regex rgx = new Regex("([0-9]{1,4})+?(\\.([0-9]{1,4}))+?(\\.([0-9]{1,4}))?(\\.([0-9]{1,4}))?(\\.([0-9]{1,4}))?", RegexOptions.IgnoreCase);
                    string[] lis = rgx.Split(rawHtml);
                }
                return fullExecLink;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
