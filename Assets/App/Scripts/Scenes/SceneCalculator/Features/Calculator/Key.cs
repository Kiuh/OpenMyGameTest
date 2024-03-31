using System;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class Key
    {
        private string internalKey;

        public Key(string value)
        {
            Validate(value);
            internalKey = value;
        }

        private void Validate(string key)
        {
            if (!Regex.IsMatch(key, @"^[a-zA-Z]+$"))
            {
                throw new ExceptionExecuteExpression("Expression key contains invalid symbols.");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Key key && internalKey == key.internalKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(internalKey);
        }
    }
}
