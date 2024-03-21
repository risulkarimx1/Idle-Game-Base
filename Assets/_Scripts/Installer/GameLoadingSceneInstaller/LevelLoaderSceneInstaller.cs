using LevelLoaderScripts;
using Services.SceneFlowServices;
using UnityEngine;
using Zenject;

namespace Installer.BootSceneInstaller
{
    [CreateAssetMenu(fileName = "LevelLoaderSceneInstaller", menuName = "Installers/LevelLoaderSceneInstaller")]
    public class LevelLoaderSceneInstaller : ScriptableObjectInstaller<LevelLoaderSceneInstaller>
    {
        [SerializeField] private LevelsConfig _levelsConfig;
        public override void InstallBindings()
        {
            Container.BindInstance(_levelsConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<GameLevelLoadingService>().AsSingle().NonLazy();
        }
    }
}