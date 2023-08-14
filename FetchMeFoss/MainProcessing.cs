using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace FetchMeFoss
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

            foreach (FossInfo fossDownload in _init.Configuration.FossDownloadData)
            {
                // Some foss items could have more than one potential download link
                foreach (string differentFossDownload in fossDownload.FossUrls)
                {
                    DataRow dRow = _fossTable.NewRow();
                    dRow["Title"] = fossDownload.FossTitle;
                    dRow["Url"] = differentFossDownload;
                    dRow["WebPage"] = fossDownload.FossWebPage;
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
                    foreach (FossInfo fossDownload in _init.Configuration.FossDownloadData)
                    {
                        // Some foss items could have more than one potential download link
                        foreach (string differentFossDownload in fossDownload.FossUrls)
                        {
                            dRow["Title"] = fossDownload.FossTitle;
                            dRow["Url"] = differentFossDownload;
                            dRow["WebPage"] = fossDownload.FossWebPage;
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
            foreach (FossInfo fossDownload in _init.Configuration.FossDownloadData)
            {
                // Some foss items could have more than one potential download link
                foreach (string differentFossDownload in fossDownload.FossUrls)
                {
                    downloadTasks.Add(DownloadPage(differentFossDownload));
                }
            }

            await Task.WhenAll(downloadTasks);
        }
        // todo 3;
        private async Task DownloadPage(string url)
        {
            // The await operator is what causes a pause in the Task.WhenAll occurs.
            // This makes every application "Wait" until it's finished downloading
            // before it starts another one. If you remove the "await Task.Delay(1)"
            // it will download everything syncronously, but gradually pull down the 
            // files one by one. Having the delay ensures the app pauses preventing
            // user confusion that the app is finished running when it is not.
            await Task.Delay(1);
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri(url);
                string fileName = Path.GetFileNameWithoutExtension(url);
                string fileExtension = Path.GetExtension(url);
                string downloadFileFullLocation = _init.Configuration.DownloadPath + fileName + fileExtension;

                _init.Logging.Log($"Downloading file {downloadFileFullLocation}");
                client.DownloadFile(uri, downloadFileFullLocation);
            }
/*
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        
                        string pageContent = await content.ReadAsStringAsync();
                    }
                }
            }*/
        }
        // todo 1; need function that performs search of html relative url and pulls that file
        // todo 1; need function that finds the most recent/up-to-date file and downloads it.
    }
}
