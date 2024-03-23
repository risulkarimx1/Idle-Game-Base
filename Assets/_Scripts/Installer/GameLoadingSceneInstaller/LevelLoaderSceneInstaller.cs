using GameCode.Mines;
using LevelLoaderScripts;
using Services.SceneFlowServices;
using UnityEngine;
using UnityEngine.Serialization;
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