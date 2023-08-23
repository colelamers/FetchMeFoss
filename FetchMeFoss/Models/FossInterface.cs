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
        // todo 1; implement a configuration update function after url fetch
        // todo 3; 
        public async Task<bool> DownloadWithHtmlParsing()
        {
            // Grab download page url info first, else swap out version in direct link
            using (HttpClient client = new HttpClient())
            {
                Uri currentUrl = new Uri(this.SoftwareItem.SiteDownloadPageLink);
                string directDownloadLink = await ParseHtmlForDownloadLink(client);
                string fileName = Path.GetFileNameWithoutExtension(directDownloadLink);
                string extension = Path.GetExtension(directDownloadLink);
                string downloadPath = _init.Configuration.DownloadPath + fileName + extension;

                using (Stream? fileDownload = await client.GetStreamAsync(directDownloadLink))
                {
                    using (Stream fs = new FileStream(downloadPath, FileMode.Create))
                    {
                        // fileDownload.CopyTo(fs);         // synchronous downloads
                        await fileDownload.CopyToAsync(fs); // asyncronous downloads
                        fs.Flush();
                    }
                    fileDownload.Flush();
                }
            }
            // todo 1; idk if i even want this yet...to early to know for Tasks
            return true;
        }
        // todo 3; 
        public async Task<bool> DownloadWithDirectLink()
        {
            // Grab download page url info first, else swap out version in direct link
            using (HttpClient client = new HttpClient())
            {
                Uri currentUrl = new Uri(this.SoftwareItem.SiteDownloadPageLink);
                await ParseForCurrentVersion();
                string downloadLink = this.SoftwareItem.FullLink;
                string downloadPath = _init.Configuration.DownloadPath + this.SoftwareItem.FileName;

                using (Stream? fileDownload = await client.GetStreamAsync(downloadLink))
                {
                    if (File.Exists(downloadPath))
                    {
                        File.Delete(downloadPath);
                    }
                    using (Stream fs = new FileStream(downloadPath, FileMode.Create))
                    {
                        // fileDownload.CopyTo(fs);         // synchronous downloads
                        await fileDownload.CopyToAsync(fs); // asyncronous downloads
                        fs.Flush();
                    }
                    //fileDownload.Flush();
                }
            }
            // todo 1; idk if i even want this yet...to early to know for Tasks
            return true; 
        }
        /**
         * default download and return very first executable file found
         * todo 3;
         */
        protected virtual async Task<string> ParseHtmlForDownloadLink(HttpClient client)
        {
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
            // Update the struct with new version data and quit searching
            SoftwareConfigInfo sci = SoftwareItem;
            sci.UriPathToExec = sci.UriPathToExec.Replace(sci.VersionNo, nVersion);
            sci.FileName = sci.FileName.Replace(sci.VersionNo, nVersion);
            sci.VersionNo = nVersion;
            SoftwareItem = sci;
        }

    }
}
