using System;
using UnityEngine;

namespace PocketZone.Interfaces
{
    /// <summary>
    /// HealthChangedEvent ������ ���������� ��� ��������� �� � ���������� ������� ��, ������������ ��
    /// </summary>
    public interface IHaveHpBar  
    {
        public event Action<float, float> HealthChangedEvent;
    }
}

