using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Lab2.BankingTests.Pages
{
    /// <summary>Таблиця клієнтів (вкладка Customers) - використовується, щоб перевірити результат відкриття рахунку.</summary>
    public class CustomersPage : BasePage
    {
        private static readonly By Rows = By.CssSelector("table tbody tr");

        public CustomersPage(IWebDriver driver) : base(driver) { }

        public void WaitUntilLoaded() => Wait.Until(d => d.FindElements(Rows).Count > 0);

        /// <summary>Номери рахунків клієнта з колонки "Account Number".</summary>
        public IReadOnlyList<int> GetAccountNumbers(string fullName)
        {
            var parts = fullName.Split(new[] { ' ' }, 2);
            var row = Driver.FindElements(Rows).FirstOrDefault(r =>
            {
                var cells = r.FindElements(By.TagName("td"));
                return cells[0].Text.Trim() == parts[0] && cells[1].Text.Trim() == parts[1];
            });

            if (row == null)
                throw new NotFoundException($"Customer '{fullName}' was not found in the customers table.");

            var text = row.FindElements(By.TagName("td"))[3].Text;
            return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }
    }
}
