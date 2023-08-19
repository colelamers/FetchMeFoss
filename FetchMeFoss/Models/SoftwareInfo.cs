namespace FetchMeFoss.Models
{
    // todo 3;
    public struct SoftwareInfo
    {
        public string ApplicationTitle { get; set; }
        public List<string> ExecutableLinks { get; set; }
        public string LinkToDownloadPage { get; set; }
        public string CdnDownloadLink { get; set; }
        public string UniqueDownloadLink { get; set; }
        // todo 4; extend and add a fetchable description?
    }
}
