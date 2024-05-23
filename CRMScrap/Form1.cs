using CRMScrap.Models;
using CRMScrap.Services.CSV;
using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Globalization;

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
            Random random = new Random();
            int delay = random.Next(1000, 4001);


            _driver.Navigate().GoToUrl("https://app.imo360crm.pt/listagem/imoveis?page=1");

            while (hasNextPage)
            {
                try
                {
                    // Use WebDriverWait to wait until the table is loaded
                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    Thread.Sleep(delay);
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
                        Thread.Sleep(delay);
                        hasNextPage = false; // If the element is found, it means we are on the last page
                    }
                    catch (NoSuchElementException)
                    {

                        var nextButton = _driver.FindElement(By.XPath("//li[@class='page-item']//a[@aria-label='Próximo »']"));
                        nextButton.Click();

                        retryCount = 0;

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
                        Thread.Sleep(2000);
                    }
                }
            }

            ICsvExporter csvExporter = new CSVHelper();
            csvExporter.ExportToCsv("output.csv", items);

            var properties = ScrapInfoFromPropertyPages(items);
            SavePropertiesToCsv("properties_output.csv", properties);
        }


        private List<RealEstateProperties> ScrapInfoFromPropertyPages(List<string[]> items)
        {
            var properties = new List<RealEstateProperties>();
            int idCounter = 1;
            Random random = new Random();
            int delay = random.Next(1000, 4001);

            foreach (var item in items)
            {
                string url = item[0];
                _driver.Navigate().GoToUrl(url);

                try
                {
                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    Thread.Sleep(delay);
                    wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"layout-wrapper\"]/div[2]/div/div/div[2]/div[2]/div[1]/div/div")));

                    // Scrape General Data
                    var property = new RealEstateProperties
                    {
                        Id = idCounter++,
                        Reference = item[1],
                        Url = item[0],
                        ExternalId = item[0].TrimEnd('/'),
                        Nature = GetElementText("//*[@id=\"geral\"]/div[1]/div[1]/ul[1]/li[2]"),
                        Condition = GetElementText("//*[@id=\"geral\"]/div[1]/div[1]/ul[2]/li[2]"),
                        Typologi = GetElementText("//*[@id=\"geral\"]/div[1]/div[1]/ul[3]/li[2]"),
                        WC = GetElementText("//*[@id=\"geral\"]/div[1]/div[1]/ul[4]/li[2]"),
                        Garage = GetElementText("//*[@id=\"geral\"]/div[1]/div[1]/ul[5]/li[2]"),
                        EnergiCertification = GetElementText("//*[@id=\"geral\"]/div[1]/div[1]/ul[6]/li[2]"),
                        YearConstraction = GetElementText("/html/body/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div/div[1]/div[1]/div[1]/ul[7]/li[2]"),
                        Business = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[1]/li[2]"),
                        Price = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[2]/li[2]"),
                        Avaibility = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[3]/li[2]"),
                        ContractNumber = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[6]/li[2]"),
                        DateStart = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[7]/li[2]"),
                        DateEnd = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[7]/li[2]"),
                        CommisionAgenci = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[9]/li[2]"),
                        Exlusiv = GetElementText("//*[@id=\"geral\"]/div[1]/div[3]/ul[10]/li[2]"),
                        AreaU = GetElementText("//*[@id=\"geral\"]/div[2]/div/ul[1]/li[2]"),
                        AreaB = GetElementText("//*[@id=\"geral\"]/div[2]/div/ul[2]/li[2]"),
                        AreaT = GetElementText("//*[@id=\"geral\"]/div[2]/div/ul[3]/li[2]")
                    };

                    // Scrape Location Data
                    _driver.FindElement(By.XPath("//*[@id=\"layout-wrapper\"]/div[2]/div/div/div[2]/div[2]/div[1]/div/ul/li[2]/a")).Click(); Thread.Sleep(1000);
                    wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"localization\"]/div")));
                    property.Latitude = GetElementText("//*[@id=\"localization\"]/div/div[1]/ul[1]/li[2]");
                    property.Longitude = GetElementText("//*[@id=\"localization\"]/div/div[1]/ul[2]/li[2]");
                    property.Address = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[1]/li[2]");
                    property.DoorNumber = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[2]/li[2]");
                    property.Floor = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[3]/li[2]");
                    property.ZipCode = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[4]/li[2]");
                    property.Location = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[5]/li[2]");
                    property.State = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[6]/li[2]");
                    property.Town = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[7]/li[2]");
                    property.Neighborhood = GetElementText("//*[@id=\"localization\"]/div/div[2]/ul[8]/li[2]");


                    _driver.FindElement(By.XPath("//*[@id=\"layout-wrapper\"]/div[2]/div/div/div[2]/div[2]/div[1]/div/ul/li[3]/a")).Click(); Thread.Sleep(1000);
                    wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"description\"]")));
                    property.Title = GetElementText("//*[@id=\"description\"]/div/h4");
                    property.Description = GetElementText("//*[@id=\"description\"]/div/div");

                    properties.Add(property);
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine($"Failed to load property details for URL: {url}");
                }
            }
            return properties;
        }

        private string GetElementText(string xpath)
        {
            try
            {
                return _driver.FindElement(By.XPath(xpath)).Text;
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
        }


        private void SavePropertiesToCsv(string filePath, List<RealEstateProperties> properties)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(properties);
            }
        }

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
