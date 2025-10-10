using TMPro;
using Ui.Hud;
using Ui.WidgetControllers;
using UnityEngine;

public class LivesTrackerWidget : MonoBehaviour
{
    [SerializeField] private TMP_Text m_livesText;

    private int m_currentLives = 0;
    private int m_maxLives = 0;
    
    private void Awake()
    {
        LivesWidgetController widgetController = HudController.Instance.LivesWidgetController;
        widgetController.OnCurrentLivesChanged += OnCurrentLivesChanged;
        widgetController.OnMaxLivesChanged += OnMaxLivesChanged;
        widgetController.BroadcastInitialValues();
    }

    private void OnCurrentLivesChanged(int newCurrentLives)
    {
        m_currentLives = newCurrentLives;
        m_livesText.text = $"{m_currentLives:00}/{m_maxLives:00}";
    }
    
    private void OnMaxLivesChanged(int newMaxLives)
    {
        m_maxLives = newMaxLives;
        m_livesText.text = $"{m_currentLives:00}/{m_maxLives:00}";
    }
}
