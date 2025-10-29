using System.Collections.Generic;
using UnityEngine;

namespace Ui.Ability
{
    public class BuyableAbilitiesList : MonoBehaviour
    {
        [SerializeField] private BuyAbilityButton m_buyAbilityButtonPrefab;
        [SerializeField] private Transform m_contentTransform;
        
        [SerializeField] private BuyAbilityScriptableObject[] m_buyableAbilities;
        
        private TowerAbilities m_towerAbilities;
        private List<BuyAbilityButton> m_buttons = new List<BuyAbilityButton>();

        private void Awake()
        {
            m_towerAbilities = FindFirstObjectByType<TowerAbilities>();
        }

        public void Start()
        {
            foreach (BuyAbilityScriptableObject ability in m_buyableAbilities)
            {
                // Don't add abilities the tower already has the ability
                if (m_towerAbilities.HasAbilityOfType(ability.AbilityScriptableObject.AbilityData))
                {
                    Debug.LogWarning($"Tower already has ability {ability.AbilityScriptableObject.Label}, not adding to buyable list.");
                    continue;
                }
                
                BuyAbilityButton button = Instantiate(m_buyAbilityButtonPrefab, m_contentTransform);
                button.Initialize(ability);
                m_buttons.Add(button);
            }
        }
    }
}