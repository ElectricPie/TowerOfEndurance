# Abilities Reference

## Basic Attack

The tower's built-in attack. Not purchasable — always active from the start of the game.

**Trigger:** Internal fire loop (not routed through the ability trigger system)  

**Behaviour:**  
Fires a projectile at the oldest living enemy on each attack cycle. The projectile uses predictive lead aiming, calculating where the target will be when the shot arrives based on the wave's angular velocity and the projectile's speed. Projectiles are pooled (10 active at a time).

When a projectile hits it:
1. Applies the basic attack damage effect to the target.
2. Fires the `OnTargetHit` event, which triggers any `OnBasicAttackHit` abilities.

**Damage:** AttributeBacked — scales with tower damage stat.

---

## Artillery

A chance-on-hit ability that unloads a burst of damage onto multiple random targets simultaneously.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  
**Cost:** ~10 at level 1, scaling to ~55 at level 5

**Behaviour:**  
Each time the basic attack hits an enemy, Artillery rolls against its trigger chance. On a successful proc it selects 2 random units from all active waves and applies an instant damage effect to each. The same unit can be selected more than once in a single proc (known issue).

**Scaling:**

| Level | Trigger Chance | Damage (× Tower Damage) |
|-------|---------------|--------------------------|
| 1     | 12%           | 3.4×                     |
| 5     | 20%           | 5.0×                     |

Trigger chance and damage coefficient both scale linearly between these values.

---

## Poison Tip

Applies a damage-over-time effect to whatever the basic attack hits.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  
**Cost:** ~10 at level 1, scaling to ~55 at level 5

**Behaviour:**  
Every time the basic attack hits an enemy, that enemy is poisoned. The poison effect ticks at a fixed interval for a fixed duration, applying a percentage of the tower's current damage stat per tick. If the same enemy is hit again before the poison expires, the duration resets but the level stays locked at the level it was first applied (known issue — upgrading mid-fight won't take effect until the next fresh application).

**Scaling:**

| Level | Damage per Tick (× Tower Damage) | Tick Rate | Duration |
|-------|-----------------------------------|-----------|----------|
| 1     | 20%                               | 0.8s      | 5s       |
| 5     | 120%                              | 0.8s      | 5s       |

Tick rate and duration are fixed across all levels. Only the damage coefficient scales.

---

## Death Sentence

A timed ability that unleashes a rapid burst of hits on a single random enemy.

**Trigger:** `Timed`  
**Max Level:** 5  
**Cost:** ~20 at level 1, scaling to ~100 at level 5

**Behaviour:**  
Fires automatically on a timer. On activation it picks one random enemy and applies a periodic effect that hits extremely rapidly for a short window — producing a flurry of damage numbers. Intended as a visual spectacle ability; specific values may change. The random target is selected from all active waves.

**Scaling:**

| Level | Trigger Interval | Effect Duration | Ticks per Activation (approx.) |
|-------|-----------------|-----------------|-------------------------------|
| 1     | 8.5s            | 1s              | ~80                           |
| 5     | 5.0s            | 1s              | ~125                          |

Each tick deals 100% of tower damage regardless of level. Trigger interval and tick rate scale between the level 1 and level 5 values above.

---

## Rapid Fire

A timed ability that temporarily overclocks the tower's attack speed.

**Trigger:** `Timed`  
**Max Level:** 5  
**Cost:** ~10 at level 1, scaling to ~55 at level 5

**Behaviour:**  
Fires automatically on a timer. On activation it applies a multiply modifier to the tower's fire rate attribute, reducing the delay between shots to 25% of its normal value (effectively 4× the attack speed) for a fixed duration. The modifier is cleanly removed when the duration expires. Rapid Fire cannot activate again while already active — re-triggers during the active window are silently ignored.

**Scaling:**

| Level | Trigger Interval | Active Duration |
|-------|-----------------|-----------------|
| 1     | 10s             | 2s              |
| 5     | 10s             | 3.5s            |

The trigger interval is fixed at 10 seconds across all levels. Only the burst duration scales up.