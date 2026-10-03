### Extend your Week 3 tower defense scene by adding creature movement along Bézier paths, a ghost HP bar, a coin collection system, and basic game state UI.

# Requirements

### Scene Setup

- Add two spawn points and one shared target location in your existing scene
- One spawn point drives quadratic Bézier movement (3 control points)
- The other drives cubic Bézier movement (4 control points)
- Towers from Week 3 may be pre-placed — no placement mechanic needed
- Remove the player movement, you are now the towers and the invading creatures are the enemy.

### Creature Movement

- Creatures spawn at their respective spawn points and move toward the target using Bézier Lerp (not Vector3.MoveTowards, not NavMesh)
- Quadratic path must have a visible arc (control point offset from the straight line)
- Cubic path must have a visible S-curve or double-arc shape
- No physics on creatures or bullets — strictly transform-based

### Player HP Bar

- Display a UI HP bar that reflects current HP out of 20
- Implement a ghost HP layer: when damage is taken, the ghost bar stays momentarily then eases down to match the real HP using an easing function (ease-out recommended)
- The real HP bar snaps immediately; the ghost bar ticks down smoothly behind it

### Combat

- A creature that reaches the target subtracts 1 HP from the player (starting HP: 20)
- Turrets from Week 3 must be able to kill a creature in one hit
- Bullets must use transform-based movement only — no Rigidbody, no physics colliders for travel

### Coin

- When a creature dies it will spawn a coin in its location that flies towards a UI element
- Once the coin reaches the UI element, the element will Punch up and ease towards the new value . e.g. 10 coins  to bank, bank has 100 coins. Bank : 101...108..110. final balance: 110;

[Video Submission](https://drive.google.com/file/d/1-yDmZ07NyaIb1g_m6X46CKSmsxjLbzjU/view?usp=sharing)
