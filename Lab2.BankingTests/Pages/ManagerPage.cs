using OpenQA.Selenium;

namespace Lab2.BankingTests.Pages
{
    /// <summary>Кабінет менеджера банку з трьома вкладками: Add Customer, Open Account, Customers.</summary>
    public class ManagerPage : BasePage
    {
        private static readonly By OpenAccountTab = By.CssSelector("button[ng-click='openAccount()']");
        private static readonly By CustomersTab = By.CssSelector("button[ng-click='showCust()']");

        public ManagerPage(IWebDriver driver) : base(driver) { }

        public OpenAccountPage GoToOpenAccount()
        {
            Click(OpenAccountTab);
            var page = new OpenAccountPage(Driver);
            page.WaitUntilLoaded();
            return page;
        }

        public CustomersPage GoToCustomers()
        {
            Click(CustomersTab);
            var page = new CustomersPage(Driver);
            page.WaitUntilLoaded();
            return page;
        }
    }
}
