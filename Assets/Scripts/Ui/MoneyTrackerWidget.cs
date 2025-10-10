using TMPro;
using Ui.Hud;
using Ui.WidgetControllers;
using UnityEngine;

public class MoneyTrackerWidget : MonoBehaviour
{
    [SerializeField] private TMP_Text m_moneyNumberText;
    
    private void Awake()
    {
        MoneyWidgetController widgetController = HudController.Instance.MoneyWidgetController;
        widgetController.OnMoneyChanged += OnMoneyChanged;
    }
    
    public void OnMoneyChanged(float newAmount)
    {
        m_moneyNumberText.text = newAmount.ToString("0");
    }
}