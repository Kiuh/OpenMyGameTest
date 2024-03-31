using System.Collections.Generic;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class CalculatorExpression : ICalculatorExpression
    {
        private Dictionary<Key, Expression> _expressions = new();

        public int Execute(string expression)
        {
            Expression expressionToExecute = new(expression);
            return expressionToExecute.Evaluate(_expressions);
        }

        public void SetExpression(string expressionKey, string rawExpression)
        {
            Key key = new(expressionKey);
            Expression expression = new(rawExpression);
            if (expression.IsNeedKey(key))
            {
                throw new ExceptionExecuteExpression("Expression contains own key.");
            }
            _expressions.Add(key, expression);
        }

        public int Get(string expressionKey)
        {
            Key key = new(expressionKey);
            if (!_expressions.TryGetValue(key, out Expression expression))
            {
                throw new ExceptionExecuteExpression("Given expression key not found.");
            }
            return expression.Evaluate(_expressions);
        }
    }
}
