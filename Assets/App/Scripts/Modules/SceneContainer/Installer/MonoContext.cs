using System.Collections.Generic;
using App.Scripts.Modules.SceneContainer.ServiceLocator;
using UnityEngine;

namespace App.Scripts.Modules.SceneContainer.Installer
{
    public class MonoContext : MonoBehaviour
    {
        public List<MonoInstaller> installers = new();

        private readonly List<IInitializable> _initializables = new();
        private readonly List<IUpdatable> _updatables = new();

        private void Start()
        {
            Setup();
            Init();
        }

        private void Setup()
        {
            ServiceContainer container = BuildContainer();
            _initializables.AddRange(container.GetServices<IInitializable>());
            _updatables.AddRange(container.GetServices<IUpdatable>());
        }

        private ServiceContainer BuildContainer()
        {
            ServiceContainer container = new();
            foreach (MonoInstaller installer in installers)
            {
                installer.InstallBindings(container);
            }

            return container;
        }

        private void Init()
        {
            foreach (IInitializable initializable in _initializables)
            {
                initializable.Init();
            }
        }

        private void Update()
        {
            foreach (IUpdatable updatable in _updatables)
            {
                updatable.Update();
            }
        }
    }
}
