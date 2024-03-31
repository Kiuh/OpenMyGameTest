using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class Expression
    {
        private const char CONST_VAR_TOKEN = '%';
        private const char DYNAMIC_VAR_TOKEN = '?';
        private const char NEGATION_TOKEN = '#';

        private string postfixExpression;
        private List<Key> neededVariables;
        private List<int> existedConstants;

        private static Dictionary<char, int> operatorsPriority =
            new()
            {
                { NEGATION_TOKEN, 4 },
                { '*', 3 },
                { '/', 3 },
                { '+', 2 },
                { '-', 2 },
                { '(', 1 },
                { DYNAMIC_VAR_TOKEN, 0 },
                { CONST_VAR_TOKEN, 0 },
            };

        public Expression(string rawExpression)
        {
            if (!Regex.IsMatch(rawExpression, @"^[a-zA-Z0-9+\-*\/() ]+$"))
            {
                throw new ExceptionExecuteExpression("Expression contains invalid symbols.");
            }
            rawExpression = ExtractVariables(rawExpression);
            rawExpression = CorrectExpression(rawExpression);
            postfixExpression = CreatePostfixExpression(rawExpression);
        }

        public int Evaluate(Dictionary<Key, Expression> availableExpressions)
        {
            if (
                availableExpressions.Keys.Intersect(neededVariables).Count()
                != neededVariables.Count
            )
            {
                throw new ExceptionExecuteExpression("Not enough data to calculate expression.");
            }
            return EvaluatePostfix(availableExpressions);
        }

        private string CorrectExpression(string expression)
        {
            expression = Regex.Replace(expression, "--", "+");
            return expression;
        }

        private int EvaluatePostfix(Dictionary<Key, Expression> availableExpressions)
        {
            Stack<int> operands = new();
            IEnumerator<int> constVarIterator = existedConstants.GetEnumerator();
            IEnumerator<Key> dynamicVarIterator = neededVariables.GetEnumerator();

            foreach (char token in postfixExpression)
            {
                if (token == CONST_VAR_TOKEN)
                {
                    _ = constVarIterator.MoveNext();
                    operands.Push(constVarIterator.Current);
                }
                else if (token == DYNAMIC_VAR_TOKEN)
                {
                    _ = dynamicVarIterator.MoveNext();
                    int operand = availableExpressions[dynamicVarIterator.Current]
                        .Evaluate(availableExpressions);
                    operands.Push(operand);
                }
                else if (token == NEGATION_TOKEN)
                {
                    int operand = operands.Pop();
                    operands.Push(-operand);
                }
                else
                {
                    int operand2 = operands.Pop();
                    int operand1 = operands.Pop();
                    int result = ExecuteOperand(token, operand1, operand2);
                    operands.Push(result);
                }
            }
            return operands.Pop();
        }

        private int ExecuteOperand(char token, int operand1, int operand2)
        {
            return token switch
            {
                '*' => operand1 * operand2,
                '/' => operand1 / operand2,
                '+' => operand1 + operand2,
                '-' => operand1 - operand2,
                _ => throw new ExceptionExecuteExpression("Invalid operand")
            };
        }

        private string ExtractVariables(string rawExpression)
        {
            rawExpression = Regex.Replace(rawExpression, @"\s+", "");
            neededVariables = Regex
                .Matches(rawExpression, @"[a-zA-Z]+")
                .Cast<Match>()
                .Select(x => new Key(x.Value))
                .ToList();
            existedConstants = Regex
                .Matches(rawExpression, @"[0-9]+")
                .Cast<Match>()
                .Select(x => int.Parse(x.Value))
                .ToList();
            rawExpression = Regex.Replace(rawExpression, @"[0-9]+", CONST_VAR_TOKEN.ToString());
            rawExpression = Regex.Replace(
                rawExpression,
                @"[a-zA-Z]+",
                DYNAMIC_VAR_TOKEN.ToString()
            );
            return rawExpression;
        }

        private string CreatePostfixExpression(string infix)
        {
            Stack<char> stack = new();
            string result = "";
            foreach (char token in infix)
            {
                switch (token)
                {
                    case CONST_VAR_TOKEN
                    or DYNAMIC_VAR_TOKEN:
                        result += token;
                        break;
                    case '(':
                        stack.Push(token);
                        break;
                    case ')':
                        while (stack.Count > 0 && stack.Peek() != '(')
                        {
                            result += stack.Pop();
                        }
                        _ = stack.Pop();
                        break;
                    case '-' when result == "":
                        stack.Push(NEGATION_TOKEN);
                        break;
                    case '-' when operatorsPriority[result.Last()] >= operatorsPriority['-']:
                        stack.Push(NEGATION_TOKEN);
                        break;
                    default:
                        while (stack.Count > 0)
                        {
                            if (operatorsPriority[token] >= operatorsPriority[stack.Peek()])
                            {
                                break;
                            }
                            result += stack.Pop();
                        }
                        stack.Push(token);
                        break;
                }
            }

            while (stack.Count != 0)
            {
                result += stack.Pop();
            }

            return string.Join("", result);
        }

        public bool IsNeedKey(Key valueKey)
        {
            return neededVariables.Contains(valueKey);
        }
    }
}
