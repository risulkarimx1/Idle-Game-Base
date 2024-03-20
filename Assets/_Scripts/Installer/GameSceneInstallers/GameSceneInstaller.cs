using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Init;
using GameCode.UI;
using GameCode.Warehouse;
using Sirenix.OdinInspector;
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
        public override void InstallBindings()
        {
            Container.Bind<HudView>().FromComponentInNewPrefab(hudViewPrefab).AsSingle();
            Container.Bind<CameraView>().FromComponentInNewPrefab(cameraRigPrefab).AsSingle();
            Container.Bind<WarehouseView>().FromComponentInNewPrefab(warehouseViewPrefab).AsSingle();
            Container.Bind<ElevatorView>().FromComponentInNewPrefab(elevatorViewPrefab).AsSingle();
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
            Container.Bind<Transform>().WithId("FirstMinePosition").FromInstance(firstMineshaftPosition).AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitializer>().AsSingle().NonLazy();
        }
    }
}