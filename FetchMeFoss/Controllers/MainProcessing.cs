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
            _init.Logger.Log("BuildDataTableFromConfiguration called...");

            _fossTable.Columns.Add("Title");
            _fossTable.Columns.Add("Url");
            _fossTable.Columns.Add("WebPage");

            foreach (SoftwareConfigInfo fossDownload in _init.Configuration.FossDownloadData)
            {
                // Some foss items could have more than one potential download link
                // todo 2; come back to this at a later date. not concerned with this now.
                DataRow dRow = _fossTable.NewRow();
                dRow["Title"] = fossDownload.AppTitle;
                // dRow["Url"] = differentFossDownload;
                dRow["WebPage"] = fossDownload.SiteDownloadPageLink;
                _fossTable.Rows.Add(dRow);
            }
            return _fossTable;
        }
        // todo 3;
        // todo 4; rename to something more dynamic like "building async functions?"
        public async Task BeginDownload()
        {
            _init.Logger.Log("BeginDownload called...");
            try
            {
                var downloadTasks = new List<Task<SoftwareConfigInfo>>();
                foreach (SoftwareConfigInfo fossDownload in _init.Configuration.FossDownloadData)
                {
                    downloadTasks.Add(DownloadExecutable(fossDownload));
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
        private async Task<SoftwareConfigInfo> DownloadExecutable(SoftwareConfigInfo fossDownload)
        {
            _init.Logger.Log("DownloadExecutable called...");
            Type fossItemType;
            string softwareKey = fossDownload.AppTitle.ToLower().Replace(" ", "");
            bool isKey = FossDataConstants.FossItemType.TryGetValue(softwareKey, out fossItemType);
            if (isKey)
            {
                var fsInterface = (FossInterface)Activator.CreateInstance(
                                  fossItemType, fossDownload, _init);

                if (fsInterface != null)
                {
                    // todo 2; optimize awaits once app is running smoother. 
                    // app is ending but then running syncronously
                    var success = await fsInterface.DownloadWithDirectLink();
                    if (!success)
                    {
                        success = await fsInterface.DownloadWithHtmlParsing();
                    }

                    // Pass by reference after interface updates VersionInfo.
                    // This is so we can update the config file later.
                    // Only if download succeeds will we do this
                    if (success)
                    {
                        fossDownload = fsInterface.SoftwareItem;
                    }
                }
            }

            return fossDownload;
        }
    }
}
