using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace TeePublic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.FirefoxBinaryPath = @"E:\Firefox\firefox.exe";
            //service.FirefoxBinaryPath = @"D:\FireFox49\firefox.exe";
            IWebDriver driver = new FirefoxDriver(service, new FirefoxOptions(), TimeSpan.FromSeconds(180));
            //driver.Url = "https://www.teepublic.com/";
            driver.Url = "https://www.teepublic.com/";

            Thread.Sleep(10000);
            IWebElement btnLogin = driver.FindElement(By.Id("login-link"));
            IWebElement btnExecLogin = driver.FindElement(By.Id("login"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("arguments[0].click();", btnLogin);

            //Enter pass
            Thread.Sleep(3000);
            driver.FindElement(By.Id("session_email")).SendKeys("lchoang1995@gmail.com");
            driver.FindElement(By.Id("session_password")).SendKeys("Thienan@111");

            Thread.Sleep(3000);
            js.ExecuteScript("arguments[0].click();", btnExecLogin);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string chromeDriverDirectory = @"D:\SOFT_HOANG\Project\ToolUpload_KH_ThanhQuach\trunk\TeePublic\bin\Debug";
            var options = new ChromeOptions();
            options.AddArgument("-no-sandbox");
            IWebDriver driver = new ChromeDriver(chromeDriverDirectory, options,
            TimeSpan.FromMinutes(2));

            driver.Navigate().GoToUrl("https://www.teepublic.com/");
            //driver.Navigate().Refresh();


            driver.FindElement(By.Id("login-link")).Click();


            //Enter pass
            driver.FindElement(By.Id("session_email")).SendKeys("lchoang1995@gmail.com");
            driver.FindElement(By.Id("session_password")).SendKeys("Thienan@111");


            driver.FindElement(By.Id("login")).Click();
        }
    }
}
