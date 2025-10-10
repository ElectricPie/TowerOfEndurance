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