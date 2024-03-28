using System.IO;

namespace Tests.Assets.App.Scripts.Tests.SceneMatrix
{
    public static class TestTools
    {
        private const string PathTests = "Assets/App/Scripts/Tests/SceneMatrix/TestCases";
        private const string TestDataFile = PathTests + "/{0}.txt";

        public static string LoadMatrixFromKey(string key)
        {
            string pathTest = string.Format(TestDataFile, key);
            return File.ReadAllText(pathTest);
        }
    }
}
