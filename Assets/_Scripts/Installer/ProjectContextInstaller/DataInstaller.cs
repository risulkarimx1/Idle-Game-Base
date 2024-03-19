using Frameworks.DataFramework;
using UnityEngine;
using Zenject;

namespace Installer.ProjectContextInstaller
{
    [CreateAssetMenu(fileName = "DataInstaller", menuName = "Installers/DataInstaller")]
    public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataHandler>().To<JsonFileDataHandler>().AsSingle().NonLazy();
            Container.Bind<IEncryptionService>().To<AesEncryptionService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DataManager>().AsSingle().NonLazy();
        }
    }
}