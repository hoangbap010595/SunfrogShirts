using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Runtime.InteropServices;

namespace SpreadShirts
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        //https://www.spreadshirt.com/userarea/-C6840/User/ApiKeys/index
        //apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b
        //sig=da24c460f069ccc8a499fd96b395e8329cd9b670
        //time=1510655123467
        private string API_KEY = "1c711bf5-b82d-40de-bea6-435b5473cf9b";//912dac48-fb39-4aef-af8c-d1260755bcad
        private string SECRET = "fd9f23cc-2432-4a69-9dad-bbd57b7b9fdd";//29503fe1-800c-430c-98f0-b8f08e13123d
        private string USER_ID = "";
        private string USER_HREF = "";
        private string SESSION_ID = "";
        private string SESSION_HREF = "";
        private string IDENTITY_ID = "";
        private string IDENTITY_HREF = "";
        private CookieContainer cookieApplication = new CookieContainer();
        private string BROWSER_CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private string BROWSER_FIREFOX = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";

        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string urlPage = "https://www.spreadshirt.com";
                HttpWebRequest wRequestPage = (HttpWebRequest)WebRequest.Create(urlPage);
                wRequestPage.Method = "GET";
                wRequestPage.Host = "www.spreadshirt.com";
                wRequestPage.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                wRequestPage.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                wRequestPage.UseDefaultCredentials = true;
                wRequestPage.UserAgent = BROWSER_FIREFOX;
                wRequestPage.Referer = urlPage;
                wRequestPage.CookieContainer = cookieApplication;

                WebResponse wResponse = wRequestPage.GetResponse();
                cookieApplication = new CookieContainer();
                foreach (Cookie cookie in ((HttpWebResponse)wResponse).Cookies)
                {
                    cookieApplication.Add(cookie);
                }

                //Login Page
                string urlLogin = "https://www.spreadshirt.com/api/v1/sessions?mediaType=json";
                string data2Send = "{\"username\":\"hoangbap1595@gmail.com\", \"password\":\"Thienan@111\", \"rememberMe\":false}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);

                HttpWebRequest wRequestLogin = (HttpWebRequest)WebRequest.Create(urlLogin);
                wRequestLogin.CookieContainer = cookieApplication;
                wRequestLogin.UserAgent = BROWSER_FIREFOX;
                wRequestLogin.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                wRequestLogin.Method = "POST";
                wRequestLogin.Accept = "*/*";
                wRequestLogin.Host = "www.spreadshirt.com";
                wRequestLogin.ContentType = "application/x-www-form-urlencoded";
                wRequestLogin.ContentLength = postDataBytes.Length;
                wRequestLogin.Referer = "https://www.spreadshirt.com/login";

                using (Stream sr = wRequestLogin.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }
                WebResponse rspLogin = wRequestLogin.GetResponse();
                cookieApplication = new CookieContainer();
                foreach (Cookie cookie in ((HttpWebResponse)rspLogin).Cookies)
                {
                    cookieApplication.Add(cookie);
                }
                var locationHeader = rspLogin.Headers["location"];
                var cookieHeader = rspLogin.Headers["Set-Cookie"];
                //fixCookies(wRequestLogin, (HttpWebResponse)rspLogin);
                String htmlString;
                using (var reader = new StreamReader(rspLogin.GetResponseStream()))
                {
                    htmlString = reader.ReadToEnd();
                }
                var obj = JObject.Parse(htmlString);
                SESSION_ID = obj["id"].ToString();
                SESSION_HREF = obj["href"].ToString();
                IDENTITY_ID = obj["identity"]["id"].ToString();
                IDENTITY_HREF = obj["identity"]["href"].ToString();
                USER_ID = obj["user"]["id"].ToString();
                USER_HREF = obj["user"]["href"].ToString();
                //string role = obj["role"].ToString();

                /*
                 *--hoangbap1595 
                 * 73338c5b-2cbe-4333-8164-b23e30a6e0da
                 * https://www.spreadshirt.com/api/v1/sessions/73338c5b-2cbe-4333-8164-b23e30a6e0da
                 * NA-2832173b-7196-4e59-bf8d-3740fdb3c71a
                 * https://www.spreadshirt.com/api/v1/identities/NA-2832173b-7196-4e59-bf8d-3740fdb3c71a
                 * 302721328
                 * https://www.spreadshirt.com/api/v1/users/302721328
                 * 
                 * --lcoang1995
                 * 4f6e5652-e7a7-47b5-bf0a-d96c8cbaf458
                 * https://www.spreadshirt.com/api/v1/sessions/4f6e5652-e7a7-47b5-bf0a-d96c8cbaf458
                 * NA-776b20df-bfb4-4d47-a225-9d1252f9fd4c
                 * https://www.spreadshirt.com/api/v1/identities/NA-776b20df-bfb4-4d47-a225-9d1252f9fd4c
                 * 302719724
                 * https://www.spreadshirt.com/api/v1/users/302719724
                 * 
                /*-------------------LOGIN FINISH-------------------*/
                if (USER_ID != "")
                    MessageBox.Show(USER_ID, "UID");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private static void fixCookies(HttpWebRequest request, HttpWebResponse response)
        {
            for (int i = 0; i < response.Headers.Count; i++)
            {
                string name = response.Headers.GetKey(i);
                if (name != "Set-Cookie")
                    continue;
                string value = response.Headers.Get(i);
                foreach (var singleCookie in value.Split(','))
                {
                    Match match = Regex.Match(singleCookie, "(.+?)=(.+?);");
                    if (match.Captures.Count == 0)
                        continue;
                    response.Cookies.Add(
                        new Cookie(
                            match.Groups[1].ToString(),
                            match.Groups[2].ToString(),
                            "/",
                            request.Host.Split(':')[0]));
                }
            }
        }
        public string postXMLData(string destinationUrl, string requestXml)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.Host = "partner.spreadshirt.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
            request.Accept = "application/json, text/plain, */*";
            request.ContentType = "application/json;charset=utf-8";
            request.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            request.ContentLength = bytes.Length;
            request.Method = "PUT";
            request.CookieContainer = cookieApplication;
            request.Referer = "https://partner.spreadshirt.com/designs/" + destinationUrl.Split('/').Last();


            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }


        private string getSession()
        {
            string tURL = "https://ws.sessioncam.com/Record/config.aspx?url=https%3A%2F%2Fwww.spreadshirt.com%2F&ae=1&sse=1510927828405&urlnc=https://www.spreadshirt.com/login";
            HttpWebRequest wGETRequest = (HttpWebRequest)WebRequest.Create(tURL);
            wGETRequest.Method = "GET";
            wGETRequest.Host = "ws.sessioncam.com";
            wGETRequest.UserAgent = BROWSER_FIREFOX;
            wGETRequest.Accept = "*/*";
            wGETRequest.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wGETRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wGETRequest.Referer = "https://www.spreadshirt.com/login";

            WebResponse responseGET = wGETRequest.GetResponse();
            StreamReader responseGETReader = new StreamReader(responseGET.GetResponseStream());
            string rsGET = responseGETReader.ReadToEnd();
            int lenght = "//<![CDATA[try{sessionCamRecorder.initialise({ \"H\":\"\"".Length+1;
            int getLength = "w25nh4nz4bv0grlbu4nitfb5".Length;
            return rsGET.Substring(lenght, getLength);
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            cookieApplication = new CookieContainer();
            //getSession()
            cookieApplication.Add(new Cookie("sc.ASP.NET_SESSIONID", getSession()));
            cookieApplication.Add(new Cookie("sprd_auth_token", SESSION_ID) { Domain = ".spreadshirt.com" });
            //https://partner.spreadshirt.com/api/v1/users/302719724/design-uploads?apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b&locale=us_US&mediaType=json&sig=bad28978fba33b86bd7f52bc4027231c81515da8&time=1510932983372
            string tURL = "https://partner.spreadshirt.com/api/v1/users/" + USER_ID + "/design-uploads?apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b&locale=us_US&mediaType=json&sig=bad28978fba33b86bd7f52bc4027231c81515da8&time=1510932983372";
            string temp = encodeURL(tURL, "", "GET", "us_US", "json", "");
            HttpWebRequest wGETRequest = (HttpWebRequest)WebRequest.Create(temp);
            wGETRequest.Method = "GET";
            wGETRequest.Host = "partner.spreadshirt.com";
            wGETRequest.UserAgent = BROWSER_FIREFOX;
            wGETRequest.Accept = "application/json, text/plain, */*";
            wGETRequest.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wGETRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wGETRequest.Referer = "https://partner.spreadshirt.com/designs";
            wGETRequest.CookieContainer = cookieApplication;

            WebResponse responseGET = wGETRequest.GetResponse();
            StreamReader responseGETReader = new StreamReader(responseGET.GetResponseStream());
            string rsGET = responseGETReader.ReadToEnd();

            foreach (Cookie cookie in ((HttpWebResponse)responseGET).Cookies)
            {
                cookieApplication.Add(cookie);
            }
            HttpWebRequest wGETRequest2 = (HttpWebRequest)WebRequest.Create(temp);
            wGETRequest2.Method = "GET";
            wGETRequest2.Host = "partner.spreadshirt.com";
            wGETRequest2.UserAgent = BROWSER_FIREFOX;
            wGETRequest2.Accept = "application/json, text/plain, */*";
            wGETRequest2.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wGETRequest2.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wGETRequest2.Referer = "https://partner.spreadshirt.com/designs";
            wGETRequest2.CookieContainer = cookieApplication;

            WebResponse responseGET2 = wGETRequest2.GetResponse();
            StreamReader responseGETReader2 = new StreamReader(responseGET2.GetResponseStream());
            string rsGET2 = responseGETReader2.ReadToEnd();

            foreach (Cookie cookie in ((HttpWebResponse)responseGET2).Cookies)
            {
                cookieApplication.Add(cookie);
            }

            //cookieApplication.Add(new Cookie("RT", "dm=spreadshirt.com&si=" + SESSION_ID + "&ss=" + getTimeStamp() + "&sl=0&tt=0&obo=0&sh=&bcn=%2F%2F36fb61a9.akstat.io%2F&ld=" + getTimeStamp() + "&nu=https%3A%2F%2Fpartner.spreadshirt.com%2Fdesigns&cl=" + getTimeStamp() + "&r=https%3A%2F%2Fwww.spreadshirt.com%2Flogin&ul=" + getTimeStamp() + "&hd=" + getTimeStamp() + "") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("cdn_h_referer", "https://www.spreadshirt.com/login") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("direct_affiliate", "7405") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("affiliate", "7405") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("any_affiliate", "7405") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("StatusTime", getTimeStamp()) { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("designerStatus", "1") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("mpStatus", "1") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("mpStatusTime", "getTimeStamp()") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("sc_cmp0", "7405") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("s_cc", "true") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("s_vi", "[CS]v1|2D0776EA05035214-4000118920008DE7[CE]") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("s_sq", @"spreadshirt-com-en%3D%2526c.%2526a.%2526activitymap.%2526page%253DPartner%252520Area%252520-%252520My%252520Designs%2526link%253DUpload%252520more%252520designs%252520or%252520just%252520drag%252520it%252520here%252520By%252520clicking%252520on%252520upload%25252C%252520I%252520confirm%252520that%252520I%252520hold%252520the%252520copyrights%252520for%252520the%252520image%252520or%252520a%252520license%252520%2526region%253Dmain-content%2526pageIDType%253D1%2526.activitymap%2526.a%2526.c%2526pid%253DPartner%252520Area%252520-%252520My%252520Designs%2526pidt%253D1%2526oid%253Dhttps%25253A%25252F%25252Fpartner.spreadshirt.com%25252F%2526ot%253DA") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("sc_v0", "7405") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("s_pv", "Partner%20Area%20-%20My%20Designs") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("PPP", "Partner Area - My Designs") { Domain = ".partner.spreadshirt.com" });
            cookieApplication.Add(new Cookie("PP", "Partner Area - My Designs") { Domain = ".partner.spreadshirt.com" });
            cookieApplication.Add(new Cookie("pmpStatus", "1") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("pmpStatusTime", getTimeStamp()) { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("psStatus", "1") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("psStatusTime", getTimeStamp()) { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("optimizelyBuckets", "{\"8727782405\":\"8725993106\"}") { Domain = ".spreadshirt.com" });
            cookieApplication.Add(new Cookie("_bizo_np_stats", "155%3D351%2C") { Domain = ".partner.spreadshirt.com" });
            cookieApplication.Add(new Cookie("_bizo_cksm", "C042FDC2CB996B70") { Domain = ".partner.spreadshirt.com" });
            cookieApplication.Add(new Cookie("_bizo_bzid", "b6091060-42d4-48c2-890c-fe04d9b2403e") { Domain = ".partner.spreadshirt.com" });

            string url = "https://partner.spreadshirt.com/api/v1/users/" + USER_ID + "/design-uploads";
            //string url = USER_HREF + "/design-uploads";
            var urlUploadImage = encodeURL(url, "createProductIdea=true");
            string boundaryString = "---------------" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileUrl = @"S:\Project Source Code\Visual Studio\SunfrogShirts\trunk\SpreadShirts\upload\Inline.png";

            HttpWebRequest wRequestDesign = (HttpWebRequest)WebRequest.Create(urlUploadImage);
            wRequestDesign.Method = "POST";
            wRequestDesign.Host = "partner.spreadshirt.com";
            wRequestDesign.UserAgent = BROWSER_FIREFOX;
            wRequestDesign.Accept = "application/json, text/plain, */*";
            wRequestDesign.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wRequestDesign.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wRequestDesign.Referer = "https://partner.spreadshirt.com/designs";
            wRequestDesign.ContentType = "multipart/form-data;boundary=" + boundaryString;
            wRequestDesign.CookieContainer = cookieApplication;

            //Write File
            MemoryStream postDataStream = new MemoryStream();
            StreamWriter postDataWriter = new StreamWriter(postDataStream);
            postDataWriter.Write("\r\n" + boundaryString + "\r\n");
            postDataWriter.Write("Content-Disposition: form-data; name=\"filedata\"; filename=\"{0}\"", Path.GetFileName(fileUrl));

            //Reader File
            FileStream fileStream = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            postDataWriter.Write("\r\n" + boundaryString + "\r\n");
            postDataWriter.Flush();
            // Set the http request body content length
            wRequestDesign.ContentLength = postDataStream.Length;

            // Dump the post data from the memory stream to the request stream
            using (Stream s = wRequestDesign.GetRequestStream())
            {
                postDataStream.WriteTo(s);
            }
            postDataStream.Close();

            // Grab the response from the server. WebException will be thrown
            // when a HTTP OK status is not returned
            WebResponse response = wRequestDesign.GetResponse();
            StreamReader responseReader = new StreamReader(response.GetResponseStream());
            string replyFromServer = responseReader.ReadToEnd();
        }


        /// <summary>
        /// take any string and encrypt it using SHA1 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        private string GetSHA1HashData(string data)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string getTimeStamp()
        {
            double unixTimestamp = Math.Round((DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, 3);
            return unixTimestamp.ToString().Replace(".", "");
        }
        private string encodeURL(string url, string defaultParam = "", string method = "POST", string locale = "", string mediaType = "", string sessionId = "")
        {

            string t_url = url.Split('?')[0];
            string t_time = getTimeStamp();
            string t_data = method + t_url + " " + t_time + " " + SECRET;
            string t_sig = GetSHA1HashData(t_data);

            int index = t_url.IndexOf('?');
            var newUrl = t_url;
            if (index == -1)
                newUrl += "?";
            else
                newUrl += "&";
            newUrl += defaultParam == "" ? "" : defaultParam + "&";
            newUrl += "apiKey=" + API_KEY;
            if (locale != "")
                newUrl += "&locale=" + locale;//us_US
            if (mediaType != "")
                newUrl += "&mediaType=" + mediaType;//json
            newUrl += "&sig=" + t_sig + "&time=" + t_time;
            if (sessionId != "")
                newUrl += "&sessionId=" + sessionId;
            return newUrl;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            getSession();
        }
    }
}
