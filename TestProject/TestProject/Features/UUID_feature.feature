Feature: UUID_feature

Background: 
When I send request to "https://httpbin.org/uuid" with "GET" type and:
	| Headers                   |
	| accept: application/json  |
	| Authorization: Bearer ABC |

Scenario: Check UUID
Then I validate that response has "200" code
	And I validate that response has "uuid" returned

Scenario: OrderId match
Given I save uuid from response
When I send request to "https://httpbin.org/post" with "POST" type and:
	| Headers                   |
	| accept: application/json  |
	| OrderId: @uuid            |
Then I validate that response has "200" code
	And I validate that response has "uuid" returned
