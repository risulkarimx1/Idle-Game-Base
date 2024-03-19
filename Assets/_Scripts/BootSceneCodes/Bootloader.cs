using Frameworks.DataFramework;
using UnityEngine;
using X1Frameworks.InitializablesManager;
using Zenject;

namespace BootSceneCodes
{
    public class Bootloader: IInitializableAfter<DataManager>
    {
        public void OnAllInitFinished()
        {
            Debug.Log("All init finished");    
        }

        public void OnInitFinishedFor(DataManager instance)
        {
            Debug.Log("Data init finished");
        }
    }
}
