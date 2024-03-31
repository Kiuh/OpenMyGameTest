using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services;
using NUnit.Framework;
using Tests.Assets.App.Scripts.Tests.SceneMatrix;

namespace Tests.SceneMatrix
{
    public class TestRotator
    {
        private IFigureParser _figureParser;
        private IFigureRotator _figureRotator;

        [SetUp]
        public void SetUp()
        {
            _figureParser = new ParserFigureDummy();
            _figureRotator = new FigureRotatorDummy();
        }

        [Test]
        [TestCase("g_block", "g_block_-1_expected", -1)]
        [TestCase("g_block", "g_block_1_expected", 1)]
        [TestCase("g_block", "g_block_2_expected", 2)]
        [TestCase("g_block", "g_block_3_expected", 3)]
        [TestCase("tri_block", "tri_block_-2_expected", -2)]
        [TestCase("tri_block", "tri_block_1_expected", 1)]
        [TestCase("tri_block", "tri_block", 4)]
        [TestCase("tri_block", "tri_block_1_expected", 5)]
        public void TestFigures(string fileKey, string expectedFileKey, int rotationCount)
        {
            ProcessFileTest(fileKey, expectedFileKey, rotationCount);
        }

        [Test]
        [TestCase("tri_block", "tri_block_1_expected", 1)]
        public void TestConcrete(string fileKey, string expectedFileKey, int rotationCount)
        {
            ProcessFileTest(fileKey, expectedFileKey, rotationCount);
        }

        private void ProcessFileTest(string fileKey, string expectedFileKey, int rotationCount)
        {
            string inputMatrixText = TestTools.LoadMatrixFromKey(fileKey);
            Grid<bool> inputMatrix = _figureParser.ParseFile(inputMatrixText);

            string expectedMatrixText = TestTools.LoadMatrixFromKey(expectedFileKey);
            Grid<bool> expectedMatrix = _figureParser.ParseFile(expectedMatrixText);

            Grid<bool> resultMatrix = _figureRotator.RotateFigure(inputMatrix, rotationCount);

            Assert.AreEqual(expectedMatrix, resultMatrix);
        }
    }
}
