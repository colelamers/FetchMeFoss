using CommonLibrary;
using FetchMeFoss.Controllers;
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

    /*
     * 
     * todo 4; this below could potentially solve my dynamic type delcaration
     * issue in the main processing. then I could just cast as a parent class?
     * interface IAnimal
        {
            string Speak();
        }

        class Dog : IAnimal
        {
            public virtual string Speak()
            {
                return "Bark!";
            }
        }

        class GoldenRetriever : Dog
        {
            public override string Speak()
            {
                return "I am a golden retriever who says " 
                           + base.Speak();
            }
        }
     * 
     */
    // todo 3;
    public interface FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; protected set;}
        // todo 3; 
        public async Task<bool> DownloadWithHtmlParsing()
        {
            _init.Logger.Log("DownloadWithHtmlParsing called...");
            bool fileDownloaded = false;
            try
            {
                // Grab download page url info first, else swap out version in direct link
                using (HttpClient client = new HttpClient())
                {
                    Uri currentUrl = new Uri(this.SoftwareItem.SiteDownloadPageLink);
                    string downloadLink = await ParseHtmlForDownloadLink(client);
                    string fileName = Path.GetFileNameWithoutExtension(downloadLink);
                    string extension = Path.GetExtension(downloadLink);
                    string downloadPath = _init.Configuration.DownloadPath + fileName + extension;
                    fileDownloaded = await DownloadExec(client, downloadLink, downloadPath);
                }
            }
            catch (Exception ex)
            {
                _init.Logger.Log("DownloadWithHtmlParsing error", ex);
            }
            return fileDownloaded;
        }
        // todo 3; 
        public async Task<bool> DownloadWithDirectLink()
        {
            _init.Logger.Log("DownloadWithDirectLink called...");
            bool fileDownloaded = false;
            try
            {
                // Grab download page url info first, else swap out version in direct link
                using (HttpClient client = new HttpClient())
                {
                    Uri currentUrl = new Uri(this.SoftwareItem.SiteDownloadPageLink);
                    await ParseForCurrentVersion();
                    string downloadLink = this.SoftwareItem.FullLink;
                    string downloadPath = _init.Configuration.DownloadPath + 
                                          this.SoftwareItem.FileName;
                    fileDownloaded = await DownloadExec(client, downloadLink, downloadPath);
                }
            }
            catch (Exception ex)
            {
                _init.Logger.Log("DownloadWithHtmlParsing error", ex);
            }
            return fileDownloaded; 
        }
        // todo 3;
        private async Task<bool> DownloadExec(HttpClient client, string downloadLink, 
                                             string downloadPath)
        {
            _init.Logger.Log("DownloadExec called...");

            // todo 2; possibly throw in commonfunctions?
            using (Stream? fileDownload = await client.GetStreamAsync(downloadLink))
            {
                if (File.Exists(downloadPath))
                {
                    _init.Logger.Log($"File exists! Deleting: {downloadPath}");
                    File.Delete(downloadPath);
                }
                // CreateNew used because I'm concerned of possible infinite loop downloads
                using (Stream fs = new FileStream(downloadPath, FileMode.CreateNew))
                {
                    // fileDownload.CopyTo(fs);         // synchronous downloads
                    await fileDownload.CopyToAsync(fs); // asyncronous downloads
                    fs.Flush();
                }
            }

            if (File.Exists(downloadPath))
            {
                _init.Logger.Log($"File has downloaded here: {downloadPath}");
                return true;
            }

            return false;
        }
        /**
         * default download and return very first executable file found
         * todo 3;
         */
        protected virtual async Task<string> ParseHtmlForDownloadLink(HttpClient client)
        {
            _init.Logger.Log("ParseHtmlForDownloadLink called...");

            Uri currentUrl = new Uri(SoftwareItem.SiteDownloadPageLink);
            string rawHtml = await client.GetStringAsync(currentUrl);
            string[] pageExecs = rawHtml.Split(
                                 new string[] { SoftwareItem.FileType }, StringSplitOptions.None);
            foreach (string unparsedExec in pageExecs)
            {
                // todo 2; may need to revise this to pull non-https items
                int httpsIndex = unparsedExec.LastIndexOf("https://");
                if (httpsIndex > 0)
                {
                    int substringLength = unparsedExec.Length - httpsIndex;
                    string executableHref = unparsedExec.Substring(
                                            httpsIndex, substringLength);

                    return executableHref + SoftwareItem.FileType;
                }
            }
            return string.Empty;
        }
        // todo 3;
        // todo 2; this can use some serious refactoring
        public async Task ParseForCurrentVersion()
        {
            _init.Logger.Log("ParseForCurrentVersion called...");

            using ( HttpClient client = new HttpClient() )
            {
                // Fetches version numbers up to 4 numbers in 4 sections with periods
                Regex rgx = new Regex("([0-9]?[0-9]?[0-9]?[0-9]?" +
                                    "\\.[0-9]?[0-9]?[0-9]?[0-9]?" +
                                    "\\.?[0-9]?[0-9]?[0-9]?[0-9]?" +
                                    "\\.?[0-9]?[0-9]?[0-9]?[0-9]?)", 
                                    RegexOptions.IgnoreCase);

                // Search for first version number
                Uri currentUrl = new Uri(SoftwareItem.SiteDownloadPageLink);
                string rawHtml = await client.GetStringAsync(currentUrl);
                string[] htmlWithVersionInfo = rgx.Split(rawHtml);
                foreach (string whichVersion in htmlWithVersionInfo)
                {
                    // Verified all values were numbers, ensuring it's a version
                    bool isItAVersion = CommonFunctions.ParseStringAsVersionNo(whichVersion);
                    if (isItAVersion)
                    {
                        string fetchedVersionNo = whichVersion;
                        if (fetchedVersionNo[fetchedVersionNo.Length - 1] == '.')
                        {
                            // Chop off period at end in case it exists
                            fetchedVersionNo = whichVersion.Remove(whichVersion.Length - 1);
                        }
                        UpdateSoftwareConfigInfo(fetchedVersionNo);
                        break;
                    }
                }
            }
        }
        // todo 3;
        public void UpdateSoftwareConfigInfo(string nVersion)
        {
            _init.Logger.Log("UpdateSoftwareConfigInfo called...");
            // Update the struct with new version data and quit searching
            SoftwareConfigInfo sci = SoftwareItem;
            sci.UriPathToExec = sci.UriPathToExec.Replace(sci.VersionNo, nVersion);
            sci.FileName = sci.FileName.Replace(sci.VersionNo, nVersion);
            sci.VersionNo = nVersion;
            SoftwareItem = sci;
        }

    }
}
