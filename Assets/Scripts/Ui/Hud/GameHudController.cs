using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Hud
{
    public class GameHudController : MonoBehaviour
    {
        [SerializeField] private GameObject m_hudPrefab;
        private GameObject m_hudInstance;

        private WaveWidgetController m_waveWidgetControllerInstance;
        private MoneyWidgetController m_moneyWidgetControllerInstance;
        private LivesWidgetController m_livesWidgetControllerInstance;
        private TowerUpgradeWidgetController m_towerUpgradeWidgetControllerInstance;
        private AbilityWidgetController m_abilityWidgetControllerInstance;

        public static GameHudController Instance { get; private set; }
        
        public WaveWidgetController WaveWidgetController
        {
            get {
                if (m_waveWidgetControllerInstance == null)
                {
                    m_waveWidgetControllerInstance = new WaveWidgetController();
                    m_waveWidgetControllerInstance.BindCallbacksToDependencies();
                }

                return m_waveWidgetControllerInstance;
            }
        }
        public MoneyWidgetController MoneyWidgetController
        {
            get {
                if (m_moneyWidgetControllerInstance == null)
                {
                    m_moneyWidgetControllerInstance = new MoneyWidgetController();
                    m_moneyWidgetControllerInstance.BindCallbacksToDependencies();
                }

                return m_moneyWidgetControllerInstance;
            }
        }
        public LivesWidgetController LivesWidgetController
        {
            get {
                if (m_livesWidgetControllerInstance == null)
                {
                    m_livesWidgetControllerInstance = new LivesWidgetController();
                    m_livesWidgetControllerInstance.BindCallbacksToDependencies();
                }

                return m_livesWidgetControllerInstance;
            }
        }
        public TowerUpgradeWidgetController TowerUpgradeWidgetController
        {
            get {
                if (m_towerUpgradeWidgetControllerInstance == null)
                {
                    m_towerUpgradeWidgetControllerInstance = new TowerUpgradeWidgetController();
                    m_towerUpgradeWidgetControllerInstance.BindCallbacksToDependencies();
                }

                return m_towerUpgradeWidgetControllerInstance;
            }
        }
        
        public AbilityWidgetController AbilityWidgetController
        {
            get {
                if (m_abilityWidgetControllerInstance == null)
                {
                    m_abilityWidgetControllerInstance = new AbilityWidgetController();
                    m_abilityWidgetControllerInstance.BindCallbacksToDependencies();
                }

                return m_abilityWidgetControllerInstance;
            }
        }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;

            m_hudInstance = Instantiate(m_hudPrefab);
        }
    }
}