using Ui.WidgetControllers;
using UnityEngine;

namespace Ui
{
    public abstract class Widget : MonoBehaviour
    {
        protected WidgetController WidgetController;
        
        public virtual void SetWidgetController(WidgetController controller)
        {
            WidgetController = controller;
        }
    }
}