using System;

namespace Ui.WidgetControllers
{
    public class MoneyWidgetController : WidgetController
    {
        public event Action<float> OnMoneyChanged = delegate { };
        
        public override void BindCallbacksToDependencies()
        {
            PlayerMoney playerMoney = UnityEngine.Object.FindFirstObjectByType<PlayerMoney>();
            playerMoney.OnMoneyChangedEvent += newAmount =>
            {
                OnMoneyChanged.Invoke(newAmount);
            };
        }

        public override void BroadcastInitialValues()
        {
            PlayerMoney playerMoney = UnityEngine.Object.FindFirstObjectByType<PlayerMoney>();
            OnMoneyChanged.Invoke(playerMoney.Amount);
        }
    }
}