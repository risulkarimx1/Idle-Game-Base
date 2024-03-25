using GameCode.Init;
using GameCode.TimeProvider;
using GameSessions;
using Services.GameInitFramework;
using Services.SceneFlowServices;
using UnityEngine;
using Zenject;

namespace Installer.ProjectContextInstaller
{
    [CreateAssetMenu(fileName = "ProjectContextInstaller", menuName = "Installers/ProjectContextInstaller")]
    public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
    {
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<SystemTimeProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneFlowService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameSessionProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameGameSessionUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitManager>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}