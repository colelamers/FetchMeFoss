namespace FetchMeFoss.Models
{
    // todo 3;
    public struct SoftwareConfigInfo
    {
        // todo 4; extend and add a fetchable description?
        public string AppTitle { get; set; }
        public string FileType { get; set; }
        public string VersionNo { get; set; }
        // todo 4; SiteDownloadPageLink can possibly grow to a list one day if warranted
        // todo 4; Rename SiteDownloadPageLink to just DownloadFromHtmlParsingLink
        public string SiteDownloadPageLink { get; set; }
        public string BaseUri { get; set; } // todo 4; break up BaseUri to be "Scheme" and "HostName"
        public string UriPathToExec { get; set; }
        public string FileName { get; set; }
        // todo 4; add a "previous version"
        // todo 4; add a "last date pulled" for version info
        // todo 4; add 32bit, 64bit, arm version search so that if first is 32, skip
        // todo 4; rename FullLink to be "DownloadDirectlyFullLink
        // todo 4; need speciality links for unique pages like chromium where link redirects
        public string FullLink 
        { 
            get 
            { 
                return BaseUri + UriPathToExec + FileName + "." + FileType; 
            } 
        }
        // Used for pattern matching and switching out if all other methods fail/is faster
    }
}
