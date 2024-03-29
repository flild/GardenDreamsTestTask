using System;
using UnityEngine;

namespace PocketZone.Interfaces
{
    /// <summary>
    /// HealthChangedEvent Должен вызываться при изменение хп и передавать текущее хп, максимальное хп
    /// </summary>
    public interface IHaveHpBar  
    {
        public event Action<float, float> HealthChangedEvent;
    }
}

