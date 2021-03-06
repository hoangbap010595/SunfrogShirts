﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainUploadV2.SpreadShirts
{
    public partial class UCItemShopSpread : UserControl
    {
        public OShop Shop { get; set; }
        public UCItemShopSpread()
        {
            InitializeComponent();
        }

        private void UCItemShopSpread_Load(object sender, EventArgs e)
        {
            lblShopID.Text = Shop.TargetID;
            lblShopName.Text = Shop.Name;
            Shop.isSelected = true;
        }

        private void toggShopSelected_Toggled(object sender, EventArgs e)
        {
            if (toggShopSelected.IsOn)
                Shop.isSelected = true;
            else
                Shop.isSelected = false;
        }
    }
}
