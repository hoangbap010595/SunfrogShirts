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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace TCProShirts
{
    public partial class Form1 : Form
    {
        private static CookieContainer cookieApplication = new CookieContainer();
        private string BROWSER_CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private string BROWSER_FIREFOX = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:49.0) Gecko/20100101 Firefox/49.0";
        private ApplicationUser User;
        private string currToken = "";
        public Form1()
        {
            InitializeComponent();
            User = new ApplicationUser();
        }

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
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            UploadProgress();
        }

        private void UploadProgress()
        {
            var urlUploadImage = "https://scalable-licensing.s3.amazonaws.com/";
            string fileUrl = @"C:\Users\Administrator\Desktop\Up TC pro\PNG\da upload\4 CL031017.png";

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

            HttpWebRequest wUpload = (HttpWebRequest)WebRequest.Create("https://api.scalablelicensing.com/rest/campaigns/cost");
            wUpload.Host = "api.scalablelicensing.com";
            wUpload.Accept = "application/json, text/plain, */*";
            //wUpload.Headers.Add("x-xsrf-token", currToken);
            wUpload.ContentType = "application/json";
            wUpload.Headers.Add("Origin", "https://pro.teechip.com");
            wUpload.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wUpload.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wUpload.CookieContainer = cookieApplication;
            var data2SendUpload = strDataUpload();
            Dictionary<string, object> dataUpload = PostDataAPI(wUpload, data2Send);
        }

        private string strDataUpload()
        {
            return ""
                + "{\"items\":["
                + "   {\"productId\":\"587d0d8ff43ea40e13382db3\",\"designInfo\":{\"sides\":{\"front\":1}}}"
                + "   ,{\"productId\":\"587d0d90f43ea40e13382dc3\",\"designInfo\":{\"sides\":{\"front\":1}}}"
                + "   ,{\"productId\":\"587d0d90f43ea40e13382dbc\",\"designInfo\":{\"sides\":{\"front\":1}}}"
                + "   ,{\"productId\":\"587d0d90f43ea40e13382dc2\",\"designInfo\":{\"sides\":{\"front\":1}}}"
                + "   ,{\"productId\":\"587d0d90f43ea40e13382dc5\",\"designInfo\":{\"sides\":{\"front\":1}}}"
                + " ]"
                + ",\"entityId\":\""+ User.EntityID + "\"}";
        }
        public static Dictionary<string, object> HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            Console.WriteLine(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
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

            WebResponse wresp = null;
            try
            {
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

        public static string ConvertImageToBase64(string path)
        {
            string base64String = string.Empty;
            if (!File.Exists(path))
                return base64String;
            // Convert Image to Base64
            using (Image img = Image.FromFile(path)) // Image Path from File Upload Controller
            {
                using (var memStream = new MemoryStream())
                {
                    img.Save(memStream, img.RawFormat);
                    byte[] imageBytes = memStream.ToArray();
                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                }
                img.Dispose();
            }
            return base64String;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
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

        private Dictionary<string, object> OptionDataAPI(HttpWebRequest wRequest, string method = "OPTIONS")
        {
            wRequest.Method = method;

            HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
            CookieContainer cookies = new CookieContainer();
            List<Cookie> lsCookie = new List<Cookie>();
            foreach (Cookie cookie in wResponse.Cookies)
            {
                lsCookie.Add(new Cookie(cookie.Name, cookie.Value));
                cookies.Add(cookie);
            }

            String htmlString;
            using (var reader = new StreamReader(wResponse.GetResponseStream()))
            {
                htmlString = reader.ReadToEnd();
            }

            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            dataReturn.Add("cookies", cookies);
            dataReturn.Add("lsCookie", lsCookie);
            dataReturn.Add("data", htmlString);
            dataReturn.Add("x-amz-id-2", wResponse.Headers["x-amz-id-2"]);
            dataReturn.Add("x-amz-request-id", wResponse.Headers["x-amz-request-id"]);

            return dataReturn;
        }



    }
}
