using UnityEngine;
using Zenject;

namespace Installer.BootScene
{
    [CreateAssetMenu(fileName = "BootSceneInstaller", menuName = "Installers/BootSceneInstaller")]
    public class BootSceneInstaller : ScriptableObjectInstaller<BootSceneInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}