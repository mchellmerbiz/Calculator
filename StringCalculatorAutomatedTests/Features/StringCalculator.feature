Feature: StringCalculator

Scenario Outline: Reverse Polish Calculator
	Given A string expression "<inputString>"
	When I parse the string into calculator tokens
	And I parse the calculator tokens into reverse polish notation
	And I evaluate the reverse polish notation tokens
	Then the result should be "<expectedEvaluation>" to "4" decimal places
Examples:
| inputString           | expectedEvaluation |
| 1+1                   | 2                  |
| 1*2+3                 | 5                  |
| 1-2/3                 | 0.3333             |
| 1.5 + (10-5) + 3      | 9.5                |
| sin(2)                | 0.9093             |
| sin(2-0)              | 0.9093             |
| cos(2)                | -0.4161            |
| 5*3-cos(2)            | 15.4162            |
| sin(2) * (5*3-cos(2)) | 14.0179            |

Scenario Outline: Reverse Polish Calculator With Variables
	Given A string expression "<inputString>"
	When I parse the string into calculator tokens
	And I parse the calculator tokens into reverse polish notation
	And I evaluate the reverse polish notation tokens with variables as follows:
		| varAlias | varValue |
		| x        | 1        |
		| y        | 2        |
		| z        | 3        |
	Then the result should be "<expectedEvaluation>" to "4" decimal places
Examples:
| inputString           | expectedEvaluation |
| x+y                   | 3                  |
| x+y*z                 | 7                  |
| x+y*z+4               | 11                 |
| sin(x)+cos(y*z)*4     | 4.6822             |
| (sin(x)+cos(y*z)*4)/1 | 4.6822             |
| (x+1)*(x+1)           | 4                  |

Scenario Outline: Reverse Polish Calculator with Optimisation
	Given A string expression "<inputString>"
	When I parse the string into calculator tokens
	And I parse the calculator tokens into reverse polish notation
	And I optimise the reverse polish notation tokens
	And I evaluate the reverse polish notation tokens
	Then the result should be "<expectedEvaluation>" to "4" decimal places
Examples:
| inputString | expectedEvaluation |
| 1*1*1*1     | 1                  |

