using Services.DataFramework;

namespace Services.GameInitFramework
{
    public interface IRequireInit
    {
        public bool InitFinished { get; }
    }
    
    public interface IInitializableAfter<T> where T : IRequireInit
    {
        public void OnInitFinishedFor(T dataManager);
    }
    
    public interface IInitializableAfterAll
    {
        public void OnAllInitFinished();
    }
    
    public interface IInitializableAfterData : IInitializableAfter<DataManager>
    {
    }
}
