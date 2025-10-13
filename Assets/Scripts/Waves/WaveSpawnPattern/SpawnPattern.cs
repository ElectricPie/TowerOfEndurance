using UnityEngine;

namespace Waves.WaveSpawnPattern
{
    public abstract class SpawnPattern
    {
        public abstract Vector3 GetRandomSpawnPoint();
        public abstract void DrawnArea();
    }
}