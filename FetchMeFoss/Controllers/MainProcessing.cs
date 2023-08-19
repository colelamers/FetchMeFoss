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
            _init.Logging.Log("BuildDataTableFromConfiguration called...");

            _fossTable.Columns.Add("Title");
            _fossTable.Columns.Add("Url");
            _fossTable.Columns.Add("WebPage");

            foreach (SoftwareInfo fossDownload in _init.Configuration.FossDownloadData)
            {
                // Some foss items could have more than one potential download link
                foreach (string differentFossDownload in fossDownload.ExecutableLinks)
                {
                    DataRow dRow = _fossTable.NewRow();
                    dRow["Title"] = fossDownload.ApplicationTitle;
                    dRow["Url"] = differentFossDownload;
                    dRow["WebPage"] = fossDownload.LinkToDownloadPage;
                    _fossTable.Rows.Add(dRow);
                }
            }

            return _fossTable;
        }
        //todo 3; 
/*        public void CompareCurrentTableWithConfig(DataTable dgvTableSource)
        {
            // todo 4; try to implement this eventually. seems a tad complicated (not difficult) atm.
            // just update the config file for now.
            if (dgvTableSource != _fossTable)
            {
                foreach (DataRow dRow in dgvTableSource.Rows)
                {
                    foreach (SoftwareInfo fossDownload in _init.Configuration.FossDownloadData)
                    {
                        // Some foss items could have more than one potential download link
                        foreach (string differentFossDownload in fossDownload.ExecutableLinks)
                        {
                            dRow["Title"] = fossDownload.ApplicationTitle;
                            dRow["Url"] = differentFossDownload;
                            dRow["WebPage"] = fossDownload.LinkToDownloadPage;
                        }
                    }
                }
            }
        }*/
        // todo 3;
        public async Task BeginDownload()
        {
            _init.Logging.Log("BeginDownload called...");

            try
            {
                List<Task> downloadTasks = new List<Task>();
                // todo 1; do i really need a task? maybe to run them all at once
                foreach (SoftwareInfo fossDownload in _init.Configuration.FossDownloadData)
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
                _init.Logging.Log("BeginDownload Error", ex);
            }

        }
        // todo 3;
        private async Task DownloadExecutable(SoftwareInfo fossDownload)
        {
            _init.Logging.Log("DownloadExecutable called...");

            // The await operator is what causes a pause in the Task.WhenAll occurs.
            // This makes every application "Wait" until it's finished downloading
            // before it starts another one. If you remove the "await Task.Delay(1)"
            // it will download everything syncronously, but gradually pull down the 
            // files one by one. Having the delay ensures the app pauses preventing
            // user confusion that the app is finished running when it is not.
            await Task.Delay(1);
            // todo 2; webclient bad?
            try
            {
                foreach (string differentFossDownload in fossDownload.ExecutableLinks)
                {
                    // todo 1; make loop keep interating until a successful download.
                    // once successful, stop searching
                    string softwareKey = fossDownload.ApplicationTitle.ToLower().Replace(" ", "");
                    bool isKey = FossDataConstants.FossItemType.TryGetValue(
                        softwareKey, out var fossItemType);
                    if (isKey)
                    {
                        Uri uri = new Uri(fossDownload.LinkToDownloadPage);
                        Type interType = FossDataConstants.FossItemType[softwareKey];
                        var fossInterface = (FossActions?)Activator.CreateInstance(
                            interType, fossDownload);
                        if (fossInterface != null)
                        {
                            var x = await fossInterface.ParseHtmlForDownloadLink(uri);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _init.Logging.Log("Error", ex);
            }
            /*

            string fileName = Path.GetFileNameWithoutExtension(url);
            string fileExtension = Path.GetExtension(url);
            string downloadFileFullLocation = _init.Configuration.DownloadPath + fileName + fileExtension;
            using (Stream? fileDownload = await client.GetStreamAsync(uri))
                {
                    using (Stream fs = new FileStream(downloadFileFullLocation, FileMode.CreateNew))
                    {
                        // fileDownload.CopyTo(fs);         // synchronous downloads
                        await fileDownload.CopyToAsync(fs); // asyncronous downloads
                    }
                }
            */
        }
        // todo 1; need function that performs search of html relative url and pulls that file
        // todo 1; need function that finds the most recent/up-to-date file and downloads it.
    }
}
