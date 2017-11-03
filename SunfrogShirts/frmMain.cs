using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SunfrogShirts
{
    public partial class frmMain : XtraForm
    {
        private ObjColor objColor = new ObjColor();
        private Category objCategory = new Category();
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbbCategory.Properties.Items.Clear();
            info_cbbCategory.Properties.Items.Clear();
            ckListBox.Items.Clear();

            List<Category> lsCategory = objCategory.getListCategory();
            foreach (Category item in lsCategory)
            {
                cbbCategory.Properties.Items.Add(item.Name);
                info_cbbCategory.Properties.Items.Add(item.Name);
            }

            cbbCategory.SelectedIndex = 0;
            info_cbbCategory.SelectedIndex = 0;
        }

        private void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbCategory.SelectedIndex)
            {
                case 0:
                case 1:
                    List<ObjColor> lsColor = objColor.getObjColorGuysTeeAndLadiesTee();
                    addListColor(lsColor);
                    break;
                case 2:
                    List<ObjColor> lsColor1 = objColor.getObjColorYouthTee();
                    addListColor(lsColor1);
                    break;
                case 3:
                    List<ObjColor> lsColor2 = objColor.getObjColorHoodie();
                    addListColor(lsColor2);
                    break;
                case 4:
                    List<ObjColor> lsColor3 = objColor.getObjColorSweatSirt();
                    addListColor(lsColor3);
                    break;
                case 5:
                    List<ObjColor> lsColor4 = objColor.getObjColorGuysVNeck();
                    addListColor(lsColor4);
                    break;
                case 6:
                    List<ObjColor> lsColor5 = objColor.getObjColorLadiesVNeck();
                    addListColor(lsColor5);
                    break;
                case 7:
                case 8:
                    List<ObjColor> lsColor7 = objColor.getObjColorUnisex();
                    addListColor(lsColor7);
                    break;
                case 9:
                case 10:
                    List<ObjColor> lsColor8 = objColor.getObjColorBlack();
                    addListColor(lsColor8);
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    List<ObjColor> lsColor9 = objColor.getObjColorWhite();
                    addListColor(lsColor9);
                    break;
                case 18:
                    List<ObjColor> lsColor10 = objColor.getObjColorHat();
                    addListColor(lsColor10);
                    break;
                case 19:
                    List<ObjColor> lsColor11 = objColor.getObjColorTruckerCap();
                    addListColor(lsColor11);
                    break;
            }
        }
        private void addListColor(List<ObjColor> lsColor)
        {
            ckListBox.Items.Clear();
            foreach (ObjColor item in lsColor)
            {
                ckListBox.Items.Add(item.Name);
            }
        }

        private void btnAddTheme_Click(object sender, EventArgs e)
        {
            var uChecked = 0;
            List<string> lsSelectColor = new List<string>();
            for (int i = 0; i < ckListBox.Items.Count; i++)
            {
                if (ckListBox.GetItemChecked(i))
                {
                    string str = ckListBox.GetItemText(ckListBox.Items[i]);
                    lsSelectColor.Add(str);
                    uChecked++;
                }
            }
           
            if (uChecked > 5)
            {
                XtraMessageBox.Show("Chọn số lượng màu nhỏ hơn 5", "Error");
                return;
            }
            if (uChecked == 0)
            {
                XtraMessageBox.Show("Không có màu nào được chọn", "Error");
                return;
            }
           
            //Add theme here
        }
    }
}
