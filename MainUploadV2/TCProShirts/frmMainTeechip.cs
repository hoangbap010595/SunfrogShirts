﻿using DevExpress.XtraEditors;
using MainUploadV2.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using TCProShirts.Models;
using TCProShirts.UControls;

namespace TCProShirts
{
    public partial class frmMainTeechip : XtraForm
    {
        private static CookieContainer cookieApplication = new CookieContainer();
        //private string BROWSER_CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private string BROWSER_FIREFOX = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:49.0) Gecko/20100101 Firefox/49.0";
        private ApplicationUser User;
        private string currToken = "";
        private List<Product> lsBulkProduct;
        private List<Category> lsCategoryProduct;
        private DataTable dtDataTemp;
        private List<Dictionary<string, object>> dt1 = new List<Dictionary<string, object>>();
        private List<Dictionary<string, object>> dt2 = new List<Dictionary<string, object>>();
        private List<UserControl> lsUserControlTheme = new List<UserControl>();
        private List<string> lsImageFileNames;
        private int currentIndexUpload = -1;
        private int currentIndexUpload2 = -1;
        //private string _IDDesign = "5a291e8b13725c5c1bf80020";

        private string POSTER = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"poster-standard\",\"id\":\"{2}-36710\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.85,\"unit\":\"percentage\"}}},\"handling\":\"default\"}";
        private string CASE = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"case-standard\",\"id\":\"{2}-38916\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":-0.002962962962962945},\"horizontal\":{\"origin\":\"C\",\"offset\":-0.00666666666666671}},\"size\":{\"width\":0.46020776295794047,\"unit\":\"percentage\"}}},\"handling\":\"default\"}";
        private string GENERAL_SLIM = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"general-slim\",\"id\":\"{2}-7838\",\"sides\":{\"right\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":4,\"unit\":\"inch\"}},\"left\":{\"artworkId\":\"@ArtworkID\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":4,\"unit\":\"inch\"}}},\"handling\":\"default\"}";
        private string MUG = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"mug-standard\",\"id\":\"{2}-67858\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\": 0}},\"size\":{\"width\":0.6855757689683196,\"unit\":\"percentage\"}}},\"handling\":\"default\"}";
        private string GENRAL = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"general-standard\",\"id\":\"{2}-75642\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":2},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":14,\"unit\":\"inch\"}}},\"handling\":\"default\"}";
        private string HAT = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"hat-standard\",\"id\":\"{2}-21944\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":2.8083610329838917,\"unit\":\"inch\"}}},\"handling\":\"default\"}";
        private string GENERAL_REDUCED = "{\"designId\":\"{0}\",\"entityId\":\"{1}}\",\"id\":\"{2}-94026\",\"printSize\":\"general-reduced\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":4,\"unit\":\"inch\"}}},\"handling\":\"default\"}";
        //Update 22.04.2018
        private string SubShirt = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-shirt\",\"id\":\"{2}-62074\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0.03497444175410258},\"horizontal\":{\"origin\":\"C\",\"offset\":-0.002222222222221848}},\"size\":{\"width\":0.4751099148262992,\"unit\":\"percentage\"},\"details\":{\"dpi\":198.18694791084513,\"widthInch\":19.5270174993609,\"heightInch\":11.418511783218014}}},\"handling\":\"default\",\"productCategory\":\"garment\",\"bleedDimensions\":{\"width\":41.1,\"height\":33.9}}";
        private string SubDress = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-dress\",\"id\":\"{2}-91123\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.4703201742063542,\"unit\":\"percentage\"},\"details\":{\"dpi\":243.55297212106382,\"widthInch\":15.889767085561674,\"heightInch\":9.291613156234126}}},\"handling\":\"default\",\"productCategory\":\"garment\",\"bleedDimensions\":{\"width\":33.785,\"height\":39.425}}";
        private string SubTank = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-tank\",\"id\":\"{2}-70481\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6632749493706753,\"unit\":\"percentage\"},\"details\":{\"dpi\":216.90276579903966,\"widthInch\":17.842096138071167,\"heightInch\":10.4332463980504}}},\"handling\":\"default\",\"productCategory\":\"garment\",\"bleedDimensions\":{\"width\":26.9,\"height\":39}}";
        private string SubMousePad = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-mousepad\",\"id\":\"{2}-93978\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.7461981945097762,\"unit\":\"percentage\"},\"details\":{\"dpi\":560.6799660688499,\"widthInch\":6.90233329921543,\"heightInch\":4.036170608817705}}},\"handling\":\"default\",\"productCategory\":\"accessories\",\"bleedDimensions\":{\"width\":9.25,\"height\":7.75}}";
        private string SubTowelHand = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-towel-hand\",\"id\":\"{2}-67192\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":359.8198198198198,\"widthInch\":11.1,\"heightInch\":10.869328993490235}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":18.5,\"height\":12}}";
        private string SubPillRect = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-pillowcase-rect\",\"id\":\"{2}-34284\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":231.53623188405797,\"widthInch\":17.25,\"heightInch\":16.89152478718077}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":28.75,\"height\":20.25}}";
        private string SubTowelBeach = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-towel-beach\",\"id\":\"{2}-67021\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":214.73118279569894,\"widthInch\":18.599999999999998,\"heightInch\":18.21347020530796}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":31,\"height\":60}}";
        private string SubPillSquare = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-pillowcase-square\",\"id\":\"{2}-46241\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":422.6455026455027,\"widthInch\":9.45,\"heightInch\":9.253617926890334}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":15.75,\"height\":15.75}}";
        private string SubCoasterSquare = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-coaster-square\",\"id\":\"{2}-12396\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":1386.8055555555557,\"widthInch\":2.88,\"heightInch\":2.820150225338007}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":4.8,\"height\":4.8}}";
        private string SubCoasterCircle = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-coaster-circle\",\"id\":\"{2}-80894\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":1386.8055555555557,\"widthInch\":2.88,\"heightInch\":2.820150225338007}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":4.8,\"height\":4.8}}";
        private string SubMagnetCircle = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-magnet-circle\",\"id\":\"{2}-97518\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":2377.3809523809523,\"widthInch\":1.68,\"heightInch\":1.6450876314471705}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":2.8,\"height\":2.8}}";
        private string SubMagnetSquare = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-magnet-square\",\"id\":\"{2}-98910\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":2802.8070175438597,\"widthInch\":1.425,\"heightInch\":1.395386830245368}}},\"handling\":\"default\",\"productCategory\":\"housewares\",\"bleedDimensions\":{\"width\":2.375,\"height\":2.375}}";
        //Update 23.04.2018
        private string SETeardrop = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-earring-teardrop\",\"id\":\"{2}-53015\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":5917.037037037037,\"widthInch\":0.6749999999999999,\"heightInch\":0.6609727090635953}}},\"handling\":\"default\",\"productCategory\":\"jewelry\",\"bleedDimensions\":{\"width\":1.125,\"height\":1.4931}}";
        private string SBCircle = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-bracelet-circle\",\"id\":\"{2}-10088\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":5733.333333333334,\"widthInch\":0.6749999999999999,\"heightInch\":0.3947093023255813}}},\"handling\":\"default\",\"productCategory\":\"jewelry\",\"bleedDimensions\":{\"width\":1.125,\"height\":1.125}}";
        private string SECircle = "{\"designId\":\"{0}\",\"entityId\":\"{1}\",\"printSize\":\"sublimation-earring-circle\",\"id\":\"{2}-39590\",\"sides\":{\"front\":{\"artworkId\":\"{3}\",\"position\":{\"vertical\":{\"origin\":\"C\",\"offset\":0},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":0.6,\"unit\":\"percentage\"},\"details\":{\"dpi\":5917.037037037037,\"widthInch\":0.6749999999999999,\"heightInch\":0.6609727090635953}}},\"handling\":\"default\",\"productCategory\":\"jewelry\",\"bleedDimensions\":{\"width\":1.125,\"height\":1.125}}";
        public frmMainTeechip()
        {
            InitializeComponent();
            User = new ApplicationUser();
        }
        private void getUser(ApplicationUser user)
        {
            if (user != null)
            {
                User = user;
                ApplicationLibary.writeLogThread(lsBoxLog, "Login Successfully", 1);
                this.Invoke((MethodInvoker)delegate { this.Text += " - [" + User.Email + "]"; });
            }
            else
            {
                this.Close();
            }
        }
        private void testSaveFile()
        {
            ApplicationLibary.saveDataTableToFileCSV(txtPath.Text, dtDataTemp);
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //string a = getStringCategory("fish","hunting");
            //btnLogin_Click(sender, e);
            User = new ApplicationUser();
            frmLoginTeechip frm = new frmLoginTeechip();
            frm.senduser = new frmLoginTeechip.SendUser(getUser);
            frm.ShowDialog();
            loadBulkProduct();
            loadBulkCategory();
            loadBulkThemes();

        }
        #region =======LoadData=======
        private void loadBulkProduct()
        {
            lsBulkProduct = GetListBulk();
            info_cbbProduct.Properties.DataSource = lsBulkProduct;
            info_cbbProduct.Properties.DisplayMember = "Name";
            info_cbbProduct.Properties.ValueMember = "_Id";

            info_cbbProduct.ItemIndex = 0;
        }
        private void loadBulkThemes()
        {
            lueThemes.Properties.DataSource = GetListThemes();
            lueThemes.Properties.DisplayMember = "Name";
            lueThemes.Properties.ValueMember = "Id";
            lueThemes.ItemIndex = 0;
        }
        private void loadBulkCategory()
        {
            lsCategoryProduct = GetListCategory();
            lueCategory.Properties.DataSource = lsCategoryProduct;
            lueCategory.Properties.DisplayMember = "Name";
            lueCategory.Properties.ValueMember = "Id";
            lueCategory.ItemIndex = 0;
        }


        #endregion
        private void enableB(bool b)
        {
            btnStart.Enabled = b;
            btnViewData.Enabled = b;
            btnApplyTheme.Enabled = b;
            btnOpenFileExcel.Enabled = b;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var urlLogin = "https://pro.teechip.com/manager/auth/login";
            //var data2Send = "{\"email\":\"lchoang1995@gmail.com\",\"password\":\"Thienan@111\"}";
            var data2Send = "{\"email\":\"quach555@yahoo.com\",\"password\":\"quach555\"}";
            //Step 1
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(urlLogin);
            wRequest.Host = "pro.teechip.com";
            wRequest.CookieContainer = new CookieContainer();
            Dictionary<string, object> stepLogin = GetDataAPI(wRequest);
            cookieApplication = (CookieContainer)stepLogin["cookies"];
            //Step 2
            HttpWebRequest wRequestLogin = (HttpWebRequest)WebRequest.Create(urlLogin);
            wRequestLogin.Referer = "https://pro.teechip.com/manager/auth/login";
            wRequestLogin.ContentType = "application/json";
            wRequestLogin.Host = "pro.teechip.com";
            wRequestLogin.CookieContainer = cookieApplication;
            wRequestLogin.Headers.Add("x-xsrf-token", currToken);
            Dictionary<string, object> step2Login = PostDataAPI(wRequestLogin, data2Send);
            cookieApplication = (CookieContainer)step2Login["cookies"];
            var rs = step2Login["data"].ToString();
            if (int.Parse(step2Login["status"].ToString()) == -1)
            {
                XtraMessageBox.Show("Sai thông tin tài khoản hoặc mật khẩu\n" + rs, "Thông báo");
                return;
            }
            var obj = JObject.Parse(rs);
            User.UserID = obj["_id"].ToString();
            User.Email = obj["email"].ToString();
            User.ApiKey = obj["apiKey"].ToString();
            //User.ViewOnlyApiKey = obj["viewOnlyApiKey"].ToString();
            User.GroupID = obj["groupId"].ToString();
            User.EntityID = obj["entities"][0]["entityId"].ToString();
            User.Authorization = "Basic " + ApplicationLibary.Base64Encode(":" + User.ApiKey);
            User.UnAuthorization = "Basic " + ApplicationLibary.Base64Encode("undefined:" + User.ApiKey);
            ApplicationLibary.writeLog(lsBoxLog, "Login Successfully", 1);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (User == null)
            {
                XtraMessageBox.Show("Please login to upload!", "Message");
                enableB(true);
            }
            enableB(false);
            if (lsUserControlTheme.Count == 0)
            {
                XtraMessageBox.Show("No Style is not selected...!", "Message");
                enableB(true);
                return;
            }
            if (ckUsingFileUpload.Checked)
            {
                currentIndexUpload2 = dtDataTemp.Rows.Count - 1;
                if (dtDataTemp == null || dtDataTemp.Rows.Count == 0)
                {
                    XtraMessageBox.Show("File data is not found!", "Message");
                    enableB(true);
                }

                Thread tStart1 = new Thread(new ThreadStart(() =>
                {
                    UploadFromFile(dt2);
                    btnStart.Invoke((MethodInvoker)delegate { btnStart.Enabled = true; });
                    btnViewData.Invoke((MethodInvoker)delegate { btnViewData.Enabled = true; });
                    btnApplyTheme.Invoke((MethodInvoker)delegate { btnApplyTheme.Enabled = true; });
                    btnOpenFileExcel.Invoke((MethodInvoker)delegate { btnOpenFileExcel.Enabled = true; });
                }));
                tStart1.Start();
            }
            else
            {
                currentIndexUpload = -1;
                if (txtTitle.Text == "")
                {
                    XtraMessageBox.Show("Title is not empty!", "Message");
                    enableB(true);
                }
                if (memoDescription.Text == "")
                {
                    XtraMessageBox.Show("Description is not empty!", "Message");
                    enableB(true);
                }
                Thread tStart = new Thread(new ThreadStart(() =>
                {
                    UploadProgress();
                    btnStart.Invoke((MethodInvoker)delegate { btnStart.Enabled = true; });
                    btnViewData.Invoke((MethodInvoker)delegate { btnViewData.Enabled = true; });
                    btnApplyTheme.Invoke((MethodInvoker)delegate { btnApplyTheme.Enabled = true; });
                    btnOpenFileExcel.Invoke((MethodInvoker)delegate { btnOpenFileExcel.Enabled = true; });
                }));
                tStart.Start();
            }
        }

        private void loadDataToTable(DataTable dt)
        {
            int x1, index;
            index = 0;
            x1 = dt.Rows.Count / 2;

            Dictionary<string, object> data;
            //for (; index < x1; index++)
            //{
            //    data = new Dictionary<string, object>();
            //    for (int j = 0; j < dtDataTemp.Columns.Count; j++)
            //    {
            //        var col = dtDataTemp.Columns[j].ColumnName;
            //        var dr = dtDataTemp.Rows[index][col].ToString();
            //        data.Add(col, dr);
            //    }
            //    dt1.Add(data);
            //}
            for (; index < dt.Rows.Count; index++)
            {
                data = new Dictionary<string, object>();
                for (int j = 0; j < dtDataTemp.Columns.Count; j++)
                {
                    var col = dtDataTemp.Columns[j].ColumnName;
                    var dr = dtDataTemp.Rows[index][col].ToString();
                    data.Add(col, dr);
                }
                dt2.Add(data);
            }

        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            var obj = lueCategory.GetSelectedDataRow();
            string currCate = ((Category)obj).Name.Replace("(", "|").Replace(")", "|");
            var x = currCate.Split('|');
            var data = memoCategory.Text;
            foreach (string item in x)
            {
                var crrText = item.Trim();
                if (crrText != "" && crrText != " " && data.IndexOf(crrText) == -1)
                    memoCategory.Text += crrText.Trim() + ",";
            }
        }
        private void btnApplyTheme_Click(object sender, EventArgs e)
        {
            Product p = new Product();
            var obj = info_cbbProduct.GetSelectedDataRow();
            p._Id = ((Product)obj)._Id;
            p.Name = ((Product)obj).Name;
            p.Colors = new List<OColor>();
            var price = txtPrice.Text.Trim().Replace(".", string.Empty);
            List<string> selColors = new List<string>();
            for (int i = 0; i < ckListBoxColor.Items.Count; i++)
            {
                if (ckListBoxColor.GetItemChecked(i))
                {
                    string str = ckListBoxColor.GetItemText(ckListBoxColor.Items[i]);
                    selColors.Add(str);
                }
            }
            if (selColors.Count == 0)
            {
                XtraMessageBox.Show("No color selected!", "Message");
                return;
            }
            var rs = Resources.bulkCode;
            JArray jArray = JArray.Parse(rs);
            foreach (var item in jArray)
            {
                if (p._Id.Equals(item["_id"].ToString()))
                {
                    p.PrintSize = item["printSize"].ToString();
                    p.Price = (price == "" || price == "0" || price == "00") ? int.Parse(item["msrp"].ToString()) : int.Parse(price);
                    var colors = JArray.Parse(item["colors"].ToString());
                    foreach (var selcolor in selColors)
                    {
                        foreach (var color in colors)
                        {
                            if (color["name"].ToString() == selcolor)
                            {
                                OColor c = new OColor();
                                c.Name = color["name"].ToString();
                                c.Hex = color["hex"].ToString();
                                c.Image = color["image"].ToString();
                                p.Colors.Add(c);
                            }
                        }
                    }
                }

            }
            UCItemProduct frm = new UCItemProduct();
            frm.Name = p._Id;
            frm.Product = p;
            int check = -1;
            foreach (UCItemProduct item in lsUserControlTheme)
            {
                if (item.Product._Id == frm.Product._Id)
                {
                    check = 1;
                    break;
                }
            }
            if (check == -1)
                addThemeToPanel(frm);
            else
            {
                XtraMessageBox.Show("Style was selected...!", "Message");
            }

        }
        private void btnChooesImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Multiselect = true;
                op.Filter = "Image .png|*.png";
                if (DialogResult.OK == op.ShowDialog())
                {
                    lsImageFileNames = op.FileNames.ToList();
                    foreach (var item in lsImageFileNames)
                    {
                        lsBoxImage.Items.Add(Path.GetFileName(item));
                    }
                    ApplicationLibary.writeLog(lsBoxLog, "Selected " + lsImageFileNames.Count + " file(s)", 1);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message);
            }
        }
        /// <summary>
        /// Chọn file Excel chứa dữ liệu cần upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFileExcel_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Excel .csv|*.csv|Excel .xlsx|*.xlsx|Excel .xls|*.xls";
                if (DialogResult.OK == op.ShowDialog())
                {
                    txtPath.Text = op.FileName;
                    dtDataTemp = new DataTable();
                    dt1 = new List<Dictionary<string, object>>();
                    dt2 = new List<Dictionary<string, object>>();
                    var x = Path.GetExtension(op.FileName);
                    if (x == ".csv")
                        dtDataTemp = ApplicationLibary.getDataExcelFromFileCSVToDataTable(op.FileName);
                    else
                        dtDataTemp = ApplicationLibary.getDataExcelFromFileToDataTable(op.FileName);
                    loadDataToTable(dtDataTemp);
                    ApplicationLibary.writeLog(lsBoxLog, "Success " + dtDataTemp.Rows.Count + " record(s) is opened", 1);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message);
            }
        }
        /// <summary>
        /// Xóa log event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            lsBoxLog.Items.Clear();
            ApplicationLibary.writeLog(lsBoxLog, "CLEAR", 1);
        }
        private void btnViewData_Click(object sender, EventArgs e)
        {
            try
            {
                pictureShowImage.ImageLocation = "";

                var obj = info_cbbProduct.GetSelectedDataRow();
                var data = ((Product)obj)._Id;
                var rs = Resources.bulkCode;
                JArray jArray = JArray.Parse(rs);
                foreach (var item in jArray)
                {
                    if (data.Equals(item["_id"].ToString()))
                    {
                        var colors = JArray.Parse(item["colors"].ToString());
                        Random r = new Random();
                        int x = r.Next(0, colors.Count);
                        var url = colors[x]["image"].ToString();
                        loadImage(url);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message, "Message");
            }
        }
        private void btnViewHashPass_Click(object sender, EventArgs e)
        {
            frmDialog frm = new frmDialog(User.HasPassword);
            frm.Text = "View HasPassword";
            frm.ShowDialog();
        }
        private void btnSaveThemes_Click(object sender, EventArgs e)
        {
            if (lsUserControlTheme.Count == 0)
            {
                XtraMessageBox.Show("No Style is not selected...!", "Message");
                return;
            }
            frmDialog frm = new frmDialog(1);
            frm.sendText = new frmDialog.SendText(getTextDialog);
            frm.Text = "Enter design name...";
            frm.ShowDialog();

        }
        private void getTextDialog(string text)
        {
            var data = "[";
            foreach (UCItemProduct item in lsUserControlTheme)
            {
                var jsData = JsonConvert.SerializeObject(item.Product);
                data += jsData + ",";
            }
            data = data.TrimEnd(',');
            data += "]";

            ApplicationLibary.writeFileToResource(text, data);
            loadBulkThemes();
        }
        private void btnClearThemes_Click(object sender, EventArgs e)
        {
            lsUserControlTheme.Clear();
            xtraScrollableTheme.Controls.Clear();
        }
        /*
         * Upload From File Excel
         */
        //Step 1
        private void UploadFromFile(List<Dictionary<string, object>> dt)
        {
            foreach (Dictionary<string, object> item in dt)
            {
                try
                {
                    currentIndexUpload++;
                    var uTitle = item["Title"];
                    var uDescription = "<div>" + item["Description"].ToString().Trim() + "</div>";
                    //var cate1 = ApplicationLibary.convertStringToJson(getStringCategory(item["Category"].ToString()));
                    //var cate2 = ApplicationLibary.convertStringToJson(getStringCategory(item["Category2"].ToString()));
                    var uCategory = ApplicationLibary.convertStringToJson(getStringCategory(item["Category"].ToString(), item["Category2"].ToString()));
                    var uUrl = item["URL"].ToString().ToLower();
                    var uStore = item["Store"].ToString();
                    var image = item["Image"].ToString();
                    var uImage = image.Split('.').Length == 1 ? image + ".png" : image;
                    var urlUploadImage = "https://scalable-licensing.s3.amazonaws.com/";
                    uDescription = uDescription.Replace("\n", "<br/>");
                    if (uTitle.ToString().Length > 50)
                        uTitle = uTitle.ToString().Substring(0, 49);
                    if (!File.Exists(uImage))
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, "File do not exists in folder: [" + image + "]", 2);
                        continue;
                    }
                    ApplicationLibary.writeLogThread(lsBoxLog, "Category: " + uCategory, 1);
                    string fileUrl = uImage;
                    var imgDessign = Path.GetFileName(fileUrl);
                    #region ============== Upload Image & Get AtWork==================
                    ApplicationLibary.writeLogThread(lsBoxLog, "Uploading: " + imgDessign, 3);
                    string fileUpload = "uploads/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + DateTime.Now.Ticks.ToString("x") + ".png";
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("key", fileUpload);
                    nvc.Add("bucket", "scalable-licensing");
                    nvc.Add("AWSAccessKeyId", "AKIAJE4QLGLTY4DH4WRA");
                    nvc.Add("Policy", "eyJleHBpcmF0aW9uIjoiMzAwMC0wMS0wMVQwMDowMDowMFoiLCJjb25kaXRpb25zIjpbeyJidWNrZXQiOiJzY2FsYWJsZS1saWNlbnNpbmcifSxbInN0YXJ0cy13aXRoIiwiJGtleSIsInVwbG9hZHMvIl0seyJhY2wiOiJwdWJsaWMtcmVhZCJ9XX0=");
                    nvc.Add("Signature", "4yVrFVzCgzWg2BH8RkrI6LVi11Y=");
                    nvc.Add("acl", "public-read");
                    Dictionary<string, object> data = HttpUploadFile(urlUploadImage, fileUrl, "file", "image/png", nvc);
                    if (int.Parse(data["status"].ToString()) == -1)
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, "Step 0: Upload Image - " + data["data"].ToString(), 2);
                        continue;
                    }
                    var urlImage = HttpUtility.UrlDecode(data["data"].ToString());
                    var data2Send = "{\"artwork\":\"" + urlImage + "\",\"AB\":{\"ab-use-dpi\":false}}";
                    HttpWebRequest wAtWork = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/artworks");
                    wAtWork.Host = "api.scalablelicensing.com";
                    wAtWork.Accept = "application/json, text/plain, */*";
                    wAtWork.ContentType = "application/json";

                    Dictionary<string, object> dataAtwork = PostDataAPI(wAtWork, data2Send);
                    var rs = dataAtwork["data"].ToString();
                    var obj = JObject.Parse(rs);
                    var atworkID = obj["artworkId"].ToString();
                    #endregion

                    #region ===============Step 1: Create Design & Get ID Design=============
                    var data2SendUpload = "{\"name\":\"" + uTitle + "\",\"entityId\":\"" + User.EntityID + "\",\"tags\":{\"style\":[" + uCategory + "]}}";

                    HttpWebRequest wCost = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/designs");
                    wCost.Accept = "application/json, text/plain, */*";
                    wCost.ContentType = "application/json";
                    wCost.PreAuthenticate = true;
                    wCost.Headers.Add("Authorization", User.Authorization);

                    Dictionary<string, object> dataUpload = PostDataAPI(wCost, data2SendUpload);
                    var rsUpload = dataUpload["data"].ToString();
                    var statusUpload = int.Parse(dataUpload["status"].ToString());
                    if (statusUpload == -1)
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, "Step 1: " + rsUpload, 2);
                        continue;
                    }
                    var objUpload = JObject.Parse(rsUpload);
                    var _IDDesign = objUpload["_id"].ToString();
                    #endregion

                    #region ===============Step 2: Create Design Line & Get DesignLine ID===============
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var data2SendLineIDPOSTER = @POSTER.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineIDCASE = @CASE.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineIDGENERAL_SLIM = @GENERAL_SLIM.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineIDHAT = @HAT.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineIDGENRAL = @GENRAL.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineIDMUG = @MUG.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineIDREDUCED = GENERAL_REDUCED.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);

                    Dictionary<string, object> lineID = new Dictionary<string, object>();
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "general-standard") != null)
                        lineID.Add("LineIDGENRAL", getDesignLineID(data2SendLineIDGENRAL));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "mug-standard") != null)
                        lineID.Add("LineIDMUG", getDesignLineID(data2SendLineIDMUG));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "poster-standard") != null)
                        lineID.Add("LineIDPOSTER", getDesignLineID(data2SendLineIDPOSTER));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "case-standard") != null)
                        lineID.Add("LineIDCASE", getDesignLineID(data2SendLineIDCASE));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "general-slim") != null)
                        lineID.Add("LineIDGENERAL_SLIM", getDesignLineID(data2SendLineIDGENERAL_SLIM));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "hat-standard") != null)
                        lineID.Add("LineIDHAT", getDesignLineID(data2SendLineIDHAT));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "general-reduced") != null)
                        lineID.Add("LineIDREDUCED", getDesignLineID(data2SendLineIDREDUCED));
                    #endregion
                    //Step 3 -- Tham số cần truyền: 
                    //      1. productId, color, price: người dùng chọn
                    var objIDReail = getAllRetailIDFromDesignID(lineID);

                    if (string.IsNullOrEmpty(uUrl))
                        uUrl = GenUrl(12);
                    else
                        uUrl = uUrl.Replace(" ", "").Trim();
                    uUrl += DateTime.Now.ToString("mmss");
                    //Step 4 -- Nhận giá trị 1 mảng _IDDesignRetail từ Step 3
                    var data2SendCampaigns = "{\"url\":\"" + uUrl + "\",\"title\":\"" + uTitle + "\",\"description\":\"" + uDescription + "\",\"duration\":24,\"policies\":{\"forever\":true,\"fulfillment\":24,\"private\":false,\"checkout\":\"direct\"},\"social\":{\"trackingTags\":{}},\"entityId\":\"" + User.EntityID + "\",\"upsells\":[],\"tags\":{\"style\":[" + uCategory + "]},\"related\": " + objIDReail + "}";
                    //data2SendCampaigns = data2SendCampaigns.Replace("\n", "<br />");
                    finishUploadImage(data2SendCampaigns, uImage);
                    dtDataTemp.Rows[currentIndexUpload]["Status"] = "Done";
                    ApplicationLibary.saveDataTableToFileCSV(txtPath.Text, dtDataTemp);
                }
                catch (Exception ex)
                {
                    ApplicationLibary.writeLogThread(lsBoxLog, ex.Message, 2);
                }
            }
        }

        //1.Upload From FormData
        private void UploadProgress()
        {
            foreach (var fileImage in lsImageFileNames)
            {
                try
                {
                    string text_all = Path.GetFileName(fileImage).Split('.')[0];

                    var cate = memoCategory.Text.Split(',');
                    string strCate = "";
                    if (cate.Length > 6)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            strCate += cate[i];
                        }
                    }
                    else
                        strCate = memoCategory.Text;
                    var uTitle = txtTitle.Text.Trim();
                    var uDescription = @"<div>" + memoDescription.Text.ToString() + "</div>";
                    var uCategory = ApplicationLibary.convertStringToJson(strCate);
                    var uUrl = txtUrl.Text.ToLower();
                    var uStore = txtStore.Text;

                    uTitle = uTitle.Replace("$name", text_all);
                    uDescription = uDescription.Replace("$name", text_all);

                    if (string.IsNullOrEmpty(uUrl) || uUrl == "{0}")
                        uUrl = GenUrl(12);
                    else
                        uUrl = uUrl.Replace(" ", "").Trim();
                    uUrl += DateTime.Now.ToString("mmss");
                    var urlUploadImage = "https://scalable-licensing.s3.amazonaws.com/";
                    if (uTitle.Length > 50)
                        uTitle = uTitle.Substring(0, 49);

                    if (!File.Exists(fileImage))
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, "File do not exists in folder: [" + fileImage + "]", 2);
                        continue;
                    }

                    var imgDessign = Path.GetFileName(fileImage);
                    #region ============== Upload Image & Get AtWork==================
                    ApplicationLibary.writeLogThread(lsBoxLog, "Uploading: " + imgDessign, 3);
                    string fileUpload = "uploads/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + DateTime.Now.Ticks.ToString("x") + ".png";
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("key", fileUpload);
                    nvc.Add("bucket", "scalable-licensing");
                    nvc.Add("AWSAccessKeyId", "AKIAJE4QLGLTY4DH4WRA");
                    nvc.Add("Policy", "eyJleHBpcmF0aW9uIjoiMzAwMC0wMS0wMVQwMDowMDowMFoiLCJjb25kaXRpb25zIjpbeyJidWNrZXQiOiJzY2FsYWJsZS1saWNlbnNpbmcifSxbInN0YXJ0cy13aXRoIiwiJGtleSIsInVwbG9hZHMvIl0seyJhY2wiOiJwdWJsaWMtcmVhZCJ9XX0=");
                    nvc.Add("Signature", "4yVrFVzCgzWg2BH8RkrI6LVi11Y=");
                    nvc.Add("acl", "public-read");
                    Dictionary<string, object> data = HttpUploadFile(urlUploadImage, fileImage, "file", "image/png", nvc);
                    if (int.Parse(data["status"].ToString()) == -1)
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, "Step 0: Upload Image - " + data["data"].ToString(), 2);
                        continue;
                    }
                    var urlImage = HttpUtility.UrlDecode(data["data"].ToString());

                    var data2Send = "{\"artwork\":\"" + urlImage + "\",\"AB\":{\"ab-use-dpi\":false}}";
                    HttpWebRequest wAtWork = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/artworks");
                    wAtWork.Host = "api.scalablelicensing.com";
                    wAtWork.Accept = "application/json, text/plain, */*";
                    wAtWork.ContentType = "application/json";

                    Dictionary<string, object> dataAtwork = PostDataAPI(wAtWork, data2Send);
                    var rs = dataAtwork["data"].ToString();
                    var obj = JObject.Parse(rs);
                    var atworkID = obj["artworkId"].ToString();
                    #endregion

                    #region ===============Step 1: Create Design & Get ID Design=============
                    var data2SendUpload = "{\"name\":\"" + uTitle + "\",\"entityId\":\"" + User.EntityID + "\",\"tags\":{\"style\":[" + uCategory + "]}}";

                    HttpWebRequest wCost = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/designs");
                    wCost.Accept = "application/json, text/plain, */*";
                    wCost.ContentType = "application/json";
                    wCost.PreAuthenticate = true;
                    wCost.Headers.Add("Authorization", User.Authorization);

                    Dictionary<string, object> dataUpload = PostDataAPI(wCost, data2SendUpload);
                    var rsUpload = dataUpload["data"].ToString();
                    var statusUpload = int.Parse(dataUpload["status"].ToString());
                    if (statusUpload == -1)
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, "Step 1: " + rsUpload, 2);
                        continue;
                    }
                    var objUpload = JObject.Parse(rsUpload);
                    var _IDDesign = objUpload["_id"].ToString();
                    #endregion
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var data2SendLineID_POSTER = @POSTER.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_CASE = @CASE.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_GENERAL_SLIM = @GENERAL_SLIM.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_HAT = @HAT.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_GENRAL = @GENRAL.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_MUG = @MUG.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_REDUCED = @GENERAL_REDUCED.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    //Update 22.04.2018
                    var data2SendLineID_SubShirt = @SubShirt.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubDress = @SubDress.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubTank = @SubTank.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubMousePad = @SubMousePad.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubTowelHand = @SubTowelHand.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubTowelBeach = @SubTowelBeach.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubPillRect = @SubPillRect.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubPillSquare = @SubPillSquare.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubCoasterSquare = @SubCoasterSquare.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubCoasterCircle = @SubCoasterCircle.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubMagnetSquare = @SubMagnetSquare.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SubMagnetCircle = @SubMagnetCircle.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    //Update 23.04.2018
                    var data2SendLineID_SETeardrop = @SETeardrop.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SBCircle = @SBCircle.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    var data2SendLineID_SECircle = @SECircle.Replace("{0}", _IDDesign).Replace("{1}", User.EntityID).Replace("{2}", unixTimestamp.ToString()).Replace("{3}", atworkID);
                    #region ===============Step 2: Create Design Line & Get DesignLine ID===============
                    Dictionary<string, object> lineID = new Dictionary<string, object>();
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "general-standard") != null)
                        lineID.Add("LineID_GENRAL", getDesignLineID(data2SendLineID_GENRAL));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "mug-standard") != null)
                        lineID.Add("LineID_MUG", getDesignLineID(data2SendLineID_MUG));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "poster-standard") != null)
                        lineID.Add("LineID_POSTER", getDesignLineID(data2SendLineID_POSTER));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "case-standard") != null)
                        lineID.Add("LineID_CASE", getDesignLineID(data2SendLineID_CASE));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "general-slim") != null)
                        lineID.Add("LineID_GENERAL_SLIM", getDesignLineID(data2SendLineID_GENERAL_SLIM));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "hat-standard") != null)
                        lineID.Add("LineID_HAT", getDesignLineID(data2SendLineID_HAT));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "general-reduced") != null)
                        lineID.Add("LineID_REDUCED", getDesignLineID(data2SendLineID_REDUCED));
                    //Update 22.04.2018
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-shirt") != null)
                        lineID.Add("LineID_SubShirt", getDesignLineID(data2SendLineID_SubShirt));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-dress") != null)
                        lineID.Add("LineID_SubDress", getDesignLineID(data2SendLineID_SubDress));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-tank") != null)
                        lineID.Add("LineID_SubTank", getDesignLineID(data2SendLineID_SubTank));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-mousepad") != null)
                        lineID.Add("LineID_SubMousePad", getDesignLineID(data2SendLineID_SubMousePad));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-towel-hand") != null)
                        lineID.Add("LineID_SubTowelHand", getDesignLineID(data2SendLineID_SubTowelHand));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-pillowcase-rect") != null)
                        lineID.Add("LineID_SubPillRect", getDesignLineID(data2SendLineID_SubPillRect));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-towel-beach") != null)
                        lineID.Add("LineID_SubTowelBeach", getDesignLineID(data2SendLineID_SubTowelBeach));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-pillowcase-square") != null)
                        lineID.Add("LineID_SubPillSquare", getDesignLineID(data2SendLineID_SubPillSquare));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-coaster-square") != null)
                        lineID.Add("LineID_SubCoasterSquare", getDesignLineID(data2SendLineID_SubCoasterSquare));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-coaster-circle") != null)
                        lineID.Add("LineID_SubCoasterCircle", getDesignLineID(data2SendLineID_SubCoasterCircle));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-magnet-circle") != null)
                        lineID.Add("LineID_SubMagnetCircle", getDesignLineID(data2SendLineID_SubMagnetCircle));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-magnet-square") != null)
                        lineID.Add("LineID_SubMagnetSquare", getDesignLineID(data2SendLineID_SubMagnetSquare));
                    //Update 23.04.2018
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-earring-teardrop") != null)
                        lineID.Add("LineID_SETeardrop", getDesignLineID(data2SendLineID_SETeardrop));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-bracelet-circle") != null)
                        lineID.Add("LineID_SBCircle", getDesignLineID(data2SendLineID_SBCircle));
                    if (lsUserControlTheme.Find(x => ((UCItemProduct)x).Product.PrintSize == "sublimation-earring-circle") != null)
                        lineID.Add("LineID_SECircle", getDesignLineID(data2SendLineID_SECircle));
                    #endregion

                    //Step 3 -- Tham số cần truyền: 
                    //      1. productId, color, price: người dùng chọn
                    var objIDReail = getAllRetailIDFromDesignID(lineID);

                    //Step 4 -- Nhận giá trị 1 mảng _IDDesignRetail từ Step 3
                    var data2SendCampaigns = "{\"url\":\"" + uUrl + "\",\"title\":\"" + uTitle + "\",\"description\":\"" + uDescription + "\",\"duration\":24,\"policies\":{\"forever\":true,\"fulfillment\":24,\"private\":false,\"checkout\":\"direct\"},\"social\":{\"trackingTags\":{}},\"entityId\":\"" + User.EntityID + "\",\"upsells\":[],\"tags\":{\"style\":[" + uCategory + "]},\"related\": " + objIDReail + "}";
                    finishUploadImage(data2SendCampaigns, fileImage);
                }
                catch (Exception ex)
                {
                    ApplicationLibary.writeLogThread(lsBoxLog, ex.Message, 2);
                }
            }
        }
        /// <summary>
        /// Load all Product from Entity User
        /// </summary>
        private void GetProductFromUser()
        {
            var url = "https://api.scalablelicensing.com/rest/campaigns/search?entityId=" + User.EntityID + "&status=active&limit=10000";
        }

        //Step 2: Get create Design Line & Get DesignLine ID
        private string getDesignLineID(string data2Send)
        {
            HttpWebRequest wLines = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/design-lines");
            wLines.Accept = "application/json, text/plain, */*";
            wLines.ContentType = "application/json";
            wLines.PreAuthenticate = true;
            wLines.Headers.Add("Authorization", User.Authorization);

            Dictionary<string, object> dataUploadLines = PostDataAPI(wLines, data2Send);
            var rsUploadLines = dataUploadLines["data"].ToString();
            var statusLines = int.Parse(dataUploadLines["status"].ToString());
            if (statusLines == -1)
            {
                ApplicationLibary.writeLogThread(lsBoxLog, "Step 2: " + rsUploadLines, 2);
                return "";
            }
            var objUploadLines = JObject.Parse(rsUploadLines);
            return objUploadLines["_id"].ToString();
        }
        //Step 3: Get Retail ID
        private string getAllRetailIDFromDesignID(Dictionary<string, object> dataDesignID)
        {
            List<Dictionary<string, object>> lsData = new List<Dictionary<string, object>>();
            Dictionary<string, object> data;
            List<string> lsCommand = new List<string>();
            //b1
            List<Dictionary<string, object>> themes = listCurrentThemes();
            foreach (Dictionary<string, object> item in themes)
            {
                var designID = "";
                switch (item["printSize"].ToString())
                {
                    case "mug-standard":
                        designID = dataDesignID["LineID_MUG"].ToString();
                        break;
                    case "poster-standard":
                        designID = dataDesignID["LineID_POSTER"].ToString();
                        break;
                    case "case-standard":
                        designID = dataDesignID["LineID_CASE"].ToString();
                        break;
                    case "general-slim":
                        designID = dataDesignID["LineID_GENERAL_SLIM"].ToString();
                        break;
                    case "hat-standard":
                        designID = dataDesignID["LineID_HAT"].ToString();
                        break;
                    case "general-reduced":
                        designID = dataDesignID["LineID_REDUCED"].ToString();
                        break;
                    //Update 22.04.2018
                    case "sublimation-shirt":
                        designID = dataDesignID["LineID_SubShirt"].ToString();
                        break;
                    case "sublimation-dress":
                        designID = dataDesignID["LineID_SubDress"].ToString();
                        break;
                    case "sublimation-tank":
                        designID = dataDesignID["LineID_SubTank"].ToString();
                        break;
                    case "sublimation-mousepad":
                        designID = dataDesignID["LineID_SubMousePad"].ToString();
                        break;
                    case "sublimation-towel-hand":
                        designID = dataDesignID["LineID_SubTowelHand"].ToString();
                        break;
                    case "sublimation-pillowcase-rect":
                        designID = dataDesignID["LineID_SubPillRect"].ToString();
                        break;
                    case "sublimation-towel-beach":
                        designID = dataDesignID["LineID_SubTowelBeach"].ToString();
                        break;
                    case "sublimation-pillowcase-square":
                        designID = dataDesignID["LineID_SubPillSquare"].ToString();
                        break;
                    case "sublimation-coaster-square":
                        designID = dataDesignID["LineID_SubCoasterSquare"].ToString();
                        break;
                    case "sublimation-coaster-circle":
                        designID = dataDesignID["LineID_SubCoasterCircle"].ToString();
                        break;
                    case "sublimation-magnet-circle":
                        designID = dataDesignID["LineID_SubMagnetCircle"].ToString();
                        break;
                    case "sublimation-magnet-square":
                        designID = dataDesignID["LineID_SubMagnetSquare"].ToString();
                        break;
                    //Update 23.04.2018
                    case "sublimation-earring-teardrop":
                        designID = dataDesignID["LineID_SETeardrop"].ToString();
                        break;
                    case "sublimation-bracelet-circle":
                        designID = dataDesignID["LineID_SBCircle"].ToString();
                        break;
                    case "sublimation-earring-circle":
                        designID = dataDesignID["LineID_SECircle"].ToString();
                        break;
                    case "general-standard":
                        designID = dataDesignID["LineID_GENRAL"].ToString();
                        break;
                }
                var colors = (List<string>)item["colors"];
                foreach (string color in colors)
                {
                    var data2SendRetail = "{\"designLineId\":\"" + designID + "\",\"productId\":\"" + item["productId"] + "\",\"color\":\"" + color + "\",\"price\":" + item["price"] + ",\"images\":[]}";
                    lsCommand.Add(data2SendRetail);
                }
            }
            foreach (var item in lsCommand)
            {
                try
                {
                    data = new Dictionary<string, object>();
                    HttpWebRequest wRetail = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/retail-products");
                    wRetail.Accept = "application/json, text/plain, */*";
                    wRetail.ContentType = "application/json";
                    wRetail.PreAuthenticate = true;
                    wRetail.Headers.Add("Authorization", User.Authorization);

                    Dictionary<string, object> dataUploadRetail = PostDataAPI(wRetail, item);
                    var rsUploadRetail = dataUploadRetail["data"].ToString();
                    var statusRetail = int.Parse(dataUploadRetail["status"].ToString());
                    if (statusRetail == -1)
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, "Step 3: " + rsUploadRetail, 2);
                    }
                    else
                    {
                        var objUploadRetail = JObject.Parse(rsUploadRetail);
                        data.Add("id", objUploadRetail["_id"].ToString());
                        data.Add("price", int.Parse(objUploadRetail["price"].ToString()));
                        if (lsData.Count == 0)
                            data.Add("default", true);
                        lsData.Add(data);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationLibary.writeLogThread(lsBoxLog, ex.Message, 2);
                }
            }
            var jsData = JsonConvert.SerializeObject(lsData);
            return jsData;
        }
        //Step 4: Finish
        private void finishUploadImage(string data2SendCampaigns, string uImage)
        {
            HttpWebRequest wCampaigns = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/campaigns");
            wCampaigns.Accept = "application/json, text/plain, */*";
            wCampaigns.ContentType = "application/json";
            wCampaigns.PreAuthenticate = true;
            wCampaigns.Headers.Add("Authorization", User.Authorization);

            Dictionary<string, object> dataUploadCampaigns = PostDataAPI(wCampaigns, data2SendCampaigns);
            var rsUploadCampaigns = dataUploadCampaigns["data"].ToString();
            var statusCampaigns = int.Parse(dataUploadCampaigns["status"].ToString());
            if (statusCampaigns == -1)
            {
                ApplicationLibary.writeLogThread(lsBoxLog, "Step 4: " + rsUploadCampaigns, 2);
                return;
            }
            var objUploadCampaigns = JObject.Parse(rsUploadCampaigns);
            var titleCampaigns = objUploadCampaigns["title"].ToString();
            var urlCampaigns = "https://pro.teechip.com/" + objUploadCampaigns["url"].ToString();
            ApplicationLibary.writeLogThread(lsBoxLog, "Upload finish: " + titleCampaigns + ", Link:" + urlCampaigns, 1);
            MoveFileUploaded(uImage);
        }
        #region ==============Function===============
        public static Dictionary<string, object> HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            HttpWebRequest wr = null;
            WebResponse wresp = null;
            try
            {
                Console.WriteLine(string.Format("Uploading {0} to {1}", file, url));
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                wr = (HttpWebRequest)WebRequest.Create(url);
                wr.Host = "scalable-licensing.s3.amazonaws.com";
                wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:49.0) Gecko/20100101 Firefox/49.0";
                wr.Accept = "application/json";
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.Headers.Add("Origin", "https://pro.teechip.com");
                wr.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                wr.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                wr.ServicePoint.Expect100Continue = false;
                wr.ProtocolVersion = HttpVersion.Version11;
                wr.Timeout = 600000;
                wr.ReadWriteTimeout = 600000;
                wr.KeepAlive = true;

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();


                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

                data.Add("data", wresp.Headers["Location"]);
                data.Add("status", 1);
                wresp.Close();
            }
            catch (Exception ex)
            {
                data.Add("data", ex.Message);
                data.Add("status", -1);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
            return data;
        }
        private Dictionary<string, object> PostDataAPI(HttpWebRequest wRequest, string data2Send)
        {
            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            CookieContainer cookies = new CookieContainer();
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);

                wRequest.Method = "POST";
                wRequest.UserAgent = BROWSER_FIREFOX;
                wRequest.ContentLength = postDataBytes.Length;
                wRequest.ServicePoint.Expect100Continue = false;
                wRequest.ProtocolVersion = HttpVersion.Version11;
                wRequest.Timeout = 600000;
                wRequest.ReadWriteTimeout = 600000;
                wRequest.KeepAlive = true;

                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }

                HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
                foreach (Cookie cookie in wResponse.Cookies)
                {
                    if (cookie.Name.Contains("x-xsrf-token") || cookie.Name.Contains("XSRF-TOKEN"))
                        currToken = cookie.Value;
                    cookies.Add(cookie);
                }

                String htmlString;
                using (var reader = new StreamReader(wResponse.GetResponseStream()))
                {
                    htmlString = reader.ReadToEnd();
                }
                wResponse.Close();
                dataReturn.Add("cookies", cookies);
                dataReturn.Add("data", htmlString);
                dataReturn.Add("status", 1);
                return dataReturn;
            }
            catch (Exception ex)
            {
                dataReturn.Add("cookies", cookies);
                dataReturn.Add("data", ex.Message);
                dataReturn.Add("status", -1);
                return dataReturn;
            }

        }
        private Dictionary<string, object> GetDataAPI(HttpWebRequest wRequest, string data2Send = "")
        {
            wRequest.Method = "GET";
            wRequest.UserAgent = BROWSER_FIREFOX;
            if (data2Send != "")
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);
                wRequest.ContentLength = postDataBytes.Length;
                wRequest.ServicePoint.Expect100Continue = false;
                wRequest.ProtocolVersion = HttpVersion.Version11;
                wRequest.Timeout = 600000;
                wRequest.ReadWriteTimeout = 600000;
                wRequest.KeepAlive = true;

                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }
            }

            HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
            CookieContainer cookies = new CookieContainer();
            foreach (Cookie cookie in wResponse.Cookies)
            {
                if (cookie.Name.Contains("x-xsrf-token") || cookie.Name.Contains("XSRF-TOKEN"))
                    currToken = cookie.Value;
                cookies.Add(cookie);
            }

            String htmlString;
            using (var reader = new StreamReader(wResponse.GetResponseStream()))
            {
                htmlString = reader.ReadToEnd();
            }
            wResponse.Close();
            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            dataReturn.Add("cookies", cookies);
            dataReturn.Add("data", htmlString);

            return dataReturn;
        }
        private void loadImage(string url)
        {
            pictureShowImage.ImageLocation = url;
        }
        private string getStringCategory(string text)
        {
            string kq = "";
            var rs = Resources.category;
            JArray jArray = JArray.Parse(rs);
            foreach (var item in jArray)
            {
                if (item["name"].ToString().ToLower().IndexOf(text.ToLower()) > -1 || item["fullName"].ToString().ToLower().IndexOf(text.ToLower()) > -1
                    || item["slug"].ToString().ToLower().IndexOf(text.ToLower()) > -1)
                {
                    kq += item["fullName"].ToString();
                }
                JArray jArray2 = JArray.Parse(item["children"].ToString());
                if (jArray2.Count > 0)
                {
                    foreach (var item2 in jArray2)
                    {
                        if (item2["slug"].ToString().ToLower().IndexOf(text.ToLower()) > -1)
                        {
                            foreach (var item3 in jArray)
                            {
                                if (item2["tagId"].ToString().ToLower().Equals(item3["_id"].ToString().ToLower()))
                                {
                                    kq += item3["fullName"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            var currKQ = kq.Replace("(", "|").Replace(")", "|");
            var x = currKQ.Split('|');
            string ressult = "";
            foreach (string item in x)
            {
                var crrText = item.Trim();
                if (crrText != "" && crrText != " " && ressult.IndexOf(crrText) == -1)
                    ressult += crrText.Trim() + ",";
            }
            return ressult;
        }
        private string getStringCategory(string text, string text2)
        {
            string kq = "";
            var rs = Resources.category;
            JArray jArray = JArray.Parse(rs);
            foreach (var item in jArray)
            {
                if (item["name"].ToString().ToLower().IndexOf(text.ToLower()) > -1
                    || item["fullName"].ToString().ToLower().IndexOf(text.ToLower()) > -1
                    || item["slug"].ToString().ToLower().IndexOf(text.ToLower()) > -1
                    || item["name"].ToString().ToLower().IndexOf(text2.ToLower()) > -1
                    || item["fullName"].ToString().ToLower().IndexOf(text2.ToLower()) > -1
                    || item["slug"].ToString().ToLower().IndexOf(text2.ToLower()) > -1)
                {
                    kq += item["fullName"].ToString();
                }
                JArray jArray2 = JArray.Parse(item["children"].ToString());
                if (jArray2.Count > 0)
                {
                    foreach (var item2 in jArray2)
                    {
                        if (item2["slug"].ToString().ToLower().IndexOf(text.ToLower()) > -1
                            || item2["slug"].ToString().ToLower().IndexOf(text2.ToLower()) > -1)
                        {
                            foreach (var item3 in jArray)
                            {
                                if (item2["tagId"].ToString().ToLower().Equals(item3["_id"].ToString().ToLower()))
                                {
                                    kq += item3["fullName"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            var currKQ = kq.Replace("(", "|").Replace(")", "|");
            var x = currKQ.Split('|');
            string ressult = "";
            int i = 0;
            foreach (string item in x)
            {
                if (i >= 200)
                    break;
                var crrText = item.Trim();
                if (crrText != "" && crrText != " " && ressult.IndexOf(crrText) == -1)
                    ressult += crrText.Trim() + ",";
                i++;
            }
            return ressult;
        }
        private List<Product> GetListBulk()
        {
            List<Product> lsProduct = new List<Product>();
            try
            {
                var rs = Resources.bulkCode;
                JArray jArray = JArray.Parse(rs);
                if (jArray.Count == 0)
                    return null;
                foreach (var item in jArray)
                {
                    Product p = new Product();
                    p._Id = item["_id"].ToString();
                    p.Name = item["name"].ToString();
                    p.Category = item["category"].ToString();
                    p.Code = item["code"].ToString();
                    p.Type = item["type"].ToString();
                    var colors = JArray.Parse(item["colors"].ToString());
                    p.Colors = new List<OColor>();
                    foreach (var color in colors)
                    {
                        OColor c = new OColor();
                        c.Name = color["name"].ToString();
                        c.Hex = color["hex"].ToString();
                        c.Image = color["image"].ToString();
                        p.Colors.Add(c);
                    }
                    lsProduct.Add(p);
                }
                return lsProduct;
            }
            catch
            {
                Product p = new Product();
                p._Id = "321as5df1as3df12";
                p.Name = "-----Select Product-----";
                lsProduct.Add(p);
                return lsProduct;
            }
        }
        private List<Category> GetListCategory()
        {
            List<Category> lsCategory = new List<Category>();
            try
            {
                var rs = Resources.category;
                JArray jArray = JArray.Parse(rs);
                foreach (var item in jArray)
                {
                    Category cate = new Category();
                    cate.Id = item["_id"].ToString();
                    cate.Name = item["fullName"].ToString();

                    lsCategory.Add(cate);
                }
                return lsCategory;
            }
            catch
            {
                Category cate = new Category();
                cate.Id = "321as5df1as3df12";
                cate.Name = "-----Select category-----";
                lsCategory.Add(cate);
                return lsCategory;
            }
        }
        private List<OTheme> GetListThemes()
        {
            List<OTheme> lsThemes = new List<OTheme>();
            try
            {
                //var rs = Resources.dataThemes;
                //JArray jArray = JArray.Parse(rs);
                //foreach (var item in jArray)
                //{
                //    OTheme theme = new OTheme();
                //    theme.Id = item["_id"].ToString();
                //    theme.Name = item["name"].ToString();

                //    lsThemes.Add(theme);
                //}
                lsThemes = ApplicationLibary.getThemeSaved();
                return lsThemes;
            }
            catch
            {
                OTheme theme = new OTheme();
                theme.Id = "321as5df1as3df12";
                theme.Name = "-----Select theme-----";
                lsThemes.Add(theme);
                return lsThemes;
            }
        }
        #endregion
        private List<Dictionary<string, object>> listCurrentThemes()
        {
            List<Dictionary<string, object>> lsData = new List<Dictionary<string, object>>();
            Dictionary<string, object> data;
            List<string> lsColor;

            foreach (UCItemProduct item in lsUserControlTheme)
            {
                Product currP = item.Product;
                data = new Dictionary<string, object>();
                lsColor = new List<string>();
                foreach (var color in currP.Colors)
                {
                    lsColor.Add(color.Name);

                }
                data.Add("printSize", currP.PrintSize);
                data.Add("productId", currP._Id);
                data.Add("colors", lsColor);
                data.Add("price", currP.Price);
                lsData.Add(data);
            }
            return lsData;
        }

        private void MoveFileUploaded(string fileName)
        {
            var path = Path.GetDirectoryName(fileName) + "\\Uploaded\\";
            var name = Path.GetFileName(fileName);
            var newDir = Path.Combine(path, name);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.Move(fileName, newDir);
        }
        /// <summary>
        /// Thêm 1 style vào theme và load vào panel
        /// </summary>
        /// <param name="frm"></param>
        private void addThemeToPanel(UCItemProduct frm)
        {
            frm.Location = new Point(10, (lsUserControlTheme.Count * frm.Height) + (lsUserControlTheme.Count * 10) + 10);
            frm.Width = xtraScrollableTheme.Width - 30;
            lsUserControlTheme.Add(frm);

            xtraScrollableTheme.Controls.Clear();
            foreach (var item in lsUserControlTheme)
            {
                xtraScrollableTheme.Controls.Add(item);
            }
        }
        private string GenUrl(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        private void ckUsingFileUpload_CheckedChanged(object sender, EventArgs e)
        {
            if (ckUsingFileUpload.Checked)
            {
                btnOpenFileExcel.Enabled = true;
                ApplicationLibary.writeLog(lsBoxLog, "Enable upload styles using file", 1);
            }
            else
            {
                btnOpenFileExcel.Enabled = false;
                ApplicationLibary.writeLog(lsBoxLog, "Disable upload styles using file", 1);
            }
        }

        private void info_cbbProduct_EditValueChanged(object sender, EventArgs e)
        {
            ckListBoxColor.Items.Clear();
            var obj = info_cbbProduct.GetSelectedDataRow();
            var data = ((Product)obj)._Id;
            var rs = Resources.bulkCode;
            JArray jArray = JArray.Parse(rs);
            foreach (var item in jArray)
            {
                if (data.Equals(item["_id"].ToString()))
                {
                    var colors = JArray.Parse(item["colors"].ToString());
                    foreach (var color in colors)
                    {
                        ckListBoxColor.Items.Add(color["name"]);
                    }
                }
            }
            btnViewData.PerformClick();
            ckCheckAll.Checked = false;
        }

        private void ckCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCheckAll.Checked)
                for (int i = 0; i < ckListBoxColor.Items.Count; i++)
                {
                    ckListBoxColor.SetItemChecked(i, true);

                }
            else
                for (int i = 0; i < ckListBoxColor.Items.Count; i++)
                {
                    ckListBoxColor.SetItemChecked(i, false);

                }
        }

        private void lueThemes_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                lsUserControlTheme.Clear();
                var obj = lueThemes.GetSelectedDataRow();
                var id = ((OTheme)obj).Id;
                List<Product> lsProduct = ApplicationLibary.loadThemeSaveByName(id);
                foreach (Product p in lsProduct)
                {
                    UCItemProduct uc = new UCItemProduct();
                    uc.Product = p;
                    addThemeToPanel(uc);
                }
            }
            catch { }
        }

        private void btnWriteLog_Click(object sender, EventArgs e)
        {
            try
            {
                string data = "========================LOG EVENT========================";
                foreach (var item in lsBoxLog.Items)
                {
                    data += "\r\n" + item.ToString();
                }
                if (data != "")
                {
                    string file = ApplicationLibary.writeDataToFileText(data);
                    Process.Start(file);
                }
            }
            catch
            {

            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
        }
    }
}
