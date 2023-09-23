using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace Driver.InfoCar.Pages;

public sealed class AuthorizationPage : PageBase
{
    private const string AuthorizationUrl = "https://info-car.pl/oauth2/login";

    public static void Authorize(string username, string password)
    {
        try
        {
            Browser.NavigateToUrl(AuthorizationUrl);

            var usernameInput = Browser.Wait.Until(driver => driver.FindElement(By.Id("username")));
            usernameInput.SendKeys(username);

            var passwordInput = Browser.Wait.Until(driver => driver.FindElement(By.Id("password")));
            passwordInput.SendKeys(password);

            var authorizeButton = Browser.Wait.Until(driver => driver.FindElement(By.Id("register-button")));
            authorizeButton.Click();

            AcceptCookiesIfNeeded();

            Browser.Wait.Until(ExpectedConditions.UrlToBe(ProfilePage.ProfileUrl));
        }
        catch (WebDriverTimeoutException)
        {
        }
    }
}