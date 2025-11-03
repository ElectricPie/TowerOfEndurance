using UnityEngine;

public class UnitMoney : MonoBehaviour
{
    [Min(1)] public float MoneyWorth = 1.0f;

    private void Awake()
    {
        gameObject.GetComponent<Unit>().OnKilledEvent += _ =>
        {
            PlayerMoney playerMoney = FindFirstObjectByType<PlayerMoney>();
            playerMoney.AddMoney(MoneyWorth);
        };
    }
}
