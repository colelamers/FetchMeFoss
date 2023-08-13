using CommonLibrary;
using System.Data;
using System.Reflection;

namespace FetchMeFoss
{
    public partial class FetchMeFoss : Form
    {
        private Initialization<Configuration> _init;
        private MainProcessing _proc;
        public FetchMeFoss()
        {
            InitializeComponent();

            _init = new Initialization<Configuration>();
            if (_init != null)
            {
                tbDownloadPath.Text = _init.Configuration?.DownloadPath;
                FillDataTableFromConfiguration();
            }
        }

        private void FillDataTableFromConfiguration()
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

            dgvFossInfo.DataSource = fossTable;
        }

        private void EnableDisableFields(bool enable)
        {
            tbDownloadPath.Enabled = enable;
            dgvFossInfo.Enabled = enable;
            btnDownload.Enabled = enable;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {

            EnableDisableFields(false);
            _proc = new MainProcessing(_init);
            EnableDisableFields(true);
        }


    }
}