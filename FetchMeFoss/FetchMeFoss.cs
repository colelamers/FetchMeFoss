using System.Data;
using CommonLibrary;

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
            if (_init != null && _init.Configuration != null)
            {
                tbDownloadPath.Text = _init.Configuration.DownloadPath;
                _proc = new MainProcessing(_init);
                dgvFossInfo.DataSource = _proc.BuildDataTableFromConfiguration();
            }
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
            // todo 1; asnyc void for this? no idea if it'll work
            EnableDisableFields(false);

            await _proc.BeginDownload();

            EnableDisableFields(true);
        }


    }
}