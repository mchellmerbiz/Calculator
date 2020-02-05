Feature: StringCalculator
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given A string expression "1+1"
	When I parse the string into calculator tokens
	And I parse the calculator tokens into reverse polish notation
	And I evaluate the reverse polish notation tokens
	Then the result should be "2"
