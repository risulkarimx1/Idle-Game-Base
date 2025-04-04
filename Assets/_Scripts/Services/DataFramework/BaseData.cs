namespace Services.DataFramework
{
    [System.Serializable]
    public abstract class BaseData
    {
        public bool IsDirty { get; set; }

        private string _dataIdentifier;
        private string _dataVersion;


        public void SetDirty()
        {
            IsDirty = true;
        }
    }
}