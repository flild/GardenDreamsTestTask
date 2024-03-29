using PocketZone.Inventory;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace PocketZone
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField]
        private ItemsConfig _itemsConfig;
        public override void InstallBindings()
        {
            Container.Bind<ItemsConfig>().FromInstance(_itemsConfig).AsSingle();
            Container.Bind<InventoryService>().AsSingle().WithArguments(_itemsConfig);
        }
    }
}
