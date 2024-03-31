using System.Collections.Generic;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding
{
    public enum Obstacle
    {
        None = 0,
        Stone = 1,
        Water = 2,
    }

    public enum StayType
    {
        CannotPass,
        CanStayAndPass,
    }

    public class ConstantPattern : IMovePattern
    {
        public List<Vector2Int> Moves;

        public IEnumerable<Vector2Int> CreateSequence(
            Grid<Obstacle> grid,
            Vector2Int position,
            Dictionary<Obstacle, StayType> passInfo
        )
        {
            List<Vector2Int> nextLocations = new();
            foreach (Vector2Int move in Moves)
            {
                position += move;
                if (!grid.IsValid(position))
                {
                    continue;
                }
                Obstacle obstacle = grid[position];
                if (passInfo[obstacle] == StayType.CanStayAndPass)
                {
                    nextLocations.Add(position);
                }
                else
                {
                    break;
                }
            }
            return nextLocations;
        }
    }

    public class InfinityMovePattern : IMovePattern
    {
        public Vector2Int Direction;

        public IEnumerable<Vector2Int> CreateSequence(
            Grid<Obstacle> grid,
            Vector2Int position,
            Dictionary<Obstacle, StayType> passInfo
        )
        {
            List<Vector2Int> nextLocations = new();
            position += Direction;
            while (grid.IsValid(position))
            {
                Obstacle obstacle = grid[position];
                if (passInfo[obstacle] == StayType.CanStayAndPass)
                {
                    nextLocations.Add(position);
                }
                else
                {
                    break;
                }
                position += Direction;
            }
            return nextLocations;
        }
    }

    public class UnitMoveData
    {
        public List<IMovePattern> MovePatterns;
        public Dictionary<Obstacle, StayType> PassInfo;

        public List<Vector2Int> GetAvailablePositions(Vector2Int pos, Grid<Obstacle> grid)
        {
            List<Vector2Int> nextLocations = new();
            foreach (IMovePattern pattern in MovePatterns)
            {
                nextLocations.AddRange(pattern.CreateSequence(grid, pos, PassInfo));
            }
            return nextLocations;
        }

        public static UnitMoveData FromUnitType(UnitType type)
        {
            return type switch
            {
                UnitType.SwordMan
                    => new UnitMoveData()
                    {
                        MovePatterns = new()
                        {
                            new ConstantPattern() { Moves = new() { Vector2Int.up } },
                            new ConstantPattern() { Moves = new() { Vector2Int.down } },
                            new ConstantPattern() { Moves = new() { Vector2Int.left } },
                            new ConstantPattern() { Moves = new() { Vector2Int.right } },
                        },
                        PassInfo = new()
                        {
                            { Obstacle.None, StayType.CanStayAndPass },
                            { Obstacle.Stone, StayType.CannotPass },
                            { Obstacle.Water, StayType.CannotPass },
                        }
                    },
                UnitType.HorseMan
                    => new UnitMoveData()
                    {
                        MovePatterns = new()
                        {
                            new InfinityMovePattern() { Direction = new(1, 1) },
                            new InfinityMovePattern() { Direction = new(-1, 1) },
                            new InfinityMovePattern() { Direction = new(1, -1) },
                            new InfinityMovePattern() { Direction = new(-1, -1) },
                        },
                        PassInfo = new()
                        {
                            { Obstacle.None, StayType.CanStayAndPass },
                            { Obstacle.Stone, StayType.CannotPass },
                            { Obstacle.Water, StayType.CannotPass },
                        }
                    },
                UnitType.Angel
                    => new UnitMoveData()
                    {
                        MovePatterns = new()
                        {
                            new InfinityMovePattern() { Direction = new(0, 1) },
                            new InfinityMovePattern() { Direction = new(0, -1) },
                            new InfinityMovePattern() { Direction = new(1, 0) },
                            new InfinityMovePattern() { Direction = new(-1, 0) },
                        },
                        PassInfo = new()
                        {
                            { Obstacle.None, StayType.CanStayAndPass },
                            { Obstacle.Stone, StayType.CannotPass },
                            { Obstacle.Water, StayType.CanStayAndPass },
                        }
                    },
                UnitType.Poor
                    => new UnitMoveData()
                    {
                        MovePatterns = new()
                        {
                            new InfinityMovePattern() { Direction = new(0, 1) },
                            new InfinityMovePattern() { Direction = new(0, -1) },
                            new ConstantPattern() { Moves = new() { new(1, 1) } },
                            new ConstantPattern() { Moves = new() { new(-1, 1) } },
                            new ConstantPattern() { Moves = new() { new(-1, -1) } },
                            new ConstantPattern() { Moves = new() { new(1, -1) } },
                        },
                        PassInfo = new()
                        {
                            { Obstacle.None, StayType.CanStayAndPass },
                            { Obstacle.Stone, StayType.CannotPass },
                            { Obstacle.Water, StayType.CannotPass },
                        }
                    },
                UnitType.Shaman
                    => new UnitMoveData()
                    {
                        MovePatterns = new()
                        {
                            new ConstantPattern() { Moves = { new(2, 1) } },
                            new ConstantPattern() { Moves = { new(1, 2) } },
                            new ConstantPattern() { Moves = { new(-1, 2) } },
                            new ConstantPattern() { Moves = { new(-2, 1) } },
                            new ConstantPattern() { Moves = { new(2, -1) } },
                            new ConstantPattern() { Moves = { new(-1, -2) } },
                            new ConstantPattern() { Moves = { new(-2, -1) } },
                            new ConstantPattern() { Moves = { new(1, -2) } },
                        },
                        PassInfo = new()
                        {
                            { Obstacle.None, StayType.CanStayAndPass },
                            { Obstacle.Stone, StayType.CannotPass },
                            { Obstacle.Water, StayType.CannotPass },
                        }
                    },
                UnitType.Barbarian
                    => new UnitMoveData()
                    {
                        MovePatterns = new()
                        {
                            new InfinityMovePattern() { Direction = new(0, 1) },
                            new InfinityMovePattern() { Direction = new(0, -1) },
                            new InfinityMovePattern() { Direction = new(1, 0) },
                            new InfinityMovePattern() { Direction = new(-1, 0) },
                            new InfinityMovePattern() { Direction = new(1, 1) },
                            new InfinityMovePattern() { Direction = new(-1, 1) },
                            new InfinityMovePattern() { Direction = new(1, -1) },
                            new InfinityMovePattern() { Direction = new(-1, -1) },
                        },
                        PassInfo = new()
                        {
                            { Obstacle.None, StayType.CanStayAndPass },
                            { Obstacle.Stone, StayType.CannotPass },
                            { Obstacle.Water, StayType.CannotPass },
                        }
                    },
                _ => throw new System.Exception("Unsupported Unit Type.")
            };
        }
    }
}
