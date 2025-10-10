using System;
using Object = UnityEngine.Object;

namespace Ui.WidgetControllers
{
    public class StatWidgetController : WidgetController
    {
        public event Action<float> OnWaveProgressChanged = delegate { };
        
        public override void BindCallbacksToDependencies()
        {
            WaveSpawner waveSpawner = Object.FindFirstObjectByType<WaveSpawner>();
            waveSpawner.OnWaveProgressChangedEvent += value =>
            {
                OnWaveProgressChanged.Invoke(value);
            };
        }

        public override void BroadcastInitialValues()
        {
            WaveSpawner waveSpawner = Object.FindFirstObjectByType<WaveSpawner>();
            OnWaveProgressChanged.Invoke(waveSpawner.CurrentWaveProgress());
        }
    }
}