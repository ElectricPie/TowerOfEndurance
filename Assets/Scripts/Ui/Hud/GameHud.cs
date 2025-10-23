using UnityEngine;

namespace Ui.Hud
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private GameObject m_gameHudGameObject;
        [SerializeField] private GameObject m_gameOverGameObject;

        private void Awake()
        {
            ShowGameHud();
        }

        private void Start()
        {
            GameManager.Instance.OnGameOverEvent += ShowGameOverScreen;
        }

        private void ShowGameHud()
        {
            m_gameHudGameObject.SetActive(true);
            m_gameOverGameObject.SetActive(false);
        }
        
        private void ShowGameOverScreen()
        {
            m_gameHudGameObject.SetActive(false);
            m_gameOverGameObject.SetActive(true);
        }
    }
}