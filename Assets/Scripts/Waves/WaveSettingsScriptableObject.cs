using System;
using UnityEngine;

namespace Waves
{
    [CreateAssetMenu(fileName="New Wave Settings", menuName="Waves/New Wave Settings", order = 0), Serializable]
    public class WaveSettingsScriptableObject : ScriptableObject
    {
        [SerializeField] private AnimationCurve m_statModifierCurve = AnimationCurve.Linear(1, 1, 40, 40);
        [SerializeField] private int m_unitsPerWave = 40;
        [SerializeField] private float m_waveRotationalSpeed = 10.0f;
        [SerializeField] private float m_timeBetweenUnits = 0.5f;
        
        [SerializeField] private Unit m_unitBase;
        
        [SerializeField] private float m_startingUnitHealth = 4.0f;
        [SerializeField] private float m_startingUnitMoneyWorth = 1.0f;
        
        public AnimationCurve StatModifierCurve => m_statModifierCurve;
        public int UnitsPerWave => m_unitsPerWave;
        public float WaveRotationalSpeed => m_waveRotationalSpeed;
        public float TimeBetweenUnits => m_timeBetweenUnits;
        
        public Unit UnitBase => m_unitBase;
        
        public float UnitHealthAtWave(int waveNumber) => m_startingUnitHealth * m_statModifierCurve.Evaluate(waveNumber);
        public float UnitMoneyWorthAtWave(int waveNumber) => m_startingUnitMoneyWorth * m_statModifierCurve.Evaluate(waveNumber);
    }
}