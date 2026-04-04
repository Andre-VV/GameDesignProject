# Weapon System Design Notes

## Current Direction

For the current version of the game, `IWeapon` is intended to represent ranged weapons only.
Each ranged weapon is responsible for firing one or more `Projectile` objects toward a given direction.

This keeps the first implementation simple and focused on the current gameplay goal:

- the player can attack enemies at range
- weapons use projectile-based behavior
- aiming is based on the direction from the player to the mouse position

## Responsibility Split

### `IWeapon`

`IWeapon` should describe a ranged weapon that can be equipped by the player and asked to fire.

Planned responsibilities:

- expose a weapon name
- expose the weapon type
- optionally expose whether hold-to-fire is supported
- receive an owner on equip
- clean up on unequip
- attempt to fire from an origin in a direction

`IWeapon` should not be responsible for:

- reading player input
- checking mouse position directly
- managing weapon slots
- updating UI
- moving projectiles after spawn

### `PlayerWeaponHandler`

`PlayerWeaponHandler` should manage player-facing weapon behavior.

Planned responsibilities:

- keep track of weapon slot 1 and weapon slot 2
- map left click to weapon 1
- map right click to weapon 2
- calculate aim direction from the player to the mouse
- pass origin and direction to the selected weapon logic
- support future pickup behavior

### `Projectile`

`Projectile` should be responsible for projectile behavior only.

Planned responsibilities:

- move with a configured speed
- destroy itself after a lifetime expires
- damage valid `IDamageable` targets on hit
- ignore the owner that fired it
- disappear on collision instead of piercing

### `playerController`

`playerController` should stay focused on movement.
Weapon handling should stay outside of the movement controller.

## Why `IWeapon` Is Ranged-Only

Melee attacks often want different hit detection methods such as:

- `Raycast`
- `BoxCast`
- `CircleCast`
- `OverlapBox`
- `OverlapCircle`

Because of that, forcing melee and ranged systems into the same interface too early may make the design awkward.
For now, it is cleaner to let `IWeapon` mean "projectile-based ranged weapon."

## Future Melee Direction

If melee combat is added later, it should likely use a separate interface instead of being forced into `IWeapon`.

Possible future names:

- `IMeleeAttack`
- `IMeleeWeapon`

That melee-side interface can own cast/overlap-based hit detection while ranged weapons continue to use projectiles.

## Planned Implementation Order

Recommended order for the first ranged weapon pass:

1. Finalize the shape of `IWeapon`
2. Implement `Projectile`
3. Implement `SingleShotWeapon`
4. Implement `PlayerWeaponHandler`
5. Add a shotgun-style secondary weapon later

## Current Scope

Current short-term goal:

- ranged weapons only
- projectile-based attacks only
- no ammo system
- player can hold up to two weapons
- weapon 1 uses left click
- weapon 2 uses right click

Future pickup behavior can overwrite weapon slot 2 for a simple first version.
