using System;
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

namespace SunfrogShirts
{
    public partial class frmMain : XtraForm
    {
        private ObjColor objColor = new ObjColor();
        private Category objCategory = new Category();
        private List<Category> lsCategory;
        private List<Category> lsCategoryProduct;
        private DataTable dtDataTemp = new DataTable();
        private int currentUpload = 0;
        private CookieContainer SessionCookieContainer;
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbbCategory.Properties.Items.Clear();
            info_cbbCategory.Properties.Items.Clear();
            ckListBox.Items.Clear();
            lsCategory = objCategory.getListCategory();
            lsCategoryProduct = objCategory.getListCategoryProduct();

            foreach (Category item in lsCategory)
            {
                cbbCategory.Properties.Items.Add(item.Name);
            }
            foreach (Category item in lsCategoryProduct)
            {
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            lsBoxLog.Items.Clear();
            CoreLibary.writeLog(lsBoxLog, "Event: CLEAR................................DONE");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dtDataTemp.Rows)
            {
                try
                {
                    info_txtTitle.Text = dr[2].ToString();
                    info_cbbCategory.SelectedItem = dr[3].ToString();
                    info_txtDescription.Text = dr[4].ToString();
                    info_txtKeyWord.Text = dr[5].ToString();
                    info_txtCollection.Text = dr[6].ToString();
                    var pathImage = dr[1].ToString();
                    if (!string.IsNullOrEmpty(pathImage))
                        info_picImageView.Image = Image.FromFile(pathImage);
                    var fbImage = dr[0].ToString();
                    if (fbImage.Contains("F"))
                        info_ckFront.Checked = true;
                    else
                        info_ckFront.Checked = false;
                    if (fbImage.Contains("B"))
                        info_ckBack.Checked = true;
                    else
                        info_ckBack.Checked = false;


                }
                catch { }
                Thread.Sleep(3000);
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var userName = sys_txtAccount.Text;
            var password = sys_txtPassword.Text;
            //data
            //username=lchoang1995%40gmail.com&password=Omega%40111&login=Login

            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create("https://manager.sunfrogshirts.com/");
            wRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
            wRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            wRequest.Referer = "https://manager.sunfrogshirts.com/";
            wRequest.Method = "POST";
            wRequest.ContentType = "application/x-www-form-urlencoded";
            wRequest.ContentLength = 65;
            wRequest.KeepAlive = true;
            wRequest.CookieContainer = new CookieContainer();

            ASCIIEncoding encoding = new ASCIIEncoding();
            var enUserName = HttpUtility.UrlEncode(userName);
            var enPassword = HttpUtility.UrlEncode(password);
            string data = "username="+ enUserName + "&password="+ enPassword + "&login=Login";
            byte[] postDataBytes = encoding.GetBytes(data);
            using (Stream sr = wRequest.GetRequestStream())
            {
                sr.Write(postDataBytes,0, postDataBytes.Length);
            }
            var cookieHeader = "";
            WebResponse resp = wRequest.GetResponse();
            cookieHeader = resp.Headers["Set-cookie"];

            SessionCookieContainer = new CookieContainer();
            foreach (Cookie cookie in ((HttpWebResponse)resp).Cookies)
            {
                SessionCookieContainer.Add(cookie);
            }
            var pageSource = "";
            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
            {
                pageSource = sr.ReadToEnd();
            }

            var dataToSend = "{\"ArtOwnerID\":0,\"IAgree\":true,\"Title\":\"hoangtest21\",\"Category\":\"78\",\"Description\":\"asdfasdf\",\"Collections\":\"ADV\",\"Keywords\":[\"adfasdf\",\"asdf\",\"asd\"],\"imageFront\":\"<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" id=\"SvgjsSvg1000\" version=\"1.1\" width=\"2400\" height=\"3200\" viewBox=\"311.00000000008 150 387.99999999984004 517.33333333312\"><text id=\"SvgjsText1052\" font-family=\"Source Sans Pro\" fill=\"#808080\" font-size=\"30\" stroke-width=\"0\" font-style=\"\" font-weight=\"\" text-decoration=\" \" text-anchor=\"start\" x=\"457.39119720458984\" y=\"241.71535301208496\"><tspan id=\"SvgjsTspan1056\" dy=\"39\" x=\"457.39119720458984\">adfasdf</tspan></text><defs id=\"SvgjsDefs1001\"></defs></svg>\",\"imageBack\":\"\"";
            dataToSend += ",\"types\":[{\"id\":8,\"name\":\"Guys Tee\",\"price\":19,\"colors\":[\"Green\"]},{\"id\":27,\"name\":\"Sweat Shirt\",\"price\":31,\"colors\":[\"Red\"]}],\"images\":[]}";
            byte[] postDataBytes2 = encoding.GetBytes(dataToSend);
            string getUrl = "https://manager.sunfrogshirts.com/Designer/php/upload-handler.cfm";

            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(getUrl);
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
            getRequest.Accept = "*/*";
            getRequest.Method = "POST";
            getRequest.ContentType = "application/json";
            getRequest.ContentLength = postDataBytes2.Length;
            getRequest.KeepAlive = true;
            getRequest.CookieContainer = SessionCookieContainer;       
            using (Stream sr = getRequest.GetRequestStream())
            {
                sr.Write(postDataBytes2, 0, postDataBytes2.Length);
            }

            var cookieHeader2 = "";
            WebResponse resp2 = getRequest.GetResponse();
            cookieHeader2 = resp2.Headers["Set-cookie"];
            HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
            using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
            {
                pageSource = sr.ReadToEnd();
            }
      

            //Response.Redirect(getUrl);
        }

        private  void Login()
        {
            var userName = sys_txtAccount.Text;
            var password = sys_txtPassword.Text;

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "username=" + userName + "&password=" + password + "&login=Login";
            byte[] postDataBytes = encoding.GetBytes(postData);

            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create("https://manager.sunfrogshirts.com/");
            wRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
            wRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            wRequest.Referer = "https://manager.sunfrogshirts.com/";
            wRequest.Method = "POST";
            wRequest.ContentType = "application/x-www-form-urlencoded";
            wRequest.ContentLength = postDataBytes.Length;
            wRequest.KeepAlive = true;

            using (var stream = wRequest.GetRequestStream())
            {
                stream.Write(postDataBytes, 0, postDataBytes.Length);
                stream.Close();
            }
            SessionCookieContainer = new CookieContainer();
            HttpWebResponse httpWebResponse;
            using (httpWebResponse = (HttpWebResponse)wRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {                 
                    foreach (Cookie cookie in httpWebResponse.Cookies)
                    {
                        SessionCookieContainer.Add(cookie);
                    }
                    while (httpWebResponse.StatusCode == HttpStatusCode.Found)
                    {
                        httpWebResponse.Close();
                        wRequest = GetNewRequest(httpWebResponse.Headers["Location"]);
                        httpWebResponse = (HttpWebResponse)wRequest.GetResponse();
                    }
                }
            }
            string page = "";
            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                while (httpWebResponse.StatusCode == HttpStatusCode.Found)
                {
                    page = streamReader.ReadToEnd();
                }       
            }
        }

        private HttpWebRequest GetNewRequest(string targetUrl)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
            request.CookieContainer = SessionCookieContainer;
            request.AllowAutoRedirect = false;
            return request;
        }
    }
}
