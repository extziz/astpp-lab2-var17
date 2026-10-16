using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Lab2.BankingTests.Pages;

namespace Lab2.BankingTests.StepDefinitions
{
    [Binding]
    public class OpenAccountSteps
    {
        private readonly IWebDriver _driver;
        private ManagerPage _manager;
        private OpenAccountPage _openAccountPage;

        // стан між кроками одного сценарію
        private string _lastAlert;
        private readonly List<int> _createdAccounts = new List<int>();

        public OpenAccountSteps(IWebDriver driver)
        {
            _driver = driver;
        }

        [Given(@"the XYZ Bank home page is opened")]
        public void GivenTheHomePageIsOpened()
        {
            new HomePage(_driver).Open();
        }

        [Given(@"I log in as Bank Manager")]
        public void GivenILogInAsBankManager()
        {
            _manager = new HomePage(_driver).LoginAsBankManager();
        }

        [Given(@"I open the ""Open Account"" tab")]
        public void GivenIOpenTheOpenAccountTab()
        {
            _openAccountPage = _manager.GoToOpenAccount();
        }

        [When(@"I select the customer ""(.*)""")]
        public void WhenISelectTheCustomer(string customer)
        {
            _openAccountPage.SelectCustomer(customer);
        }

        [When(@"I select the currency ""(.*)""")]
        public void WhenISelectTheCurrency(string currency)
        {
            _openAccountPage.SelectCurrency(currency);
        }

        [When(@"I press the Process button")]
        public void WhenIPressTheProcessButton()
        {
            _openAccountPage.ClickProcess();
            _lastAlert = _openAccountPage.AcceptAlertIfShown(System.TimeSpan.FromSeconds(3));
            RememberCreatedAccount();
        }

        [When(@"I open an account for ""(.*)"" in ""(.*)""")]
        public void WhenIOpenAnAccountFor(string customer, string currency)
        {
            _openAccountPage.SelectCustomer(customer).SelectCurrency(currency).ClickProcess();
            _lastAlert = _openAccountPage.AcceptAlertIfShown(System.TimeSpan.FromSeconds(3));
            RememberCreatedAccount();
        }

        [Then(@"the alert ""(.*)"" is shown")]
        public void ThenTheAlertIsShown(string expectedPrefix)
        {
            Assert.That(_lastAlert, Is.Not.Null, "The site did not show an alert.");
            Assert.That(_lastAlert, Does.StartWith(expectedPrefix));
            Assert.That(_createdAccounts, Has.Count.EqualTo(1), "The alert has no account number.");
        }

        [Then(@"the ""Customers"" tab shows (\d+) accounts for ""(.*)"" including the new one")]
        public void ThenCustomersTabShowsAccountsIncludingNew(int expectedCount, string customer)
        {
            var accounts = _manager.GoToCustomers().GetAccountNumbers(customer);

            Assert.That(accounts, Has.Count.EqualTo(expectedCount));
            Assert.That(accounts, Is.SupersetOf(_createdAccounts),
                $"Customer '{customer}' does not have the newly created account(s).");
        }

        [Then(@"the ""Customers"" tab shows (\d+) accounts for ""(.*)""")]
        public void ThenCustomersTabShowsAccounts(int expectedCount, string customer)
        {
            var accounts = _manager.GoToCustomers().GetAccountNumbers(customer);

            Assert.That(accounts, Has.Count.EqualTo(expectedCount));
        }

        [Then(@"the second account number is greater than the first by 1")]
        public void ThenSecondAccountNumberIsGreaterByOne()
        {
            Assert.That(_createdAccounts, Has.Count.EqualTo(2));
            Assert.That(_createdAccounts[1], Is.EqualTo(_createdAccounts[0] + 1));
        }

        [Then(@"the customer and currency fields are reset to their placeholders")]
        public void ThenFieldsAreReset()
        {
            Assert.That(_openAccountPage.SelectedCustomer, Is.EqualTo("---Customer Name---"));
            Assert.That(_openAccountPage.SelectedCurrency, Is.EqualTo("---Currency---"));
        }

        [Then(@"no account is created and no alert is shown")]
        public void ThenNoAccountCreated()
        {
            Assert.That(_lastAlert, Is.Null, $"Unexpected alert: {_lastAlert}");
            Assert.That(_createdAccounts, Is.Empty);
        }

        [Then(@"the browser marks the ""(Customer|Currency)"" field as required")]
        public void ThenTheFieldIsMarkedAsRequired(string field)
        {
            var invalid = field == "Customer"
                ? _openAccountPage.IsCustomerFieldInvalid
                : _openAccountPage.IsCurrencyFieldInvalid;

            Assert.That(invalid, Is.True, $"The '{field}' field was not flagged by form validation.");
        }

        private void RememberCreatedAccount()
        {
            var number = OpenAccountPage.ParseAccountNumber(_lastAlert);
            if (number.HasValue) _createdAccounts.Add(number.Value);
        }
    }
}
