Feature: User Management
  As an Administrator to the System
  I want to be certain that I can Login into the system with
  my adminstrator credentials.
  Then, I want to make sure that Users can register an account.
  After that, I need to verify that the previously registered account
  exists in the database.
  Now, I want to make sure that I can Edit the user.
  And that I can Delete the user.

Scenario: 01: Login with Administrator Credentials
    Given I have the following admin credentials:
        | Email                  | Password |
        | admin@documentally.com | admin    |
    When I send a POST request to "/authentication/login"
    Then the response status code should be 200
    And the response should contain the following authenticated user data:
        | Id | FirstName | LastName | Email                  | Token |
        |    | Admin     | Admin    | admin@documentally.com |       |

Scenario: 02: Register a new User Account
    Given I have entered the following registration details:
        | FirstName | LastName | Email                    | Password    |
        | John      | Doe      | johndoe@documentally.com | password123 |
    When I send a POST request to "/authentication/register"
    Then the response status code should be 200
    And the register response should contain the following data and I should save the newly registered user info into the context:
        | Id | FirstName | LastName | Email                    | Token |
        |    | John      | Doe      | johndoe@documentally.com |       |

Scenario: 03: Fetch newly registered user By Id
    Given I have stored the newly registered user in the Context Scenario
    When I send a GET request to "/users/{id}"
    Then the response status code should be 200
    And the response should contain the following user data:
        | Id | FirstName | LastName | Email                    | Password | Role | CreatedAtUtc |
        |    | John      | Doe      | johndoe@documentally.com |          | 0    |              |

#Scenario: Update User
#    Given I have the following user details:
#        | Id  | FirstName | LastName | Email                    |
#        | > 0 | John      | Doe      | johndoe@documentally.com |
#    When I send a PUT request to "/users/{id}"
#    Then the response status code should be 200

#Scenario: Delete User
#    Given I have a valid user ID
#    When I send a DELETE request to "/users/{id}"
#    Then the response status code should be 200