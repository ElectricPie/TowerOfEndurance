using UnityEngine;

namespace Ui.Ability
{
    public class BuyableAbilitiesList : MonoBehaviour
    {
        [SerializeField] private BuyAbilityButton m_buyAbilityButtonPrefab;
        [SerializeField] private Transform m_contentTransform;
        
        [SerializeField] private BuyAbilityScriptableObject[] m_buyableAbilities;

        public void Awake()
        {
            foreach (BuyAbilityScriptableObject ability in m_buyableAbilities)
            {
                BuyAbilityButton button = Instantiate(m_buyAbilityButtonPrefab, m_contentTransform);
                button.Initialize(ability);
            }
        }
    }
}