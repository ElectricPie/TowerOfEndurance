using UnityEngine;

public class UnitMoney : MonoBehaviour
{
    [Min(1)] public float MoneyWorth = 1.0f;

    private void Awake()
    {
        PlayerMoney playerMoney = FindFirstObjectByType<PlayerMoney>();
        gameObject.GetComponent<Unit>().OnKilledEvent += _ =>
        {
            playerMoney.AddMoney(MoneyWorth);
        };
    }
}
