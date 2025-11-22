# RD_Tween

### Lightweight Tween System for Unity. Fast, clean and fully pooled

RD_Tween is a lightweight, high‑performance tweening library for Unity that brings features while staying compact, predictable, and easy to extend.

* Transform animations: **Move / Rotate / Scale / Jump**
* UI & graphics fading: **CanvasGroup**, **Graphic**, **SpriteRenderer**
* Powerful modifiers: **SetLoops**, **LoopType.Yoyo**, **SetEase**, **SetDelay**, **SetSpeed**, **SetRelative**, **From / FromRelative**
* Update routing: **Update / Late / Fixed / Manual** with **unscaled time** support
* Global control: **Kill**, **KillAll**, **KillById**, **IsTweening**
* Sequences: **Append / Join / Insert / Prepend / AppendCallback / AppendInterval**
* Fully pooled tweens (near‑zero allocations)
* Automatic TweenRunner (no setup required)

---

# 🚀 Installation

Simply place the `RD_Tween` folder into your Unity project.
No external dependencies, no ScriptableObjects, no extra setup.

---

# 🚀 Quick Start

## MoveTo

```csharp
transform.MoveTo(new Vector3(5, 0, 0), 2f);
```

## RotateTo

```csharp
transform.RotateTo(new Vector3(0, 180, 0), 1f);
```

## RotateByX/Y/Z

```csharp
transform.RotateByY(360f, 1f);
```

## ScaleTo

```csharp
transform.ScaleTo(new Vector3(2, 2, 2), 1.5f);
```

## JumpTo

```csharp
transform.JumpTo(Vector3.forward * 6, 2f, 5f);
```

---

# ✨ Modifiers & Options

## Loops

```csharp
transform
    .MoveTo(Vector3.right * 5, 1f)
    .SetLoops(-1, TweenLoopType.Yoyo); // -1 = infinite
```

## Ease

```csharp
.SetEase(CurvesType.EaseInOutCubic);
```

## Speed

```csharp
.SetSpeed(1.2f);
```

## Delay

```csharp
.SetDelay(0.5f);
```

## Callbacks

```csharp
.OnPlay(() => Debug.Log("Started"))
.OnComplete(() => Debug.Log("Finished"));
```

## Join (parallel tweens)

```csharp
transform
    .MoveTo(Vector3.forward * 5, 2f)
    .Join(transform.RotateTo(new Vector3(0, 180, 0), 1f).SetLoops(-1));
```

## From / FromRelative / SetRelative

```csharp
transform
    .MoveTo(new Vector3(5, 0, 0), 2f)
    .From(new Vector3(-5, 0, 0));
```

```csharp
transform
    .MoveTo(Vector3.right * 2, 1f)
    .FromRelative(Vector3.left * 5f);
```

```csharp
transform
    .MoveTo(Vector3.right * 3, 1f)
    .SetRelative(); // MoveTo = current + target
```

---

# ⏱ UpdateType + Unscaled Time

```csharp
.SetUpdate(TweenUpdateType.Late, isUnscaled: true);
```

Available:

* `Update`
* `Late`
* `Fixed`
* `Manual` (use `Tween.ManualUpdate(deltaTime)`)

---

# 🆔 Id & Global Control

## SetId

```csharp
.SetId("move_player");
```

## KillAll

```csharp
Tween.KillAll();
```

## Kill by target

```csharp
Tween.Kill(transform);
```

## Kill by Id

```csharp
Tween.KillById("move_player");
```

## Check if target is tweening

```csharp
if (Tween.IsTweening(transform)) { ... }
```

---

# 🌈 Fade Utilities (UI & Graphics)

### CanvasGroup

```csharp
canvasGroup.FadeTo(0f, 0.8f);
```

### Image / Text / TMP (Graphic)

```csharp
image.FadeTo(0.3f, 1f);
```

### SpriteRenderer

```csharp
sprite.FadeTo(1f, 0.5f);
```

---

# 🎬 Sequences

Powerful timeline-style system

### Create

```csharp
var seq = Tween.Sequence();
```

### Append

```csharp
seq.Append(transform.MoveTo(new Vector3(3, 0, 0), 1f));
```

### Join

```csharp
seq.Join(transform.RotateByY(360f, 1f));
```

### Insert

```csharp
seq.Insert(0.5f, transform.ScaleTo(Vector3.one * 2, 1f));
```

### AppendCallback

```csharp
seq.AppendCallback(() => Debug.Log("Halfway!"));
```

### AppendInterval / PrependInterval

```csharp
seq.AppendInterval(0.5f);
seq.PrependInterval(0.3f);
```

### Prepend

```csharp
seq.Prepend(transform.ScaleTo(Vector3.one * 0.2f, 0.4f));
```

### Loops

```csharp
seq.SetLoops(-1, TweenLoopType.Yoyo);
```

### Play

```csharp
seq.Play();
```

---

# 🧪 Full Example

```csharp
transform
    .MoveTo(new Vector3(5, 0, 0), 2f)
    .Join(transform.RotateTo(new Vector3(0, 90, 0), 1f).SetLoops(-1))
    .RotateTo(new Vector3(0, 180, 0), 1f)
    .ScaleTo(new Vector3(2, 2, 2), 1.5f)
    .JumpTo(Vector3.forward * 6, 2f, 5)
    .SetLoops(-1, TweenLoopType.Yoyo)
    .SetSpeed(1.2f)
    .SetEase(CurvesType.EaseInOutQuad)
    .SetDelay(0.3f)
    .SetUpdate(TweenUpdateType.Late, true)
    .OnComplete(() => Debug.Log("Tween Complete"));
```

---

# 🧩 Sequence Example

```csharp
Tween.Sequence()
    .Append(transform.MoveTo(Vector3.right * 3, 1f))
    .AppendCallback(() => Debug.Log("1 done"))
    .Join(transform.RotateByY(360, 1f))
    .Insert(0.5f, transform.ScaleTo(Vector3.one * 2, 0.7f))
    .AppendInterval(0.3f)
    .Append(transform.MoveTo(Vector3.left * 3, 1f))
    .SetLoops(-1, TweenLoopType.Yoyo)
    .Play();
```

---

# 🧼 Notes

* Fully pooled system → almost no GC allocations
* Tweens auto-kill when the target is destroyed
* Update routing through Update/Late/Fixed/Manual
* No hidden overhead: a single `[RD_TweenRunner]` object
* Clear
