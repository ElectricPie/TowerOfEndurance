using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Waves.WaveSpawnPattern
{
    [Serializable]
    public class ConeSpawnPattern : SpawnPattern
    {
        [SerializeField]
        private Transform m_towerTransform;
        [SerializeField]
        private float m_spawnPointAngle = 35.0f;
        [SerializeField]
        private float m_spawnPointWidth = 4.5f;
        [SerializeField]
        private Vector3 m_spawnPointOffset = new Vector3(0.0f, -0.8f, 0.0f);
        [SerializeField]
        private float m_unitSpawnDistance = 2.5f;
        [SerializeField]
        private float m_coneAngle = 40.0f;
        [SerializeField, Tooltip("Higher values make it more likely to spawn closer to the outside of the cone")] 
        private float m_biasWeighting = 2.0f;
        
        public override Vector3 GetRandomSpawnPoint()
        {
            float halfConeAngle = m_coneAngle / 2;
            float randomAngle = Random.Range(m_spawnPointAngle - halfConeAngle, m_spawnPointAngle + halfConeAngle);
            
            float weighting = Mathf.Pow(Random.value, 1.0f / m_biasWeighting);
            float randomDistance = Mathf.Lerp(m_spawnPointWidth - m_unitSpawnDistance, m_spawnPointWidth + m_unitSpawnDistance, weighting);
            
            return CalculatePositionAroundTower(randomAngle, randomDistance) + m_spawnPointOffset;
        }

        public override void DrawnArea()
        {
            // Draw spawn point
            Vector3 spawnPoint = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointWidth) + m_spawnPointOffset;
            Gizmos.DrawSphere(spawnPoint, 0.5f);
            
            float halfConeAngle = m_coneAngle / 2;
            
            Vector3[] insideSpawn = {
                CalculatePositionAroundTower(m_spawnPointAngle - halfConeAngle, m_spawnPointWidth - m_unitSpawnDistance) + m_spawnPointOffset,
                CalculatePositionAroundTower(m_spawnPointAngle + halfConeAngle, m_spawnPointWidth - m_unitSpawnDistance) + m_spawnPointOffset
            };
            Vector3[] outsideSpawn = {
                CalculatePositionAroundTower(m_spawnPointAngle - halfConeAngle, m_spawnPointWidth + m_unitSpawnDistance) + m_spawnPointOffset,
                CalculatePositionAroundTower(m_spawnPointAngle + halfConeAngle, m_spawnPointWidth + m_unitSpawnDistance) + m_spawnPointOffset
            };
            
            // Draw cone boundaries
            Gizmos.DrawLine(insideSpawn[0], outsideSpawn[0]);
            Gizmos.DrawLine(insideSpawn[1], outsideSpawn[1]);
            Gizmos.DrawLine(insideSpawn[0], insideSpawn[1]);
            Gizmos.DrawLine(outsideSpawn[0], outsideSpawn[1]);
        }
        
        private Vector3 CalculatePositionAroundTower(float angleInDegrees, float distanceFromTower)
        {
            float angleInRadians = angleInDegrees * (Mathf.PI / 180);
        
            float x = distanceFromTower * Mathf.Cos(angleInRadians);
            float z = distanceFromTower * Mathf.Sin(angleInRadians);
        
            return m_towerTransform.position + new Vector3(x, 0, z);
        }
    }
}