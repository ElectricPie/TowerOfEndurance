using Ui.Tooltip.Ability;
using UnityEngine;

namespace Ui.Tooltip
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private AbilityTooltipWidget m_abilityTooltipPrefab;
        private AbilityTooltipWidget m_abilityTooltipInstance;

        public void ShowTooltip(AbilityTooltipData tooltipData, Vector3 position)
        {
            if (m_abilityTooltipInstance == null)
            {
                m_abilityTooltipInstance = Instantiate(m_abilityTooltipPrefab, transform);
            }
            
            m_abilityTooltipInstance.SetData(tooltipData);
            m_abilityTooltipInstance.gameObject.SetActive(true);
            m_abilityTooltipInstance.transform.position = position;
        }

        public void HideTooltip()
        {
            // TODO: Not hiding when ability is bought
            if (m_abilityTooltipInstance == null)
                return;

            m_abilityTooltipInstance.gameObject.SetActive(false);
        }
    }
}