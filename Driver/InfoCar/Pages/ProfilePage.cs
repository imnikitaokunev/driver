using Driver.Extensions;
using OpenQA.Selenium;

namespace Driver.InfoCar.Pages;

public sealed class ProfilePage : PageBase
{
    public const string ProfileUrl = "https://info-car.pl/new/konto";
    private static bool _isAuthorized;
    
    public static bool CheckIfAuthorized()
    {
        try
        {
            Browser.NavigateToUrl(ProfileUrl);
            Browser.Wait.Until(IsAuthorizationVerified); 
            return _isAuthorized;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    private static bool IsAuthorizationVerified(IWebDriver driver)
    {
        if (driver.IsElementPresent(By.Id("form-login")))
        {
            return true;
        }

        if (driver.IsElementPresent(By.ClassName("my-account-header")))
        {
            _isAuthorized = true;
            return true;
        }

        return false;
    }
}