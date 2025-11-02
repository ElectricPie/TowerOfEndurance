using System;
using UnityEngine;

namespace AbilitySystem.Effect
{
    public class TestAttributeApplicator : MonoBehaviour
    {
        [SerializeField] private EffectsContainer m_effectsContainer;
        
        [SerializeField] private GameEffectScriptableObject m_primaryAttributes;
        [SerializeField] private GameEffectScriptableObject m_secondaryAttributes;

        protected void Start()
        {
            m_effectsContainer.ApplyEffect(gameObject, m_primaryAttributes);
            m_effectsContainer.ApplyEffect(gameObject, m_secondaryAttributes);
        }
    }
}