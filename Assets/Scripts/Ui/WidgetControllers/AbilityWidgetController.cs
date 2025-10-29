using System;
using System.Collections.Generic;
using Ui.Ability;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Ui.WidgetControllers
{
    public class AbilityWidgetController : WidgetController
    {
        public event Action<AbilityInstance> OnAbilityPurchasedEvent = delegate { };
        
        private TowerAbilities m_towerAbilities;
        private PlayerMoney m_playerMoney;
        
        private Dictionary<GameObject, BuyAbilityScriptableObject> m_purchaseButtons = new Dictionary<GameObject, BuyAbilityScriptableObject>();
        
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
            
            AbilityInstance newAbility = m_towerAbilities.AddAbility(abilityToBuy.AbilityScriptableObject);
            OnAbilityPurchasedEvent.Invoke(newAbility);
            m_purchaseButtons.Remove(buttonGameObject);
            Object.Destroy(buttonGameObject);
        }

        public void TryBuyRandomAbility(float cost)
        {
            // Get a random ability
            int randomAbilityIndex = Random.Range(0, m_purchaseButtons.Count - 1);
            KeyValuePair<GameObject, BuyAbilityScriptableObject> randomAbilityEntry = new List<KeyValuePair<GameObject, BuyAbilityScriptableObject>>(m_purchaseButtons)[randomAbilityIndex];
            
            // Check it's not already owned and destroy the corresponding button if so
            if (m_towerAbilities.HasAbilityOfType(randomAbilityEntry.Value.AbilityScriptableObject.AbilityData))
            {
                Debug.LogWarning($"Tower already has ability {randomAbilityEntry}, cannot buy again.");
                m_purchaseButtons.Remove(randomAbilityEntry.Key);
                Object.Destroy(randomAbilityEntry.Key);
                return;
            }
            
            if (!m_playerMoney.RemoveMoney(cost)) 
                return;
            
            AbilityInstance newAbility = m_towerAbilities.AddAbility(randomAbilityEntry.Value.AbilityScriptableObject);
            OnAbilityPurchasedEvent.Invoke(newAbility);
            m_purchaseButtons.Remove(randomAbilityEntry.Key);
            Object.Destroy(randomAbilityEntry.Key);
        }
        
        public void RegisterBuyButton(BuyAbilityScriptableObject ability, GameObject buttonGameObject)
        {
            m_purchaseButtons.TryAdd(buttonGameObject, ability);
        }
    }
}