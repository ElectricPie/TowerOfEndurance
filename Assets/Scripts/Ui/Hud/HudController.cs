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
        
        private StatWidgetController m_statWidgetControllerInstance;

        public StatWidgetController StatWidgetController
        {
            get {
                if (m_statWidgetControllerInstance == null)
                {
                    m_statWidgetControllerInstance = new StatWidgetController();
                    m_statWidgetControllerInstance.BindCallbacksToDependencies();
                }

                return m_statWidgetControllerInstance;
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