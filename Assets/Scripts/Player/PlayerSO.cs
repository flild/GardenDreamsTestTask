
using UnityEngine;

namespace PocketZone.Player
{
    [CreateAssetMenu(fileName ="playerData", menuName = "ScriptableObject/Player/data")]
    public class PlayerSO: ScriptableObject
    {
        [SerializeField]
        private PlayerData playerData;
        public PlayerData Data
        {
            get { return new PlayerData(playerData.Health, playerData.MaxHealth, playerData.MoveSpeed); }
        }
    }
}
