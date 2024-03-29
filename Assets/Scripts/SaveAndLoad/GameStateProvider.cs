using PocketZone.Interfaces;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

namespace PocketZone
{
    public class GameStateProvider : IGameStateProvider
    {
        public GameStateData GameState { get; private set; }
        public const string InventoryFilePath = "/PlayerInventory.dat";
        //chat-gpt
        public async Task SaveGameStateAsync()
        {
            var formatter = new BinaryFormatter();
            using(var file = File.OpenWrite(Application.persistentDataPath + InventoryFilePath))
            {
                formatter.Serialize(file, GameState);
            }
        }
        //chat-gpt
        public async void LoadGameState()
        {
            var formatter = new BinaryFormatter();
            if (File.Exists(Application.persistentDataPath + InventoryFilePath))
            {
                using (var file = File.OpenRead(Application.persistentDataPath + InventoryFilePath))
                {

                    GameState = (GameStateData)formatter.Deserialize(file);
                }
            }
            else
            {
                GameState = new();
                await SaveGameStateAsync();
            }
        }

    }
}
