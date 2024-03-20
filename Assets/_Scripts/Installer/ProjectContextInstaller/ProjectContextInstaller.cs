using BootSceneScripts;
using Frameworks.GameInitFramework;
using Services.SceneFlowServices;
using UnityEngine;
using Zenject;

namespace Installer.ProjectContextInstaller
{
    [CreateAssetMenu(fileName = "ProjectContextInstaller", menuName = "Installers/ProjectContextInstaller")]
    public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneFlowService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitManager>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}