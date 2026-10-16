using System;
using System.IO;
using OpenQA.Selenium;
using BoDi;
using TechTalk.SpecFlow;

namespace Lab2.BankingTests.Support
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _container;
        private readonly ScenarioContext _scenario;
        private IWebDriver _driver;
        private int _stepNo;

        public Hooks(IObjectContainer container, ScenarioContext scenario)
        {
            _container = container;
            _scenario = scenario;
        }

        [BeforeScenario]
        public void StartBrowser()
        {
            _driver = WebDriverFactory.Create();
            // кожен сценарій - новий браузер, отже порожній localStorage і чисті початкові дані сайту
            _container.RegisterInstanceAs(_driver);
        }

        [AfterStep]
        public void MakeStepScreenshot()
        {
            _stepNo++;
            if (!TestSettings.SaveScreenshots) return;
            Save($"{Safe(_scenario.ScenarioInfo.Title)}_{_stepNo:00}");
        }

        [AfterScenario]
        public void StopBrowser()
        {
            if (_scenario.TestError != null) Save("FAILED_" + Safe(_scenario.ScenarioInfo.Title));
            _driver?.Quit();
        }

        private void Save(string name)
        {
            try
            {
                var dir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(dir);
                ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile(Path.Combine(dir, name + ".png"));
            }
            catch (UnhandledAlertException)
            {
                // відкритий alert блокує знімок - просто пропускаємо
            }
            catch (WebDriverException)
            {
            }
        }

        private static string Safe(string text)
        {
            foreach (var c in Path.GetInvalidFileNameChars()) text = text.Replace(c, '_');
            return text.Replace(' ', '_');
        }
    }
}
