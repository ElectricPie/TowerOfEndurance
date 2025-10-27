using UnityEngine;

namespace Ui.Hud
{
    public class MainMenuHudController : MonoBehaviour
    {
        [SerializeField] private GameObject m_hudPrefab;
        private GameObject m_hudInstance;

        public static MainMenuHudController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            
            m_hudInstance = Instantiate(m_hudPrefab);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}