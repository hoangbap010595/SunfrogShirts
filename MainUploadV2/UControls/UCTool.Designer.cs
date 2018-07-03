namespace MainUploadV2.UControls
{
    partial class UCTool
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnToolName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnVersion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnHasMD5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnLink = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnAddAccount = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(3, 52);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(869, 448);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnToolName,
            this.gridColumnVersion,
            this.gridColumnHasMD5,
            this.gridColumnPrice,
            this.gridColumnLink});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            // 
            // gridColumnToolName
            // 
            this.gridColumnToolName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnToolName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnToolName.Caption = "Tên Tool";
            this.gridColumnToolName.FieldName = "ToolName";
            this.gridColumnToolName.Name = "gridColumnToolName";
            this.gridColumnToolName.Visible = true;
            this.gridColumnToolName.VisibleIndex = 0;
            // 
            // gridColumnVersion
            // 
            this.gridColumnVersion.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnVersion.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnVersion.Caption = "Version";
            this.gridColumnVersion.FieldName = "Version";
            this.gridColumnVersion.Name = "gridColumnVersion";
            this.gridColumnVersion.Visible = true;
            this.gridColumnVersion.VisibleIndex = 1;
            // 
            // gridColumnHasMD5
            // 
            this.gridColumnHasMD5.Caption = "HasMD5";
            this.gridColumnHasMD5.FieldName = "HasMD5";
            this.gridColumnHasMD5.Name = "gridColumnHasMD5";
            this.gridColumnHasMD5.Visible = true;
            this.gridColumnHasMD5.VisibleIndex = 2;
            // 
            // gridColumnPrice
            // 
            this.gridColumnPrice.Caption = "Price";
            this.gridColumnPrice.DisplayFormat.FormatString = "#,###";
            this.gridColumnPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridColumnPrice.FieldName = "Price";
            this.gridColumnPrice.Name = "gridColumnPrice";
            this.gridColumnPrice.Visible = true;
            this.gridColumnPrice.VisibleIndex = 3;
            // 
            // gridColumnLink
            // 
            this.gridColumnLink.Caption = "Link";
            this.gridColumnLink.FieldName = "Link";
            this.gridColumnLink.Name = "gridColumnLink";
            this.gridColumnLink.Visible = true;
            this.gridColumnLink.VisibleIndex = 4;
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAccount.Location = new System.Drawing.Point(733, 12);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new System.Drawing.Size(139, 34);
            this.btnAddAccount.TabIndex = 2;
            this.btnAddAccount.Text = "Add New";
            this.btnAddAccount.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // UCTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddAccount);
            this.Controls.Add(this.gridControl1);
            this.Name = "UCTool";
            this.Size = new System.Drawing.Size(875, 503);
            this.Load += new System.EventHandler(this.UCAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnToolName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnVersion;
        private DevExpress.XtraEditors.SimpleButton btnAddAccount;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnHasMD5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPrice;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLink;
    }
}
