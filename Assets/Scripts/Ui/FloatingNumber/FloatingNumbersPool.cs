using System.Collections;
using AbilitySystem.Ability.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Ui.FloatingNumber
{
    public class FloatingNumbersPool : MonoBehaviour
    {
        [SerializeField, RequiredIn(PrefabKind.InstanceInScene)]
        private TowerWaves m_towerWaves;
        [SerializeField, RequiredIn(PrefabKind.InstanceInScene)]
        private Transform m_cameraTransform;
        
        [SerializeField, Required] private FloatingNumber m_floatingNumberPrefab;
        [SerializeField, Min(1)] private int m_poolSize = 20;
        [SerializeField, Min(0)] private float m_showTime = 0.5f;
        [SerializeField] private Vector3 m_offsetFromUnit = new Vector3(0.0f, 2.0f, 0.0f);

        [SerializeField] private AttributeIdScriptableObject m_incomingDamageAttributeId;
        
        private ObjectPool<FloatingNumber> m_numberPool;
        
        protected void Awake()
        {
            m_numberPool = new ObjectPool<FloatingNumber>(() =>
                {
                    FloatingNumber newFloatingNumber = Instantiate(m_floatingNumberPrefab);
                    newFloatingNumber.Camera = m_cameraTransform;
                    return newFloatingNumber;
                },
                floatingNumber =>
                {
                    floatingNumber.gameObject.SetActive(true);
                },
                floatingNumber =>
                {
                    floatingNumber.gameObject.SetActive(false);
                    floatingNumber.transform.position = Vector3.zero;
                },
                floatingNumber =>
                {
                    Destroy(floatingNumber);
                }, true, m_poolSize);

            m_towerWaves.OnUnitSpawnedEvent += (newUnit, _) =>
            {
                newUnit.AttributeSet.GetAttribute(m_incomingDamageAttributeId).OnCurrentValueChangedEvent += newValue =>
                {
                    FloatingNumber floatingNumber = m_numberPool.Get();
                    floatingNumber.SetValue(newValue, newUnit.transform.position + m_offsetFromUnit, 1 / m_showTime);
                
                    StartCoroutine(HideNumber(floatingNumber));
                };
            };
        }

        private IEnumerator HideNumber(FloatingNumber floatingNumber)
        {
            yield return new WaitForSeconds(m_showTime);
            
            m_numberPool.Release(floatingNumber);
        }
    }
}