using UnityEngine;

namespace Waves.WaveSpawnPattern
{
    public class LineSpawnPattern : SpawnPattern
    {
        [SerializeField]
        private Transform m_transform;
        [SerializeField]
        private float m_spawnPointAngle;
        [SerializeField]
        private float m_spawnPointDistance;
        [SerializeField]
        private Vector3 m_spawnPointOffset;
        [SerializeField]
        private float m_unitSpawnPointVariation;
        
        public override Vector3 GetRandomSpawnPoint()
        {
            Vector3 insideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance - m_unitSpawnPointVariation);
            insideOfSpawn += m_spawnPointOffset;
            Vector3 outsideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance + m_unitSpawnPointVariation);
            outsideOfSpawn += m_spawnPointOffset;
            float randomLerpValue = Random.Range(0.0f, 1.0f);
            return Vector3.Lerp(insideOfSpawn, outsideOfSpawn, randomLerpValue);
        }

        public override void DrawnArea()
        {
            // Draw spawn point
            Vector3 spawnPoint = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance) + m_spawnPointOffset;
            Gizmos.DrawSphere(spawnPoint, 0.5f);
        
            // Draw spawn variation
            Vector3 insideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance - m_unitSpawnPointVariation);
            insideOfSpawn += m_spawnPointOffset;
            Vector3 outsideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance + m_unitSpawnPointVariation);
            outsideOfSpawn += m_spawnPointOffset;
            Gizmos.DrawLine(insideOfSpawn, outsideOfSpawn);
        }
        
        private Vector3 CalculatePositionAroundTower(float angleInDegrees, float distanceFromTower)
        {
            float angleInRadians = angleInDegrees * (Mathf.PI / 180);
        
            float x = distanceFromTower * Mathf.Cos(angleInRadians);
            float z = distanceFromTower * Mathf.Sin(angleInRadians);
        
            return m_transform.position + new Vector3(x, 0, z);
        }
    }
}