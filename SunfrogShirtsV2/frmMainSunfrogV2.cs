using DevExpress.XtraEditors;
using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;
using SunfrogShirtsV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunfrogShirtsV2
{
    public partial class frmMainSunfrogV2 : MaterialForm
    {
        private ApplicationUser User;
        private MaterialSkinManager mSkin;
        private CookieContainer cookieContainer;
        private List<OProduct> listProductDesign;
        private List<string> lsImageFileNames;
        private List<UCItemDesign> lsItemDesign;
        private List<Cookie> lsCookies;
        //Data
        private DataTable dtDataTemp;
        List<Dictionary<string, object>> listDataUpload;

        public frmMainSunfrogV2()
        {
            InitializeComponent();
        }

        private void getUser(ApplicationUser user, List<Cookie> cookies, List<OProduct> oProduct)
        {
            this.User = user;
            this.lsCookies = cookies;
            this.listProductDesign = oProduct;
            this.Invoke((MethodInvoker)delegate { this.Text = "[Your Seller ID]: " + user.Id; });

            lsItemDesign = new List<UCItemDesign>();

            foreach (OProduct p in listProductDesign)
            {
                UCItemDesign uc = new UCItemDesign(p);
                addThemeToPanel(uc);
            }
        }

        private void frmMainSunfrogV2_Load(object sender, EventArgs e)
        {
            mSkin = MaterialSkinManager.Instance;
            mSkin.AddFormToManage(this);
            //mSkin.Theme = MaterialSkinManager.Themes.DARK;
            OCategory category = new OCategory();
            cbbCategory.DataSource = category.getListCategoryProduct();
            cbbCategory.ValueMember = "Id";
            cbbCategory.DisplayMember = "Name";
            lsImageFileNames = new List<string>();
            lsItemDesign = new List<UCItemDesign>();
            listProductDesign = new List<OProduct>();
            //loadDesign();
            //Load Login
            frmLogin frm = new frmLogin();
            frm.senduser = new frmLogin.SendUser(getUser);
            frm.ShowDialog();

        }

        private void loadDesign()
        {
            HttpWebRequest rDesign = (HttpWebRequest)WebRequest.Create(ApplicationLibary.UrlDataDesign);
            rDesign.Host = "manager.sunfrog.com";
            rDesign.Referer = "https://manager.sunfrog.com/Designer/";

            Dictionary<string, object> dataDesign = GetDataAPI(rDesign);
            JArray objDesign = JArray.Parse(dataDesign["data"].ToString());
            listProductDesign = new List<OProduct>();
            for (int i = 0; i < objDesign.Count; i++)
            {

                OProduct p = new OProduct();
                p.Id = int.Parse(objDesign[i]["id"].ToString());
                p.Description = objDesign[i]["description"].ToString();
                p.BasePrice = double.Parse(objDesign[i]["basePrice"].ToString());
                p.BackPrintPrice = double.Parse(objDesign[i]["backPrintPrice"].ToString());
                p.ImageFront = objDesign[i]["imageFront"].ToString();
                p.ImageBack = objDesign[i]["imageBack"].ToString();
                p.Colors = new List<OColor>();
                JArray jColors = JArray.Parse(objDesign[i]["colors"].ToString());
                for (int j = 0; j < jColors.Count; j++)
                {
                    OColor c = new OColor();
                    c.Id = int.Parse(jColors[j]["id"].ToString());
                    c.Name = jColors[j]["name"].ToString();
                    c.Value = jColors[j]["value"].ToString();
                    c.Texture = jColors[j]["texture"].ToString();
                    p.Colors.Add(c);
                }
                listProductDesign.Add(p);
            }

        }
        private void addThemeToPanel(UCItemDesign frm)
        {
            frm.Location = new Point(10, (lsItemDesign.Count * frm.Height) + (lsItemDesign.Count * 10) + 10);
            frm.Width = xtraScrollableTheme.Width - 30;
            lsItemDesign.Add(frm);

            xtraScrollableTheme.Invoke((MethodInvoker)delegate { xtraScrollableTheme.Controls.Clear(); });
            foreach (var item in lsItemDesign)
            {
                xtraScrollableTheme.Invoke((MethodInvoker)delegate { xtraScrollableTheme.Controls.Add(item); });
            }
        }
        /// <summary>
        /// Upload file to Sunfrog
        /// </summary>
        /// <param name="dr">Nội dụng dữ liệu cần upload</param>
        private void UploadAndDownload()
        {
            foreach (string itemImage in lsImageFileNames)
            {
                string text_all = Path.GetFileName(itemImage).Split('.')[0];
                //Config Data
                //var frontbackImage = dr["FrontBack"].ToString().Trim();
                var pathImage = itemImage;
                var title = txtTitle.Text.Replace("$name", text_all);
                var category = 52; //cbbCategory.SelectedValue.ToString();
                var description = txtDescription.Text.Replace("$name", text_all);
                var keyword = ApplicationLibary.convertStringToJson(txtKeyword.Text);
                var collection = txtCollections.Text;
                var themes = getListTheme();
                var imgBase64 = ApplicationLibary.ConvertImageToBase64(pathImage);
                System.Drawing.Image img = System.Drawing.Image.FromFile(pathImage);

                if (title == "")
                    title = text_all;
                if (description == "")
                    title = text_all;

                var strBack = "";
                var strFront = "";
                if (ckSetBack.Checked)
                    strBack = "<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" xmlns:xlink=\\\"http://www.w3.org/1999/xlink\\\" id=\\\"SvgjsSvg1006\\\" version=\\\"1.1\\\" width=\\\"2400\\\" height=\\\"3200\\\" viewBox=\\\"311.00000000008 100 387.99999999984004 517.33333333312\\\"><g id=\\\"SvgjsG1052\\\" transform=\\\"scale(0.08399999999996445 0.08399999999996445) translate(3761.9047619073062 1569.8412698418072)\\\"><image id=\\\"SvgjsImage1053\\\" xlink:href=\\\"__dataURI:0__\\\" width=\\\"" + img.Width + "\\\" height=\\\"" + img.Height + "\\\"></image></g><defs id=\\\"SvgjsDefs1007\\\"></defs></svg>";
                if (ckSetFront.Checked)
                    strFront = "<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" xmlns:xlink=\\\"http://www.w3.org/1999/xlink\\\" id=\\\"SvgjsSvg1000\\\" version=\\\"1.1\\\" width=\\\"2400\\\" height=\\\"3200\\\" viewBox=\\\"311.00000000008 150 387.99999999984004 517.33333333312\\\"><g id=\\\"SvgjsG1052\\\" transform=\\\"scale(0.08399999999996445 0.08399999999996445) translate(3761.9047619073062 2165.0793650801543)\\\"><image id=\\\"SvgjsImage1053\\\" xlink:href=\\\"__dataURI:0__\\\" width=\\\"" + img.Width + "\\\" height=\\\"" + img.Height + "\\\"></image></g><defs id=\\\"SvgjsDefs1001\\\"></defs></svg>";

                var dataToSend = "";
                dataToSend = "{";
                dataToSend += "\"ArtOwnerID\": 0,\"IAgree\": true";
                dataToSend += ",\"Title\":\"" + title + "\"";
                dataToSend += ",\"Category\":\"" + category + "\"";
                dataToSend += ",\"Description\":\"" + description + "\"";
                dataToSend += ",\"Collections\":\"" + collection + "\"";
                dataToSend += ",\"Keywords\": [" + keyword + "]";
                dataToSend += ",\"imageFront\":\"" + strFront + "\"";
                dataToSend += ",\"imageBack\":\"" + strBack + "\"";
                dataToSend += ",\"types\": [" + themes + "]";
                dataToSend += ",\"images\":[{\"id\":\"__dataURI:0__\",\"uri\":\"data:image/png;base64," + imgBase64 + "\"}]";
                dataToSend += "}";

                string urlUpload = "https://manager.sunfrog.com/Designer/php/upload-handler.cfm";
                HttpWebRequest requestUpload = (HttpWebRequest)WebRequest.Create(urlUpload);
                requestUpload.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                requestUpload.Host = "manager.sunfrog.com";
                requestUpload.Headers.Add("X-Requested-With", "XMLHttpRequest");
                requestUpload.ContentType = "application/json";
                requestUpload.Referer = "https://manager.sunfrog.com/Designer/";
                //requestUpload.CookieContainer = cookieContainer;
                ApplicationLibary.TryAddCookie(requestUpload, lsCookies);

                Dictionary<string, object> dataUpload = PostDataAPI(requestUpload, dataToSend);
                if (int.Parse(dataUpload["status"].ToString()) == -1)
                {
                    ApplicationLibary.writeLogThread(lsBoxLog, "Step 2: " + dataUpload["data"].ToString(), 1);
                    continue;
                }
                string sourceCode = dataUpload["data"].ToString();
                var obj = JObject.Parse(sourceCode);
               // if (bool.Parse(obj["result"].ToString()) == true)
                //    ApplicationLibary.writeLogThread(lsBoxLog, "Collection:" + obj["collectionName"].ToString() + " - " + obj["description"].ToString(), 2);
                //else
                    ApplicationLibary.writeLogThread(lsBoxLog, "Uploaded " + obj["description"].ToString(), 1);
                //Chuyển hình sang folder Uploaded
                //moveImageUploaded(pathImage);
            }
        }

        private string getListTheme()
        {
            string types = "";
            foreach (UCItemDesign item in lsItemDesign)
            {
                string data = item.GetStyleSelected;
                if (data != "")
                {
                    types += data + ",";
                }
            }
            types = types.TrimEnd(',');
            return types;
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
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (lsBoxLog.Items.Count > 0)
            {
                lsBoxLog.Items.Clear();
                ApplicationLibary.writeLog(lsBoxLog, "Cleared", 1);
            }
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
                    listDataUpload = new List<Dictionary<string, object>>();
                    var x = Path.GetExtension(op.FileName);
                    if (x == ".csv")
                        dtDataTemp = ApplicationLibary.getDataExcelFromFileCSVToDataTable(op.FileName);
                    else
                        dtDataTemp = ApplicationLibary.getDataExcelFromFileToDataTable(op.FileName);
                    //loadDataToTable(dtDataTemp);
                    ApplicationLibary.writeLog(lsBoxLog, "Success " + dtDataTemp.Rows.Count + " record(s) is opened", 1);
                    // testSaveFile();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message);
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
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (ckUsingFileUpload.Checked) { }
            else
            {
                if (lsImageFileNames.Count == 0)
                {
                    XtraMessageBox.Show("Please choose image design!");
                    return;
                }
                if (txtKeyword.Text.Split(',').Length > 3 || txtKeyword.Text == "")
                {
                    XtraMessageBox.Show("Please enter ~3 keyword!");
                    return;
                }
                if (getListTheme() == "")
                {
                    XtraMessageBox.Show("Please choose theme upload !");
                    return;
                }
                Thread t = new Thread(new ThreadStart(() =>
                {
                    UploadAndDownload();
                }));
                t.Start();
            }
        }
        #region POST & GET & PUT
        private Dictionary<string, object> PostDataAPI(HttpWebRequest wRequest, string data2Send)
        {
            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            CookieContainer cookies = new CookieContainer();
            String htmlString;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);
                wRequest.Method = "POST";
                wRequest.UserAgent = ApplicationLibary.BROWSER_FIREFOX;
                wRequest.ContentLength = postDataBytes.Length;
                wRequest.Headers.Add("Origin", ApplicationLibary.Origin);//chrome-extension://aejoelaoggembcahagimdiliamlcdmfm
                wRequest.ServicePoint.Expect100Continue = false;
                wRequest.ProtocolVersion = HttpVersion.Version11;
                wRequest.Timeout = 90000;
                wRequest.ReadWriteTimeout = 90000;
                wRequest.KeepAlive = true;
                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }

                using (HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse())
                {
                    foreach (Cookie cookie in wResponse.Cookies)
                    {
                        cookies.Add(cookie);
                    }
                    using (var reader = new StreamReader(wResponse.GetResponseStream()))
                    {
                        htmlString = reader.ReadToEnd();
                    }
                    wResponse.Close();
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
        private Dictionary<string, object> PutDataAPI(HttpWebRequest wRequest, string data2Send)
        {
            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            CookieContainer cookies = new CookieContainer();
            String htmlString;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);
                wRequest.Method = "PUT";
                wRequest.UserAgent = ApplicationLibary.BROWSER_FIREFOX;
                wRequest.ContentLength = postDataBytes.Length;
                wRequest.Headers.Add("Origin", ApplicationLibary.Origin);//chrome-extension://aejoelaoggembcahagimdiliamlcdmfm
                wRequest.ServicePoint.Expect100Continue = false;
                wRequest.ProtocolVersion = HttpVersion.Version11;
                wRequest.Timeout = 90000;
                wRequest.ReadWriteTimeout = 90000;
                wRequest.KeepAlive = true;

                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }

                using (HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse())
                {
                    foreach (Cookie cookie in wResponse.Cookies)
                    {
                        cookies.Add(cookie);
                    }
                    using (var reader = new StreamReader(wResponse.GetResponseStream()))
                    {
                        htmlString = reader.ReadToEnd();
                    }
                    wResponse.Close();
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
            wRequest.UserAgent = ApplicationLibary.BROWSER_FIREFOX;
            wRequest.Headers.Add("Origin", ApplicationLibary.Origin);
            wRequest.ServicePoint.Expect100Continue = false;
            wRequest.ProtocolVersion = HttpVersion.Version11;
            wRequest.Timeout = 90000;
            wRequest.ReadWriteTimeout = 90000;
            wRequest.KeepAlive = true;

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

        #endregion


    }
}
