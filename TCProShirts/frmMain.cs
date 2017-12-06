using DevExpress.XtraEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
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
using TCProShirts.Properties;
using TCProShirts.UControls;

namespace TCProShirts
{
    public partial class frmMain : XtraForm
    {
        private static CookieContainer cookieApplication = new CookieContainer();
        //private string BROWSER_CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private string BROWSER_FIREFOX = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:49.0) Gecko/20100101 Firefox/49.0";
        private ApplicationUser User;
        private string currToken = "";
        private List<Product> lsBulkProduct;
        private List<Category> lsCategoryProduct;
        private DataTable dtDataTemp;
        private List<UserControl> lsUserControlTheme = new List<UserControl>();
        private List<string> lsImageFileNames;
        public frmMain()
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
            }else
            {
                this.Close();
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //var x = ApplicationLibary.convertStringToJson(getStringCategory("Fishing"));
            frmLogin frm = new frmLogin();
            frm.senduser = new frmLogin.SendUser(getUser);
            frm.ShowDialog();
            Thread t = new Thread(new ThreadStart(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    loadBulkProduct();
                    loadBulkCategory();
                });
            }));
            t.Start();
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
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var urlLogin = "https://pro.teechip.com/manager/auth/login";
            var data2Send = "{\"email\":\"lchoang1995@gmail.com\",\"password\":\"Thienan@111\"}";
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
            User.Code = obj["referralCode"].ToString();
            User.ApiKey = obj["apiKey"].ToString();
            User.ViewOnlyApiKey = obj["viewOnlyApiKey"].ToString();
            User.GroupID = obj["groupId"].ToString();
            User.EntityID = obj["entities"][0]["entityId"].ToString();
            User.PayableId = obj["payable"]["payableId"].ToString();
            User.Authorization = "Basic " + ApplicationLibary.Base64Encode(":" + User.ApiKey);
            User.UnAuthorization = "Basic " + ApplicationLibary.Base64Encode("undefined:" + User.ApiKey);
            ApplicationLibary.writeLog(lsBoxLog, "Login Successfully", 1);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            //btnLogin_Click(sender, e);
            btnStart.Enabled = false;
            btnViewData.Enabled = false;
            btnApplyTheme.Enabled = false;
            if (lsUserControlTheme.Count == 0)
            {
                XtraMessageBox.Show("No Style is not selected...!", "Message");
                return;
            }
            if (ckUsingFileUpload.Checked)
            {
                if (dtDataTemp == null || dtDataTemp.Rows.Count == 0)
                {
                    XtraMessageBox.Show("File data is not found!", "Message");
                    goto next;
                }

                Thread tStart = new Thread(new ThreadStart(() =>
                {
                    UploadFromFile();
                    btnStart.Invoke((MethodInvoker)delegate { btnStart.Enabled = true; });
                    btnViewData.Invoke((MethodInvoker)delegate { btnViewData.Enabled = true; });
                    btnApplyTheme.Invoke((MethodInvoker)delegate { btnApplyTheme.Enabled = true; });
                }));
                tStart.Start();
                next:
                btnStart.Enabled = true;
                btnViewData.Enabled = true;
                btnApplyTheme.Enabled = true;
            }
            else
            {
                if (txtTitle.Text == "")
                {
                    XtraMessageBox.Show("Title is not empty!", "Message");
                    goto next;
                }
                if (memoDescription.Text == "")
                {
                    XtraMessageBox.Show("Description is not empty!", "Message");
                    goto next;
                }
                Thread tStart = new Thread(new ThreadStart(() =>
                {
                    UploadProgress();
                    btnStart.Invoke((MethodInvoker)delegate { btnStart.Enabled = true; });
                    btnViewData.Invoke((MethodInvoker)delegate { btnViewData.Enabled = true; });
                    btnApplyTheme.Invoke((MethodInvoker)delegate { btnApplyTheme.Enabled = true; });
                }));
                tStart.Start();
                next:
                btnStart.Enabled = true;
                btnViewData.Enabled = true;
                btnApplyTheme.Enabled = true;
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
            p.Price = double.Parse(txtPrice.Text.Trim());
            p.Colors = new List<OColor>();

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
                op.Filter = "Excel .xlsx|*.xlsx|Excel .xls|*.xls";
                if (DialogResult.OK == op.ShowDialog())
                {
                    txtPath.Text = op.FileName;
                    dtDataTemp = new DataTable();
                    dtDataTemp = ApplicationLibary.getDataExcelFromFileToDataTable(op.FileName);
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
                btnViewData.Enabled = false;
                Thread t = new Thread(new ThreadStart(() =>
                {
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
                    btnViewData.Invoke((MethodInvoker)delegate { btnViewData.Enabled = true; });
                }));
                t.Start();
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
            if(lsUserControlTheme.Count == 0)
            {
                XtraMessageBox.Show("No Style is not selected...!","Message");
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
        private void UploadFromFile()
        {
            foreach (DataRow item in dtDataTemp.Rows)
            {
                try
                {
                    var uTitle = item["Title"];
                    var uDescription = item["Description"];
                    var uCategory = ApplicationLibary.convertStringToJson(getStringCategory(item["Category"].ToString()));
                    var uUrl = item["URL"].ToString().ToLower();
                    var uStore = item["Store"].ToString();
                    var uImage = item["Image"].ToString();
                    var urlUploadImage = "https://scalable-licensing.s3.amazonaws.com/";
                    //string fileUrl = @"C:\Users\Administrator\Desktop\Up TC pro\PNG\da upload\4 CL031017.png";
                    //string fileUrl = @"C:\Users\HoangLe\Desktop\Up TC pro\PNG\da upload\4 CL031017.png";
                    if (!File.Exists(uImage))
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, uImage + " - File do not exists in folder!", 2);
                        continue;
                    }
                    string fileUrl = uImage;
                    var imgDessign = Path.GetFileName(fileUrl);
                    #region ============== Upload Image & Get AtWork==================
                    ApplicationLibary.writeLogThread(lsBoxLog, "Uploading: " + imgDessign, 1);
                    string fileUpload = "uploads/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + DateTime.Now.Ticks.ToString("x") + ".png";
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("key", fileUpload);
                    nvc.Add("bucket", "scalable-licensing");
                    nvc.Add("AWSAccessKeyId", "AKIAJE4QLGLTY4DH4WRA");
                    nvc.Add("Policy", "eyJleHBpcmF0aW9uIjoiMzAwMC0wMS0wMVQwMDowMDowMFoiLCJjb25kaXRpb25zIjpbeyJidWNrZXQiOiJzY2FsYWJsZS1saWNlbnNpbmcifSxbInN0YXJ0cy13aXRoIiwiJGtleSIsInVwbG9hZHMvIl0seyJhY2wiOiJwdWJsaWMtcmVhZCJ9XX0=");
                    nvc.Add("Signature", "4yVrFVzCgzWg2BH8RkrI6LVi11Y=");
                    nvc.Add("acl", "public-read");
                    Dictionary<string, object> data = HttpUploadFile(urlUploadImage, fileUrl, "file", "image/png", nvc);

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
                        ApplicationLibary.writeLogThread(lsBoxLog, rsUpload, 2);
                        return;
                    }
                    var objUpload = JObject.Parse(rsUpload);
                    var _IDDesign = objUpload["_id"].ToString();
                    #endregion

                    #region ===============Step 2: Create Design Line & Get DesignLine ID===============
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var data2SendLineID = "{\"designId\":\"" + _IDDesign + "\",\"entityId\":\"" + User.EntityID + "\",\"printSize\":\"general-standard\",\"id\":\"" + unixTimestamp + "-9779\",\"sides\":{\"front\":{\"artworkId\":\"" + atworkID + "\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":2},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":14,\"unit\":\"inch\"}}},\"handling\":\"default\"}";
                    HttpWebRequest wLines = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/design-lines");
                    wLines.Accept = "application/json, text/plain, */*";
                    wLines.ContentType = "application/json";
                    wLines.PreAuthenticate = true;
                    wLines.Headers.Add("Authorization", User.Authorization);

                    Dictionary<string, object> dataUploadLines = PostDataAPI(wLines, data2SendLineID);
                    var rsUploadLines = dataUploadLines["data"].ToString();
                    var statusLines = int.Parse(dataUploadLines["status"].ToString());
                    if (statusLines == -1)
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, rsUploadLines, 2);
                        return;
                    }
                    var objUploadLines = JObject.Parse(rsUploadLines);
                    var _IDDesignLine = objUploadLines["_id"].ToString();
                    #endregion
                    //Step 3 -- Tham số cần truyền: 
                    //      1. productId, color, price: người dùng chọn
                    var objIDReail = getAllRetailIDFromDesignID(_IDDesignLine);

                    if (string.IsNullOrEmpty(uUrl))
                        uUrl = string.Format("{0}", imgDessign.Split('.')[0].Replace(" ", "").Trim());
                    else
                        uUrl = uUrl.Replace(" ", "").Trim();
                    //Step 4 -- Nhận giá trị 1 mảng _IDDesignRetail từ Step 3
                    var data2SendCampaigns = "{\"url\":\"" + uUrl + "\",\"title\":\"" + uTitle + "\",\"description\":\"<div>" + uDescription + "</div>\",\"duration\":24,\"policies\":{\"forever\":true,\"fulfillment\":24,\"private\":false,\"checkout\":\"direct\"},\"social\":{\"trackingTags\":{}},\"entityId\":\"" + User.EntityID + "\",\"upsells\":[],\"tags\":{\"style\":[" + uCategory + "]},\"related\": " + objIDReail + "}";
                    finishUploadImage(data2SendCampaigns, uImage);
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
        //1.Upload From FormData
        private void UploadProgress()
        {
            foreach (var fileImage in lsImageFileNames)
            {
                try
                {
                    var uTitle = txtTitle.Text.Trim();
                    var uDescription = memoDescription.Text;
                    var uCategory = ApplicationLibary.convertStringToJson(memoCategory.Text);
                    var uUrl = txtUrl.Text.ToLower();
                    var uStore = txtStore.Text;
                    if (string.IsNullOrEmpty(uUrl) || uUrl == "{0}")
                        uUrl = string.Format("{0}", fileImage.Split('.')[0].Replace(" ", "").Trim());
                    else
                        uUrl = uUrl.Replace(" ", "").Trim();

                    var urlUploadImage = "https://scalable-licensing.s3.amazonaws.com/";

                    var imgDessign = Path.GetFileName(fileImage);
                    #region ============== Upload Image & Get AtWork==================
                    ApplicationLibary.writeLogThread(lsBoxLog, "Uploading: " + imgDessign, 1);
                    string fileUpload = "uploads/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + DateTime.Now.Ticks.ToString("x") + ".png";
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("key", fileUpload);
                    nvc.Add("bucket", "scalable-licensing");
                    nvc.Add("AWSAccessKeyId", "AKIAJE4QLGLTY4DH4WRA");
                    nvc.Add("Policy", "eyJleHBpcmF0aW9uIjoiMzAwMC0wMS0wMVQwMDowMDowMFoiLCJjb25kaXRpb25zIjpbeyJidWNrZXQiOiJzY2FsYWJsZS1saWNlbnNpbmcifSxbInN0YXJ0cy13aXRoIiwiJGtleSIsInVwbG9hZHMvIl0seyJhY2wiOiJwdWJsaWMtcmVhZCJ9XX0=");
                    nvc.Add("Signature", "4yVrFVzCgzWg2BH8RkrI6LVi11Y=");
                    nvc.Add("acl", "public-read");
                    Dictionary<string, object> data = HttpUploadFile(urlUploadImage, fileImage, "file", "image/png", nvc);

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
                        ApplicationLibary.writeLogThread(lsBoxLog, rsUpload, 2);
                        return;
                    }
                    var objUpload = JObject.Parse(rsUpload);
                    var _IDDesign = objUpload["_id"].ToString();
                    #endregion

                    #region ===============Step 2: Create Design Line & Get DesignLine ID===============
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var data2SendLineID = "{\"designId\":\"" + _IDDesign + "\",\"entityId\":\"" + User.EntityID + "\",\"printSize\":\"general-standard\",\"id\":\"" + unixTimestamp + "-9779\",\"sides\":{\"front\":{\"artworkId\":\"" + atworkID + "\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":2},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":14,\"unit\":\"inch\"}}},\"handling\":\"default\"}";
                    HttpWebRequest wLines = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/design-lines");
                    wLines.Accept = "application/json, text/plain, */*";
                    wLines.ContentType = "application/json";
                    wLines.PreAuthenticate = true;
                    wLines.Headers.Add("Authorization", User.Authorization);

                    Dictionary<string, object> dataUploadLines = PostDataAPI(wLines, data2SendLineID);
                    var rsUploadLines = dataUploadLines["data"].ToString();
                    var statusLines = int.Parse(dataUploadLines["status"].ToString());
                    if (statusLines == -1)
                    {
                        ApplicationLibary.writeLogThread(lsBoxLog, rsUploadLines, 2);
                        return;
                    }
                    var objUploadLines = JObject.Parse(rsUploadLines);
                    var _IDDesignLine = objUploadLines["_id"].ToString();
                    #endregion

                    //Step 3 -- Tham số cần truyền: 
                    //      1. productId, color, price: người dùng chọn
                    var objIDReail = getAllRetailIDFromDesignID(_IDDesignLine);

                    //Step 4 -- Nhận giá trị 1 mảng _IDDesignRetail từ Step 3
                    var data2SendCampaigns = "{\"url\":\"" + uUrl + "\",\"title\":\"" + uTitle + "\",\"description\":\"<div>" + uDescription + "</div>\",\"duration\":24,\"policies\":{\"forever\":true,\"fulfillment\":24,\"private\":false,\"checkout\":\"direct\"},\"social\":{\"trackingTags\":{}},\"entityId\":\"" + User.EntityID + "\",\"upsells\":[],\"tags\":{\"style\":[" + uCategory + "]},\"related\": " + objIDReail + "}";
                    finishUploadImage(data2SendCampaigns, fileImage);
                }
                catch (Exception ex)
                {
                    ApplicationLibary.writeLogThread(lsBoxLog, ex.Message, 2);
                }
            }
        }
        //Step 3: Get Retail ID
        private string getAllRetailIDFromDesignID(string designID)
        {
            List<Dictionary<string, object>> lsData = new List<Dictionary<string, object>>();
            Dictionary<string, object> data;
            List<string> lsCommand = new List<string>();
            //b1
            List<Dictionary<string, object>> themes = listCurrentThemes();
            foreach (Dictionary<string, object> item in themes)
            {
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
                        ApplicationLibary.writeLogThread(lsBoxLog, rsUploadRetail, 2);
                    }
                    else
                    {
                        var objUploadRetail = JObject.Parse(rsUploadRetail);
                        data.Add("id", objUploadRetail["_id"].ToString());
                        data.Add("price", double.Parse(objUploadRetail["price"].ToString()));
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
        private void finishUploadImage(string data2SendCampaigns,string uImage)
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
                ApplicationLibary.writeLogThread(lsBoxLog, rsUploadCampaigns, 2);
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
                if (item["name"].ToString().Contains(text) || item["fullName"].ToString().Contains(text)
                    || item["slug"].ToString().Contains(text))
                {
                    kq += item["fullName"];
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
    }
}
