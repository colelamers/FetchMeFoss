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
        public string SiteDownloadPageLink { get; set; }
        public string BaseUri { get; set; }
        public string UriPathToExec { get; set; }
        public string FileName { get; set; }
        // todo 4; add a 'last date pulled' for version info
        // todo 4; add 32bit, 64bit, arm version search so that if first is 32, skip
        public string FullLink { get { return BaseUri + UriPathToExec + FileName; } }
        // Used for pattern matching and switching out if all other methods fail/is faster
    }
}
