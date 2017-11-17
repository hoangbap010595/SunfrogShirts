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
        private CookieContainer cookieJar = new CookieContainer();
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string urlPage = "https://www.spreadshirt.com";
            HttpWebRequest wRequestPage = (HttpWebRequest)WebRequest.Create(urlPage);
            wRequestPage.Method = "GET";
            wRequestPage.Host = "www.spreadshirt.com";
            wRequestPage.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            wRequestPage.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wRequestPage.UseDefaultCredentials = true;
            wRequestPage.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
            wRequestPage.Referer = urlPage;
            wRequestPage.CookieContainer = cookieJar;

            var cookieHeader = "";
            WebResponse wResponse = wRequestPage.GetResponse();
            cookieHeader = wResponse.Headers["Set-cookie"];

            foreach (Cookie cookie in ((HttpWebResponse)wResponse).Cookies)
            {
                cookieJar.Add(cookie);
            }

            //Login Page
            string urlLogin = "https://www.spreadshirt.com/api/v1/sessions?mediaType=json";
            string data2Send = "{\"username\":\"lchoang1995@gmail.com\", \"password\":\"Thienan@111\", \"rememberMe\":false}";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postDataBytes = encoding.GetBytes(data2Send);

            HttpWebRequest wRequestLogin = (HttpWebRequest)WebRequest.Create(urlLogin);
            wRequestLogin.CookieContainer = cookieJar;
            wRequestLogin.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
            wRequestLogin.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wRequestLogin.Method = "POST";
            wRequestLogin.Accept = "*/*";
            wRequestLogin.Host = "www.spreadshirt.com";
            wRequestLogin.ContentType = "application/x-www-form-urlencoded";
            wRequestLogin.ContentLength = postDataBytes.Length;
            wRequestLogin.Referer = "https://www.spreadshirt.com/login";
            wRequestLogin.UseDefaultCredentials = true;
            wRequestLogin.PreAuthenticate = true;
            wRequestLogin.Credentials = CredentialCache.DefaultCredentials;

            using (Stream sr = wRequestLogin.GetRequestStream())
            {
                sr.Write(postDataBytes, 0, postDataBytes.Length);
            }
            WebResponse rspLogin = wRequestLogin.GetResponse();
            cookieJar = new CookieContainer();
            foreach (Cookie cookie in ((HttpWebResponse)wResponse).Cookies)
            {
                cookieJar.Add(cookie);
            }
            var locationHeader = "";
            locationHeader = rspLogin.Headers["location"];
            String htmlString;
            using (var reader = new StreamReader(rspLogin.GetResponseStream()))
            {
                htmlString = reader.ReadToEnd();
            }   
            var obj = JObject.Parse(htmlString);
            string id = obj["id"].ToString();
            string href = obj["href"].ToString();
            string identityHref = obj["identity"]["href"].ToString();
            string identityId = obj["identity"]["id"].ToString();
            string userHref = obj["user"]["href"].ToString();
            string userId = obj["user"]["id"].ToString();
            string role = obj["role"].ToString();

            //https://partner.spreadshirt.com/api/v1/users/302719724/ideas/5a0ac4a0aa0c6d5484cf1e98?apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b&locale=us_US&mediaType=json&sig=c313820f219028718036e05a93d375196f36b49d&time=1510629969266
            string url = " http://api.spreadshirt.net/api/v1/shops/" + userId + "/designs?apiKey=" + API_KEY;

            url = "https://partner.spreadshirt.com/api/v1/users/" + userId + "/ideas/5a0ac4a0aa0c6d5484cf1e98?apiKey=" + API_KEY + "&locale=us_US&mediaType=json&sig=@SIG&time=1510629969266";
            var temp = url.Split('?')[0];
            string time = "1510629969266";
            string data = "PUT " + temp + " " + time + " " + SECRET;
            string sig = GetSHA1HashData(data);
            url = url.Replace("@SIG", sig);

            StreamReader payload = new StreamReader(@"D:\SOFT_HOANG\Project\SunfrogShirts\trunk\SpreadShirts\upload\createDesign.json");
            string data2 = payload.ReadToEnd();
            postXMLData(url, data2);
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
            request.CookieContainer = cookieJar;
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //string userId = "25263";
            //string url = "http://api.spreadshirt.net/api/v1/users/" + userId + "/designs";
            //StreamReader payload = new StreamReader(@"D:\SOFT_HOANG\Project\SunfrogShirts\trunk\SpreadShirts\upload\createDesign.xml");
            //string data = payload.ReadToEnd();

            string url = "http://api.spreadshirt.net/api/v1/users/".Split('?')[0];
            string time = DateTime.Now.TimeOfDay.ToString();
            string data = "POST " + url + " " + time + " " + SECRET;
            string sig = GetSHA1HashData(data);
        }


        /// <summary>
        /// take any string and encrypt it using SHA1 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        private string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }
    }
}
