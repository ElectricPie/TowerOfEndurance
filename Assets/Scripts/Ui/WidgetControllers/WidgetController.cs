namespace Ui.WidgetControllers
{
    public abstract class WidgetController
    {
        public abstract void BindCallbacksToDependencies();
        public abstract void BroadcastInitialValues();
    }
}