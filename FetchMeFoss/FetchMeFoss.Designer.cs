namespace FetchMeFoss
{
    partial class FetchMeFoss
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvFossInfo = new DataGridView();
            btnDownload = new Button();
            tbDownloadPath = new TextBox();
            ssGuiObject = new StatusStrip();
            tsslText = new ToolStripStatusLabel();
            lbDownloadPath = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvFossInfo).BeginInit();
            ssGuiObject.SuspendLayout();
            SuspendLayout();
            // 
            // dgvFossInfo
            // 
            dgvFossInfo.AllowUserToOrderColumns = true;
            dgvFossInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFossInfo.EditMode = DataGridViewEditMode.EditOnF2;
            dgvFossInfo.Location = new Point(63, 107);
            dgvFossInfo.Name = "dgvFossInfo";
            dgvFossInfo.RowTemplate.Height = 25;
            dgvFossInfo.Size = new Size(684, 345);
            dgvFossInfo.TabIndex = 0;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(450, 39);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(76, 38);
            btnDownload.TabIndex = 1;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // tbDownloadPath
            // 
            tbDownloadPath.Location = new Point(160, 48);
            tbDownloadPath.Name = "tbDownloadPath";
            tbDownloadPath.Size = new Size(284, 23);
            tbDownloadPath.TabIndex = 2;
            // 
            // ssGuiObject
            // 
            ssGuiObject.Items.AddRange(new ToolStripItem[] { tsslText });
            ssGuiObject.Location = new Point(0, 508);
            ssGuiObject.Name = "ssGuiObject";
            ssGuiObject.Size = new Size(826, 22);
            ssGuiObject.TabIndex = 3;
            ssGuiObject.Text = "ssGuiObject";
            // 
            // tsslText
            // 
            tsslText.Name = "tsslText";
            tsslText.Size = new Size(45, 17);
            tsslText.Text = "tsslText";
            // 
            // lbDownloadPath
            // 
            lbDownloadPath.AutoSize = true;
            lbDownloadPath.Location = new Point(63, 51);
            lbDownloadPath.Name = "lbDownloadPath";
            lbDownloadPath.Size = new Size(91, 15);
            lbDownloadPath.TabIndex = 4;
            lbDownloadPath.Text = "Download Path:";
            // 
            // FetchMeFoss
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(826, 530);
            Controls.Add(lbDownloadPath);
            Controls.Add(ssGuiObject);
            Controls.Add(tbDownloadPath);
            Controls.Add(btnDownload);
            Controls.Add(dgvFossInfo);
            Name = "FetchMeFoss";
            Text = "Fetch Me Foss";
            ((System.ComponentModel.ISupportInitialize)dgvFossInfo).EndInit();
            ssGuiObject.ResumeLayout(false);
            ssGuiObject.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvFossInfo;
        private Button btnDownload;
        private TextBox tbDownloadPath;
        private StatusStrip ssGuiObject;
        private ToolStripStatusLabel tsslText;
        private Label lbDownloadPath;
    }
}