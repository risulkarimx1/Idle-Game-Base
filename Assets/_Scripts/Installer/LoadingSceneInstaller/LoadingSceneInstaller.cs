using Services.LoadingScreen;
using UnityEngine;
using Zenject;

namespace Installer.LoadingSceneInstaller
{
    [CreateAssetMenu(fileName = "LoadingSceneInstaller", menuName = "Installers/LoadingSceneInstaller")]
    public class LoadingSceneInstaller : ScriptableObjectInstaller<LoadingSceneInstaller>
    {
        [SerializeField] private LoadingView _loadingViewPrefab;
        public override void InstallBindings()
        {
            Container.Bind<LoadingView>().FromComponentInNewPrefab(_loadingViewPrefab).AsSingle();
            Container.Bind<LoadingController>().AsSingle();
        }
    }
}