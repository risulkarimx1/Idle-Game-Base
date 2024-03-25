using GameSessionLoaderScripts;
using UnityEngine;
using Zenject;

namespace Installer.GameLoadingSceneInstaller
{
    [CreateAssetMenu(fileName = "LevelLoaderSceneInstaller", menuName = "Installers/LevelLoaderSceneInstaller")]
    public class GameSessionLoaderSceneInstaller : ScriptableObjectInstaller<GameSessionLoaderSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSessionLoader>().AsSingle().NonLazy();
        }
    }
}