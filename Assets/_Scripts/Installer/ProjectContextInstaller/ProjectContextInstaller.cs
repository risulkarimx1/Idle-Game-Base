using Frameworks.DataFramework;
using Frameworks.InitializablesManager;
using UnityEngine;
using Zenject;

namespace Installer.ProjectContextInstaller
{
    [CreateAssetMenu(fileName = "ProjectContextInstaller", menuName = "Installers/ProjectContextInstaller")]
    public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameInitManager>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}