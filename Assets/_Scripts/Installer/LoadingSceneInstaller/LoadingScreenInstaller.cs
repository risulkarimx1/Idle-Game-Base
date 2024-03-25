using Services.LoadingScreen;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installer.LoadingSceneInstaller
{
    [CreateAssetMenu(fileName = "LoadingSceneInstaller", menuName = "Installers/LoadingSceneInstaller")]
    public class LoadingScreenInstaller : ScriptableObjectInstaller<LoadingScreenInstaller>
    {
        [FormerlySerializedAs("_loadingViewPrefab")] [SerializeField] private LoadingView loadingViewPrefab;

        public override void InstallBindings()
        {
            Container.Bind<LoadingView>().FromComponentInNewPrefab(loadingViewPrefab).AsSingle();
            Container.Bind<LoadingController>().AsSingle();
        }
    }
}