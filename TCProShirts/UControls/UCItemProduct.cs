using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCProShirts.Models;
using DevExpress.XtraEditors;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Net;

namespace TCProShirts.UControls
{
    public partial class UCItemProduct : XtraUserControl
    {
        private Graphics g;
        private Rectangle r;
        private Pen p;

        public Product Product;
        public UCItemProduct(Product product)
        {
            InitializeComponent();
            Product = product;
        }
        public UCItemProduct()
        {
            InitializeComponent();
            Product = new Product();
            Product.Name = "Thieu Den";
            Product.Price = 12;
            List<OColor> ls = new List<OColor>();
            ls.Add(new OColor() { Hex = "#59591", Name = "white", Image = "" });
            Product.Colors = ls;
        }

        private void loadColor()
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (Product.Colors[0].Image == "")
                    return;
                try
                {
                    var request = WebRequest.Create(Product.Colors[0].Image);

                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        productImage.Invoke((MethodInvoker)delegate { productImage.Image = Bitmap.FromStream(stream); });
                    }
                }
                catch { }
            }));
            t.Start();
            productTitleName.Text = Product.Name;
            productPrice.Text = "$"+ Product.Price.ToString("N2");
            g = CreateGraphics();
            // Create solid brush.
            for (int i = 0; i < 10; i++)
            {
                if (i < 10)
                {
                    SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml("#595912" + i));
                    r = new Rectangle(140 + (i * 45) + 10, 50, 35, 35);
                    g.FillEllipse(brush, r);
                }
            }
            for (int i = 10; i < Product.Colors.Count; i++)
            {
                SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml(Product.Colors[0].Hex + i));
                r = new Rectangle(140 + (i * 45) + 10, 100, 35, 35);
                g.FillEllipse(brush, r);
            }

            g.Dispose();
        }

        private void UCItemProduct_Paint(object sender, PaintEventArgs e)
        {
            loadColor();
        }
    }
}
