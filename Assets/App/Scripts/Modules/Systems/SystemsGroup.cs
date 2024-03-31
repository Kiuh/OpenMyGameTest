using System.Collections.Generic;

namespace App.Scripts.Modules.Systems
{
    public class SystemsGroup
    {
        private readonly List<ISystem> _systems = new();

        private readonly SystemContext _context = new();

        public SystemsGroup() { }

        public SystemsGroup(IEnumerable<ISystem> systems)
        {
            _ = AddSystems(systems);
        }

        public SystemsGroup AddSystem(ISystem system)
        {
            system.Context = _context;
            _systems.Add(system);
            return this;
        }

        public SystemsGroup AddSystems(IEnumerable<ISystem> systems)
        {
            foreach (ISystem system in systems)
            {
                _ = AddSystem(system);
            }

            return this;
        }

        public void Init()
        {
            foreach (ISystem system in _systems)
            {
                system.Init();
            }
        }

        public void Update(float dt)
        {
            foreach (ISystem system in _systems)
            {
                system.Update(dt);
            }

            _context.Clean();
        }

        public void Cleanup()
        {
            foreach (ISystem system in _systems)
            {
                system.Cleanup();
            }
        }
    }
}
