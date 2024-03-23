using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Finance;
using GameCode.Init;
using GameCode.Mines;
using GameCode.Mineshaft;
using GameCode.Signals;
using GameCode.Tutorial;
using GameCode.UI;
using GameCode.Utils;
using GameCode.Warehouse;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Installer.GameSceneInstallers
{
    [CreateAssetMenu(fileName = "GameSceneInstaller", menuName = "Installers/GameSceneInstaller")]
    public class GameSceneInstaller : ScriptableObjectInstaller<GameSceneInstaller>
    {
        [BoxGroup("Scene Objects to spawn")] [SerializeField] private Transform firstMineshaftPosition;
        [BoxGroup("Game Controllers and Configs")] [SerializeField] private GameConfig gameConfig;
        
        [BoxGroup("Game Scene Prefabs")] [SerializeField] private HudView hudViewPrefab;
        [BoxGroup("Game Scene Prefabs")] [SerializeField] private CameraView cameraRigPrefab;
        [BoxGroup("Game Scene Prefabs")] [SerializeField] private WarehouseView warehouseViewPrefab;
        [BoxGroup("Game Scene Prefabs")] [SerializeField] private ElevatorView elevatorViewPrefab;
        [BoxGroup("Game Scene Prefabs")] [SerializeField] private MineSelectionView mineSelectionViewPrefab;
        public override void InstallBindings()
        {
            InstallSignals();

            Container.BindInterfacesAndSelfTo<CompositeDisposable>().AsSingle();
            Container.Bind<HudView>().FromComponentInNewPrefab(hudViewPrefab).AsSingle();
            Container.Bind<CameraView>().FromComponentInNewPrefab(cameraRigPrefab).AsSingle();
            Container.Bind<WarehouseView>().FromComponentInNewPrefab(warehouseViewPrefab).AsSingle();
            Container.Bind<ElevatorView>().FromComponentInNewPrefab(elevatorViewPrefab).AsSingle();
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
            Container.Bind<Transform>().WithId(GameConstants.FirtMinePositionObjectTag).FromInstance(firstMineshaftPosition).AsSingle();

            Container.BindInterfacesAndSelfTo<TutorialModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraController>().AsSingle().NonLazy();
            
            // finance 
            Container.BindInterfacesAndSelfTo<DepositRateCalculator>().AsSingle();
            Container.BindInterfacesAndSelfTo<FinanceModel>().AsSingle();
            
            // Hud
            Container.BindInterfacesAndSelfTo<HudController>().AsSingle().NonLazy();
            
            // Mine Shaft
            Container.BindInterfacesAndSelfTo<MineshaftCollectionModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MineshaftFactory>().AsSingle();
            
            // Elevator
            Container.BindInterfacesAndSelfTo<ElevatorModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ElevatorController>().AsSingle().NonLazy();
            
            // Warehouse
            Container.BindInterfacesAndSelfTo<WarehouseModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<WarehouseController>().AsSingle().NonLazy();
            
            // Mine Selection
            Container.Bind<MineSelectionView>().FromComponentInNewPrefab(mineSelectionViewPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<MineSelectionController>().AsSingle();
            
            // Initiate Game Data Services
            Container.BindInterfacesAndSelfTo<MineDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameInitializer>().AsSingle().NonLazy();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<GameSignals.MineshaftCreatedSignal>();
        }
    }
}