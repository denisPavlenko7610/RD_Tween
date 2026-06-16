# RD.Tween

**A zero-allocation tweening library for Unity — a complete, more reliable replacement for DOTween.**

If you know DOTween, you already know RD.Tween. The API is intentionally similar, but the architecture is cleaner: object pooling, strict typing, no hidden GC allocations.

---

## Installation

### Via UPM (Package Manager)

Add the package by Git URL in Unity's Package Manager:

```
https://github.com/denisPavlenko7610/RD_Tween.git
```

### Via manifest.json

Add to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.rd.rdtween": "https://github.com/denisPavlenko7610/RD_Tween.git"
  }
}
```

### Assembly Reference

Reference `RD.Tween` in your `.asmdef`:

```json
{
    "references": ["RD.Tween"]
}
```

---

## Quick Start

```csharp
using RD_Tween.Runtime;

// Move over 1 second
transform.MoveTo(new Vector3(0, 5, 0), 1f);

// Scale
transform.ScaleTo(Vector3.one * 2f, 0.5f);

// Fade in (CanvasGroup / Graphic / SpriteRenderer)
canvasGroup.FadeTo(1f, 0.3f);

// Combine in a sequence
Tween.Sequence()
    .Append(transform.MoveTo(new Vector3(0, 5, 0), 1f))
    .Join(transform.ScaleTo(Vector3.one * 1.5f, 0.5f))
    .AppendCallback(() => Debug.Log("Done!"));
```

---

## Extension Methods

### Transform

| Method | Description |
|--------|-------------|
| `MoveTo(Vector3 target, float duration)` | Animate world position |
| `LocalMoveTo(Vector3 target, float duration)` | Animate local position |
| `ScaleTo(Vector3 target, float duration)` | Animate local scale |
| `RotateTo(Vector3 euler, float duration)` | Animate euler rotation |
| `LocalRotateTo(Quaternion target, float duration)` | Animate local rotation (Quaternion) |
| `RotateBy(Vector3 axis, float angle, float duration)` | Additive rotation around axis |
| `RotateByX(float angle, float duration)` | Additive rotation around X |
| `RotateByY(float angle, float duration)` | Additive rotation around Y |
| `RotateByZ(float angle, float duration)` | Additive rotation around Z |
| `JumpTo(Vector3 target, float duration, float height)` | Jump to position |
| `PunchPosition(Vector3 punch, float duration, int vibrato, float elasticity)` | Punch position |
| `PunchScale(Vector3 punch, float duration, int vibrato, float elasticity)` | Punch scale |
| `PunchRotation(Vector3 punch, float duration, int vibrato, float elasticity)` | Punch rotation |
| `ShakePosition(float strength, float duration, int vibrato, float randomness)` | Shake position |
| `ShakeScale(float strength, float duration, int vibrato, float randomness)` | Shake scale |

### RectTransform

| Method | Description |
|--------|-------------|
| `AnchorPositionTo(Vector2 target, float duration)` | Animate `anchoredPosition` |
| `SizeDeltaTo(Vector2 target, float duration)` | Animate `sizeDelta` |
| `AnchorMinTo(Vector2 target, float duration)` | Animate `anchorMin` |
| `AnchorMaxTo(Vector2 target, float duration)` | Animate `anchorMax` |
| `PivotTo(Vector2 target, float duration)` | Animate `pivot` |

### Graphics / UI

| Method | Description |
|--------|-------------|
| `ColorTo(this Graphic, Color target, float duration)` | Animate `Graphic.color` |
| `ColorTo(this SpriteRenderer, Color target, float duration)` | Animate `SpriteRenderer.color` |
| `ColorTo(this Camera, Color target, float duration)` | Animate `Camera.backgroundColor` |
| `FadeTo(this Graphic, float target, float duration)` | Animate alpha of `Graphic` |
| `FadeTo(this CanvasGroup, float target, float duration)` | Animate `CanvasGroup.alpha` |
| `FadeTo(this SpriteRenderer, float target, float duration)` | Animate alpha of `SpriteRenderer` |
| `FillAmountTo(this Image, float target, float duration)` | Animate `Image.fillAmount` |

---

## Generic `Tween.To` API

Tween any property with getter/setter:

```csharp
// float
Tween.To(target, () => health, v => health = v, 100f, 2f);

// Vector3
Tween.To(target, () => pos, v => pos = v, Vector3.one, 1f);

// Vector2
Tween.To(target, () => size, v => size = v, new Vector2(100, 50), 0.5f);

// Color
Tween.To(target, () => color, v => color = v, Color.red, 0.5f);

// Quaternion
Tween.To(target, () => rot, v => rot = v, Quaternion.identity, 1f);
```

Returns [`PropertyTween<T>`](Runtime/Tweens/PropertyTween.cs) with full fluent API.

---

## Sequences

Build timelines with `Tween.Sequence()`:

```csharp
Tween.Sequence()
    .Append(transform.MoveTo(targetA, 1f))      // after previous
    .Join(transform.ScaleTo(Vector3.one * 2f, 0.5f))  // parallel with last
    .AppendInterval(0.5f)                        // pause
    .AppendCallback(() => SpawnParticles())      // callback at position
    .Insert(0.3f, sprite.FadeTo(0f, 0.5f))      // insert at specific time
    .PrependInterval(0.2f)                       // delay before everything
    .SetLoops(-1, LoopType.Yoyo);                // infinite yoyo
```

| Method | Description |
|--------|-------------|
| `Append(IControllableTween)` | Append after previous |
| `Join(IControllableTween)` | Start parallel with last appended |
| `Insert(float atTime, IControllableTween)` | Insert at specific timeline position |
| `Prepend(IControllableTween)` | Insert before all others |
| `AppendInterval(float duration)` | Add pause |
| `PrependInterval(float duration)` | Add delay before start |
| `AppendCallback(Action)` | Fire callback at timeline position |
| `AppendCoroutine(MonoBehaviour, Func<IEnumerator>)` | Run coroutine in sequence |

---

## Easing Curves

All standard easing functions via [`CurvesType`](Runtime/CurvesType.cs):

```csharp
transform.MoveTo(target, 1f)
    .SetEase(CurvesType.EaseOutBounce);

transform.ScaleTo(Vector3.one * 2f, 0.5f)
    .SetEase(CurvesType.EaseInOutBack);
```

### Available Curves

| Type | In | Out | InOut |
|------|----|-----|-------|
| `Linear` | — | — | `Linear` |
| `Quad` | `EaseInQuad` | `EaseOutQuad` | `EaseInOutQuad` |
| `Cubic` | `EaseInCubic` | `EaseOutCubic` | `EaseInOutCubic` |
| `Quart` | `EaseInQuart` | `EaseOutQuart` | `EaseInOutQuart` |
| `Quint` | `EaseInQuint` | `EaseOutQuint` | `EaseInOutQuint` |
| `Sine` | `EaseInSine` | `EaseOutSine` | `EaseInOutSine` |
| `Expo` | `EaseInExpo` | `EaseOutExpo` | `EaseInOutExpo` |
| `Circ` | `EaseInCirc` | `EaseOutCirc` | `EaseInOutCirc` |
| `Elastic` | `EaseInElastic` | `EaseOutElastic` | `EaseInOutElastic` |
| `Back` | `EaseInBack` | `EaseOutBack` | `EaseInOutBack` |
| `Bounce` | `EaseInBounce` | `EaseOutBounce` | `EaseInOutBounce` |

Custom easing: `.SetEase(t => t * t)` — equivalent to `EaseInQuad`.

---

## Control & Settings

Every tween returns [`TweenActionCore`](Runtime/Core/TweenActionCore.cs) (or [`PropertyTween<T>`](Runtime/Tweens/PropertyTween.cs)) with fluent API:

```csharp
var tween = transform.MoveTo(target, 2f)
    .SetEase(CurvesType.EaseOutCubic)
    .SetDelay(0.5f)                          // delay before start
    .SetSpeed(2f)                            // speed multiplier
    .SetLoops(3, LoopType.Yoyo)              // 3 loops, yoyo style
    .SetLoops(-1, LoopType.Restart)          // infinite loops
    .SetRelative(true)                       // relative to current value
    .SetAutoKill(false)                      // don't auto-remove after completion
    .SetId("myTween")                        // id for KillById
    .SetUpdate(UpdateType.Late, isUnscaled: false)  // update timing
    .OnComplete(() => Debug.Log("Complete!"))
    .OnPlay(() => Debug.Log("Started!"))
    .OnUpdate(() => { /* every frame */ })
    .OnPause(() => Debug.Log("Paused"))
    .OnKill(() => Debug.Log("Killed"))
    .OnRewind(() => Debug.Log("Rewound"))
    .OnStepComplete(idx => Debug.Log($"Step {idx} done"));
```

### Runtime Control

```csharp
tween.Pause();
tween.Resume();
tween.PlayForward();
tween.PlayBackwards();
tween.PlayFromEnd();       // go to end, then play backwards
tween.Restart();
tween.Rewind();
tween.Goto(1.5f);          // scrub to 1.5 seconds
tween.GotoNormalized(0.5f); // scrub to 50%
tween.Complete();           // jump to end
tween.Kill();               // stop and remove
```

### `From` — Override Start Value

```csharp
// Start from absolute value
transform.MoveTo(Vector3.zero, 1f).From(new Vector3(10, 0, 0));

// Start from offset relative to end
transform.MoveTo(Vector3.zero, 1f).FromRelative(new Vector3(0, 5, 0));

// Start from current value (default behavior)
transform.MoveTo(Vector3.zero, 1f).FromCurrent();
```

---

## Global API

Static class [`Tween`](Runtime/Tween.cs) — equivalent to `DOTween`:

```csharp
// Kill tweens
Tween.Kill(target);                // kill all tweens on target
Tween.Kill(target, complete: true); // complete then kill
Tween.KillAll();
Tween.KillAll(complete: true);
Tween.KillById("myId");
Tween.KillById("myId", complete: true);

// Pause / Resume all
Tween.PauseAll();
Tween.ResumeAll();

// Complete all
Tween.CompleteAll();

// Query
bool active = Tween.IsTweening(target);
bool activeById = Tween.IsTweeningId("myId");
int count = Tween.ActiveCount();

// Scrub all tweens
Tween.GotoAll(0.5f);              // all to 50%
Tween.GotoAll(1f, play: true);    // all to end, then play
```

---

## Coroutine Helpers

[`TweenUtility`](Runtime/Extensions/TweenUtility.cs):

```csharp
// Delayed call (like DOVirtual.DelayedCall)
TweenUtility.DelayedCall(2f, () => Debug.Log("2 seconds passed"));

// Wait in coroutine
yield return tween.WaitForCompletion();
yield return tween.WaitForKill();
yield return tween.WaitForElapsedLoops(3);
yield return tween.WaitForPosition(0.5f);

// Coroutine inside sequence
sequence.AppendCoroutine(this, () => MyCoroutine());
```

---

## Enums

| Enum | Values | Where |
|------|--------|-------|
| `UpdateType` | `Normal`, `Late`, `Fixed` | `SetUpdate()` |
| `LoopType` | `Restart`, `Yoyo`, `Incremental` | `SetLoops()` |

---

## DOTween Migration Table

| DOTween | RD.Tween |
|---------|----------|
| `DOMove` / `DOLocalMove` | `MoveTo` / `LocalMoveTo` |
| `DORotate` / `DOLocalRotate` | `RotateTo` / `LocalRotateTo` |
| `DOScale` | `ScaleTo` |
| `DOJump` | `JumpTo` |
| `DOPunchPosition` / `PunchScale` / `PunchRotation` | `PunchPosition` / `PunchScale` / `PunchRotation` |
| `DOShakePosition` / `ShakeScale` | `ShakePosition` / `ShakeScale` |
| `DOAnchorPos` | `AnchorPositionTo` |
| `DOSizeDelta` | `SizeDeltaTo` |
| `DOColor` | `ColorTo` |
| `DOFade` | `FadeTo` |
| `DOFillAmount` | `FillAmountTo` |
| `DOTween.To()` | `Tween.To()` |
| `DOTween.Sequence()` | `Tween.Sequence()` |
| `Ease.OutBounce` (enum) | `CurvesType.EaseOutBounce` (Func) |
| `SetEase(Ease)` | `SetEase(Func<float,float>)` |
| `SetLoops(-1)` | `SetLoops(-1)` |
| `SetRelative()` | `SetRelative()` |
| `From()` / `From(true)` | `From()` / `FromRelative()` |
| `SetDelay()` | `SetDelay()` |
| `SetSpeed()` / `timeScale` | `SetSpeed()` |
| `SetId()` / `DOTween.Kill(id)` | `SetId()` / `Tween.KillById(id)` |
| `SetUpdate(UpdateType)` | `SetUpdate(UpdateType, isUnscaled)` |
| `SetAutoKill(false)` | `SetAutoKill(false)` |
| `OnComplete` / `OnPlay` / `OnUpdate` / `OnKill` / `OnPause` / `OnRewind` / `OnStepComplete` | Same |
| `Goto()` / `GotoNormalized()` | Same |
| `PlayBackwards()` / `PlayForward()` | Same |
| `Rewind()` / `Restart()` | Same |
| `Complete()` / `Kill()` | Same |
| `DOVirtual.DelayedCall` | `TweenUtility.DelayedCall` |
| `DOGoto` (global) | `Tween.GotoAll` |
| `DOPause` / `DOPlay` (global) | `Tween.PauseAll` / `Tween.ResumeAll` |
| Object pooling | Object pooling (256 PropertyTween, 64 Sequence) |

---

## Architecture

### Engine Flow

```
Tween (static entry point)
  └── TweenManager (singleton)
        └── TweenRunner (MonoBehaviour, DontDestroyOnLoad)
              ├── Update       → scaled + unscaled
              ├── LateUpdate   → scaled + unscaled
              ├── FixedUpdate  → scaled + unscaled
              └── 6 active tween lists
```

### Class Hierarchy

```
TweenActionCore
  ├── PropertyTween<T>  (pooled, typed property tween)
  └── TweenSequence     (pooled, timeline of tweens)
```

### Object Pooling

Both [`PropertyTween<T>`](Runtime/Tweens/PropertyTween.cs) and [`TweenSequence`](Runtime/Sequences/TweenSequence.cs) use object pooling — tweens are returned to the pool on `Kill()` or `Complete()`, avoiding GC pressure.

- `PropertyTween<T>` — pool of 256
- `TweenSequence` — pool of 64

### Multi-Step Tweens

[`TweenActionCore`](Runtime/Core/TweenActionCore.cs) supports multi-step animations directly, without needing a `Sequence`:

```csharp
var core = new TweenActionCore();
core.ResetCore(target, autoPlay: true);
core.AddStep(0.5f, t => value = Mathf.Lerp(0, 100, t), CurvesType.EaseOutQuad);
core.AddStep(0.3f, t => value = Mathf.Lerp(100, 50, t), CurvesType.EaseInCubic);
core.AppendCallback(() => Debug.Log("Both steps complete"));
```

---

## Important Notes

- **Target is mandatory** — tweens are bound to an object. If the target is destroyed (`Target == null`), the tween is auto-killed.
- **AutoKill is `true` by default** — tweens are removed after completion. Use `SetAutoKill(false)` for reusable animations.
- **Single callback per event** — `OnComplete`, `OnPlay`, etc. accept exactly one `Action`. Wrap multiple calls in a single delegate if needed.
- **Thread safety** — all tween operations must run on the Unity main thread.
- **Unscaled time** — use `SetUpdate(UpdateType.Normal, isUnscaled: true)` for timeScale-independent animations.

---

*RD.Tween — animate your game, not the garbage collector.*
