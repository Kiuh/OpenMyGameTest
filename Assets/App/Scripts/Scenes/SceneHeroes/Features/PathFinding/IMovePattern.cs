using System.Collections.Generic;
using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding
{
    public interface IMovePattern
    {
        public IEnumerable<Vector2Int> CreateSequence(
            Grid<Obstacle> grid,
            Vector2Int position,
            Dictionary<Obstacle, StayType> passInfo
        );
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
                if (grid.IsValid(position) && passInfo[grid[position]] == StayType.CanStayAndPass)
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
            while (grid.IsValid(position) && passInfo[grid[position]] == StayType.CanStayAndPass)
            {
                nextLocations.Add(position);
                position += Direction;
            }
            return nextLocations;
        }
    }
}
