using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Effect
{
    public class AttributeApplicator : MonoBehaviour
    {
        [SerializeField] private EffectsContainer m_effectsContainer;
        [SerializeField] private List<GameEffectScriptableObject> m_attributeEffects;

        protected void Start()
        {
            foreach (GameEffectScriptableObject attributeEffect in m_attributeEffects)
            {
                m_effectsContainer.ApplyEffect(gameObject, attributeEffect);
            }
        }
    }
}