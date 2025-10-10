using System;
using TMPro;
using UnityEngine;

namespace Ui.TowerUpgrade
{
    public class TowerUpgradeButton : MonoBehaviour
    {
        public event Action OnClickedEvent = delegate { };
        
        [SerializeField] private TMP_Text m_upgradeCostText;
        
        public void UpdateCost(float newCost)
        {
            m_upgradeCostText.text = $"£{newCost}";
        }

        public void OnClicked()
        {
            OnClickedEvent.Invoke();
        }
    }
}