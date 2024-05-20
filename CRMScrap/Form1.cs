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
            ScrapInfoOnImovelTable();
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
                
                // if (_driver.FindElements(By.XPath("//element_after_login")).Count > 0)
                // {
                //     _isLoggedIn = true;
                // }   
                
                _driver.Navigate().GoToUrl("https://app.imo360crm.pt/listagem/imoveis"); Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void ScrapInfoOnImovelTable()
        { 
            var items = AllParameters();
            //_driver.Navigate().GoToUrl("https://app.imo360crm.pt/listagem/imoveis"); Thread.Sleep(1000);        
            // Find elements that contain the product details
            IReadOnlyCollection<IWebElement> productElements = _driver.FindElements(By.XPath("//*[@id=\"customtable\"]"));

            // Loop through the product elements and extract the desired information
            foreach (IWebElement productElement in productElements)
            {
                string url = productElement.FindElement(By.XPath("//*[@id=\"tech-companies-1-clone\"]/tbody/tr[1]/td[1]")).Text;
                string reference = productElement.FindElement(By.XPath("//*[@id=\"tech-companies-1-clone\"]/tbody/tr[1]/td[1]")).Text;

                // Add the item details to the list
                items.Add(new string[] { url, reference });
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
