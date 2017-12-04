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

namespace TCProShirts
{
    public partial class frmMain : XtraForm
    {
        private static CookieContainer cookieApplication = new CookieContainer();
        private string BROWSER_CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private string BROWSER_FIREFOX = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:49.0) Gecko/20100101 Firefox/49.0";
        private ApplicationUser User;
        private string currToken = "";
        List<Product> lsBulkProduct;
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
                ApplicationLibary.writeLog(lsBoxLog, "Login Successfully", 1);
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            loadBulkProduct();
            //frmLogin frm = new frmLogin();
            //frm.senduser = new frmLogin.SendUser(getUser);
            //frm.ShowDialog();
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(User.ApiKey);
            //UploadProgress();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string url = "https://oo-prod.s3.amazonaws.com/public/blanks/unisex-crewneck-tshirt-v2-front/DBDBDB.png";
            loadImage(url);
        }

        private void GetProductFromUser()
        {
            var url = "https://api.scalablelicensing.com/rest/campaigns/search?entityId=" + User.EntityID + "&status=active&limit=10000";
        }
        private void UploadProgress()
        {
            var urlUploadImage = "https://scalable-licensing.s3.amazonaws.com/";
            //string fileUrl = @"C:\Users\Administrator\Desktop\Up TC pro\PNG\da upload\4 CL031017.png";
            string fileUrl = @"C:\Users\HoangLe\Desktop\Up TC pro\PNG\da upload\4 CL031017.png";
            #region ============== Upload Image & Get AtWork==================
            ApplicationLibary.writeLog(lsBoxLog, "Uploading: " + Path.GetFileName(fileUrl), 1);
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
            var data2SendUpload = "{\"name\":\"HoangTesst2202\",\"entityId\":\"" + User.EntityID + "\",\"tags\":{\"style\":[\"usa-colleges-p_to_t\",\"usa-colleges\",\"usa-colleges-u_to_z\",\"Goddaughter\",\"Children\",\"Family & Relationships\"]}}";
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
                ApplicationLibary.writeLog(lsBoxLog, rsUpload, 2);
                return;
            }
            var objUpload = JObject.Parse(rsUpload);
            var _IDDesign = objUpload["_id"].ToString();
            #endregion

            #region ===============Step 2: Create Design Line & Get DesignLine ID===============
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var data2SendLineID = "{\"designId\":\"" + _IDDesign + "\",\"entityId\":\"" + User.EntityID + "\",\"printSize\":\"general-standard\",\"id\":\""+ unixTimestamp + "-9779\",\"sides\":{\"front\":{\"artworkId\":\"" + atworkID + "\",\"position\":{\"vertical\":{\"origin\":\"T\",\"offset\":2},\"horizontal\":{\"origin\":\"C\",\"offset\":0}},\"size\":{\"width\":14,\"unit\":\"inch\"}}},\"handling\":\"default\"}";
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
                ApplicationLibary.writeLog(lsBoxLog, rsUploadLines, 2);
                return;
            }
            var objUploadLines = JObject.Parse(rsUploadLines);
            var _IDDesignLine = objUploadLines["_id"].ToString();
            #endregion

            //Step 3 -- Tham số cần truyền: 
            //      1. productId, color, price: người dùng chọn
            //      2. 
            var data2SendRetail = "{\"designLineId\":\"" + _IDDesignLine + "\",\"productId\":\"587d0d8ff43ea40e13382dad\",\"color\":\"Chocolate\",\"price\":2838,\"images\":[]}";
            HttpWebRequest wRetail = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/retail-products");
            wRetail.Accept = "application/json, text/plain, */*";
            wRetail.ContentType = "application/json";
            wRetail.PreAuthenticate = true;
            wRetail.Headers.Add("Authorization", User.Authorization);

            Dictionary<string, object> dataUploadRetail = PostDataAPI(wRetail, data2SendRetail);
            var rsUploadRetail = dataUploadRetail["data"].ToString();
            var statusRetail = int.Parse(dataUploadRetail["status"].ToString());
            if (statusRetail == -1)
            {
                ApplicationLibary.writeLog(lsBoxLog, rsUploadRetail, 2);
                return;
            }
            var objUploadRetail = JObject.Parse(rsUploadRetail);
            var _IDDesignRetail = objUploadRetail["_id"].ToString();

            //Step 4 -- Nhận giá trị 1 mảng _IDDesignRetail từ Step 3
            var data2SendCampaigns = "{\"url\":\"hoangtest3333\",\"title\":\"HoangTesst3333\",\"description\":\"<div> Mo ta haong mo ta</div>\",\"duration\":24,\"policies\":{\"forever\":true,\"fulfillment\":24,\"private\":false,\"checkout\":\"direct\"},\"social\":{\"trackingTags\":{}},\"entityId\":\"" + User.EntityID + "\",\"upsells\":[],\"tags\":{\"style\":[\"usa-colleges-p_to_t\",\"usa-colleges\",\"usa-colleges-u_to_z\",\"Goddaughter\",\"Children\",\"Family & Relationships\"]},\"related\":[{\"id\":\"" + _IDDesignRetail + "\",\"price\":2939,\"default\":true}]}";
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
                ApplicationLibary.writeLog(lsBoxLog, rsUploadCampaigns, 2);
                return;
            }
            var objUploadCampaigns = JObject.Parse(rsUploadCampaigns);
            var titleCampaigns = objUploadCampaigns["title"].ToString();
            var urlCampaigns = "https://pro.teechip.com" + objUploadCampaigns["url"].ToString();
            ApplicationLibary.writeLog(lsBoxLog, "Upload finish: " + titleCampaigns + ", Link:" + urlCampaigns, 1);
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
            var request = WebRequest.Create(url);

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                pictureShowImage.Invoke((MethodInvoker)delegate { pictureShowImage.Image = Bitmap.FromStream(stream); });
            }
        }
        private List<Product> GetListBulk()
        {
            var rs = Resources.bulkCode;
            List<Product> lsProduct = new List<Product>();
            JArray jArray = JArray.Parse(rs);
            int length = jArray.Count;
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
        private List<Dictionary<string,object>> getAllRetailIDFromDesignID(string designID)
        {
            List<Dictionary<string, object>> lsData = new List<Dictionary<string, object>>();


            return lsData;
        }
        #endregion

        private void test()
        {
            var listColor = new List<OColor>();
            listColor.Add(new OColor() { Name = "" });
            List <Product> data = new List<Product>();
            data.Add(new Product() { _Id = "", Colors = listColor });
        }
    }
}
