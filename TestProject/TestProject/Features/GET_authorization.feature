Feature: GET_authorization

Scenario: Check response

When I send request to "https://httpbin.org/bearer" with "GET" type and:
	| Headers                  |
	| accept: application/json |
	| Authorization: Bearer A  |
Then I validate that response has "200" code
	And I validate that response has "true" for "authenticated"
	And I validate that response has "ABC" for "token"
