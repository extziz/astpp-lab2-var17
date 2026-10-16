using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using System.Linq;

namespace Lab2.BankingTests.Pages
{
    /// <summary>Форма відкриття рахунку (вкладка Open Account).</summary>
    public class OpenAccountPage : BasePage
    {
        private static readonly By CustomerSelect = By.Id("userSelect");
        private static readonly By CurrencySelect = By.Id("currency");
        private static readonly By ProcessButton = By.CssSelector("form[name='myForm'] button[type='submit']");

        public OpenAccountPage(IWebDriver driver) : base(driver) { }

        public void WaitUntilLoaded() => WaitVisible(CustomerSelect);

        public OpenAccountPage SelectCustomer(string fullName)
        {
            ChooseOption(CustomerSelect, fullName);
            return this;
        }

        public OpenAccountPage SelectCurrency(string currency)
        {
            ChooseOption(CurrencySelect, currency);
            return this;
        }

        public OpenAccountPage ClickProcess()
        {
            Click(ProcessButton);
            return this;
        }

        /// <summary>Текст alert (і закриває його) або null, якщо сайт alert не показав.</summary>
        public string AcceptAlertIfShown(TimeSpan timeout)
        {
            var alert = TryGetAlert(timeout);
            if (alert == null) return null;
            var text = alert.Text;
            alert.Accept();
            return text;
        }

        /// <summary>Витягує номер нового рахунку з тексту "Account created successfully with account Number :1016".</summary>
        public static int? ParseAccountNumber(string alertText)
        {
            var match = Regex.Match(alertText ?? "", @"Number\s*:\s*(\d+)");
            return match.Success ? int.Parse(match.Groups[1].Value) : (int?)null;
        }

        /// <summary>Чи позначив браузер поле як таке, що не пройшло HTML5-валідацію (required).</summary>
        public bool IsCustomerFieldInvalid => IsInvalid(CustomerSelect);

        public bool IsCurrencyFieldInvalid => IsInvalid(CurrencySelect);

        public string SelectedCustomer => SelectedOptionText(CustomerSelect);

        public string SelectedCurrency => SelectedOptionText(CurrencySelect);

        // у Selenium 4.51 SelectElement винесено з основного пакета, тому працюємо з <option> напряму
        private void ChooseOption(By selectLocator, string optionText)
        {
            var option = WaitVisible(selectLocator)
                .FindElements(By.TagName("option"))
                .FirstOrDefault(o => o.Text.Trim() == optionText);

            if (option == null)
                throw new NoSuchElementException($"Option '{optionText}' was not found in {selectLocator}.");

            option.Click();
        }

        private bool IsInvalid(By selectLocator) =>
            !string.IsNullOrEmpty(WaitVisible(selectLocator).GetDomProperty("validationMessage"));

        private string SelectedOptionText(By selectLocator) =>
            WaitVisible(selectLocator)
                .FindElements(By.TagName("option"))
                .First(o => o.Selected)
                .Text.Trim();
    }
}
