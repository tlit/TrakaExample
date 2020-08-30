Feature: UserOperations
	In order to administer Users of the system
	I wish to perform operations on the User List

@smoketest
Scenario Outline: Return pages of the User List
	When I request page <pageNumber> of the User List
	Then the response code is 200
	And the response page is <pageNumber>
	And the response includes <perPage> items per page
	And the response indicates <totalItems> items in total
	And the response indicates <totalPages> pages in total
	And the response data includes <itemCount> Users
Examples: 
| pageNumber | perPage | totalItems | totalPages | itemCount |
| 1          | 6       | 12         | 2          | 6         |
| 2          | 6       | 12         | 2          | 6         |
| 200        | 6       | 12         | 2          | 0         |

@smoketest
Scenario Outline: Return a single User
	When I request details for User ID <UserId>
	Then the response code is 200
	And the response contains a single User
	And the User's first name is <firstName>
	And the User's last name is <lastName>
	And the User's email address is <email>
	And the User's avatar is a valid URI
Examples:
| UserId | firstName | lastName | email                  |
| 1      | George    | Bluth    | george.bluth@reqres.in |
| 4      | Eve       | Holt     | eve.holt@reqres.in     |
| 9      | Tobias    | Funke    | tobias.funke@reqres.in |

@smoketest
Scenario: Attempt to return an invalid User
	When I request details for User ID 23
	Then the response code is 404
	
@smoketest
Scenario Outline: Add a User
	When I add the User '<firstName> <lastName>'
	Then the response code is 201
	And the User's first name is <firstName>
	And the User's last name is <lastName>
Examples:
| UserId	| firstName | lastName |
| 55		| Leann     | Hope     |
| 28		| Anita     | Lemon    |
| 12		| Katja     | Fish     |
