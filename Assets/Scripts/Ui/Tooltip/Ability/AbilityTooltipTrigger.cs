using Ui.Ability;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Tooltip.Ability
{
    public class AbilityTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MonoBehaviour m_abilityTooltipComponent;
        [SerializeField] private Vector2 m_tooltipOffset;
        
        private TooltipManager m_tooltipManager;
        
        private IAbilityTooltipInterface m_abilityTooltipInterface;

        private void Awake()
        {
            m_tooltipManager = FindFirstObjectByType<TooltipManager>();
            
            m_abilityTooltipInterface = (IAbilityTooltipInterface)m_abilityTooltipComponent;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 tooltipPosition = (Vector2)transform.position + m_tooltipOffset;
            AbilityTooltipData tooltipData = new AbilityTooltipData(
                m_abilityTooltipInterface.GetAbilityData(), 
                Mathf.CeilToInt(m_abilityTooltipInterface.GetAbilityCost()));
            
            m_tooltipManager.ShowTooltip(tooltipData, tooltipPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_tooltipManager.HideTooltip();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere((Vector2)transform.position + m_tooltipOffset, 10.0f);
        }
    }
}