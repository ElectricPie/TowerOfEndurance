using AbilitySystem.Ability;

namespace Ui.Tooltip.Ability
{
    public interface IAbilityTooltipInterface
    {
        public AbilityScriptableObject GetAbilityData();
        public float GetAbilityLevel();
    }
}