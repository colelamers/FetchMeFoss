using CommonLibrary;
using FetchMeFoss.Controllers;
using FetchMeFoss.Models;

namespace FetchMeFoss
{
    // todo 3;
    public partial class FetchMeFoss : Form
    {
        private Init.Initialization<Configuration> _init;
        private MainProcessing _proc;
        // todo 3;
        public FetchMeFoss()
        {
            InitializeComponent();
            ApplicationSetup();
        }
        // todo 3;
        private void ApplicationSetup()
        {
            // Initialize the config file and logger
            _init = new Init.Initialization<Configuration>();
            _proc = new MainProcessing(_init);
            FillTable();
        }
        private void FillTable()
        {
            tbDownloadPath.Text = _init.Configuration.DownloadPath;
            dgvFossInfo.DataSource = _proc.BuildDataTableFromConfiguration();
        }
        // todo 3;
        private void EnableDisableFields(bool enable)
        {
            tbDownloadPath.Enabled = enable;
            dgvFossInfo.Enabled = enable;
            btnDownload.Enabled = enable;
        }
        // todo 3;
        private async void btnDownload_Click(object sender, EventArgs e)
        {
            EnableDisableFields(false);
            //_proc.CompareCurrentTableWithConfig((DataTable)dgvFossInfo.DataSource);
            await _proc.BeginDownload();
            EnableDisableFields(true);
        }
    }
}