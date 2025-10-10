using System;

namespace Ui.WidgetControllers
{
    public class WaveWidgetController : WidgetController
    {
        public event Action<float> OnWaveProgressChanged = delegate { };
        public event Action<int> OnWaveNumberChanged = delegate { };
        
        public override void BindCallbacksToDependencies()
        {
            WaveSpawner waveSpawner = UnityEngine.Object.FindFirstObjectByType<WaveSpawner>();
            
            waveSpawner.OnWaveProgressChangedEvent += value =>
            {
                OnWaveProgressChanged.Invoke(value);
            };
            waveSpawner.OnWaveStartedEvent += waveNumber =>
            {
                OnWaveNumberChanged.Invoke(waveNumber);
            };
        }

        public override void BroadcastInitialValues()
        {
            WaveSpawner waveSpawner = UnityEngine.Object.FindFirstObjectByType<WaveSpawner>();
            OnWaveProgressChanged.Invoke(waveSpawner.CurrentWaveProgress());
            OnWaveNumberChanged.Invoke(waveSpawner.CurrentWave);
        }
    }
}