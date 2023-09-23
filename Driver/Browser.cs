using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Driver;

public static class Browser
{
    public static IWebDriver WebDriver { get; private set; }
    public static WebDriverWait Wait { get; private set; }

    public static void Initialize()
    {
        WebDriver = new ChromeDriver();
        Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(10));

        WebDriver.Manage().Window.Maximize();
        WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    public static void NavigateToUrl(string url)
    {
        WebDriver.Navigate().GoToUrl(url);
    }
}