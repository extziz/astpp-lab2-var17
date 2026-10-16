# Лабораторна робота №2 - Web-тестування з Selenium (варіант 17)

**Сайт:** https://www.globalsqa.com/angularJs-protractor/BankingProject
**Сценарій:** вхід `Bank Manager Login`, операція `Open Account`.

Стек: C# / .NET 8, Selenium WebDriver 4, SpecFlow 3.9 (Gherkin) + NUnit, патерн Page Object.

## Структура

| Папка / файл | Призначення |
|---|---|
| `Features/OpenAccount.feature` | Опис поведінки мовою Gherkin (10 тестових прогонів) |
| `StepDefinitions/OpenAccountSteps.cs` | Реалізація кроків Gherkin - лише виклики Page Object і `Assert` |
| `Pages/` | Page Objects: `HomePage`, `ManagerPage`, `OpenAccountPage`, `CustomersPage`, `BasePage` |
| `Support/Hooks.cs` | Запуск/закриття браузера на кожен сценарій, знімки екрана |
| `Support/WebDriverFactory.cs`, `TestSettings.cs` | Створення драйвера і налаштування запуску |

## Запуск у Visual Studio

1. Відкрити `Lab2.BankingTests.sln` (Visual Studio 2022, workload ".NET desktop development").
2. Для підсвітки та навігації по `.feature` встановити розширення **Reqnroll/SpecFlow for Visual Studio** (Extensions -> Manage Extensions -> "SpecFlow").
3. `Test -> Test Explorer -> Run All Tests`. Потрібен встановлений Chrome; драйвер Selenium завантажує автоматично (Selenium Manager).

## Запуск з консолі

```bash
dotnet test
# без вікна браузера + знімки екрана у bin/Debug/net8.0/Screenshots
HEADLESS=true SAVE_SCREENSHOTS=true dotnet test
# Windows PowerShell:  $env:HEADLESS="true"; dotnet test
# інший браузер: BROWSER=firefox
```
