Feature: UserLogin
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@smoketest @login
Scenario: Login success
	When I login using valid credentials
	Then the response code is 200
	And the response contains a token

@smoketest @login
Scenario: Login fails when password is not supplied
	When I login without a password
	Then the response code is 400
	And the response error is "Missing password"

@smoketest @login
Scenario: Login fails when user is not recognised
	When I login using an unrecognised user
	Then the response code is 400
	And the response error is "user not found"
