using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Tooltip
{
    public class TooltipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, TextArea] private string m_tooltipTextField;
        [SerializeField] private Vector2 m_tooltipOffset;

        private TooltipManager m_tooltipManager;

        private void Awake()
        {
            m_tooltipManager = FindFirstObjectByType<TooltipManager>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Pointer entered tooltip button area.");
            
            Vector3 tooltipPosition = (Vector2)transform.position + m_tooltipOffset;
            m_tooltipManager.ShowTooltip(m_tooltipTextField, tooltipPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Pointer exited tooltip button area.");
            
            m_tooltipManager.HideTooltip();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere((Vector2)transform.position + m_tooltipOffset, 10.0f);
        }
    }
}