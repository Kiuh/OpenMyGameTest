using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{
    public class ParserFigureDummy : IFigureParser
    {
        public Grid<bool> ParseFile(string text)
        {
            //реализовать верный алгоритм парсинга фигуры
            var grid = new Grid<bool>(new Vector2Int(2, 2));

            grid[0, 0] = true;
            grid[1, 0] = true;

            return grid;
        }
    }
}