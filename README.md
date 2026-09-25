## Objective

**Build a small level where the player must cross a map while avoiding fire from three turret types, then reach a goal point to win.**

### 1. Turrets

Implement three turret types, each with distinct targeting and attack behavior:

- Turret Range Shape Attack

- Flame Turret Cone Continuously expels fire within its cone when the player is inside it

- Sniper Turret Line of sight Fires a single shot when the player enters its sight line

- Shotgun Turret Cone/radius (your choice) Fires a spread shotgun blast when the player is in range

Each turret should only fire when the player is within its defined range/detection area.

### 2. Range Visualization

Add a LineRenderer to each turret that draws the outline of its range/detection shape (cone, radius, or line), so the player can see danger zones before entering them.

### 3. Player & Level Flow

- Player starts at the left side of the map.

- Player must navigate to a specified goal location on the right (or elsewhere), avoiding all turret projectiles along the way.

- On hit: the scene restarts immediately.

- On reaching the goal: all turrets stop firing, and a Win UI is displayed.

[Video Submission](https://drive.google.com/file/d/1vucLTl1gRD__3P7fkkRT1q1ZoeMEX_6X/view?usp=sharing)
