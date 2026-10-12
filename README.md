# Reprise

*Sekiro*-inspired combat, rebuilt around guns. Instead of deflecting swords, you counter enemy bullets with precise
timing, manage stamina, and build up a special meter. Built solo in Unity. All code and pixel art by me.

▶️ **Download (in development):** [pierrymarbles.itch.io/reprise](https://pierrymarbles.itch.io/reprise)
🌐 **Early browser prototype** (as *Gunslinger's Reprisal*): [pierrymarbles.itch.io/gunslingers-reprisal](https://pierrymarbles.itch.io/gunslingers-reprisal)

<!-- TODO: add 2–3 screenshots or a short gameplay GIF here, e.g. ![Gameplay](docs/gameplay.gif) -->

## Features
- **Bullet counters:** a timed counter window against incoming enemy bullets
- **Enemy combo strings:** enemies attack in authored sequences that the player learns to read
- **Stamina and health:** separate meters for the player and enemies
- **Special meter:** built up in combat, with its own animation
- **Hit feedback:** hit indicators, particles, sound and time effects

## Technical highlights
- **ScriptableObject event system** (`GameEvent` / `GameEventListener`) that decouples gameplay from UI and audio
- **Data-driven combos** (`Attack`, `AttackString`) that define enemy attack sequences
- **Systems split by responsibility:** input, movement, shooting, counters, stamina, UI and audio each in their own component

## Built with
Unity 2021.3 · C# · Aseprite
