using BootSceneCodes;
using Frameworks.InitializablesManager;
using UnityEngine;
using Zenject;

namespace Installer.BootSceneInstaller
{
    [CreateAssetMenu(fileName = "BootSceneInstaller", menuName = "Installers/BootSceneInstaller")]
    public class BootSceneInstaller : ScriptableObjectInstaller<BootSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Bootloader>().AsSingle().NonLazy();
        }
    }
}