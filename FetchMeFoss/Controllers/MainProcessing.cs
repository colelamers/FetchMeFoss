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
                foreach (SoftwareConfigInfo fossDownload in _init.Configuration.FossDownloadData)
                {
                    downloadTasks.Add(InitializeWebPageDownload(fossDownload));
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
            InitializeWebPageDownload(SoftwareConfigInfo fossDownload)
        {
            _init.Logger.Log($"InitializeWebPageDownload called...");

            // todo 2; optimize awaits once app is running smoother. 
            // app is ending but then running syncronously
            // Force app title .ToLower() for key testing
            Type fossItemType;
            string softwareKey = fossDownload.AppTitle.ToLower().Replace(" ", "");
            bool isKey = FossObjectConsts.FossItemType.TryGetValue(softwareKey, 
                                                                   out fossItemType);
            _init.Logger.Log($"Key: {softwareKey}");
            if (isKey)
            {
                var fsInterface = (FossInterface)Activator.CreateInstance(
                                  fossItemType, fossDownload, _init);
                if (fsInterface != null)
                {
                    // Update Version, direct download first, html parse if that
                    // fails
                    await fsInterface.ParseForCurrentVersion();
                    bool success = await fsInterface.DownloadWithDirectLink();
                    if (!success)
                    {
                        success = await fsInterface.DownloadWithHtmlParsing();
                    }

                    // Pass by reference to update VersionInfo so it can update
                    // the config file if the download succeeded.
                    if (success)
                    {
                        fossDownload = fsInterface.SoftwareItem;
                    }
                }
            }
            else
            {
                _init.Logger.Log($"Key not found {softwareKey}");
            }
            return fossDownload;
        }
    }
}
