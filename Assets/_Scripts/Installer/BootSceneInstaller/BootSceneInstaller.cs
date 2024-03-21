using BootSceneScripts;
using Installer.ProjectContextInstaller;
using UnityEngine;
using Zenject;

namespace Installer.BootSceneInstaller
{
    [CreateAssetMenu(fileName = "BootSceneInstaller", menuName = "Installers/BootSceneInstaller")]
    public class BootSceneInstaller : ScriptableObjectInstaller<BootSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootSceneRelay>().AsSingle().NonLazy();
        }
    }
}