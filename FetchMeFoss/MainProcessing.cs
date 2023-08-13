using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace FetchMeFoss
{
    // todo 3;
    public class MainProcessing
    {
        private Init.Initialization<Configuration> _init;
        // todo 3;
        public MainProcessing(Init.Initialization<Configuration> initialization)
        {
            _init = initialization;
        }
        // todo 3;
        public DataTable BuildDataTableFromConfiguration()
        {
            DataTable fossTable = new DataTable();
            fossTable.Columns.Add("Title");
            fossTable.Columns.Add("Url");
            fossTable.Columns.Add("WebPage");

            foreach (FossInfo fossDownload in _init.Configuration.FossDownloadData)
            {
                // Some foss items could have more than one potential download link
                foreach (string differentFossDownload in fossDownload.FossUrls)
                {
                    DataRow dRow = fossTable.NewRow();
                    dRow["Title"] = fossDownload.FossTitle;
                    dRow["Url"] = differentFossDownload;
                    dRow["WebPage"] = fossDownload.FossWebPage;
                    fossTable.Rows.Add(dRow);
                }
            }

            return fossTable;
        }
        // todo 3;
        public async Task BeginDownload()
        {
            // todo 1; do i really need a task? maybe to run them all at once
        }
    }
}
