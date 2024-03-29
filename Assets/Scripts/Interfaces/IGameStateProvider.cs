using System.Threading.Tasks;
using UnityEngine;

namespace PocketZone.Interfaces
{
    public interface IGameStateProvider
    {
        Task SaveGameStateAsync();
        void LoadGameState();
    }
}

