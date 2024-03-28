using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{
    public class ParserFigureDummy : IFigureParser
    {
        public Grid<bool> ParseFile(string text)
        {
            List<int> parsedValues = ParsePlainText(text);
            Vector2Int size = new() { x = parsedValues[0], y = parsedValues[1] };
            if (size.x <= 0 || size.y <= 0)
            {
                throw new ExceptionParseFigure("Incorrect figure matrix size.");
            }
            Grid<bool> grid = new(size);
            foreach (int index in parsedValues.Skip(2))
            {
                if (index < 0 || index >= grid.Size.x * grid.Size.y)
                {
                    throw new ExceptionParseFigure("Cell index out of range.");
                }
                SetValueToGrid(grid, index, true);
            }
            return grid;
        }

        private List<int> ParsePlainText(string text)
        {
            string[] sliced = text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            if (sliced.Length != 3)
            {
                throw new ExceptionParseFigure("Incorrect number of lines in file.");
            }

            List<int> parsedValues = new() { ParseInt(sliced[0]), ParseInt(sliced[1]) };

            string[] positions = sliced[2].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string position in positions)
            {
                parsedValues.Add(ParseInt(position));
            }

            return parsedValues;
        }

        private int ParseInt(string text)
        {
            if (!int.TryParse(text, out int parsed))
            {
                throw new ExceptionParseFigure("Parsing data have incorrect type.");
            }
            return parsed;
        }

        private void SetValueToGrid(Grid<bool> grid, int index, bool value)
        {
            int x = index % grid.Size.x;
            int y = index / grid.Size.x;
            grid[x, y] = value;
        }
    }
}
