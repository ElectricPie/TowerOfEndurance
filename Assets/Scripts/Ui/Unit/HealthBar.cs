using UnityEngine;
using UnityEngine.UI;

namespace Ui.Unit
{
    public class UnitHealthBar : MonoBehaviour
    {
        [SerializeField] private UnitHealth m_unitHealth;
        [SerializeField] private Slider m_slider;

        private Camera m_camera;

        private void Awake()
        {
            if (m_unitHealth == null)
            {
                throw new System.Exception($"HealthBar script on {name} is missing reference to UnitHealth component.");
            }

            if (m_slider == null)
            {
                throw new System.Exception($"HealthBar script on {name} is missing reference to Slider component.");
            }

            m_slider.gameObject.SetActive(false);

            m_unitHealth.OnUnitMaxHealthChangedEvent += OnMaxHealthChanged;
            m_unitHealth.OnUnitCurrentHealthChangedEvent += OnCurrentHealthChanged;

            m_slider.maxValue = m_unitHealth.MaxHealth;
            m_slider.value = m_unitHealth.CurrentHealth;

            m_camera = Camera.main;
        }

        private void Update()
        {
            transform.rotation = m_camera.transform.rotation;
        }

        private void OnCurrentHealthChanged(GameObject unit, float currentHealth)
        {
            m_slider.gameObject.SetActive(currentHealth < m_unitHealth.MaxHealth);
            
            m_slider.value = currentHealth;
        }

        private void OnMaxHealthChanged(GameObject unit, float newMaxHealth)
        {
            m_slider.maxValue = newMaxHealth;
        }
    }
}