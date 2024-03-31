using App.Scripts.Features.Scenes.SceneSelector.StateSteps;
using App.Scripts.Features.Scenes.SceneSelector.Transitions;
using App.Scripts.Modules.SceneContainer.Installer;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Modules.StateMachine.States;

namespace App.Scripts.Features.CoreGame
{
    public class InstallerSimpleStateMachine : MonoInstaller
    {
        protected override void OnInstallBindings()
        {
            GraphStates graph = new();

            graph.AddState(KeyGameStates.Initialize, BuildStateInitialize());
            graph.AddState(KeyGameStates.Play, BuildStatePlay());
            graph.AddState(KeyGameStates.ExitScene, BuildStateExit());

            OnCompleteBuildGraph(graph);

            graph.SetStartNode(KeyGameStates.Initialize);

            graph.AddCompleteTransition(KeyGameStates.Initialize, KeyGameStates.Play);
            graph.AddTransition(
                KeyGameStates.Play,
                KeyGameStates.ExitScene,
                new TransitionChangeScene()
            );

            StateMachine stateMachine = new(graph);

            Container.SetServiceInterfaces(new ControllerStateMachine(stateMachine));
        }

        protected virtual void OnCompleteBuildGraph(GraphStates graphStates) { }

        protected StateStepContainer GetNode(GraphStates graphStates, string id)
        {
            return graphStates.GetNode(id) as StateStepContainer;
        }

        private IState BuildStateInitialize()
        {
            StateStepContainer stateInitialize = new();
            return stateInitialize;
        }

        private IState BuildStatePlay()
        {
            StateStepContainer stateInitialize = new();
            stateInitialize.AddStep(Container.CreateInstance<StepControlSceneSelection>());
            stateInitialize.AddStep(Container.CreateInstance<StateStepExecuteSystems>());
            return stateInitialize;
        }

        private IState BuildStateExit()
        {
            StateStepContainer stateInitialize = new();
            stateInitialize.AddStep(Container.CreateInstance<StepChangeScene>());
            return stateInitialize;
        }
    }
}
