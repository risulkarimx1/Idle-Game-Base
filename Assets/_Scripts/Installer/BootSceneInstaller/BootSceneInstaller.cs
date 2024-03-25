using BootSceneScripts;
using UnityEngine;
using Zenject;

namespace Installer.BootSceneInstaller
{
    [CreateAssetMenu(fileName = "BootSceneInstaller", menuName = "Installers/BootSceneInstaller")]
    public class BootSceneInstaller : ScriptableObjectInstaller<BootSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootSceneManager>().AsSingle().NonLazy();
        }
    }
}