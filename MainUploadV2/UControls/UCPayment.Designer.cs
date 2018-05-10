namespace MainUploadV2.UControls
{
    partial class UCPayment
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
            this.gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumDateCreate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDateExpires = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnLoginID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnAccType = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.gridColumn,
            this.gridColumDateCreate,
            this.gridColumnDateExpires,
            this.gridColumnLoginID,
            this.gridColumnAccType});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // gridColumn
            // 
            this.gridColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn.Caption = "FullName";
            this.gridColumn.FieldName = "FullName";
            this.gridColumn.Name = "gridColumn";
            this.gridColumn.Visible = true;
            this.gridColumn.VisibleIndex = 0;
            // 
            // gridColumDateCreate
            // 
            this.gridColumDateCreate.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumDateCreate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumDateCreate.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumDateCreate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumDateCreate.Caption = "Ngày tạo";
            this.gridColumDateCreate.DisplayFormat.FormatString = "dd-MM-yyyy";
            this.gridColumDateCreate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumDateCreate.FieldName = "DateCreate";
            this.gridColumDateCreate.MaxWidth = 120;
            this.gridColumDateCreate.MinWidth = 120;
            this.gridColumDateCreate.Name = "gridColumDateCreate";
            this.gridColumDateCreate.Visible = true;
            this.gridColumDateCreate.VisibleIndex = 1;
            this.gridColumDateCreate.Width = 120;
            // 
            // gridColumnDateExpires
            // 
            this.gridColumnDateExpires.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDateExpires.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDateExpires.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDateExpires.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDateExpires.Caption = "Ngày hết hạn";
            this.gridColumnDateExpires.DisplayFormat.FormatString = "dd-MM-yyyy";
            this.gridColumnDateExpires.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumnDateExpires.FieldName = "DateExpires";
            this.gridColumnDateExpires.MaxWidth = 120;
            this.gridColumnDateExpires.MinWidth = 120;
            this.gridColumnDateExpires.Name = "gridColumnDateExpires";
            this.gridColumnDateExpires.Visible = true;
            this.gridColumnDateExpires.VisibleIndex = 2;
            this.gridColumnDateExpires.Width = 120;
            // 
            // gridColumnLoginID
            // 
            this.gridColumnLoginID.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnLoginID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnLoginID.Caption = "LoginID";
            this.gridColumnLoginID.FieldName = "LoginID";
            this.gridColumnLoginID.Name = "gridColumnLoginID";
            this.gridColumnLoginID.Visible = true;
            this.gridColumnLoginID.VisibleIndex = 3;
            // 
            // gridColumnAccType
            // 
            this.gridColumnAccType.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnAccType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnAccType.Caption = "Loại tài khoản";
            this.gridColumnAccType.FieldName = "AccType";
            this.gridColumnAccType.Name = "gridColumnAccType";
            this.gridColumnAccType.Visible = true;
            this.gridColumnAccType.VisibleIndex = 4;
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
            // UCPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddAccount);
            this.Controls.Add(this.gridControl1);
            this.Name = "UCPayment";
            this.Size = new System.Drawing.Size(875, 503);
            this.Load += new System.EventHandler(this.UCAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumDateCreate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDateExpires;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLoginID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnAccType;
        private DevExpress.XtraEditors.SimpleButton btnAddAccount;
    }
}
