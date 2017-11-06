﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SunfrogShirts
{
    public partial class frmMain : XtraForm
    {
        private ObjColor objColor = new ObjColor();
        private Category objCategory = new Category();
        private List<Category> lsCategory;
        private List<Category> lsCategoryProduct;
        private DataTable dtDataTemp = new DataTable();
        private CookieContainer cookieContainer;
        private System.Windows.Forms.Timer timeOnline = new System.Windows.Forms.Timer();
        private int timeRight = 0;
        private string currentImageUploadOne = "";
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lsCategory = objCategory.getListCategory();
            lsCategoryProduct = objCategory.getListCategoryProduct();
            ckListBox.Items.Clear();
            lsBoxLog.Items.Clear();

            info_cbbCategory.Properties.DataSource = lsCategoryProduct;
            info_cbbCategory.Properties.DisplayMember = "Name";
            info_cbbCategory.Properties.ValueMember = "Id";

            cbbCategory.Properties.DataSource = lsCategory;
            cbbCategory.Properties.DisplayMember = "Name";
            cbbCategory.Properties.ValueMember = "Id";

            cbbCategory.ItemIndex = 0;
            info_cbbCategory.ItemIndex = 0;
            cookieContainer = new CookieContainer();
            timeOnline.Interval = 1000;
            timeOnline.Enabled = false;
            timeOnline.Tick += TimeOnline_Tick;
        }

        private void TimeOnline_Tick(object sender, EventArgs e)
        {
            timeRight++;
            int hour = timeRight / 60;
            int min = timeRight % 60;
            string shour = hour < 10 ? "0" + hour : hour.ToString();
            string smin = min < 10 ? "0" + min : min.ToString();
            lblTimeOnline.Text = shour + ":" + smin;
        }

        /// <summary>
        /// Chọn màu cho themes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbCategory_EditValueChanged(object sender, EventArgs e)
        {
            var obj = cbbCategory.GetSelectedDataRow();
            switch (((SunfrogShirts.Category)obj).Id)
            {
                case 8:
                case 34:
                    List<ObjColor> lsColor = objColor.getObjColorGuysTeeAndLadiesTee();
                    addListColor(lsColor);
                    break;
                case 35:
                    List<ObjColor> lsColor1 = objColor.getObjColorYouthTee();
                    addListColor(lsColor1);
                    break;
                case 19:
                    List<ObjColor> lsColor2 = objColor.getObjColorHoodie();
                    addListColor(lsColor2);
                    break;
                case 27:
                    List<ObjColor> lsColor3 = objColor.getObjColorSweatSirt();
                    addListColor(lsColor3);
                    break;
                case 50:
                    List<ObjColor> lsColor4 = objColor.getObjColorGuysVNeck();
                    addListColor(lsColor4);
                    break;
                case 116:
                    List<ObjColor> lsColor5 = objColor.getObjColorLadiesVNeck();
                    addListColor(lsColor5);
                    break;
                case 118:
                case 119:
                    List<ObjColor> lsColor7 = objColor.getObjColorUnisex();
                    addListColor(lsColor7);
                    break;
                case 120:
                case 128:
                    List<ObjColor> lsColor8 = objColor.getObjColorBlack();
                    addListColor(lsColor8);
                    break;
                case 129:
                case 145:
                case 137:
                case 138:
                case 139:
                case 140:
                case 143:
                    List<ObjColor> lsColor9 = objColor.getObjColorWhite();
                    addListColor(lsColor9);
                    break;
                case 147:
                    List<ObjColor> lsColor10 = objColor.getObjColorHat();
                    addListColor(lsColor10);
                    break;
                case 148:
                    List<ObjColor> lsColor11 = objColor.getObjColorTruckerCap();
                    addListColor(lsColor11);
                    break;
            }
        }

        /// <summary>
        /// Add màu tương ứng với themes đã chọn
        /// </summary>
        /// <param name="lsColor">List color tương ứng</param>
        private void addListColor(List<ObjColor> lsColor)
        {
            ckListBox.Items.Clear();
            foreach (ObjColor item in lsColor)
            {
                ckListBox.Items.Add(item.Name);
            }
        }

        /// <summary>
        /// Add new themes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    dtDataTemp = CoreLibary.getDataExcelFromFileToDataTable(op.FileName);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Clear log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            lsBoxLog.Items.Clear();
            CoreLibary.writeLog(lsBoxLog, "CLEAR", 1);
        }



        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmWait frm = new frmWait();
            frm.SetCaption("Đăng nhập hệ thống!");
            frm.SetDescription("Vui lòng chờ...");
            Thread t = new Thread(new ThreadStart(() =>
            {
                var userName = sys_txtAccount.Text;
                var password = sys_txtPassword.Text;
                //data
                //username=lchoang1995%40gmail.com&password=Omega%40111&login=Login
                ASCIIEncoding encoding = new ASCIIEncoding();
                var enUserName = HttpUtility.UrlEncode(userName);
                var enPassword = HttpUtility.UrlEncode(password);
                string data = "username=" + enUserName + "&password=" + enPassword + "&login=Login";
                byte[] postDataBytes = encoding.GetBytes(data);

                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create("https://manager.sunfrogshirts.com/");
                wRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
                wRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                wRequest.Referer = "https://manager.sunfrogshirts.com/";
                wRequest.Method = "POST";
                wRequest.ContentType = "application/x-www-form-urlencoded";
                wRequest.ContentLength = postDataBytes.Length;
                wRequest.KeepAlive = true;
                wRequest.CookieContainer = cookieContainer;

                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }
                var cookieHeader = "";
                WebResponse resp = wRequest.GetResponse();
                cookieHeader = resp.Headers["Set-cookie"];

                foreach (Cookie cookie in ((HttpWebResponse)resp).Cookies)
                {
                    cookieContainer.Add(cookie);
                }

                var pageSource = "";
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    pageSource = sr.ReadToEnd();
                }
                //<strong style="font-size:1.5em; line-height:15px; padding-bottom:0px;" class="clearfix">(.*?)</strong>
                var regex = "<strong style=\"font-size:1.5em; line-height:15px; padding-bottom:0px;\" class=\"clearfix\">(?<myId>.*?)</strong>";
                MatchCollection matchCollection = Regex.Matches(pageSource, regex);

                if (matchCollection.Count == 0)
                {
                    XtraMessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Thông báo");
                    frm.Invoke((MethodInvoker)delegate { frm.Close(); });
                    lblTimeOnline.Invoke((MethodInvoker)delegate { lblTimeOnline.Visible = false; });
                    btnLogin.Invoke((MethodInvoker)delegate { btnLogin.Visible = true; });
                    lblPassword.Invoke((MethodInvoker)delegate { lblPassword.Visible = true; });
                    CoreLibary.writeLogThread(lsBoxLog, "User Login", 2);
                    return;
                }
                foreach (Match match in matchCollection)
                {
                    this.Invoke((MethodInvoker)delegate { this.Text += " - [Your Id: " + match.Groups["myId"].Value + "]"; });
                }
                frm.Invoke((MethodInvoker)delegate { frm.Close(); });
                lblTimeOnline.Invoke((MethodInvoker)delegate { lblTimeOnline.Visible = true; });
                btnLogin.Invoke((MethodInvoker)delegate { btnLogin.Visible = false; });
                lblPassword.Invoke((MethodInvoker)delegate { lblPassword.Visible = false; });
                CoreLibary.writeLogThread(lsBoxLog, "User Login", 1);
            }));
            t.Start();
            frm.ShowDialog();
            timeOnline.Enabled = true;
        }

        private void UploadAndDownload(DataRow dr)
        {
            //Config Data
            var frontbackImage = dr[0].ToString().Trim();
            var pathImage = dr[1].ToString().Trim();
            var title = dr[2].ToString().Trim();
            var category = dr[3].ToString().Trim();
            var description = dr[4].ToString().Trim();
            var keyword = dr[5].ToString().Trim();
            var collection = dr[6].ToString().Trim();
            var themes = dr[7].ToString().Trim();
            var imgBase64 = CoreLibary.ConvertImageToBase64(pathImage);

            themes = "{\"id\":8,\"name\":\"Guys Tee\",\"price\":19,\"colors\":[\"Orange\",\"Yellow\"]}";
            themes += ",{\"id\":19,\"name\":\"Hoodie\",\"price\":34,\"colors\":[\"White\",\"Green\"]}";
            //2. Upload Image
            //var strFrontText = "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' id='SvgjsSvg1000' version='1.1' width='2400' height='3200' viewBox='311.00000000008 150 387.99999999984004 517.33333333312'><text id='SvgjsText1052' font-family='Source Sans Pro' fill='#808080' font-size='30' stroke-width='0' font-style='' font-weight='' text-decoration=' ' text-anchor='start' x='457.39119720458984' y='241.71535301208496'><tspan id='SvgjsTspan1056' dy='39' x='457.39119720458984'>adfasdf</tspan></text><defs id='SvgjsDefs1001'></defs></svg>";
            //Back:        <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' id='SvgjsSvg1006' version='1.1' width='2400' height='3200' viewBox='311.00000000008 100 387.99999999984004 517.33333333312'><g id='SvgjsG1052' transform='scale(0.08399999999996445 0.08399999999996445) translate(3761.9047619073062 1569.8412698418072)'><image id='SvgjsImage1053' xlink:href='__dataURI:0__' width='4500' height='5400'></image></g><defs id='SvgjsDefs1007'></defs></svg>
            var strBack = "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' id='SvgjsSvg1006' version='1.1' width='2400' height='3200' viewBox='311.00000000008 100 387.99999999984004 517.33333333312'><g id='SvgjsG1052' transform='scale(0.08399999999996445 0.08399999999996445) translate(3761.9047619073062 1569.8412698418072)'><image id='SvgjsImage1053' xlink:href='__dataURI:0__' width='4500' height='5400'></image></g><defs id='SvgjsDefs1007'></defs></svg>";
            var strFront = "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' id='SvgjsSvg1000' version='1.1' width='2400' height='3200' viewBox='311.00000000008 150 387.99999999984004 517.33333333312'><g id='SvgjsG1052' transform='scale(0.08399999999996445 0.08399999999996445) translate(3761.9047619073062 2165.0793650801543)'><image id='SvgjsImage1053' xlink:href='__dataURI:0__' width='4500' height='5400'></image></g><defs id='SvgjsDefs1001'></defs></svg>";
            if (frontbackImage == "F")
                strBack = "";
            else
                strFront = "";

            var dataToSend = "";
            dataToSend += "{";
            dataToSend += " \"ArtOwnerID\":0";
            dataToSend += " ,\"IAgree\":true";
            dataToSend += " ,\"Title\":\"" + title + "\"";
            dataToSend += " ,\"Category\":\"" + category + "\"";
            dataToSend += " ,\"Description\":\"" + description + "\"";
            dataToSend += " ,\"Collections\":\"" + collection + "\"";
            dataToSend += " ,\"Keywords\":[" + keyword + "]";
            dataToSend += " ,\"imageFront\":\"" + strFront + "\"";
            dataToSend += " ,\"imageBack\":\"" + strBack + "\"";
            dataToSend += " ,\"types\":[";
            dataToSend += "     " + themes + "";
            dataToSend += " ]";
            //dataToSend += " ,\"images\":[]";
            dataToSend += " ,\"images\":[{\"id\":\"__dataURI: 0__\",\"uri\":\"data:image/png;base64," + imgBase64 + "\"}]";
            dataToSend += "}";

            byte[] postDataBytes2 = Encoding.UTF8.GetBytes(dataToSend);
            string getUrl = "https://manager.sunfrogshirts.com/Designer/php/upload-handler.cfm";

            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(getUrl);
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
            getRequest.Accept = "*/*";
            getRequest.AllowWriteStreamBuffering = true;
            getRequest.AllowAutoRedirect = true;
            getRequest.Method = "POST";
            getRequest.ContentType = "application/json; charset=UTF-8";
            getRequest.ContentLength = postDataBytes2.Length;
            getRequest.KeepAlive = true;
            getRequest.CookieContainer = cookieContainer;

            using (Stream sr = getRequest.GetRequestStream())
            {
                sr.Write(postDataBytes2, 0, postDataBytes2.Length);
            }

            string sourceCode = "";
            HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
            using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
            {
                sourceCode = sr.ReadToEnd();
            }
            var dirSave = Directory.GetCurrentDirectory() + "\\Uploaded\\";
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }
            var obj = JObject.Parse(sourceCode);
            for (int i = 0; i < obj["products"].Count(); i++)
            {
                string url = "";
                string name = "";
                if (frontbackImage == "F")
                {
                    name = obj["products"][i]["imageFront"].ToString().Split('/')[6];
                    url = "http:" + obj["products"][i]["imageFront"].ToString();
                }
                else
                {
                    name = obj["products"][i]["imageBack"].ToString().Split('/')[6];
                    url = "http:" + obj["products"][i]["imageBack"].ToString();
                }
                string fileName = dirSave + name;
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, fileName);
            }
            if (int.Parse(obj["result"].ToString()) == 0)
            {
                CoreLibary.writeLogThread(lsBoxLog, "Collection:" + obj["collectionName"].ToString() + " - " + obj["description"].ToString(), 2);
            }
        }

        /// <summary>
        /// Upload 1 hình 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            CoreLibary.writeLog(lsBoxLog, "Uploading One Collection", 3);
            Thread t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    dtDataTemp = new DataTable();
                    dtDataTemp.Columns.Add("FronBack");
                    dtDataTemp.Columns.Add("Image");
                    dtDataTemp.Columns.Add("Title");
                    dtDataTemp.Columns.Add("Category");
                    dtDataTemp.Columns.Add("Description");
                    dtDataTemp.Columns.Add("Keyword");
                    dtDataTemp.Columns.Add("Collection");
                    dtDataTemp.Columns.Add("Themes");
                    dtDataTemp.Columns.Add("Status");
                    DataRow dr = dtDataTemp.NewRow();
                    //Load Data
                    if (info_ckFront.Checked)
                        dr[0] = "F";
                    else
                        dr[0] = "B";
                    //if (info_ckBack.Checked)
                    //    dr[0] = "B";
                    //else
                    //    dr[0] = "F";

                    dr[1] = currentImageUploadOne;
                    dr[2] = info_txtTitle.Text.Trim();
                    var obj = info_cbbCategory.GetSelectedDataRow();
                    var category = ((SunfrogShirts.Category)obj).Id;
                    dr[3] = category;
                    dr[4] = info_txtDescription.Text.Trim();
                    dr[5] = CoreLibary.convertStringToJson(info_txtKeyWord.Text.Trim());
                    dr[6] = info_txtCollection.Text.Trim();
                    dtDataTemp.Rows.Add(dr);
                    foreach (DataRow drItem in dtDataTemp.Rows)
                    {
                        UploadAndDownload(drItem);
                        CoreLibary.writeLogThread(lsBoxLog, "Upload Item " + drItem[2].ToString(), 1);
                    }
                }
                catch (Exception ex)
                {
                    CoreLibary.writeLogThread(lsBoxLog, "Upload Image..Sunfrog", 2);
                    CoreLibary.writeLogThread(lsBoxLog, ex.Message, 2);
                    XtraMessageBox.Show("Upload không thành công.\n" + ex.Message, "Thông báo");
                }
                btnUpdate.Invoke((MethodInvoker)delegate { btnUpdate.Enabled = true; });
            }));
            t.Start();
        }
        /// <summary>
        /// Upload hình lấy thông tin từ file Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            //Check data
            if (dtDataTemp.Rows.Count == 0)
            {
                XtraMessageBox.Show("Không có dữ liệu thực hiện!");
                return;
            }
            Thread t = new Thread(new ThreadStart(() =>
            {
                foreach (DataRow dr in dtDataTemp.Rows)
                {
                    try
                    {

                    }
                    catch { }
                }
            }));
            t.Start();
        }

        private void info_picImageView_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "PNG .png|*.png|JPG .jpg|*.jpg";
                if (DialogResult.OK == op.ShowDialog())
                {
                    currentImageUploadOne = op.FileName;
                    info_picImageView.Image = Image.FromFile(currentImageUploadOne);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
