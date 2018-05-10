namespace MainUploadV2
{
    partial class frmMainAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainAdmin));
            this.btnManageAccount = new DevExpress.XtraEditors.SimpleButton();
            this.groupControlMain = new DevExpress.XtraEditors.GroupControl();
            this.btnPayment = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnManageAccount
            // 
            this.btnManageAccount.Location = new System.Drawing.Point(12, 32);
            this.btnManageAccount.Name = "btnManageAccount";
            this.btnManageAccount.Size = new System.Drawing.Size(181, 34);
            this.btnManageAccount.TabIndex = 0;
            this.btnManageAccount.Text = "Manage Account";
            this.btnManageAccount.Click += new System.EventHandler(this.btnManageAccount_Click);
            // 
            // groupControlMain
            // 
            this.groupControlMain.Location = new System.Drawing.Point(199, 12);
            this.groupControlMain.Name = "groupControlMain";
            this.groupControlMain.Size = new System.Drawing.Size(860, 488);
            this.groupControlMain.TabIndex = 1;
            // 
            // btnPayment
            // 
            this.btnPayment.Location = new System.Drawing.Point(12, 72);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(181, 34);
            this.btnPayment.TabIndex = 0;
            this.btnPayment.Text = "Manage Payment";
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // frmMainAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 512);
            this.Controls.Add(this.groupControlMain);
            this.Controls.Add(this.btnPayment);
            this.Controls.Add(this.btnManageAccount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1087, 551);
            this.MinimumSize = new System.Drawing.Size(1087, 551);
            this.Name = "frmMainAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Admin";
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnManageAccount;
        private DevExpress.XtraEditors.GroupControl groupControlMain;
        private DevExpress.XtraEditors.SimpleButton btnPayment;
    }
}