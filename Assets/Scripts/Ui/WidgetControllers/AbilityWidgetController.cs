using Ui.Ability;
using UnityEngine;

namespace Ui.WidgetControllers
{
    public class AbilityWidgetController : WidgetController
    {
        private TowerAbilities m_towerAbilities;
        private PlayerMoney m_playerMoney;
        
        public override void BindCallbacksToDependencies()
        {
            m_towerAbilities = Object.FindFirstObjectByType<TowerAbilities>();
            m_playerMoney = Object.FindFirstObjectByType<PlayerMoney>();
        }

        public override void BroadcastInitialValues()
        {
            throw new System.NotImplementedException();
        }
        
        public void TryBuyAbility(BuyAbilityScriptableObject abilityToBuy, GameObject buttonGameObject)
        {
            if (m_playerMoney.RemoveMoney(abilityToBuy.Cost))
            {
                m_towerAbilities.AddAbility(abilityToBuy.AbilityScriptableObject);
                Object.Destroy(buttonGameObject);
            }
        }
    }
}