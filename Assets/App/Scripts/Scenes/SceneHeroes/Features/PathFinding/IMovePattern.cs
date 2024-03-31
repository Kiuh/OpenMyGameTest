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
}
