# Planned Abilities

Abilities planned for future implementation, based on the WC3 custom map [Random Ability TD](https://www.epicwar.com/maps/288604/). Stats and behaviours may change during implementation. Entries marked **TBD** require further design clarity.

---

## Russian Roulette

A timed ability that gambles with a random enemy's health — equal odds to devastate it or restore it.

**Trigger:** `Timed`  
**Max Level:** 5  

**Behaviour:**  
Fires automatically on a timer. On activation it selects one random enemy and rolls a 50/50 chance. A damage roll deals between 20% and 100% of the target's maximum health as damage; a heal roll restores between 20% and 100% of the target's maximum health. The percentage is rolled randomly within that range each activation. The ability fires progressively faster with each level.

**Scaling:**

| Level | Trigger Interval |
|-------|-----------------|
| 1     | 5.5s            |
| 5     | 3.5s            |

Damage/heal magnitude (20–100% of max health) is fixed across all levels. Only the trigger interval scales linearly between these values.

---

## Laser Mass

An on-hit ability that turns every basic attack into an AOE strike.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  

**Behaviour:**  
Every basic attack hit triggers an instant AOE damage effect centred on the target, hitting the target and all surrounding units. Unlike other proc-based abilities there is no trigger chance — it fires on every hit.

**Scaling:**

| Level | Damage (× Tower Damage) |
|-------|------------------------|
| 1     | 0.8×                   |
| 5     | 1.2×                   |

Trigger chance is fixed at 100%. Damage scales linearly between these values.

---

## Tudududududu

A passive trade-off ability that massively increases tower attack speed at the cost of reduced damage per hit.

**Trigger:** Passive — always active  
**Max Level:** 5  

**Behaviour:**  
Permanently applies a +500% attack speed modifier and a negative damage modifier to the tower. The attack speed bonus is fixed across all levels; the damage penalty improves slightly each level. Net DPS typically increases due to the large attack speed gain.

**Scaling:**

| Level | Attack Speed Bonus | Damage Modifier |
|-------|-------------------|----------------|
| 1     | +500%             | −64%           |
| 5     | +500%             | −56%           |

Attack speed bonus is fixed. Damage modifier improves by +2% per level (the penalty shrinks).

---

## Summon Hwaryong

A chance-on-hit ability that summons a dragon to unleash an AOE blast on the target and nearby units.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  

**Behaviour:**  
Each basic attack hit rolls against the trigger chance. On a successful proc, a dragon is summoned that fires at the target, dealing damage to the target and all surrounding units.

**Scaling:**

| Level | Trigger Chance | Damage (× Tower Damage) |
|-------|---------------|--------------------------|
| 1     | 12%           | 1.6×                     |
| 5     | 20%           | 2.0×                     |

Both trigger chance and damage scale linearly between these values.

---

## Twin Twister

A passive ability that summons two twisters which continuously damage and slow any unit they pass through.

**Trigger:** Passive — always active once acquired  
**Max Level:** 5  

**Behaviour:**  
Upon acquisition, two twisters are permanently summoned. They move around the play area; any unit they pass through takes damage and is slowed. Both twisters persist for the rest of the game.

**Scaling:**

| Level | Damage (× Tower Damage per hit) | Slow |
|-------|---------------------------------|------|
| 1     | 0.6×                            | 20%  |
| 5     | 1.4×                            | 20%  |

Slow is fixed across all levels. Damage scales linearly between these values.

---

## Thunderbolt

A chance-on-hit ability that fires a lightning bolt that chains through multiple enemies.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  

**Behaviour:**  
Each basic attack hit rolls against the trigger chance. On a successful proc, a lightning bolt strikes the target and then bounces to up to 4 additional units, dealing damage and applying a slow to each. The same unit may be hit by multiple bounces.

**Scaling:**

| Level | Trigger Chance | Damage (× Tower Damage) | Slow |
|-------|---------------|--------------------------|------|
| 1     | 12%           | 1.6×                     | 50%  |
| 5     | 20%           | 2.4×                     | 50%  |

Slow is fixed across all levels. Trigger chance and damage both scale linearly between these values.

---

## Striker

An on-hit proc that stuns the target and deals bonus damage on any attack.

**Trigger:** Any attack  
**Max Level:** 5  

**Behaviour:**  
Any attack has a fixed 15% chance to proc. On proc, the target is stunned for 1 second and takes bonus damage. Trigger chance and stun duration are fixed across all levels; only damage scales.

**Scaling:**

| Level | Trigger Chance | Damage (× Tower Damage) | Stun Duration |
|-------|---------------|--------------------------|---------------|
| 1     | 15%           | 1.15×                    | 1s            |
| 5     | 15%           | 1.35×                    | 1s            |

Trigger chance and stun duration are fixed. Damage scales linearly between these values.

---

## Phoenix

Grants the player additional lives and applies a damage effect upon defeat — exact mechanism TBD.

**Trigger:** Passive  
**Max Level:** 5  

**Behaviour:**  
Grants extra lives on acquisition. Also applies a "Damage upon Defeat" effect — the precise trigger and target of this damage is TBD.

**Scaling:**

| Level | Extra Lives | Damage upon Defeat (× Tower Damage) |
|-------|-------------|-------------------------------------|
| 1     | 3           | 1.6×                                |
| 5     | 7           | 2.0×                                |

Both values scale linearly between these values. Damage upon Defeat application mechanic is TBD.

---

## Avenger

Builds stacking bonus damage on the current target with each consecutive hit, rewarding sustained fire.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  

**Behaviour:**  
Each time the basic attack hits the same target, a stack of bonus damage is added to subsequent hits against that target, up to a maximum. Stacks are tracked per target. Whether stacks reset when switching targets is TBD.

**Scaling:**

| Level | Damage Increase per Stack | Maximum Stacks |
|-------|--------------------------|----------------|
| 1     | 5%                       | 12             |
| 5     | 5%                       | 28             |

Damage per stack is fixed. Maximum stacks scale linearly between these values.

---

## Machine Gun

A passive ability that permanently increases the tower's attack speed.

**Trigger:** Passive — always active  
**Max Level:** 5  

**Behaviour:**  
Applies a flat additive modifier to the tower's attack speed attribute. Stacks with other attack speed modifiers.

**Scaling:**

| Level | Attack Speed Bonus |
|-------|--------------------|
| 1     | +30%               |
| 5     | +70%               |

Scales linearly between these values.

---

## Orbital

Summons an orb that orbits the tower and periodically attacks nearby units with a beam.

**Trigger:** Passive — always active once acquired  
**Max Level:** 5  

**Behaviour:**  
Upon acquisition, an orb is permanently summoned that orbits the tower. It periodically fires a beam at units within range, dealing damage independently of the tower's own attacks. Persists for the rest of the game.

**Scaling:**

| Level | Orb Damage (× Tower Damage) | Orb Attack Interval |
|-------|-----------------------------|---------------------|
| 1     | 0.8×                        | 0.6s                |
| 5     | 1.2×                        | 0.4s                |

Both values scale linearly between these values.

---

## Roar of the Wild

Amplifies the damage output of all summoned units and abilities.

**Trigger:** Applies when summons deal damage  
**Max Level:** 5  

**Behaviour:**  
Whenever a summoned unit or ability (e.g. Hwaryong, Twin Twister, Orbital, Autovan, Nature's Army, Gather the Troops) deals damage, that damage is amplified by this ability's bonus. Functions as a passive multiplier applied at the moment summon damage is dealt.

**Scaling:**

| Level | Summons Damage Bonus |
|-------|---------------------|
| 1     | +60%                |
| 5     | +140%               |

Scales linearly between these values.

---

## Assassin

A chance proc that multiplicatively amplifies final damage, applied after all other modifiers.

**Trigger:** Any attack  
**Max Level:** 5  

**Behaviour:**  
Any attack has a fixed 15% chance to proc. On proc, the total damage dealt is multiplied by the ability's coefficient as the last step in the damage calculation pipeline — after all additive and other modifiers have already been applied.

**Scaling:**

| Level | Trigger Chance | Final Damage Multiplier |
|-------|---------------|------------------------|
| 1     | 15%           | ×1.30                  |
| 5     | 15%           | ×1.70                  |

Trigger chance is fixed. Final damage multiplier scales linearly between these values.

---

## Overdrive

A timed ability that temporarily boosts tower damage.

**Trigger:** `Timed`  
**Max Level:** 5  

**Behaviour:**  
Fires automatically on a fixed 10-second interval. On activation it applies a +200% damage modifier to the tower for the active duration. The modifier is cleanly removed when the duration expires. Cannot re-activate while already active.

**Scaling:**

| Level | Trigger Interval | Damage Bonus | Active Duration |
|-------|-----------------|--------------|-----------------|
| 1     | 10s             | +200%        | 1.9s            |
| 5     | 10s             | +200%        | 3.1s            |

Trigger interval and damage bonus are fixed. Only active duration scales linearly between these values.

---

## Nature's Army

Summons three treants that autonomously attack enemies.

**Trigger:** Passive — treants are always active once acquired  
**Max Level:** 5  

**Behaviour:**  
Upon acquisition, three treants are permanently summoned. They independently target and attack enemies at their own attack rate. Treants persist for the rest of the game.

**Scaling:**

| Level | Treant Damage (× Tower Damage) | Treant Attack Interval |
|-------|-------------------------------|------------------------|
| 1     | 0.4×                          | 1.2s                   |
| 5     | 0.6×                          | 0.8s                   |

Both values scale linearly between these values.

---

## Stone Emperor

A chance-on-hit ability that sends 8 stone projectiles at random enemies, dealing damage and slowing them.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  

**Behaviour:**  
Each basic attack hit rolls against the trigger chance. On proc, 8 stone projectiles are launched at random enemies, each dealing damage and applying a slow. Multiple projectiles can hit the same enemy.

**Scaling:**

| Level | Trigger Chance | Damage per Projectile (× Tower Damage) | Slow per Hit |
|-------|---------------|----------------------------------------|--------------|
| 1     | 12%           | 0.8×                                   | 50%          |
| 5     | 20%           | 1.2×                                   | 50%          |

Slow per hit is fixed. Trigger chance and damage scale linearly between these values.

---

## Loss of Will

Increases tower damage on every attack — exact accumulation mechanic TBD.

**Trigger:** Any attack  
**Max Level:** 5  

**Behaviour:**  
Whether this accumulates infinitely per hit over the course of a run (a growing stacking buff) or functions as a flat per-level passive modifier is TBD and requires design clarification.

**Scaling:**

| Level | Damage Increase |
|-------|----------------|
| 1     | +0.06%         |
| 5     | +0.14%         |

Scales linearly between these values. Accumulation mechanic TBD.

---

## Immolation

A timed ability that strikes a random location with a damaging AOE blast.

**Trigger:** `Timed`  
**Max Level:** 5  

**Behaviour:**  
Fires automatically on a fixed 5-second timer. On activation it selects a random location and deals damage to any unit at or near that point. The trigger interval is fixed; only damage scales with level.

**Scaling:**

| Level | Trigger Interval | Damage (× Tower Damage) |
|-------|-----------------|--------------------------|
| 1     | 5s              | 6.0×                     |
| 5     | 5s              | 14.0×                    |

Trigger interval is fixed. Damage scales linearly between these values.

---

## Weapon Master *(TBD)*

Summons weapons that strike all active units on proc.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  

**Behaviour:**  
Details TBD. On proc, weapons are summoned that hit all active units for damage. Full design intent requires further clarification.

**Scaling:**

| Level | Trigger Chance | Damage (× Tower Damage) |
|-------|---------------|--------------------------|
| 1     | 8%            | 1.3×                     |
| 5     | 8%            | 1.7×                     |

Trigger chance is fixed. Damage scales linearly between these values. Full design TBD.

---

## Tower of Pandora

A timed ability that randomly shifts the tower's final damage output each cycle.

**Trigger:** `Timed`  
**Max Level:** 5  

**Behaviour:**  
Fires automatically on a fixed 10-second timer. Each activation rolls against the trigger chance. On a successful roll, the tower's final damage output is randomly set to between 20% and 500% of its current base, applying until the next activation replaces it.

**Scaling:**

| Level | Trigger Interval | Chance to Modify |
|-------|-----------------|-----------------|
| 1     | 10s             | 24%             |
| 5     | 10s             | 32%             |

Trigger interval is fixed. Proc chance scales linearly between these values.

---

## Tower of Greed

A passive ability that increases money earned per kill and units spawned per wave.

**Trigger:** Passive — always active  
**Max Level:** 5  

**Behaviour:**  
Applies a flat additive bonus to the money rewarded on each unit kill. Also increases the number of non-boss units spawned per wave, providing more kill opportunities at the cost of increased pressure.

**Scaling:**

| Level | Extra Money per Kill | Extra Spawns per Wave |
|-------|---------------------|-----------------------|
| 1     | +3                  | +3                    |
| 5     | +7                  | +7                    |

Both values scale linearly between these values.

---

## Autovan

A passive summon that deploys a van to continuously push through and damage enemies.

**Trigger:** Passive — always active once acquired  
**Max Level:** 5  

**Behaviour:**  
Upon acquisition, a van is permanently deployed. It moves through the play area dealing continuous damage per second to any unit it contacts. Persists for the rest of the game.

**Scaling:**

| Level | Damage (× Tower Damage per second) |
|-------|-----------------------------------|
| 1     | 0.7×                              |
| 5     | 1.1×                              |

Scales linearly between these values.

---

## Wave Magic

A chance-on-hit ability that sends AOE waves radiating outward from the tower.

**Trigger:** `OnBasicAttackHit`  
**Max Level:** 5  

**Behaviour:**  
Each basic attack hit rolls against the trigger chance. On a successful proc, waves radiate outward from the tower's position, dealing damage and applying a slow to all units they pass through.

**Scaling:**

| Level | Trigger Chance | Damage (× Tower Damage) | Slow |
|-------|---------------|--------------------------|------|
| 1     | 6%            | 1.3×                     | 50%  |
| 5     | 10%           | 1.7×                     | 50%  |

Slow is fixed across all levels. Trigger chance and damage scale linearly between these values.

---

## Gather the Troops

Summons three knights that autonomously attack enemies.

**Trigger:** Passive — knights are always active once acquired  
**Max Level:** 5  

**Behaviour:**  
Upon acquisition, three knights are permanently summoned. They independently target and attack enemies at their own attack rate. Knights persist for the rest of the game.

**Scaling:**

| Level | Knight Damage (× Tower Damage) | Knight Attack Interval |
|-------|-------------------------------|------------------------|
| 1     | 0.8×                          | 1.1s                   |
| 5     | 1.2×                          | 0.9s                   |

Both values scale linearly between these values.