using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CRMScrap
{
    public partial class Form1 : Form
    {
        private ChromeDriver _driver;
        private Thread _thread;
        //private WebScrap _scrap;
        //private bool _isLoggedIn = false;

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

            _driver.Navigate().GoToUrl("https://app.imo360crm.pt/listagem/imoveis"); Thread.Sleep(1000);

            var homesTable = _driver.FindElement(By.XPath("//*[@id=\"customtable\"]/div/div[2]/div")).FindElement(By.TagName("tbody"));
            var rows = homesTable.FindElements(By.TagName("tr"));

            foreach (var row in rows)
            {
                var webElement = row.FindElement(By.XPath("./td[1]/a"));
                var url = webElement.GetAttribute("href");
                var reference = webElement.GetAttribute("text");

                items.Add(new string[] { url, reference });
            }

            foreach (var item in items)
            {
                Console.WriteLine($"URL: {item[0]}, Reference: {item[1]}");
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

        public class RealEstateProperties
        {
            public string Reference { get; set; }
            public string Nature { get; set; }
            public string Condition { get; set; }
            public string Typologi { get; set; }
            public string EnergiCertification { get; set; }
            public string YearConstraction { get; set; }
            public string Business { get; set; }
            public string Price { get; set; }
            public string Avaibility { get; set; }
            public string ContractNumber { get; set; }
            public string DateStart { get; set; }
            public string DateEnd { get; set; }
            public string CommisionAgenci { get; set; }
            public string Exlusiv { get; set; }
            public string AreaU { get; set; }
            public string AreaB { get; set; }
            public string AreaT { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Address { get; set; }
            public string DoorNumber { get; set; }
            public string Floor { get; set; }
            public string ZipCode { get; set; }
            public string Location { get; set; }
            public string State { get; set; }
            public string Town { get; set; }
            public string Neighborhood { get; set; }
            public string Description { get; set; }
            public List<string> Features { get; set; }
        }
    }
}
