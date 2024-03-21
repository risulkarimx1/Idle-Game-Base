using System.Collections.Generic;
using GameCode.Finance;
using GameCode.Init;
using Services.SceneFlowServices;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Mineshaft
{
    public class MineshaftFactory : IMineshaftFactory
    {
        private readonly MineshaftCollectionModel _collectionModel;
        private readonly FinanceModel _financeModel;
        private readonly GameConfig _config;
        private readonly CompositeDisposable _disposable;

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
            // Utilize the helper method for creating a single mineshaft
            CreateAndRegisterMineshaft(mineshaftNumber, mineshaftLevel, position);
        }

        public void CreateMineshaftBatch(Dictionary<int, int> mineshaftLevels, Vector2 position)
        {
            foreach (var entry in mineshaftLevels)
            {
                // Utilize the helper method within the loop for creating each mineshaft
                var view = CreateAndRegisterMineshaft(entry.Key, entry.Value, position);
                position = view.NextShaftView.NextShaftPosition; // Adjust position for the next mineshaft
            }
        }

        // Helper method to eliminate code duplication
        private MineshaftView CreateAndRegisterMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position)
        {
            var view = Object.Instantiate(_config.MineshaftConfig.MineshaftPrefab, position, Quaternion.identity);
            SceneFlowService.MoveObjectToScene(view.gameObject, SceneFlowService.GameScene);
            
            var mineshaftModel = new MineshaftModel(mineshaftNumber, mineshaftLevel, _config, _financeModel, _disposable);
            new MineshaftController(view, mineshaftModel, this, _config, _disposable);
            
            _collectionModel.RegisterMineshaft(mineshaftNumber, mineshaftModel, view);
            
            return view; // Return the view to possibly use it, e.g., for positioning in batch creation
        }
    }
}