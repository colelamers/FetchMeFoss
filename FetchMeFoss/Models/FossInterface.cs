using CommonLibrary;
using FetchMeFoss.Controllers;
using System.Text.RegularExpressions;

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
        public Init.Initialization<Configuration> _init { get; protected set; }
        public Regex RgxCustomVersion { get; set; }
        // todo 3;
        protected async Task<bool> DownloadExec(HttpClient client, string downloadLink,
                                                string downloadPath)
        {
            try
            {
                _init.Logger.Log($"FossInterface-DownloadExec called...");
                using (Stream? fileDownload = await client.GetStreamAsync(downloadLink))
                {
                    if (File.Exists(downloadPath))
                    {
                        _init.Logger.Log($"File exists! Deleting: {downloadPath}");
                        File.Delete(downloadPath);
                    }

                    // CreateNew used because I'm concerned of possible
                    // infinite loop downloads
                    using (Stream fs = new FileStream(downloadPath, FileMode.CreateNew))
                    {
                        // fileDownload.CopyTo(fs);         // synchronous downloads
                        await fileDownload.CopyToAsync(fs); // asyncronous downloads
                        fs.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                _init.Logger.Log("DownloadExec Error", ex);
            }

            if (File.Exists(downloadPath))
            {
                _init.Logger.Log($"File downloaded: {downloadPath}");
                return true;
            }
            else
            {
                _init.Logger.Log($"File did not download. Link: {downloadLink}");
                return false;
            }
        }
        /**
         * default download and return very first executable file found
         * todo 3;
         */
        protected async Task<string> ParseHtmlForDownloadLink(HttpClient client)
        {
            _init.Logger.Log($"FossInterface-ParseHtmlForDownloadLink called...");

            Uri currentUrl = new Uri(SoftwareItem.SiteDownloadPageLink);
            string rawHtml = await client.GetStringAsync(currentUrl);
            string[] splitParams = new string[] { SoftwareItem.FileType };
            string[] pageExecs = rawHtml.Split(splitParams, StringSplitOptions.None);
            string foundString = IteratePotentialLinks(pageExecs);
            return foundString;
        }
        //todo 3;
        protected string IteratePotentialLinks(string[] pageExecs)
        {
            foreach (string unparsedExec in pageExecs)
            {
                // Assumption that version number is
                // contained within download link
                if (unparsedExec.Contains(SoftwareItem.VersionNo))
                {
                    // Finds the file type ending, and the
                    // last occurace of https
                    int httpsIndex = unparsedExec.LastIndexOf("https://");
                    if (httpsIndex > 0)
                    {
                        int substringLength = unparsedExec.Length - httpsIndex;
                        string execHref = unparsedExec.Substring(httpsIndex, substringLength);
                        return execHref + SoftwareItem.FileType;
                    }
                }
            }
            return string.Empty;
        }
        // todo 3;
        protected void UpdateSoftwareConfigInfo(string nVersion)
        {
            _init.Logger.Log($"FossInterface-UpdateSoftwareConfigInfo called...");

            // Update the struct with new version data and quit searching
            SoftwareConfigInfo sci = SoftwareItem;
            sci.UriPathToExec = sci.UriPathToExec.Replace(sci.VersionNo, nVersion);
            sci.FileName = sci.FileName.Replace(sci.VersionNo, nVersion);
            sci.VersionNo = nVersion;
            SoftwareItem = sci;
        }
        // todo 3; 
        public async Task<bool> DownloadWithHtmlParsing()
        {
            _init.Logger.Log($"FossInterface-DownloadWithHtmlParsing called...");
            bool fileDownloaded = false;
            // Grab download page url info first, else swap out version in
            // direct link
            using (HttpClient client = new HttpClient())
            {
                string downloadLink = await ParseHtmlForDownloadLink(client);
                string fileName = Path.GetFileNameWithoutExtension(downloadLink);
                string extension = Path.GetExtension(downloadLink);
                string downloadPath = _init.Configuration.DownloadPath + fileName + extension;
                fileDownloaded = await DownloadExec(client, downloadLink, downloadPath);
            }
            return fileDownloaded;
        }
        // todo 3; 
        public async Task<bool> DownloadWithDirectLink()
        {
            _init.Logger.Log($"FossInterface-DownloadWithDirectLink called...");
            bool fileDownloaded = false;
            if (string.IsNullOrWhiteSpace(SoftwareItem.BaseUri) ||
                string.IsNullOrWhiteSpace(SoftwareItem.UriPathToExec) ||
                string.IsNullOrWhiteSpace(SoftwareItem.FileName))
            {
                // Return false immediately if anything of these main items are empty
                return false;
            }

            // Grab download page url info first, else swap out version in direct link
            using (HttpClient client = new HttpClient())
            {
                string downloadPath = _init.Configuration.DownloadPath +
                                      this.SoftwareItem.FileName + "." + 
                                      this.SoftwareItem.FileType;
                fileDownloaded = await DownloadExec(client, this.SoftwareItem.FullLink, 
                                                    downloadPath);
            }
            return fileDownloaded;
        }
        // todo 3;
        public async Task ParseForCurrentVersion()
        {
            _init.Logger.Log($"FossInterface-ParseForCurrentVersion called...");
            using (HttpClient client = new HttpClient())
            {
                // Default if none set
                if (RgxCustomVersion == null)
                {
                    // Mandatory #.#; more or less numbers and additional versions
                    RgxCustomVersion = new Regex("([0-9]+[0-9]?[0-9]?[0-9]?\\.+" +
                                                 "[0-9]+[0-9]?[0-9]?[0-9]?\\." +
                                                 "[0-9]?[0-9]?[0-9]?[0-9]?" +
                                                 "(?=[.][0-9]*))",
                                                  RegexOptions.IgnoreCase);
                }

                // Search for first version number
                Uri currentUrl = new Uri(SoftwareItem.SiteDownloadPageLink);
                string rawHtml = await client.GetStringAsync(currentUrl);
                string[] htmlWithVersionInfo = RgxCustomVersion.Split(rawHtml);

                // Assumption is the first "version" found is the most recent
                // In descending order
                foreach (string whichVersion in htmlWithVersionInfo)
                {
                    // Verified all values were numbers, ensuring it's a version
                    bool isItAVersion = ParseStringAsVersionNo(whichVersion);
                    if (isItAVersion)
                    {
                        // Remove last char if it is a period in the string
                        string fetchedVersionNo = whichVersion;
                        if (fetchedVersionNo[fetchedVersionNo.Length - 1].Equals("."))
                        {
                            fetchedVersionNo = whichVersion.Remove(whichVersion.Length - 1);
                        }
                        UpdateSoftwareConfigInfo(fetchedVersionNo);
                        break;
                    }
                }
            }
        }
        // todo 3;
        public static bool ParseStringAsVersionNo(string stringWithVersionNumber)
        {
            bool isPossibleVersion = false;
            string[] versionSplit = stringWithVersionNumber.Split(
                                    ".", StringSplitOptions.RemoveEmptyEntries);

            // Verify all split values are numbers, if not,
            // it's not a VerisonNo
            foreach (string number in versionSplit)
            {
                int isNum;
                if (int.TryParse(number, out isNum))
                {
                    isPossibleVersion = true;
                }
                else
                {
                    isPossibleVersion = false;
                    break;
                }
            }
            return isPossibleVersion;
        }
    }
}
