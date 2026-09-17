# Player Rocket Barrage — Spec

## 1. Player Movement

Input: 4-directional (up, down, left, right)
Movement type: Cardinal-only (no diagonals unless specified otherwise)
Constraints: TBD (speed, acceleration, screen/world bounds)

## 2. Rocket Barrage System

### 2.1 Trigger
Fires automatically on a timer (every N seconds — TBD default, e.g. 3s)
Origin point: player's current position at time of fire

### 2.2 Rocket Spawn Pattern
Rockets spawn in a radial burst, evenly spaced around 360°
Spacing formula: 360 / rocketCount degrees apart
First rocket offset: TBD (spec example starts at 45°, i.e. offset = half-spacing)

### Example (4 rockets):
Rocket	Angle
1	45°
2	135°
3	225°
4	315°

### 2.3 Rocket Behavior
Travel in a straight line along their assigned angle from spawn point
Constant velocity (TBD speed value)
Lifetime/despawn condition: TBD (off-screen, max distance, timer, or collision)

## 3. Power-Ups

### 3.1 Placement
Distributed around the map (static spawn points or random — TBD)

### 3.2 Effect
On pickup: increases rocketCount by +1
Cap: rocketCount max = 8
Pickup beyond cap: TBD (no effect, or convert to another bonus e.g. score/health)

[Video Submission](https://drive.google.com/file/d/1kZJglKgE3mODwj3okDuFHlYj48-h-P0C/view?usp=sharing)
