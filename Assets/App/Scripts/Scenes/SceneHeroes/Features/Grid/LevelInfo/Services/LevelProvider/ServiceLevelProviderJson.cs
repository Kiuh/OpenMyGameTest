using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Serializable;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Services.LevelProvider
{
    public class ServiceLevelProviderJson : IServiceLevelProvider
    {
        private readonly Config _config;

        public ServiceLevelProviderJson(Config config)
        {
            _config = config;
        }

        public ILevelInfo GetLevel(int index)
        {
            TextAsset levelData = _config.levels[index];

            try
            {
                LevelInfoTarget levelInfoTarget = JsonConvert.DeserializeObject<LevelInfoTarget>(
                    levelData.text
                );

                if (levelInfoTarget is null)
                {
                    return null;
                }

                UnitInfo unitInfo =
                    new(levelInfoTarget.UnitType, levelInfoTarget.PlaceUnit.ToVector2Int());

                return new LevelGridInfo(
                    levelInfoTarget.gridSize.ToVector2Int(),
                    levelInfoTarget.Obstacles,
                    unitInfo
                );
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public int LevelCount()
        {
            return _config.levels.Count;
        }

        [Serializable]
        public class Config
        {
            public List<TextAsset> levels;
        }
    }
}
