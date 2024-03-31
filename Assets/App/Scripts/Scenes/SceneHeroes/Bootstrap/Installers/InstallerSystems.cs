using App.Scripts.Modules.SceneContainer.Installer;
using App.Scripts.Modules.Systems;
using App.Scripts.Scenes.SceneHeroes.Features.GameInput.Systems;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.Systems;
using App.Scripts.Scenes.SceneHeroes.Features.LevelNavigation.Systems;
using App.Scripts.Scenes.SceneHeroes.Features.SaveField;
using App.Scripts.Scenes.SceneHeroes.Features.Units.UnitSelection.Systems;
using App.Scripts.Scenes.SceneHeroes.Features.Units.UnitSelection.UnitTypeSelector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.SceneHeroes.Bootstrap.Installers
{
    public class InstallerSystems : MonoInstaller
    {
        [SerializeField]
        private Camera gameCamera;

        [SerializeField]
        private ViewSelectorItem viewSelectorHero;

        [SerializeField]
        private ViewSelectorItem viewSelectorClickMode;

        [SerializeField]
        private Button buttonSave;

        [SerializeField]
        private ServiceSaveModelConfig.Config configSaveField;

        protected override void OnInstallBindings()
        {
            SystemsGroup systemGroup = new();

            _ = systemGroup.AddSystem(Container.CreateInstance<SystemRequestUpdateLevel>());
            _ = systemGroup.AddSystem(Container.CreateInstance<SystemRebuildLevel>());
            _ = systemGroup.AddSystem(
                Container.CreateInstanceWithArguments<SystemSwitchUnit>(viewSelectorHero)
            );
            _ = systemGroup.AddSystem(
                Container.CreateInstanceWithArguments<SystemFieldClick>(gameCamera)
            );
            _ = systemGroup.AddSystem(
                Container.CreateInstanceWithArguments<SystemChangeClickHandler>(
                    viewSelectorClickMode
                )
            );
            _ = systemGroup.AddSystem(Container.CreateInstance<SystemResetInput>());

            ServiceSaveModelConfig serviceSaveField = new(configSaveField);
            _ = systemGroup.AddSystem(
                Container.CreateInstanceWithArguments<SystemSaveField>(buttonSave, serviceSaveField)
            );

            Container.SetServiceSelf(systemGroup);
        }
    }
}
