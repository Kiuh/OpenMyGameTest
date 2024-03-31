using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using NUnit.Framework;
using Tests.Assets.App.Scripts.Tests.SceneMatrix;

namespace Tests.SceneMatrix
{
    public class TestParser
    {
        private IFigureParser _figureParser;

        [SetUp]
        public void SetUp()
        {
            _figureParser = new ParserFigureDummy();
        }

        [Test]
        public void TestDefaultFiguresParsing()
        {
            Grid<bool> g_block = new(new(3, 3));
            g_block[1, 0] = true;
            g_block[1, 1] = true;
            g_block[1, 2] = true;
            g_block[0, 2] = true;

            Grid<bool> tri_block = new(new(3, 2));
            tri_block[0, 0] = true;
            tri_block[1, 0] = true;
            tri_block[2, 0] = true;
            tri_block[1, 1] = true;

            TestFigureParsing("g_block", g_block);
            TestFigureParsing("tri_block", tri_block);
        }

        private void TestFigureParsing(string key, Grid<bool> expected)
        {
            string text = TestTools.LoadMatrixFromKey(key);

            Grid<bool> grid = _figureParser.ParseFile(text);

            Assert.AreEqual(expected, grid);
        }

        [Test]
        public void TestFileLength()
        {
            string errorMessage = "Incorrect number of lines in file.";
            string text1 = TestTools.LoadMatrixFromKey("too_short_block");
            string text2 = TestTools.LoadMatrixFromKey("too_long_block");

            ExceptionParseFigure ex1 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text1)
            );
            ExceptionParseFigure ex2 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text2)
            );

            Assert.AreEqual(errorMessage, ex1.Message);
            Assert.AreEqual(errorMessage, ex2.Message);
        }

        [Test]
        public void TestIncorrectDataTypes()
        {
            string errorMessage = "Parsing data have incorrect type.";
            string text1 = TestTools.LoadMatrixFromKey("incorrect_size_type_block");
            string text2 = TestTools.LoadMatrixFromKey("incorrect_index_type_block");

            ExceptionParseFigure ex1 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text1)
            );
            ExceptionParseFigure ex2 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text2)
            );

            Assert.AreEqual(errorMessage, ex1.Message);
            Assert.AreEqual(errorMessage, ex2.Message);
        }

        [Test]
        public void TestIncorrectSize()
        {
            string errorMessage = "Incorrect figure matrix size.";
            string text1 = TestTools.LoadMatrixFromKey("incorrect_size_block_1");
            string text2 = TestTools.LoadMatrixFromKey("incorrect_size_block_2");

            ExceptionParseFigure ex1 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text1)
            );
            ExceptionParseFigure ex2 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text2)
            );

            Assert.AreEqual(errorMessage, ex1.Message);
            Assert.AreEqual(errorMessage, ex2.Message);
        }

        [Test]
        public void TestIncorrectIndexes()
        {
            string errorMessage = "Cell index out of range.";
            string text1 = TestTools.LoadMatrixFromKey("incorrect_index_block_1");
            string text2 = TestTools.LoadMatrixFromKey("incorrect_index_block_2");

            ExceptionParseFigure ex1 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text1)
            );
            ExceptionParseFigure ex2 = Assert.Throws<ExceptionParseFigure>(
                () => _figureParser.ParseFile(text2)
            );

            Assert.AreEqual(errorMessage, ex1.Message);
            Assert.AreEqual(errorMessage, ex2.Message);
        }
    }
}
