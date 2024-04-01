using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding
{
    public class PositionedObstacle
    {
        public Vector2Int Position;
        public Obstacle Type;
    }

    public class Location
    {
        public Vector2Int Position;
        public int Depth;
        public int Score;
        public Location Parent;
        public int Full => Depth + Score;

        public void ComputeScore(Vector2Int pos, Vector2Int target)
        {
            Vector2Int delta = target - pos;
            Score = Math.Abs(delta.x) + Math.Abs(delta.y);
        }
    }

    public class ServiceUnitNavigator : IServiceUnitNavigator
    {
        public List<Vector2Int> FindPath(
            UnitType unitType,
            Vector2Int from,
            Vector2Int to,
            Grid<int> gridMatrix
        )
        {
            UnitMoveData unitMoveData = UnitMoveData.FromUnitType(unitType);
            List<PositionedObstacle> grid = new();

            for (int i = 0; i < gridMatrix.Width; i++)
            {
                for (int j = 0; j < gridMatrix.Height; j++)
                {
                    grid.Add(
                        new PositionedObstacle()
                        {
                            Type = (Obstacle)gridMatrix[j, i],
                            Position = new(i, j)
                        }
                    );
                }
            }

            return CreatePath(unitMoveData, grid, from, to);
        }

        public List<Vector2Int> CreatePath(
            UnitMoveData unitMoveData,
            List<PositionedObstacle> grid,
            Vector2Int inStart,
            Vector2Int inFinish
        )
        {
            Location current = null;
            Location start = new() { Position = inStart };
            Location target = new() { Position = inFinish };
            List<Location> toProcess = new();
            List<Location> processed = new();
            int depth = 0;

            toProcess.Add(start);

            while (toProcess.Count > 0)
            {
                int lowest = toProcess.Min(l => l.Full);
                current = toProcess.First(l => l.Full == lowest);
                processed.Add(current);
                _ = toProcess.Remove(current);

                if (processed.FirstOrDefault(l => l.Position == target.Position) != null)
                {
                    Location iterator = processed.FirstOrDefault(l =>
                        l.Position == target.Position
                    );
                    List<Vector2Int> result = new();
                    while (iterator.Parent != null)
                    {
                        result.Add(iterator.Position);
                        iterator = iterator.Parent;
                    }
                    result.Reverse();
                    return result;
                }

                List<Location> availableLocations = unitMoveData
                    .GetAvailablePositions(current.Position, grid)
                    .Select(x => new Location() { Position = x })
                    .ToList();
                depth++;

                foreach (Location nextLocation in availableLocations)
                {
                    if (processed.FirstOrDefault(l => l.Position == nextLocation.Position) != null)
                    {
                        continue;
                    }

                    if (toProcess.FirstOrDefault(l => l.Position == nextLocation.Position) == null)
                    {
                        nextLocation.Depth = depth;
                        nextLocation.ComputeScore(nextLocation.Position, target.Position);
                        nextLocation.Parent = current;
                        toProcess.Insert(0, nextLocation);
                    }
                    else
                    {
                        if (depth + nextLocation.Score < nextLocation.Full)
                        {
                            nextLocation.Depth = depth;
                            nextLocation.Parent = current;
                        }
                    }
                }
            }

            return null;
        }
    }
}
