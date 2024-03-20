using Frameworks.DataFramework;

namespace Frameworks.GameInitFramework
{
    public interface IRequireInit
    {
        public bool InitFinished { get; }
    }
    
    public interface IInitializableAfter<T> where T : IRequireInit
    {
        public void OnInitFinishedFor(T instance);
    }
    
    public interface IInitializableAfterAll
    {
        public void OnAllInitFinished();
    }
    
    public interface IInitializableAfterData : IInitializableAfter<DataManager>
    {
    }
}
