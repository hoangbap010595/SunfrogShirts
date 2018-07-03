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
            this.gridColumnLoginID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnToolName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDateExpires = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnThemMoiTHanhToan = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
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
            this.gridColumnLoginID,
            this.gridColumnToolName,
            this.gridColumnPrice,
            this.gridColumnDate,
            this.gridColumnDateExpires,
            this.gridColumnStatus});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            // 
            // gridColumnLoginID
            // 
            this.gridColumnLoginID.AppearanceCell.ForeColor = System.Drawing.Color.Purple;
            this.gridColumnLoginID.AppearanceCell.Options.UseForeColor = true;
            this.gridColumnLoginID.Caption = "LoginID";
            this.gridColumnLoginID.FieldName = "LoginID";
            this.gridColumnLoginID.Name = "gridColumnLoginID";
            this.gridColumnLoginID.Visible = true;
            this.gridColumnLoginID.VisibleIndex = 0;
            // 
            // gridColumnToolName
            // 
            this.gridColumnToolName.Caption = "ToolName";
            this.gridColumnToolName.FieldName = "ToolName";
            this.gridColumnToolName.Name = "gridColumnToolName";
            this.gridColumnToolName.Visible = true;
            this.gridColumnToolName.VisibleIndex = 1;
            // 
            // gridColumnPrice
            // 
            this.gridColumnPrice.Caption = "Price";
            this.gridColumnPrice.DisplayFormat.FormatString = "#,###";
            this.gridColumnPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridColumnPrice.FieldName = "Price";
            this.gridColumnPrice.Name = "gridColumnPrice";
            this.gridColumnPrice.Visible = true;
            this.gridColumnPrice.VisibleIndex = 2;
            // 
            // gridColumnDate
            // 
            this.gridColumnDate.AppearanceCell.ForeColor = System.Drawing.Color.DarkGreen;
            this.gridColumnDate.AppearanceCell.Options.UseForeColor = true;
            this.gridColumnDate.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDate.Caption = "Date Payment";
            this.gridColumnDate.DisplayFormat.FormatString = "dd-MM-yyyy";
            this.gridColumnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumnDate.FieldName = "Date";
            this.gridColumnDate.Name = "gridColumnDate";
            this.gridColumnDate.Visible = true;
            this.gridColumnDate.VisibleIndex = 3;
            // 
            // gridColumnDateExpires
            // 
            this.gridColumnDateExpires.AppearanceCell.ForeColor = System.Drawing.Color.Maroon;
            this.gridColumnDateExpires.AppearanceCell.Options.UseForeColor = true;
            this.gridColumnDateExpires.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDateExpires.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDateExpires.Caption = "Date Expires";
            this.gridColumnDateExpires.DisplayFormat.FormatString = "dd-MM-yyyy";
            this.gridColumnDateExpires.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumnDateExpires.FieldName = "DateExpires";
            this.gridColumnDateExpires.Name = "gridColumnDateExpires";
            this.gridColumnDateExpires.Visible = true;
            this.gridColumnDateExpires.VisibleIndex = 4;
            // 
            // gridColumnStatus
            // 
            this.gridColumnStatus.Caption = "Status";
            this.gridColumnStatus.FieldName = "Status";
            this.gridColumnStatus.Name = "gridColumnStatus";
            this.gridColumnStatus.Visible = true;
            this.gridColumnStatus.VisibleIndex = 5;
            // 
            // btnThemMoiTHanhToan
            // 
            this.btnThemMoiTHanhToan.Location = new System.Drawing.Point(3, 12);
            this.btnThemMoiTHanhToan.Name = "btnThemMoiTHanhToan";
            this.btnThemMoiTHanhToan.Size = new System.Drawing.Size(139, 34);
            this.btnThemMoiTHanhToan.TabIndex = 2;
            this.btnThemMoiTHanhToan.Text = "Thanh toán mới";
            this.btnThemMoiTHanhToan.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(148, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(139, 34);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "Add New";
            this.simpleButton1.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(293, 12);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(139, 34);
            this.simpleButton2.TabIndex = 2;
            this.simpleButton2.Text = "Add New";
            this.simpleButton2.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // UCPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnThemMoiTHanhToan);
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
        private DevExpress.XtraEditors.SimpleButton btnThemMoiTHanhToan;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLoginID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnToolName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPrice;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDateExpires;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnStatus;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}
