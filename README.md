# RD_Tween

# Setup

- Add on boostarap sceen gameobject with class TweenUpdater

# Using

## use Transform extensions like:

- MoveTo
```C#
transform
    .MoveTo(new Vector3(5, 0, 0), 2f) //move to vector by 2 seconds
    .Play();
```
- RotateTo - to rotate to some vector
```C#
transform
    .RotateTo(new Vector3(0, 180, 0), 1f)
    .Play();
```
- RotateByX / RotateByY / RotateByZ - to rotate around local axis
```C#
 transform
    .RotateByY(360f, 1f) //move around 360 degree by 1 second
    .Play();
```
- JumpTo
```C#
transform
    .JumpTo(Vector3.forward * 6, 2f, 5)
    .Play();
```
- ScaleTo
```C#
transfomr
    .ScaleTo(new Vector3(2, 2, 2), 1.5f)
    .Play();
```

## Use modifiers

- Join - to join action with main actions
```C# 
.Join(transform.RotateTo(Vector3.up * 90, 1f).Loop(-1))
```
- Loop - to setup count of loops // -1 - means infinite loops count
```C#
.Loop(-1)
```
- OnComplete - to call some method on finish
```C#
.OnComplete(() => Debug.Log("Tween Complete"))
```
- SetEase - to setup curve type
```C#
.SetEase(CurvesType.EaseInOutQuad)
```
- SetSpeed - to comtrol tween speed
```C#
.SetSpeed(1.2f)
```
- Simple example
```C#
 transform.MoveTo(new Vector3(5, 0, 0), 2f).Join(transform.RotateTo(Vector3.up * 90, 1f).Loop(-1))
            .RotateTo(new Vector3(0, 180, 0), 1f)
            .ScaleTo(new Vector3(2, 2, 2), 1.5f)
            .JumpTo(Vector3.forward * 6, 2f, 5)
            .Loop(-1)
            .SetSpeed(1.2f)
            .SetEase(CurvesType.EaseInOutQuad)
            .OnComplete(() => Debug.Log("Tween Complete"))
            .Play();
```
