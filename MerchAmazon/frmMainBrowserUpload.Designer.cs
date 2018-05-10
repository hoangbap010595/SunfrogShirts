namespace MerchAmazon
{
    partial class frmMainBrowserUpload
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainBrowserUpload));
            this.webBrowserMain = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserMain
            // 
            this.webBrowserMain.Location = new System.Drawing.Point(12, 79);
            this.webBrowserMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserMain.Name = "webBrowserMain";
            this.webBrowserMain.Size = new System.Drawing.Size(1137, 494);
            this.webBrowserMain.TabIndex = 0;
            this.webBrowserMain.Url = new System.Uri(resources.GetString("webBrowserMain.Url"), System.UriKind.Absolute);
            // 
            // frmMainBrowserUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 585);
            this.Controls.Add(this.webBrowserMain);
            this.Name = "frmMainBrowserUpload";
            this.Text = "frmMainBrowserUpload";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserMain;
    }
}