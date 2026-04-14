# Ability Implementation Requirements

What needs to be built before the planned abilities can be implemented. Based on analysis of the current ability system state.

---

## 1. Passive / On-Acquisition Trigger

**Affects:** Machine Gun, Tudududududu, Tower of Greed, Phoenix, Orbital, Twin Twister, Nature's Army, Autovan, Gather the Troops

`TowerAbilities.AddAbility` throws `ArgumentOutOfRangeException` for any unknown trigger. There is no path for abilities that activate once when purchased and stay on permanently. A `Passive` enum value needs adding to `AbilityTrigger`, and `AddAbility` needs to call `TryActivate(null)` immediately when one is added.

---

## 2. `OnAnyDamage` Fires Only from Basic Attack Hit

**Affects:** Striker, Assassin, Loss of Will

In `TowerAbilities.Start`, the `OnAnyDamage` list is fired inside the `OnTargetHit` callback — meaning it only triggers when the basic attack hits. Striker and Assassin need to fire on *any* attack, including ability damage. This requires ability instances to fire the `OnAnyDamage` list when they deal damage, which needs a callback or event passed into them at init time.

---

## 3. Slow / Speed Attribute on Units

**Affects:** Thunderbolt, Stone Emperor, Wave Magic, Twin Twister

`UnitAttributeSet` only has `IncomingDamage` and `Health`. Units have no speed attribute and their movement speed comes from `TowerWaves.CurrentWaveRpm`, which is set externally and not driven by per-unit attributes. To support slows:
- Add a `Speed` attribute to `UnitAttributeSet`
- Unit movement must read from that attribute
- Slows applied as `Infinite` negative `Multiply` modifiers, removed after a duration

---

## 4. Stun System

**Affects:** Striker

No stun exists anywhere. Options are a dedicated `Stunned` boolean on `Unit`, or a `Speed` attribute (from above) set to 0 via `Override`. Either way, the stun needs timed removal.

---

## 5. AOE / Radius-Based Targeting

**Affects:** Laser Mass, Summon Hwaryong, Wave Magic, Immolation, Thunderbolt (splash)

`TowerWaves` has `GetAllUnits()`, `GetRandomUnit()`, and `GetOldestUnit()` but nothing position-based. Units orbit the tower so they have world positions — a `GetUnitsInRadius(Vector3 position, float radius)` method needs adding to `TowerWaves`.

---

## 6. Chain / Bounce Logic

**Affects:** Thunderbolt

No bounce system exists. This is self-contained in the ability instance — pick a target, apply damage, pick another unit not already hit, repeat up to N times. No new system needed, just new ability code.

---

## 7. Summon System

**Affects:** Orbital, Twin Twister, Nature's Army, Autovan, Gather the Troops, Summon Hwaryong (dragon)

The largest gap. There is no concept of a persistent entity spawned by an ability that attacks independently. Needs:
- A `SummonInstance` base class (or `MonoBehaviour`) with its own attack coroutine and targeting logic
- Summons must read tower damage via `AttributeBacked` magnitude so they scale with the tower
- A `SummonManager` or list on the tower to track active summons (so Roar of the Wild can amplify their damage)
- Each summon type (orbiting beam, patrolling van, melee knight, treant) has different movement and targeting behaviour

---

## 8. Final Damage Multiplier Stage

**Affects:** Assassin, Tower of Pandora

The current damage recalculation formula is `(BaseValue + addSum) * multiplyProduct`. There is no post-modifier stage. Assassin must multiply *after* all other modifiers are applied. Options:
- Add a `FinalMultiply` to `ModifierOperation` and handle it as a second pass in `RecalculateAttribute`
- Or handle it in ability code by reading the current damage value and applying an `Override` — simpler but couples ability logic to attribute internals

---

## 9. Per-Target Stack Tracking

**Affects:** Avenger

No stacking system exists. The ability instance needs a `Dictionary<Unit, int>` tracking consecutive hit counts per target, a way to detect when the target changes (via the `target` parameter in `TryActivate`), and a dynamic infinite modifier on tower damage that updates each time the stack count changes.

---

## 10. Health-Percentage Damage / Heal

**Affects:** Russian Roulette

`AttributeBacked` magnitude can reference a target attribute but has no random-range support. Russian Roulette needs `Random.Range(0.2f, 1f) * target.MaxHealth`. Cleanest handled in ability code rather than a new magnitude type.

Healing also has no precedent — `UnitAttributeSet` reduces health when `IncomingDamage` increases. A negative `Add` modifier to `IncomingDamage` would restore health, but max-health clamping is not implemented and would need to be added.

---

## 11. Tower of Greed Integration

**Affects:** Tower of Greed

Requires two hooks outside the ability system:
- `PlayerMoney` needs a settable bonus-per-kill value that abilities can write to
- `GeneratedWaveSpawner` / `TowerWaves` needs an extra-unit-count that abilities can increment

---

## 12. Phoenix Extra Lives

**Affects:** Phoenix

`PlayerLivesManager` has no public "grant lives" API. A `GrantLives(int count)` method needs adding and the ability instance calls it on activation.

---

## Summary

| What to build | Abilities unblocked |
|---|---|
| `Passive` trigger + `AddAbility` activation | Machine Gun, Tudududududu, Tower of Greed, Phoenix, Orbital, Twin Twister, Nature's Army, Autovan, Gather the Troops |
| `OnAnyDamage` fires from ability hits | Striker, Assassin, Loss of Will |
| Speed attribute + slow/stun support | Thunderbolt, Stone Emperor, Wave Magic, Twin Twister, Striker |
| `GetUnitsInRadius` on TowerWaves | Laser Mass, Summon Hwaryong, Wave Magic, Immolation |
| Summon system | Orbital, Twin Twister, Nature's Army, Autovan, Gather the Troops, Summon Hwaryong |
| Final damage multiplier stage | Assassin, Tower of Pandora |
| Per-target stack tracking | Avenger |
| Health-% damage + heal + max-health clamp | Russian Roulette |
| Tower of Greed hooks (money/spawn) | Tower of Greed |
| `PlayerLivesManager.GrantLives` | Phoenix |
| Chain/bounce (ability code only) | Thunderbolt |

Roar of the Wild, Loss of Will, and Weapon Master are also blocked on design clarity before implementation can start.

---

## Implementation Priority

Each item ranked by effort vs. abilities fully enabled. "Fully enabled" means no other dependencies remain once this is done.

### 1. Passive Trigger *(Very Low effort)*
Add `Passive` to `AbilityTrigger` and call `TryActivate(null)` in `AddAbility`. Under 10 lines.
- **Fully enables:** Machine Gun, Tudududududu
- **Unblocks trigger support for:** Tower of Greed, Phoenix, Orbital, Twin Twister, Nature's Army, Autovan, Gather the Troops

### 2. `GetUnitsInRadius` on TowerWaves *(Very Low effort)*
Iterate `GetAllUnits()` and filter by distance. ~10 lines.
- **Fully enables:** Laser Mass, Immolation
- **Partially unblocks:** Summon Hwaryong, Wave Magic, Thunderbolt

### 3. Final Damage Multiplier Stage *(Low–Medium effort)*
Add `FinalMultiply` to `ModifierOperation`, handle as a second pass after `(base + add) * multiply` in `AttributeSet.RecalculateAttribute`.
- **Fully enables:** Assassin, Tower of Pandora

### 4. Slow / Speed Attribute + Stun *(Medium effort)*
Add `Speed` to `UnitAttributeSet`, wire unit movement to read it, implement timed `Infinite` modifier removal. Stun piggybacks as an `Override` to 0.
- **Fully enables:** Stone Emperor, Striker
- **Partially unblocks:** Thunderbolt (still needs chain code), Wave Magic (still needs `GetUnitsInRadius`), Twin Twister (still needs summon system)

### 5. Chain / Bounce Logic *(Low effort)*
Self-contained in Thunderbolt's ability instance — track already-hit units, pick next random unhit unit, repeat N times. Requires item 4 (slow) to complete Thunderbolt.
- **Fully enables:** Thunderbolt *(when combined with item 4)*

### 6. Summon System *(High effort)*
New `SummonInstance` base, per-summon attack coroutines, targeting, `AttributeBacked` damage scaling, and a manager to track active summons.
- **Fully enables:** Orbital, Nature's Army, Gather the Troops
- **Partially enables:** Autovan, Twin Twister (need collision/movement on top), Summon Hwaryong (needs dragon visual)

### 7. Tower of Greed Hooks *(Low effort)*
Add `BonusMoneyPerKill` to `PlayerMoney` kill reward path and `ExtraSpawnCount` to the wave spawner. Requires item 1 (Passive trigger).
- **Fully enables:** Tower of Greed

### 8. `PlayerLivesManager.GrantLives` *(Very Low effort)*
Add a single public method. Requires item 1 (Passive trigger).
- **Fully enables:** Phoenix *(lives-granting only — Damage upon Defeat is TBD)*

### 9. `OnAnyDamage` Fires from Ability Hits *(Low–Medium effort)*
Pass a callback or event into `AbilityInitData` so ability instances can fire the `OnAnyDamage` list when they deal damage.
- **Fully enables:** Nothing on its own — unblocks correctness for Striker, Assassin, and Loss of Will once their other dependencies are met

### 10. Per-Target Stack Tracking — Avenger *(Medium effort)*
`Dictionary<Unit, int>` in the ability instance, target-change detection, dynamic modifier updates.
- **Fully enables:** Avenger

### 11. Health-Percentage Damage / Heal — Russian Roulette *(Medium effort)*
Random range computed in ability code. Max-health clamping needs adding to `UnitAttributeSet` to prevent health exceeding its base value.
- **Fully enables:** Russian Roulette

---

### Recommended Order

| Priority | What | Abilities fully enabled |
|----------|------|------------------------|
| 1 | Passive trigger | Machine Gun, Tudududududu |
| 2 | `GetUnitsInRadius` | Laser Mass, Immolation |
| 3 | Final damage multiplier | Assassin, Tower of Pandora |
| 4 | Slow + stun system | Stone Emperor, Striker |
| 5 | Chain/bounce code | Thunderbolt *(with #4)* |
| 6 | Summon system | Orbital, Nature's Army, Gather the Troops |
| 7 | Tower of Greed hooks | Tower of Greed |
| 8 | `GrantLives` | Phoenix *(partial)* |
| 9 | `OnAnyDamage` from ability hits | Unblocks Striker/Assassin correctness |
| 10 | Per-target stacking | Avenger |
| 11 | Health-% + heal | Russian Roulette |