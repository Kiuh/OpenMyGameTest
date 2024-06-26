using App.Scripts.Scenes.SceneCalculator.Features.Calculator;
using NUnit.Framework;
using UnityEngine;
using File = System.IO.File;

namespace Tests.SceneCalculator
{
    public class TestCalculator
    {
        private const string PathTestCase =
            "Assets/App/Scripts/Tests/SceneCalculator/TestCases/{0}.json";

        [Test]
        [TestCase("3 +5", 8)]
        [TestCase("-3 +5", 2)]
        [TestCase("2 - (3 +5) ", -6)]
        [TestCase("8 *2 - (3 +5)  ", 8)]
        public void TestCalculatorOneLineExpression(string expression, int expected)
        {
            CalculatorExpression calculator = new();

            int result = calculator.Execute(expression);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("scenario_0")]
        public void TestParamsCalculator(string testFileKey)
        {
            string testPath = string.Format(PathTestCase, testFileKey);

            string testText = File.ReadAllText(testPath);
            TestExpression testData = JsonUtility.FromJson<TestExpression>(testText);

            CalculatorExpression calculator = new();

            foreach (TestParamExpression expression in testData.expressions)
            {
                calculator.SetExpression(expression.key, expression.value);
            }

            foreach (TestExpressionCase expressionCase in testData.expected)
            {
                int result = calculator.Get(expressionCase.key);
                Assert.AreEqual(expressionCase.result, result);
            }
        }
    }
}
