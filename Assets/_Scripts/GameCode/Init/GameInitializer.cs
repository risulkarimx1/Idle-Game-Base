using System;
using Services.DataFramework;
using UniRx;
using Zenject;

namespace GameCode.Init
{
    public class GameInitializer : IDisposable
    {
        [Inject] private CompositeDisposable _disposable;
        [Inject] private IDataManager _dataManager;

        public async void Dispose()
        {
            await _dataManager.SaveAllAsync();
            _disposable?.Dispose();
        }
    }
}