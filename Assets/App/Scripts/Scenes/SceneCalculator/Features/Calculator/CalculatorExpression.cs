using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class CalculatorExpression : ICalculatorExpression
    {
        public class Expression
        {
            public static Expression FromString(string rawExpression)
            {
                return new Expression();
            }

            public string Key;
            public int? Value;
            public string RawExpression;
            public List<Expression> Dependencies;
        }

        private Dictionary<string, Expression> _expressions;

        public int Execute(string expression)
        {
            ValidateExpression(expression);
            return ExecuteExpression(expression);
        }

        public void SetExpression(string expressionKey, string rawExpression)
        {
            ValidateExpressionKey(expressionKey);
            Expression expression = Expression.FromString(rawExpression);
            _expressions.Add(expressionKey, expression);
            CalculateCache();
        }

        public int Get(string expressionKey)
        {
            if (!_expressions.TryGetValue(expressionKey, out Expression expression))
            {
                throw new ExceptionExecuteExpression("Given expression key not found.");
            }
            if (!expression.Value.HasValue)
            {
                throw new ExceptionExecuteExpression("Not enough data for expression executing.");
            }
            return expression.Value.Value;
        }

        private void ValidateExpressionKey(string key)
        {
            if (!Regex.IsMatch(key, @"^[a-zA-Z]+$"))
            {
                throw new ExceptionExecuteExpression("Expression key contains invalid symbols.");
            }
        }

        private void ValidateExpressionSelfCycle(string expression, string key)
        {
            if (expression.Contains(key))
            {
                throw new ExceptionExecuteExpression("Expression contains own key.");
            }
        }

        private void ValidateExpression(string expression)
        {
            if (!Regex.IsMatch(expression, @"^[a-zA-Z0-9+\-*/()]+$"))
            {
                throw new ExceptionExecuteExpression("Expression contains invalid symbols.");
            }
            //if (expression.Count(x => x == '(') == expression.Count(x => x == ')')) { }
        }

        private int ExecuteExpression(string expression)
        {
            return 0;
        }

        private void CalculateCache() { }
    }
}
