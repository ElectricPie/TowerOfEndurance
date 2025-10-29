using System.Collections.Generic;
using Ui.Ability;
using UnityEngine;

namespace Ui.WidgetControllers
{
    public class AbilityWidgetController : WidgetController
    {
        private TowerAbilities m_towerAbilities;
        private PlayerMoney m_playerMoney;
        
        private Dictionary<GameObject, AbilityData> m_purchaseButtons = new Dictionary<GameObject, AbilityData>();
        
        public override void BindCallbacksToDependencies()
        {
            m_towerAbilities = Object.FindFirstObjectByType<TowerAbilities>();
            m_playerMoney = Object.FindFirstObjectByType<PlayerMoney>();
        }

        public override void BroadcastInitialValues() { }
        
        public void TryBuyAbility(BuyAbilityScriptableObject abilityToBuy, GameObject buttonGameObject)
        {
            // Prevent buying multiple of the same ability
            if (m_towerAbilities.HasAbilityOfType(abilityToBuy.AbilityScriptableObject.AbilityData))
            {
                Debug.LogWarning($"Tower already has ability {abilityToBuy.AbilityScriptableObject.Label}, cannot buy again.");
                Object.Destroy(buttonGameObject);
                return;
            }

            // Check for sufficient money
            if (!m_playerMoney.RemoveMoney(abilityToBuy.Cost)) 
                return;
            
            m_towerAbilities.AddAbility(abilityToBuy.AbilityScriptableObject);
            m_purchaseButtons.Remove(buttonGameObject);
            Object.Destroy(buttonGameObject);
        }
        
        public void RegisterBuyButton(AbilityData abilityData, GameObject buttonGameObject)
        {
            m_purchaseButtons.TryAdd(buttonGameObject, abilityData);
        }
    }
}