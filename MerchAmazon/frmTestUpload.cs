using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace MerchAmazon
{
    public partial class frmTestUpload : Form
    {
        public frmTestUpload()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Step1_Login("VN.Ngfk1@gmail.com", "AMZNoNPa$$#1235!");
        }


        /// <summary>
        /// Step 1: Login To Amazon
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void Step1_Login(string username, string password)
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.FirefoxBinaryPath = @"E:\Firefox\firefox.exe";
            //service.FirefoxBinaryPath = @"D:\FireFox49\firefox.exe";
            IWebDriver driver = new FirefoxDriver(service);
            //driver.Url = "https://www.teepublic.com/dashboard";
            driver.Url = "https://merch.amazon.com/dashboard";

            Thread.Sleep(10000);
            IWebElement btnExecLogin = driver.FindElement(By.Id("signInSubmit"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            if (btnExecLogin != null)
            {
                //Enter pass
                Thread.Sleep(3000);
                driver.FindElement(By.Id("ap_email")).SendKeys(username);
                driver.FindElement(By.Id("ap_password")).SendKeys(password);

                Thread.Sleep(3000);
                js.ExecuteScript("arguments[0].click();", btnExecLogin);
            }

            Thread.Sleep(3000);
            //Go to Upload
            driver.Navigate().GoToUrl("https://merch.amazon.com/merch-tshirt/title-setup/new/upload_art");

            Thread.Sleep(5000);
            //Select Arg Select
            string[] idProductType = new string[5] {
                "data-draft-shirt-type-native_0",//Standard T-Shirt
                "data-draft-shirt-type-native_1",//Premium T-Shirt
                "data-draft-shirt-type-native_2",//Long Sleeve T-Shirt
                "data-draft-shirt-type-native_3",//Sweatshirt
                "data-draft-shirt-type-native_4" //Pullover Hoodie
            };
            IWebElement selectProductType = driver.FindElement(By.Id("data-draft-shirt-type-native"));
            IWebElement btnExecUploadFile = driver.FindElement(By.Id("data-draft-tshirt-assets-front-image-asset-cas-shirt-art-image-file-upload-browse-button-announce"));
            IWebElement btnFront = driver.FindElement(By.Id("gear-front-button-announce"));
            IWebElement btnBack = driver.FindElement(By.Id("gear-back-button-announce"));
            IWebElement btnSaveAndContinue = driver.FindElement(By.Id("save-and-continue-upload-art-announce"));


            //driver.FindElementByLinkText("Upload Files").click();
            //driver.SetLogLevel(Level.ALL);
            IWebElement element = driver.FindElement(By.XPath("//input[@name='file_1']"));
            OpenQA.Selenium.Remote.LocalFileDetector detector = new OpenQA.Selenium.Remote.LocalFileDetector();

            //Select ProductType

            SelectElement SelectAnEducation = new SelectElement(selectProductType);
            SelectAnEducation.SelectByIndex(3);

        }

    }

    public class ProductType
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static List<ProductType> InitProductType() {
            List<ProductType> data = new List<ProductType>();
            data.Add(new ProductType() { Code = "HOUSE_BRAND", Name = "Standard T-Shirt" });
            data.Add(new ProductType() { Code = "PREMIUM_BRAND", Name = "Premium T-Shirt" });
            data.Add(new ProductType() { Code = "STANDARD_LONG_SLEEVE", Name = "Long Sleeve T-Shirt" });
            data.Add(new ProductType() { Code = "STANDARD_SWEATSHIRT", Name = "Sweatshirt" });
            data.Add(new ProductType() { Code = "STANDARD_PULLOVER_HOODIE", Name = "Pullover Hoodie" });
            return data;
        }
    }
}
