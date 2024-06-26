using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Scripts.Modules.Grid;
using App.Scripts.Modules.Serializer;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Serializable;
using App.Scripts.Scenes.SceneHeroes.Features.PathFinding;
using NUnit.Framework;
using UnityEngine;

namespace Tests.SceneHeroes
{
    public class TestPathFinding
    {
        private const string PathTest = "Assets/App/Scripts/Tests/SceneHeroes/TestCases/{0}.json";

        [Test]
        [TestCase("test_field_path(0)")]
        [TestCase("test_field_path(1)")]
        [TestCase("test_field_path(2)")]
        [TestCase("test_field_path(3)")]
        [TestCase("test_field_path(4)")]
        [TestCase("test_field_path(5)")]
        [TestCase("test_field_path(6)")]
        [TestCase("test_field_path(7)")]
        [TestCase("test_field_path(8)")]
        [TestCase("test_field_path(9)")]
        public void TestPathFindingSimplePasses(string testData)
        {
            ServiceUnitNavigator serviceUnitNavigator = new();

            string testCaseText = File.ReadAllText(string.Format(PathTest, testData));

            JsonConverter serializer = new();

            LevelInfoTarget testCase = serializer.Deserialize<LevelInfoTarget>(testCaseText);

            Grid<int> grid = new(testCase.gridSize.ToVector2Int());

            foreach (ObstacleSerializable obstacle in testCase.Obstacles)
            {
                grid[obstacle.Place.ToVector2Int()] = obstacle.ObstacleType;
            }

            List<Vector2Int> path = serviceUnitNavigator.FindPath(
                testCase.UnitType,
                testCase.PlaceUnit.ToVector2Int(),
                testCase.target.ToVector2Int(),
                grid
            );

            Debug.Log(
                $"{testCase.UnitType}\n"
                    + $"{grid}\n{testCase.PlaceUnit.ToVector2Int()}"
                    + $"\n{testCase.target.ToVector2Int()}"
            );

            if (testCase.targetStepCount < 0 && path is null)
            {
                return;
            }

            Debug.Log($"\n Path: {path.Select(x => x.ToString()).Aggregate((x, y) => $"{x} {y}")}");

            Assert.AreEqual(testCase.targetStepCount, path.Count, $"Step count invalid.");
        }
    }
}
