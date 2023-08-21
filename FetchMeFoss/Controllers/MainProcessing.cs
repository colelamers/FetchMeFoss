using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using FetchMeFoss.Concretes;
using FetchMeFoss.Models;

namespace FetchMeFoss.Controllers
{
    // todo 3;
    public class MainProcessing
    {
        private Init.Initialization<Configuration> _init;
        private DataTable _fossTable = new DataTable();
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

            // The await operator is what causes a pause in the Task.WhenAll occurs.
            // This makes every application "Wait" until it's finished downloading
            // before it starts another one. If you remove the "await Task.Delay(1)"
            // it will download everything syncronously, but gradually pull down the 
            // files one by one. Having the delay ensures the app pauses preventing
            // user confusion that the app is finished running when it is not.
            await Task.Delay(1);
            // todo 2; webclient bad?

            // todo 1; make loop keep interating until a successful download.
            // once successful, stop searching
            string softwareKey = fossDownload.AppTitle.ToLower().Replace(" ", "");
            bool isKey = FossDataConstants.FossItemType.TryGetValue(
                softwareKey, out Type? fossItemType);

            if (isKey)
            {
                try
                {
                    var fossAction = (FossInterface)Activator.CreateInstance(fossItemType, fossDownload, _init);
                    if (fossAction != null)
                    {
                        // todo 2; optimize awaits once app is running smoother. 
                        // todo 1; app is ending but then running syncronously. try to resolve this.
                        fossAction.PerformDownload();
                        //  var x = await fossAction.ParseHtmlForDownloadLink(uri);
                    }
                }
                catch (Exception ex)
                {
                    _init.Logger.Log("Error", ex);
                }
            }
        }
        // todo 1; need function that performs search of html relative url and pulls that file
        // todo 1; need function that finds the most recent/up-to-date file and downloads it.
    }
}
