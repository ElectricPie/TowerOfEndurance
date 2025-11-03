using System;
using AbilitySystem.Ability.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Unit
{
    public class UnitHealthBar : MonoBehaviour
    {
        [SerializeField] private AttributeSet m_attributeSet;
        [SerializeField] private AttributeIdScriptableObject m_maxHealthAttributeId;
        [SerializeField] private AttributeIdScriptableObject m_healthAttributeId;
        
        [SerializeField] private Slider m_slider;

        private Camera m_camera;

        private void Awake()
        {
            m_camera = Camera.main;
        }

        protected void Start()
        {
            m_attributeSet.GetAttribute(m_maxHealthAttributeId).OnCurrentValueChangedEvent += newValue =>
            {
                m_slider.maxValue = newValue;
            };
            m_slider.maxValue = m_attributeSet.GetAttributeValue(m_maxHealthAttributeId);
            m_attributeSet.GetAttribute(m_healthAttributeId).OnCurrentValueChangedEvent += newValue =>
            {
                gameObject.SetActive(true);
                m_slider.value = newValue;
            };
            m_slider.value = m_attributeSet.GetAttributeValue(m_healthAttributeId);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.rotation = m_camera.transform.rotation;
        }
    }
}