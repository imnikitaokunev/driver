using OpenQA.Selenium;

namespace Driver.InfoCar.Pages;

public abstract class PageBase
{
    protected static void AcceptCookiesIfNeeded()
    {
        try
        {
            var acceptCookiesButton = Browser.WebDriver.FindElement(By.XPath("//app-accept-cookies//button"));
            acceptCookiesButton.Click();
        }
        catch (NoSuchElementException)
        {
        }
    }
}