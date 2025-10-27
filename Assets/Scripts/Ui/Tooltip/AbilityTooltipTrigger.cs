using Ui.Ability;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Tooltip
{
    [RequireComponent(typeof(BuyAbilityButton))]
    public class AbilityTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, TextArea] private string m_tooltipTextField;
        [SerializeField] private Vector2 m_tooltipOffset;
        [SerializeField] private AbilityData m_ability;
        
        private TooltipManager m_tooltipManager;
        private BuyAbilityButton m_buyAbilityButton;

        private void Awake()
        {
            m_tooltipManager = FindFirstObjectByType<TooltipManager>();
            m_buyAbilityButton = GetComponent<BuyAbilityButton>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 tooltipPosition = (Vector2)transform.position + m_tooltipOffset;
            AbilityTooltipData tooltipData = new AbilityTooltipData(m_buyAbilityButton.Ability, m_tooltipTextField);
            
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