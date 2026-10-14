# TITAN STRIKE — Stage 1 Prototype

This repository now contains a Unity-ready scaffold for the first playable prototype of TITAN STRIKE.

## Overview

The current prototype includes:
- 3D scene generation with ground and props
- Player with CharacterController movement
- Third-person and first-person camera support
- Basic fire system with ammo and reload flow
- Health, armor, and hit logic
- Runtime-created mobile touch controls for movement and actions
- Scene bootstrap script for quick setup

## Stage 1 target features

- 3D scene and test ground
- playable character movement
- first-person and third-person camera
- touch controls
- basic weapon system
- health system

## Unity setup

1. Open Unity Hub.
2. Create a new 3D URP project or open this repository as the project root.
3. Ensure the Unity project includes the `Packages/manifest.json` file.
4. Open any scene and attach `Stage1Bootstrap` to an empty GameObject.
5. Press Play.

## Runtime notes

- Desktop controls: WASD movement, mouse look, left mouse fire, Space jump, Shift sprint.
- Mobile controls: the bootstrap creates a touch-control rig automatically.
- This is a prototype foundation, not a full production-scale game yet.

## Files added

- `Packages/manifest.json`
- `ProjectSettings/ProjectVersion.txt`
- `Assets/Scripts/Player/PlayerController.cs`
- `Assets/Scripts/Player/PlayerHealth.cs`
- `Assets/Scripts/Weapons/WeaponController.cs`
- `Assets/Scripts/Cameras/CameraController.cs`
- `Assets/Scripts/AI/EnemyHealth.cs`
- `Assets/Scripts/UI/MobileControlRig.cs`
- `Assets/Scripts/Stage1/Stage1Bootstrap.cs`

## Current status

### Working / implemented
- Basic 3D scene generation
- Player movement and jump
- Camera controller structure
- Weapon firing flow
- Touch UI and desktop fallback controls
- Health / armor base logic

### Not complete yet
- Full character animation rig
- Real 3D character models
- Multiplayer infrastructure
- Open world map, vehicles, AI, and mission systems
- Full art assets and sound design

## Next step

The next milestone is to add enemy AI, proper level geometry, a scene file, and improved camera smoothing and interaction logic.
