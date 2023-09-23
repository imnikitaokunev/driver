using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Driver.InfoCar.Pages;

public sealed class SchedulePage : PageBase
{
    private const string ScheduleUrl = "https://info-car.pl/new/prawo-jazdy/sprawdz-wolny-termin";
    private const string Category = "B";
    private const string CategoryId = "b";
    private const string Province = "mazowieckie";
    private const string ProvinceId = "mazowieckie";
    private const string Organization = "WORD Warszawa M/E Odlewnicza";
    private const string OrganizationId = "word-warszawa m/e odlewnicza";

    public static IEnumerable<DateTime> FindNearestPracticalExamDates()
    {
        try
        {
            Browser.NavigateToUrl(ScheduleUrl);
            
            AcceptCookiesIfNeeded();

            SelectValueFromDropdown("province", Province, ProvinceId);
            SelectValueFromDropdown("organization", Organization, OrganizationId);
            SelectValueFromDropdown("category-select", Category, CategoryId);

            FindElementAndClick(By.ClassName("ghost-btn__container"));
            FindElementAndClick(By.Id("practical-container"));

            return ParseNearestExamDates();
        }
        catch (WebDriverTimeoutException)
        {
        }
        
        return new List<DateTime>();
    }

    private static void SelectValueFromDropdown(string dropdown, string searchKey, string valueId)
    {
        var scrollAction = new Actions(Browser.WebDriver);
        scrollAction.ScrollByAmount(0, 200);
        scrollAction.Perform();
        
        var selector = Browser.Wait.Until(driver => driver.FindElement(By.Id(dropdown)));
        
        selector.SendKeys(searchKey);
        var value = Browser.Wait.Until(driver => driver.FindElement(By.Id(valueId)));
        value.Click();
    }

    private static void FindElementAndClick(By by)
    {
        var element = Browser.Wait.Until(driver => driver.FindElement(by));
        element.Click();
    }

    private static IEnumerable<DateTime> ParseNearestExamDates()
    {
        var exams = Browser.WebDriver.FindElements(By.TagName("h5"));
        var cultureInfo = new CultureInfo("pl-PL");

        return exams.Select(exam => DateTime.Parse(exam.Text, cultureInfo)).ToList();
    }
}