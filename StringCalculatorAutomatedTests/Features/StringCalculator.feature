Feature: StringCalculator

Scenario Outline: Reverse Polish Calculator
	Given A string expression "<inputString>"
	When I parse the string into calculator tokens
	And I parse the calculator tokens into reverse polish notation
	And I evaluate the reverse polish notation tokens
	Then the result should be "<expectedEvaluation>"
Examples:
| inputString           | expectedEvaluation |
| 1+1                   | 2                  |
| 1*2+3                 | 5                  |
| 1-2/3                 | 0.3333             |
| 1.5 + (10-5) + 3      | 9.5                |
| sin(2)                | 0.9093             |
| cos(2)                | -0.4161            |
| 5*3-cos(2)            | 15.4162            |
| sin(2) * (5*3-cos(2)) | 14.0179            |

