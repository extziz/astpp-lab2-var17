using OpenQA.Selenium;
using Lab2.BankingTests.Support;

namespace Lab2.BankingTests.Pages
{
    /// <summary>Початкова сторінка з вибором ролі (Customer Login / Bank Manager Login).</summary>
    public class HomePage : BasePage
    {
        private static readonly By BankManagerLoginButton =
            By.XPath("//button[normalize-space()='Bank Manager Login']");

        public HomePage(IWebDriver driver) : base(driver) { }

        public HomePage Open()
        {
            Driver.Navigate().GoToUrl(TestSettings.BaseUrl);
            WaitVisible(BankManagerLoginButton);
            return this;
        }

        public ManagerPage LoginAsBankManager()
        {
            Click(BankManagerLoginButton);
            return new ManagerPage(Driver);
        }
    }
}
