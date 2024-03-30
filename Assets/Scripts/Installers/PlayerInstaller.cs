using PocketZone.Player;
using PocketZone.Player.Gun;
using UnityEngine;
using Zenject;

namespace PocketZone
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        private InputReaderSO _inputReader;
        [SerializeField]
        private PlayerView _playerView;
        [SerializeField]
        private PlayerSO _PlayerDataSO;
        public override void InstallBindings()
        {
            Container.Bind<InputReaderSO>().FromInstance(_inputReader).AsSingle();
            Container.Bind<PlayerView>().FromInstance(_playerView).AsSingle();
            Container.BindInterfacesTo<GunModel>().AsSingle().WithArguments(_playerView.GetGunView, _inputReader);
            Container.BindInterfacesTo<PlayerMovmentController>().AsSingle().WithArguments(_PlayerDataSO.Data.MoveSpeed);
            Container.Bind<PlayerSO>().FromInstance(_PlayerDataSO).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle();

            
            
        }
    }
}

