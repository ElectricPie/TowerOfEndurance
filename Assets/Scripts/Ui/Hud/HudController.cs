using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Hud
{
    public class HudController : MonoBehaviour
    {
        public static HudController Instance => m_instance;


        [SerializeField] private GameObject m_hudPrefab;
        private GameObject m_hudInstance;
        
        private static HudController m_instance;
        
        private WaveWidgetController m_waveWidgetControllerInstance;
        private MoneyWidgetController m_moneyWidgetControllerInstance;

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
        
        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(this);
                return;
            }
            
            m_instance = this;

            Instantiate(m_hudPrefab);
        }
    }
}