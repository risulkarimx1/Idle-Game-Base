using System;
using System.Collections.Generic;
using GameCode.Finance;
using GameCode.Init;
using GameCode.Persistance;
using Services.DataFramework;
using Services.SceneFlowServices;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GameCode.Mineshaft
{
    public class MineshaftFactory : IMineshaftFactory
    {
        private readonly MineshaftCollectionModel _collectionModel;
        private readonly FinanceModel _financeModel;
        private readonly GameConfig _config;
        private readonly CompositeDisposable _disposable;
        private HashSet<MineshaftController> _controllers = new();
        [Inject] private DataManager _dataManager;

        [Inject]
        public MineshaftFactory(MineshaftCollectionModel collectionModel, FinanceModel financeModel, GameConfig config,
            CompositeDisposable disposable)
        {
            _collectionModel = collectionModel;
            _financeModel = financeModel;
            _config = config;
            _disposable = disposable;
        }

        public void CreateMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position)
        {
             var controller = CreateAndRegisterMineshaft(mineshaftNumber, mineshaftLevel, position);
             if (_controllers.Add(controller))
             {
                 SetDataStream();
             }
            
        }

        public void CreateMineshaftBatch(Dictionary<int, int> mineshaftLevels, Vector2 position)
        {
            var controllers = new List<MineshaftController>();
            foreach (var entry in mineshaftLevels)
            {
                var controller = CreateAndRegisterMineshaft(entry.Key, entry.Value, position);
                position = controller.View.NextShaftView.NextShaftPosition;
                controllers.Add(controller);
                
            }
            
            foreach (var controller in controllers)
            {
                if (_controllers.Add(controller))
                {
                    SetDataStream();
                }    
            }
            
        }

        private void SetDataStream()
        {
            foreach (var controller in _controllers)
            {
                  controller.Model.Level.Subscribe(level => _dataManager.Get<GameLevelData>().SetMineshaftLevel(controller.Model.MineshaftNumber, level))
                    .AddTo(_disposable);
            }
        }

        private MineshaftController CreateAndRegisterMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position)
        {
            var view = Object.Instantiate(_config.MineshaftConfig.MineshaftPrefab, position, Quaternion.identity);
            SceneFlowService.MoveObjectToScene(view.gameObject, SceneFlowService.GameScene);
            
            var mineshaftModel = new MineshaftModel(mineshaftNumber, mineshaftLevel, _config, _financeModel, _disposable);
            var mineShaftController = new MineshaftController(view, mineshaftModel, this, _config, _disposable);
            
            _collectionModel.RegisterMineshaft(mineshaftNumber, mineshaftModel, view);
            return mineShaftController;
        }
    }
}