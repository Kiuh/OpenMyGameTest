using System;
using System.Collections.Generic;
using System.IO;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.ProviderResource;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider
{
    public class FigureProviderFiles : IFigureProvider
    {
        private readonly Config _config;
        private readonly IFigureParser _parser;
        private readonly IProviderResource _providerResource;

        public int TotalFiguresCount => _config.fileKeys.Count;

        public FigureProviderFiles(
            Config config,
            IFigureParser parser,
            IProviderResource providerResource
        )
        {
            _config = config;
            _parser = parser;
            _providerResource = providerResource;
        }

        public Grid<bool> GetFigure(int index)
        {
            string fileKey = _config.fileKeys[index];
            string filePath = Path.Combine(_config.pathLevels, fileKey);

            string textAsset = _providerResource.LoadTextResource(filePath);
            return _parser.ParseFile(textAsset);
        }

        [Serializable]
        public class Config
        {
            public List<string> fileKeys;
            public string pathLevels;
        }
    }
}
