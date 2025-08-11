using System;
using Player;
using UnityEngine;

[RequireComponent(typeof(UnitHealth))]
public class UnitMoney : MonoBehaviour
{
    [Min(1)] public float MoneyWorth = 1.0f;

    private void Awake()
    {
        gameObject.GetComponent<UnitHealth>().OnKilledEvent += (_, killer) =>
        {
            PlayerOwnedObject player = killer.GetComponent<PlayerOwnedObject>();
            if (player == null)
                return;
                    
            player.Owner.MoneyManager.AddMoney(MoneyWorth);
        };
    }
}
