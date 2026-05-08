# ITCS-5231-001-Group-4-Repository

# ZomBlob
ZomBlob is a fast-paced top-down zombie shooter built in Unity, featuring mouse-based aiming, right-click zoom for improved accuracy, wave-based enemy encounters, weapon pickups, ammo management, and survival-focused combat. The game includes seven usable weapons: six firearms and a default baseball bat, with each weapon supported by custom firing logic, sound effects, reload behavior, bullet tracers, muzzle flashes, casing ejections, magazine drops, and weapon-specific spread behavior that rewards standing still and controlled shooting. The enemy system supports wave spawning, boss encounters, ammo drops, player damage, and intelligent zombie movement designed to pressure the player even in open combat spaces.

## Team Roles
### Sirjan Gyawali — Player / Weapon / Combat
Player combat experience, including movement, aiming, health, weapon logic, inventory slots, ammo handling, reload behavior, melee combat, weapon VFX, muzzle flashes, shell casings, magazine drops, tracers, weapon spread, and gun/reload audio. Turned the combat system from a basic shooting prototype into a polished, data-driven weapon system with distinct behavior for each gun. Also handled major debugging across player systems, weapon prefabs, physics behavior, ammo logic, and combat feedback.

### Jie Zhou — Enemy / Wave / Spawn
Enemy-side gameplay systems, including zombie movement, NavMesh spawning, wave progression, boss spawning, enemy damage behavior, health handling, ragdoll/death behavior, ammo drop chance, and wave counter support. Work helped connect the combat systems to the survival loop by giving the player active threats and structured enemy waves. Also drastically improved enemy behavior, player attack detection, spawn safety, and wave pacing.

### Ian Holt — Map / UI / Animation
Visual and presentation side of the project, including the main map, terrain/environment design, title screen, pause menu, UI elements, camera sweep/menu presentation, health bar visuals, animation experimentation, and overall scene polish. Work helped move the project beyond the test arena by building out the playable environment and interface structure. He also contributed visual updates, audio/menu integration, and late-stage polish for the final playable build.

## Key Features
- Fast-paced top-down zombie survival gameplay
- Right-click zoom for increased aiming precision
- Seven weapons total: six guns plus default baseball bat
- Weapon-specific ammo types and shared ammo pools, such as Glock and MP5 both using 9mm
- Dynamic spread system where accuracy worsens while moving or firing rapidly
- Bullet tracers, muzzle flashes, casing ejections, reload sounds, gunshot sounds, and physical magazine drops
- Wave-based zombie spawning with boss encounters
- Ammo drops from enemies and weapon crate/random weapon systems
- Player health, damage feedback, pause/menu flow, and UI support
- Demo-ready combat loop with polished weapon feedback
