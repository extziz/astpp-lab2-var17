Feature: Open Account (Bank Manager)
  As a bank manager of the XYZ Bank
  I want to open a new account for an existing customer in a chosen currency
  So that the customer can start using the new account

  Background:
    Given the XYZ Bank home page is opened
    And I log in as Bank Manager
    And I open the "Open Account" tab

  @smoke
  Scenario Outline: Manager opens a new account for a customer
    When I select the customer "<customer>"
    And I select the currency "<currency>"
    And I press the Process button
    Then the alert "Account created successfully with account Number :" is shown
    And the "Customers" tab shows 4 accounts for "<customer>" including the new one

    Examples:
      | customer           | currency |
      | Hermoine Granger   | Dollar   |
      | Harry Potter       | Pound    |
      | Ron Weasly         | Rupee    |
      | Albus Dumbledore   | Dollar   |
      | Neville Longbottom | Pound    |

  Scenario: New account does not appear for other customers
    When I select the customer "Harry Potter"
    And I select the currency "Dollar"
    And I press the Process button
    Then the alert "Account created successfully with account Number :" is shown
    And the "Customers" tab shows 3 accounts for "Ron Weasly"

  Scenario: Consecutive accounts get consecutive numbers
    When I open an account for "Harry Potter" in "Dollar"
    And I open an account for "Harry Potter" in "Rupee"
    Then the second account number is greater than the first by 1
    And the "Customers" tab shows 5 accounts for "Harry Potter" including the new one

  Scenario: Form is cleared after the account is opened
    When I select the customer "Ron Weasly"
    And I select the currency "Pound"
    And I press the Process button
    Then the alert "Account created successfully with account Number :" is shown
    And the customer and currency fields are reset to their placeholders

  Scenario: Account is not opened when nothing is selected
    When I press the Process button
    Then no account is created and no alert is shown
    And the browser marks the "Customer" field as required

  Scenario: Account is not opened when the currency is not selected
    When I select the customer "Harry Potter"
    And I press the Process button
    Then no account is created and no alert is shown
    And the browser marks the "Currency" field as required
    And the "Customers" tab shows 3 accounts for "Harry Potter"
