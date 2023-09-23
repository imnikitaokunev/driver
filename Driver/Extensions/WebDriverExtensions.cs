using OpenQA.Selenium;

namespace Driver.Extensions;

public static class WebDriverExtensions
{
    public static bool IsElementPresent(this IWebDriver driver, By by)
    {
        try
        {
            driver.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}