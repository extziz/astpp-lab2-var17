using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Lab2.BankingTests.Support
{
    public static class WebDriverFactory
    {
        public static IWebDriver Create()
        {
            IWebDriver driver;

            if (string.Equals(TestSettings.Browser, "firefox", StringComparison.OrdinalIgnoreCase))
            {
                var options = new FirefoxOptions();
                if (TestSettings.Headless) options.AddArgument("-headless");
                driver = new FirefoxDriver(options);
            }
            else
            {
                var options = new ChromeOptions();
                options.AddArgument("--window-size=1280,900");
                options.AddArgument("--disable-notifications");
                if (TestSettings.Headless) options.AddArgument("--headless=new");
                // потрібно, якщо Chrome запускається від root / у контейнері
                if (Environment.GetEnvironmentVariable("NO_SANDBOX") == "true") options.AddArgument("--no-sandbox");
                driver = new ChromeDriver(options);
            }

            driver.Manage().Window.Size = new System.Drawing.Size(1280, 900);
            return driver;
        }
    }
}
