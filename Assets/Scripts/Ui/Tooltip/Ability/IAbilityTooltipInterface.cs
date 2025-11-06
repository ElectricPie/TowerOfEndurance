using AbilitySystem.Ability;

namespace Ui.Tooltip.Ability
{
    public interface IAbilityTooltipInterface
    {
        public AbilityScriptableObject GetAbility();
        public int GetAbilityLevel();
    }
}