using CommonLibrary;
using FetchMeFoss.Models;
using System.Data;

namespace FetchMeFoss.Controllers
{
    // todo 3;
    public class MainProcessing
    {
        private Init.Initialization<Configuration> _init;
        private DataTable _fossTable = new DataTable();
        private List<SoftwareConfigInfo> _updateConfigs = new List<SoftwareConfigInfo>();
        // todo 3;
        public MainProcessing(Init.Initialization<Configuration> initialization)
        {
            _init = initialization;
        }
        // todo 3;
        public DataTable BuildDataTableFromConfiguration()
        {
            _init.Logger.Log($"BuildDataTableFromConfiguration called...");
            _fossTable.Columns.Add("Title");
            _fossTable.Columns.Add("Url");
            _fossTable.Columns.Add("WebPage");
            foreach (SoftwareConfigInfo fossDownload in _init.Configuration.FossDownloadData)
            {
                // Some foss items could have more than one potential
                // download link
                DataRow dRow = _fossTable.NewRow();
                dRow["Title"] = fossDownload.AppTitle;
                dRow["Url"] = fossDownload.BaseUri;
                dRow["WebPage"] = fossDownload.SiteDownloadPageLink;
                _fossTable.Rows.Add(dRow);
            }
            return _fossTable;
        }
        // todo 3;
        // todo 4; rename to something more dynamic like "building async functions?"
        public async Task BeginDownload()
        {
            _init.Logger.Log($"BeginDownload called...");
            try
            {
                var downloadTasks = new List<Task<SoftwareConfigInfo>>();
                for (int i = 0; i < _init.Configuration.FossDownloadData.Count; ++i)
                {
                    _init.Logger.Log($"Index:{i}");
                    SoftwareConfigInfo sci = _init.Configuration.FossDownloadData[i];
                    downloadTasks.Add(InitializeWebPageDownload(sci));
                }
                var updatedSoftwareItems = await Task.WhenAll(downloadTasks);
                _init.Configuration.FossDownloadData = updatedSoftwareItems.ToList();
                _init.SaveConfiguration();
            }
            catch (Exception ex)
            {
                _init.Logger.Log("BeginDownload Error", ex);
            }
        }
        // todo 3;
        private async Task<SoftwareConfigInfo> 
            InitializeWebPageDownload(SoftwareConfigInfo sci)
        {
            _init.Logger.Log($"InitializeWebPageDownload called...");

            // todo 4; optimize awaits once app is running smoother. app is ending but then running syncronously

            // Force app title .ToLower() for key testing
            Type fossType;
            string softwareKey = sci.AppTitle.ToLower().Replace(" ", "");
            bool isKey = FossObjectConsts.FossItemType.TryGetValue(softwareKey, out fossType);
            _init.Logger.Log($"Key: {softwareKey}");
            if (isKey)
            {
                FossInterface fi = (FossInterface)Activator.CreateInstance(fossType, sci, _init);
                if (fi != null)
                {
                    // todo 1; test this!!! recently added 2023/09/03
                    sci = await DownloadingItem(sci, fi);
                }
            }
            else
            {
                _init.Logger.Log($"Key not found {softwareKey}");
            }
            return sci;
        }
        // todo 3;
        private async Task<SoftwareConfigInfo>
            DownloadingItem(SoftwareConfigInfo sci, FossInterface fi)
        {
            // Check if version info even exists. If not, it means
            // that the software download doesn't contain the version
            // info in it's filename. 
            if (!string.IsNullOrWhiteSpace(sci.VersionNo))
            {
                await fi.ParseForCurrentVersion();
            }

            // todo 1; capture these successes below with a text file getting updated issue a textfile report after download completion

            // Attempt direct download page first
            bool success = await fi.DownloadWithDirectLink();
            if (!success)
            {
                // If direct download failed, then attempt an html
                // parse on the download page if one exists.
                // 
                // Empty can mean either one does not exist, it's a
                // CDN, or it's locked behind an account log in.
                if (!string.IsNullOrWhiteSpace(sci.SiteDownloadPageLink))
                {
                    success = await fi.DownloadWithHtmlParsing();
                }
            }

            // Pass by reference to update VersionInfo so it can update
            // the config file if the download succeeded.
            if (success)
            {
                sci = fi.SoftwareItem;
            }
            return sci;
        }
    }
}
