using System;

namespace Ui.WidgetControllers
{
    public class LivesWidgetController : WidgetController
    {
        public event Action<int> OnCurrentLivesChanged = delegate { };
        public event Action<int> OnMaxLivesChanged = delegate { };

        public override void BindCallbacksToDependencies()
        {
            PlayerLivesManager livesManager = UnityEngine.Object.FindFirstObjectByType<PlayerLivesManager>();
            livesManager.OnCurrentLivesChangedEvent += newValue => { OnCurrentLivesChanged.Invoke(newValue); };
            livesManager.OnMaxLivesChangedEvent += newValue => { OnMaxLivesChanged.Invoke(newValue); };
        }

        public override void BroadcastInitialValues()
        {
            PlayerLivesManager livesManager = UnityEngine.Object.FindFirstObjectByType<PlayerLivesManager>();
            OnMaxLivesChanged.Invoke(livesManager.MaxLives);
            OnCurrentLivesChanged.Invoke(livesManager.CurrentLives);
        }
    }
}