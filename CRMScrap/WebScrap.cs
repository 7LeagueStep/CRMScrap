using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CRMScrap;

/*public class WebScrap
{
    private IWebDriver _driver = new ChromeDriver();
    private bool _isLoggedIn;

    internal void ScrapInfoOnImovelTable()
    { 
        if (!_isLoggedIn)
        {
            throw new InvalidOperationException("You must be logged in to scrape information.");
        }
        
        var items = AllParameters();
        _driver.Navigate().GoToUrl("https://app.imo360crm.pt/listagem/imoveis"); Thread.Sleep(1000);        
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
}*/