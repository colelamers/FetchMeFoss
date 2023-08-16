using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using FetchMeFoss.Interfaces;
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
        public void CompareCurrentTableWithConfig(DataTable dgvTableSource)
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
        }
        // todo 3;
        public async Task BeginDownload()
        {
            List<Task> downloadTasks = new List<Task>();
            // todo 1; do i really need a task? maybe to run them all at once
            foreach (SoftwareInfo fossDownload in _init.Configuration.FossDownloadData)
            {
                // Some foss items could have more than one potential download link
                foreach (string differentFossDownload in fossDownload.ExecutableLinks)
                {
                    downloadTasks.Add(DownloadPage(differentFossDownload, fossDownload));
                }
            }

            await Task.WhenAll(downloadTasks);
        }
        // todo 3;
        private async Task DownloadPage(string url, SoftwareInfo fossDownload)
        {
            // The await operator is what causes a pause in the Task.WhenAll occurs.
            // This makes every application "Wait" until it's finished downloading
            // before it starts another one. If you remove the "await Task.Delay(1)"
            // it will download everything syncronously, but gradually pull down the 
            // files one by one. Having the delay ensures the app pauses preventing
            // user confusion that the app is finished running when it is not.
            await Task.Delay(1);
            // todo 2; webclient bad?
            Uri uri = new Uri(url);
            string fileName = Path.GetFileNameWithoutExtension(url);
            string fileExtension = Path.GetExtension(url);
            string downloadFileFullLocation = _init.Configuration.DownloadPath + fileName + fileExtension;

            using (HttpClient client = new HttpClient())
            {
                using (Stream? fileDownload = await client.GetStreamAsync(uri))
                {
                    // todo 1; this snippet lets me dynamically address type for the interface.
                    // making it easier to cast based off of the name only.
                    Type k = FossDataConstants.FossItemType[fossDownload.ApplicationTitle.ToLower()];
                    FossInterface foInt = (FossInterface)Activator.CreateInstance(k);

                    // todo 1; this code was tested on the krita site. it's good for parsing in general.
                    if (!string.IsNullOrEmpty(fossDownload.LinkToDownloadPage))
                    {
                        var x = await client.GetStringAsync(new Uri(fossDownload.LinkToDownloadPage));
                        var y = x.Split(new string[] { ".exe" }, StringSplitOptions.None);
                        foreach (string exe in y)
                        {
                            string appendExe = ".exe";
                            int httpsIndex = exe.LastIndexOf("https://");
                            if (httpsIndex > 0)
                            {
                                int substringLength = exe.Length - httpsIndex;
                                string executableHref = exe.Substring(httpsIndex, substringLength);
                                string fullExecLink = executableHref + appendExe;
                            }
                        }
                    }

                    using (Stream fs = new FileStream(downloadFileFullLocation, FileMode.CreateNew))
                    {
                        // fileDownload.CopyTo(fs); // synchronous downloads
                        await fileDownload.CopyToAsync(fs); // asyncronous downloads
                    }
                }
            }
        }
        // todo 1; need function that performs search of html relative url and pulls that file
        // todo 1; need function that finds the most recent/up-to-date file and downloads it.
    }
}
