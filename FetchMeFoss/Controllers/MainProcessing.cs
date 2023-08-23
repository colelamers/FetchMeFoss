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
                List<Task> downloadTasks = new List<Task>();
                // todo 1; do i really need a task? maybe to run them all at once
                // todo 1; think about revising this loop here and adding the when all elsewhere?
                //         issue is that i can't update the config as new info is found.
                foreach (SoftwareConfigInfo fossDownload in _init.Configuration.FossDownloadData)
                {
                    // Some foss items could have more than one potential download link
                    // todo 1; don't iterate here, iterate in DownloadExecutable
                    downloadTasks.Add(DownloadExecutable(fossDownload));
                    // todo 1; add every which way something can get download in here.
                }
                await Task.WhenAll(downloadTasks);
            }
            catch (Exception ex)
            {
                _init.Logger.Log("BeginDownload Error", ex);
            }
        }
        // todo 3;
        private async Task DownloadExecutable(SoftwareConfigInfo fossDownload)
        {
            _init.Logger.Log("DownloadExecutable called...");
            try
            {
                // todo 1; make loop keep interating until a successful download.
                // once successful, stop searching
                Type fossItemType;
                string softwareKey = fossDownload.AppTitle.ToLower().Replace(" ", "");
                bool isKey = FossDataConstants.FossItemType.TryGetValue(
                             softwareKey, out fossItemType);
                if (isKey)
                {
                    var fossInterface = (FossInterface)Activator.CreateInstance(
                                        fossItemType, fossDownload, _init);

                    if (fossInterface != null)
                    {
                        // todo 2; optimize awaits once app is running smoother. 
                        // app is ending but then running syncronously
                        await fossInterface.DownloadWithDirectLink();
                        //await fossInterface.DownloadWithHtmlParsing();

                        /*
                        // How to dynamically make type-casts
                        // super dangerous if not caught properly. I would require try-catch
                        // plus a switch case for every possible type to feel comfortable. as
                        // well as have unit tests on it
                        dynamic k = Convert.ChangeType(fossInterface, fossItemType);
                        k.ParseHtmlForDownloadLink(new HttpClient());*/
                    }
                }
            }
            catch (Exception ex)
            {
                _init.Logger.Log("DownloadExecutable error", ex);
            }
        }
    }
}
