using System;

namespace Lab2.BankingTests.Support
{
    /// <summary>
    /// Налаштування запуску. Значення за замовчуванням - видимий Chrome;
    /// змінити можна змінними оточення (корисно для CI та запуску без екрана):
    ///   BROWSER=chrome|firefox, HEADLESS=true, SAVE_SCREENSHOTS=true
    /// </summary>
    public static class TestSettings
    {
        public const string BaseUrl = "https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login";

        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

        public static string Browser => Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";

        public static bool Headless => IsTrue("HEADLESS");

        public static bool SaveScreenshots => IsTrue("SAVE_SCREENSHOTS");

        private static bool IsTrue(string name) =>
            string.Equals(Environment.GetEnvironmentVariable(name), "true", StringComparison.OrdinalIgnoreCase);
    }
}
