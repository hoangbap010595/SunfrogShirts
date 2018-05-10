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
            service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            //service.FirefoxBinaryPath = @"D:\FireFox49\firefox.exe";
            IWebDriver driver = new FirefoxDriver(service,new FirefoxOptions(),TimeSpan.FromSeconds(180));
            //driver.Url = "https://www.teepublic.com/";
            driver.Url = "https://www.teepublic.com/";
            //driver.Navigate().Refresh();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login-link"))).Click();

            // Test the autocomplete response - Explicit Wait
            //IWebElement autocomplete = wait.Until(x => x.FindElement(By.ClassName("login-link")));

            //string autoCompleteResults = autocomplete.Text;
            //autocomplete.Click();

            //By addItem = By.Id("login-link");

            //driver.FindElement(By.Id("")).Click();
            //wait.Until(ExpectedConditions.ElementToBeClickable(addItem)).Click();

            //Enter pass
            //driver.FindElement(By.Id("session_email")).SendKeys("lchoang1995@gmail.com");
            //driver.FindElement(By.Id("session_password")).SendKeys("Thienan@111");

            //driver.FindElement(By.Id("login")).Click();
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
