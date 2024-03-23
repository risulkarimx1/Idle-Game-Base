using LevelLoaderScripts;
using UnityEngine;
using Zenject;

namespace Installer.BootSceneInstaller
{
    [CreateAssetMenu(fileName = "LevelLoaderSceneInstaller", menuName = "Installers/LevelLoaderSceneInstaller")]
    public class LevelLoaderSceneInstaller : ScriptableObjectInstaller<LevelLoaderSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameLevelLoadingService>().AsSingle().NonLazy();
        }
    }
}