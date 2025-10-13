using UnityEngine;

namespace Waves.WaveSpawnPattern
{
    public class ConeSpawnPattern : SpawnPattern
    {
        [SerializeField]
        private Transform m_towerTransform;
        [SerializeField]
        private float m_spawnPointAngle;
        [SerializeField]
        private float m_spawnPointDistance;
        [SerializeField]
        private Vector3 m_spawnPointOffset;
        [SerializeField]
        private float m_unitSpawnPointVariation;
        [SerializeField]
        private float m_coneAngle;
        
        public override Vector3 GetRandomSpawnPoint()
        {
            float halfConeAngle = m_coneAngle / 2;
            float randomAngle = Random.Range(m_spawnPointAngle - halfConeAngle, m_spawnPointAngle + halfConeAngle);
            float randomDistance = Random.Range(m_spawnPointDistance - m_unitSpawnPointVariation, m_spawnPointDistance + m_unitSpawnPointVariation);
            
            return CalculatePositionAroundTower(randomAngle, randomDistance) + m_spawnPointOffset;
        }

        public override void DrawnArea()
        {
            // Draw spawn point
            Vector3 spawnPoint = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance) + m_spawnPointOffset;
            Gizmos.DrawSphere(spawnPoint, 0.5f);
            
            float halfConeAngle = m_coneAngle / 2;
            
            Vector3[] insideSpawn = {
                CalculatePositionAroundTower(m_spawnPointAngle - halfConeAngle, m_spawnPointDistance - m_unitSpawnPointVariation) + m_spawnPointOffset,
                CalculatePositionAroundTower(m_spawnPointAngle + halfConeAngle, m_spawnPointDistance - m_unitSpawnPointVariation) + m_spawnPointOffset
            };
            Vector3[] outsideSpawn = {
                CalculatePositionAroundTower(m_spawnPointAngle - halfConeAngle, m_spawnPointDistance + m_unitSpawnPointVariation) + m_spawnPointOffset,
                CalculatePositionAroundTower(m_spawnPointAngle + halfConeAngle, m_spawnPointDistance + m_unitSpawnPointVariation) + m_spawnPointOffset
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