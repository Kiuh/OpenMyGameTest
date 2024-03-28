using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services
{
    public class FigureRotatorDummy : IFigureRotator
    {
        public Grid<bool> RotateFigure(Grid<bool> grid, int rotateCount)
        {
            int rotations = ((rotateCount % 4) + 4) % 4;
            Dictionary<Vector2Int, bool> rotatedMatrix = new();

            // Create rotated matrix
            foreach (var index in IterateMatrix(Vector2Int.zero, grid.Size))
            {
                var rotatedIndex = RotateVector2Int(index, rotations);
                rotatedMatrix.Add(rotatedIndex, grid[index]);
            }

            // Create new grid
            var newSize = RotateVector2Int(grid.Size, rotations);
            newSize = AbsVector2Int(newSize);
            var rotated = new Grid<bool>(newSize);

            // Fill new grid
            var orderedIndexes = rotatedMatrix.Keys.OrderBy(v => v.x + v.y);
            var shift = AbsVector2Int(orderedIndexes.First());
            foreach (var index in orderedIndexes)
            {
                rotated[index + shift] = rotatedMatrix[index];
            }

            return rotated;
        }

        private IEnumerable<Vector2Int> IterateMatrix(Vector2Int start, Vector2Int end)
        {
            for (int i = start.x; i < end.x; i++)
            {
                for (int j = start.y; j < end.y; j++)
                {
                    yield return new Vector2Int(i, j);
                }
            }
        }

        private Vector2Int RotateVector2Int(Vector2Int vector, int count)
        {
            return (count) switch
            {
                0 => vector,
                1 => new Vector2Int(vector.y, -vector.x),
                2 => new Vector2Int(-vector.x, -vector.y),
                3 => new Vector2Int(-vector.y, vector.x),
                _ => throw new System.ArgumentException()
            };
        }

        private Vector2Int AbsVector2Int(Vector2Int vector)
        {
            return Vector2Int.Max(vector, vector * -1);
        }
    }
}
