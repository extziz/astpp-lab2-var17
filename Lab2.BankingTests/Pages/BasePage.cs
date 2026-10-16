using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Lab2.BankingTests.Support;

namespace Lab2.BankingTests.Pages
{
    /// <summary>Базовий клас усіх Page Object: драйвер, очікування і спільні дії.</summary>
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TestSettings.DefaultTimeout);
        }

        protected IWebElement WaitVisible(By locator) =>
            Wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return element.Displayed ? element : null;
            });

        protected void Click(By locator) => Wait.Until(d =>
        {
            var element = d.FindElement(locator);
            if (!element.Displayed || !element.Enabled) return false;
            element.Click();
            return true;
        });

        /// <summary>Чекає на JS alert і повертає його, або null, якщо alert не з'явився за вказаний час.</summary>
        protected IAlert TryGetAlert(TimeSpan timeout)
        {
            var wait = new WebDriverWait(Driver, timeout) { PollingInterval = TimeSpan.FromMilliseconds(100) };
            try
            {
                return wait.Until(d =>
                {
                    try { return d.SwitchTo().Alert(); }
                    catch (NoAlertPresentException) { return null; }
                });
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }
    }
}
