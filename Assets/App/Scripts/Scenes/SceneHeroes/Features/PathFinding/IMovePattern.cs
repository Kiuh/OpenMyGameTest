using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding
{
    public interface IMovePattern
    {
        public IEnumerable<Vector2Int> CreateSequence(
            List<PositionedObstacle> grid,
            Vector2Int position,
            Dictionary<Obstacle, StayType> passInfo
        );
    }

    public class ConstantPattern : IMovePattern
    {
        public List<Vector2Int> Moves;

        public IEnumerable<Vector2Int> CreateSequence(
            List<PositionedObstacle> grid,
            Vector2Int position,
            Dictionary<Obstacle, StayType> passInfo
        )
        {
            List<Vector2Int> nextLocations = new();
            foreach (Vector2Int move in Moves)
            {
                position += move;
                PositionedObstacle obstacle = grid.FirstOrDefault(x => x.Position == position);
                if (obstacle == null || passInfo[obstacle.Type] != StayType.CanStayAndPass)
                {
                    break;
                }
                nextLocations.Add(position);
            }
            return nextLocations;
        }
    }

    public class InfinityMovePattern : IMovePattern
    {
        public Vector2Int Direction;

        public InfinityMovePattern(Vector2Int direction)
        {
            Direction = direction;
        }

        public IEnumerable<Vector2Int> CreateSequence(
            List<PositionedObstacle> grid,
            Vector2Int position,
            Dictionary<Obstacle, StayType> passInfo
        )
        {
            List<Vector2Int> nextLocations = new();
            position += Direction;
            PositionedObstacle nextObstacle = grid.FirstOrDefault(x => x.Position == position);
            while (nextObstacle != null && passInfo[nextObstacle.Type] == StayType.CanStayAndPass)
            {
                nextLocations.Add(position);
                position += Direction;
                nextObstacle = grid.FirstOrDefault(x => x.Position == position);
            }
            return nextLocations;
        }
    }
}
