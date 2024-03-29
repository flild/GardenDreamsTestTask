using System;

namespace PocketZone.Player
{
    [Serializable]
    public class PlayerData
    {
        public float Health;
        public float MaxHealth;
        public float MoveSpeed;
        public PlayerData(float health, float maxHealth, float moveSpeed)
        {
            Health = health;
            MaxHealth = maxHealth;
            MoveSpeed = moveSpeed;

        }
    }
}

