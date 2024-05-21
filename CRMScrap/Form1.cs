﻿using CRMScrap.Services.CSV;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CRMScrap
{
    public partial class Form1 : Form
    {
        private ChromeDriver _driver;
        private Thread _thread;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void TxtBoxChanged(object sender, EventArgs e)
        {
            if (Username_TextBox.Text != "" && Password_TextBox.Text != "")
            {
                Login_BTN.ForeColor = Color.Lime;
                Login_BTN.Cursor = Cursors.Hand;
            }
            else
            {
                Login_BTN.ForeColor = Color.Red;
                Login_BTN.Cursor = Cursors.No;
            }
        }

        private void LoginButton(object sender, EventArgs e)
        {
            if (Login_BTN.Cursor == Cursors.Hand)
            {
                _thread = new Thread(Result);
                _thread.Start();
            }
        }

        private void Result()
        {
            Login_BTN.ForeColor = Color.Gold;
            Login_BTN.UseWaitCursor = true;
            Login_BTN.Text = "Testing...";
            OpenSelenium(); Thread.Sleep(3000);
            Login(Username_TextBox.Text, Password_TextBox.Text); Thread.Sleep(100);
            ScrapInfoOnImmovelTable();
            //CloseSelenium();
            Login_BTN.ForeColor = Color.Lime;
            Login_BTN.UseWaitCursor = false;
            Login_BTN.Text = "Login";
        }

        private void OpenSelenium()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            _driver = new ChromeDriver(service);
            _driver.Navigate().GoToUrl("https://app.imo360crm.pt/login");
        }

        private void Login(string username, string password)
        {
            try
            {
                _driver.FindElements(By.XPath("//*[@id=\"username\"]"))[0].SendKeys(username); Thread.Sleep(3000);
                _driver.FindElements(By.XPath("/html/body/div[2]/div/div/div/div[1]/div[2]/div/form/div[2]/input"))[0].SendKeys(password); Thread.Sleep(2000);
                _driver.FindElement(By.XPath("//*[@id=\"login\"]/div[3]/button")).Click(); Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ScrapInfoOnImmovelTable()
        {
            var items = AllParameters();
            bool hasNextPage = true;
            int retryCount = 0;
            const int maxRetries = 5;

            _driver.Navigate().GoToUrl("https://app.imo360crm.pt/listagem/imoveis");

            while (hasNextPage)
            {
                try
                {
                    // Use WebDriverWait to wait until the table is loaded
                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"customtable\"]/div/div[2]/div")).FindElement(By.TagName("tbody")));

                    var homesTable = _driver.FindElement(By.XPath("//*[@id=\"customtable\"]/div/div[2]/div")).FindElement(By.TagName("tbody"));
                    var rows = homesTable.FindElements(By.TagName("tr"));

                    foreach (var row in rows)
                    {
                        var webElement = row.FindElement(By.XPath("./td[1]/a"));
                        var url = webElement.GetAttribute("href");
                        var reference = webElement.GetAttribute("text");

                        items.Add(new string[] { url, reference });
                    }

                    // Check if the next page button is disabled
                    try
                    {
                        _driver.FindElement(By.XPath("//li[@class='page-item disabled' and @aria-disabled='true' and @aria-label='Próximo »']"));
                        hasNextPage = false; // If the element is found, it means we are on the last page
                    }
                    catch (NoSuchElementException)
                    {
                        // If the element is not found, click the next page button
                        var nextButton = _driver.FindElement(By.XPath("//li[@class='page-item']//a[@aria-label='Próximo »']"));
                        nextButton.Click();

                        // Reset retry count after successful navigation
                        retryCount = 0;

                        // Wait for the next page to load
                        Thread.Sleep(1000);
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        hasNextPage = false; // Stop trying after maxRetries
                    }
                    else
                    {
                        // Optionally, log the retry attempt
                        Console.WriteLine($"Retry attempt {retryCount}...");
                        Thread.Sleep(2000); // Wait before retrying
                    }
                }
            }

            ICsvExporter csvExporter = new CSVHelper();
            csvExporter.ExportToCsv("output.csv", items);
        }

        //private void ScrapInfoOnImmovelTable()
        //{
        //    var items = AllParameters();

        //    _driver.Navigate().GoToUrl("https://app.imo360crm.pt/listagem/imoveis"); Thread.Sleep(1000);

        //    var homesTable = _driver.FindElement(By.XPath("//*[@id=\"customtable\"]/div/div[2]/div")).FindElement(By.TagName("tbody"));
        //    var rows = homesTable.FindElements(By.TagName("tr"));

        //    foreach (var row in rows)
        //    {
        //        var webElement = row.FindElement(By.XPath("./td[1]/a"));
        //        var url = webElement.GetAttribute("href");
        //        var reference = webElement.GetAttribute("text");

        //        items.Add(new string[] { url, reference });
        //    }

        //    ICsvExporter csvExporter = new CSVHelper();
        //    csvExporter.ExportToCsv("output.csv", items);
        //}

        private static List<string[]> AllParameters()
        {
            return new List<string[]>();
        }

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            CloseSelenium();
        }
        private void CloseSelenium()
        {
            _driver.Quit();
        }
    }
}
