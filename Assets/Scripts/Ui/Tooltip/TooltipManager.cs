using UnityEngine;

namespace Ui.Tooltip
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private TooltipWidget m_tooltipPrefab;
        private TooltipWidget m_tooltipInstance;

        public void ShowTooltip(string tooltipText, Vector3 position)
        {
            if (m_tooltipInstance == null)
            {
                m_tooltipInstance = Instantiate(m_tooltipPrefab, transform);
            }
            
            m_tooltipInstance.gameObject.SetActive(true);
            m_tooltipInstance.transform.position = position;
        }

        public void HideTooltip()
        {
            if (m_tooltipInstance == null)
                return;

            m_tooltipInstance.gameObject.SetActive(false);
        }
    }
}