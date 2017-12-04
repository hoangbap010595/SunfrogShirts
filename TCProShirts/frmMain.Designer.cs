namespace TCProShirts
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.pictureShowImage = new DevExpress.XtraEditors.PictureEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtPrice = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControlColor = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.info_cbbProduct = new DevExpress.XtraEditors.LookUpEdit();
            this.btnDesignNext = new DevExpress.XtraEditors.SimpleButton();
            this.btnDesignBack = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.lsBoxLog = new DevExpress.XtraEditors.ListBoxControl();
            this.xtraTabControlMain = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageUpload = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageDesign = new DevExpress.XtraTab.XtraTabPage();
            this.xtraScrollableMain = new DevExpress.XtraEditors.XtraScrollableControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureShowImage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info_cbbProduct.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lsBoxLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlMain)).BeginInit();
            this.xtraTabControlMain.SuspendLayout();
            this.xtraTabPageUpload.SuspendLayout();
            this.xtraTabPageDesign.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(30, 13);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(113, 36);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(30, 120);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(162, 42);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(30, 187);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(162, 42);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "simpleButton1";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // pictureShowImage
            // 
            this.pictureShowImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureShowImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureShowImage.Location = new System.Drawing.Point(4, 23);
            this.pictureShowImage.Name = "pictureShowImage";
            this.pictureShowImage.Properties.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.pictureShowImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureShowImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureShowImage.Properties.ZoomAccelerationFactor = 1D;
            this.pictureShowImage.Size = new System.Drawing.Size(280, 384);
            this.pictureShowImage.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtPrice);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.panelControlColor);
            this.groupControl1.Controls.Add(this.simpleButton4);
            this.groupControl1.Controls.Add(this.simpleButton5);
            this.groupControl1.Controls.Add(this.simpleButton3);
            this.groupControl1.Controls.Add(this.info_cbbProduct);
            this.groupControl1.Controls.Add(this.btnDesignNext);
            this.groupControl1.Controls.Add(this.btnDesignBack);
            this.groupControl1.Controls.Add(this.pictureShowImage);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupControl1.Location = new System.Drawing.Point(695, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(289, 611);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Design";
            // 
            // txtPrice
            // 
            this.txtPrice.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtPrice.EditValue = "10.00";
            this.txtPrice.Location = new System.Drawing.Point(91, 575);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtPrice.Properties.Appearance.Options.UseFont = true;
            this.txtPrice.Size = new System.Drawing.Size(76, 26);
            this.txtPrice.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(6, 581);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter Price:";
            // 
            // panelControlColor
            // 
            this.panelControlColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelControlColor.Location = new System.Drawing.Point(4, 450);
            this.panelControlColor.Name = "panelControlColor";
            this.panelControlColor.Size = new System.Drawing.Size(280, 118);
            this.panelControlColor.TabIndex = 2;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.simpleButton4.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.simpleButton4.Appearance.Options.UseFont = true;
            this.simpleButton4.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.simpleButton4.Location = new System.Drawing.Point(145, 371);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(135, 33);
            this.simpleButton4.TabIndex = 0;
            this.simpleButton4.Text = "Back";
            // 
            // simpleButton5
            // 
            this.simpleButton5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.simpleButton5.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.simpleButton5.Appearance.ForeColor = System.Drawing.Color.DarkGreen;
            this.simpleButton5.Appearance.Options.UseFont = true;
            this.simpleButton5.Appearance.Options.UseForeColor = true;
            this.simpleButton5.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.simpleButton5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.ImageOptions.Image")));
            this.simpleButton5.Location = new System.Drawing.Point(173, 572);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(111, 35);
            this.simpleButton5.TabIndex = 0;
            this.simpleButton5.Text = "Apply";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.simpleButton3.Location = new System.Drawing.Point(7, 371);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(135, 33);
            this.simpleButton3.TabIndex = 0;
            this.simpleButton3.Text = "Front";
            // 
            // info_cbbProduct
            // 
            this.info_cbbProduct.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.info_cbbProduct.Location = new System.Drawing.Point(37, 414);
            this.info_cbbProduct.Name = "info_cbbProduct";
            this.info_cbbProduct.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.info_cbbProduct.Properties.Appearance.Options.UseFont = true;
            this.info_cbbProduct.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.info_cbbProduct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.info_cbbProduct.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("_Id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", 100, "Name")});
            this.info_cbbProduct.Properties.DropDownRows = 10;
            this.info_cbbProduct.Size = new System.Drawing.Size(215, 28);
            this.info_cbbProduct.TabIndex = 0;
            // 
            // btnDesignNext
            // 
            this.btnDesignNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDesignNext.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnDesignNext.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDesignNext.ImageOptions.Image")));
            this.btnDesignNext.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDesignNext.Location = new System.Drawing.Point(254, 413);
            this.btnDesignNext.Name = "btnDesignNext";
            this.btnDesignNext.Size = new System.Drawing.Size(30, 30);
            this.btnDesignNext.TabIndex = 0;
            // 
            // btnDesignBack
            // 
            this.btnDesignBack.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDesignBack.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnDesignBack.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDesignBack.ImageOptions.Image")));
            this.btnDesignBack.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDesignBack.Location = new System.Drawing.Point(5, 413);
            this.btnDesignBack.Name = "btnDesignBack";
            this.btnDesignBack.Size = new System.Drawing.Size(30, 30);
            this.btnDesignBack.TabIndex = 1;
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.lsBoxLog);
            this.groupControl5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl5.Location = new System.Drawing.Point(0, 394);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(695, 217);
            this.groupControl5.TabIndex = 4;
            this.groupControl5.Text = "Progress";
            // 
            // lsBoxLog
            // 
            this.lsBoxLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.lsBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsBoxLog.HorizontalScrollbar = true;
            this.lsBoxLog.ItemHeight = 22;
            this.lsBoxLog.Location = new System.Drawing.Point(2, 20);
            this.lsBoxLog.Name = "lsBoxLog";
            this.lsBoxLog.ShowFocusRect = false;
            this.lsBoxLog.Size = new System.Drawing.Size(691, 195);
            this.lsBoxLog.TabIndex = 0;
            // 
            // xtraTabControlMain
            // 
            this.xtraTabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlMain.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlMain.Name = "xtraTabControlMain";
            this.xtraTabControlMain.SelectedTabPage = this.xtraTabPageUpload;
            this.xtraTabControlMain.Size = new System.Drawing.Size(695, 394);
            this.xtraTabControlMain.TabIndex = 5;
            this.xtraTabControlMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageUpload,
            this.xtraTabPageDesign});
            // 
            // xtraTabPageUpload
            // 
            this.xtraTabPageUpload.Controls.Add(this.simpleButton1);
            this.xtraTabPageUpload.Controls.Add(this.btnLogin);
            this.xtraTabPageUpload.Controls.Add(this.simpleButton2);
            this.xtraTabPageUpload.Name = "xtraTabPageUpload";
            this.xtraTabPageUpload.Size = new System.Drawing.Size(689, 342);
            this.xtraTabPageUpload.Text = "Upload";
            // 
            // xtraTabPageDesign
            // 
            this.xtraTabPageDesign.Controls.Add(this.xtraScrollableMain);
            this.xtraTabPageDesign.Name = "xtraTabPageDesign";
            this.xtraTabPageDesign.Size = new System.Drawing.Size(689, 366);
            this.xtraTabPageDesign.Text = "Themes";
            // 
            // xtraScrollableMain
            // 
            this.xtraScrollableMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraScrollableMain.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.xtraScrollableMain.Appearance.Options.UseBackColor = true;
            this.xtraScrollableMain.Location = new System.Drawing.Point(3, 62);
            this.xtraScrollableMain.Name = "xtraScrollableMain";
            this.xtraScrollableMain.Size = new System.Drawing.Size(683, 301);
            this.xtraScrollableMain.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.xtraTabControlMain);
            this.Controls.Add(this.groupControl5);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto TCProShirts - [1.0.0]";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureShowImage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info_cbbProduct.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lsBoxLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlMain)).EndInit();
            this.xtraTabControlMain.ResumeLayout(false);
            this.xtraTabPageUpload.ResumeLayout(false);
            this.xtraTabPageDesign.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.PictureEdit pictureShowImage;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnDesignNext;
        private DevExpress.XtraEditors.SimpleButton btnDesignBack;
        private DevExpress.XtraEditors.LookUpEdit info_cbbProduct;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private DevExpress.XtraEditors.ListBoxControl lsBoxLog;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlMain;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageDesign;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageUpload;
        private DevExpress.XtraEditors.PanelControl panelControlColor;
        private DevExpress.XtraEditors.TextEdit txtPrice;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableMain;
    }
}

