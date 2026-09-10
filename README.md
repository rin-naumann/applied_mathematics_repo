# ACTIVITY 1

### Assignment: Grid Movement & Proximity Detection (No Physics Engine)

### Objective
Build a small scene demonstrating custom collision/proximity logic without using Unity's built-in physics (no Rigidbody, no Unity.Physics, no physics-based collision callbacks).

Add a video link to google drive for demonstration of the mechanics in the readme document.

### Requirements

**Player Controller**
Player can move Up, Down, Left, and Right using input.
Movement must be handled manually (e.g. via Transform.position updates), not through physics forces.

**No-Go Zones**
- Place one or more "No-Go Zones" in the scene.
- Continuously check the distance between the Player and each No-Go Zone.
- If the Player enters a defined proximity threshold:
-- The zone should visibly shake and change color to red (warning state).
- If the Player gets even closer (a second, smaller threshold) or remains in the zone too long, the scene should restart.

**Finish Zone**
- Place a "Finish Zone" in the scene.
- When the Player gets within a defined proximity of the Finish Zone, display a Win UI (e.g. a panel with "You Win!").

**Constraints**
- No Rigidbody, Rigidbody2D, Unity.Physics, or physics-based trigger/collision events (OnCollisionEnter, OnTriggerEnter, etc.) may be used.
- All detection must be done via manually in Update().

[Video Submission](https://drive.google.com/file/d/1xTJR-shRhK08HVQ_-XAxwIzo-t8_phlY/view?usp=sharing)
