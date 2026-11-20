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

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 tooltipPosition = GetTooltipPosition();
            AbilityTooltipData tooltipData = new AbilityTooltipData(
                m_abilityTooltipInterface.GetAbility(), 
                m_abilityTooltipInterface.GetAbilityLevel());
            
            m_tooltipManager.ShowTooltip(tooltipData, tooltipPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_tooltipManager.HideTooltip();
        }
        
        protected void Awake()
        {
            m_tooltipManager = FindFirstObjectByType<TooltipManager>();
            
            m_abilityTooltipInterface = (IAbilityTooltipInterface)m_abilityTooltipComponent;
        }

        protected void OnDestroy()
        {
            m_tooltipManager.HideTooltip();
        }
        
        
        private Vector3 GetTooltipPosition()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            float scaleFactor = canvas != null ? canvas.rootCanvas.scaleFactor : 1.0f;

            return (Vector2)transform.position + m_tooltipOffset * scaleFactor;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(GetTooltipPosition(), 10.0f);
        }
    }
}