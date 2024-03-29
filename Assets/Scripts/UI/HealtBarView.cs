using PocketZone.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PocketZone
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        private Image _activeHealthImg;
        public void OnHealthChange(float health, float MaxHealth)
        {
            _activeHealthImg.fillAmount = health/ MaxHealth;
        }
    }
}

