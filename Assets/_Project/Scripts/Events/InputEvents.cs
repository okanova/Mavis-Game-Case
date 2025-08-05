using System;
using UnityEngine;

public static class InputEvents
{
    public static event Action<Vector3> OnClick;
    public static event Action<Vector3> OnHold;
    public static event Action<Vector3> OnRelease;

    public static event Action<float> OnZoom;
    public static event Action<Vector2> OnDrag;
    public static event Action<Vector2> OnSwipe;

    public static void TriggerClick(Vector3 pos) => OnClick?.Invoke(pos);
    public static void TriggerHold(Vector3 pos) => OnHold?.Invoke(pos);
    public static void TriggerRelease(Vector3 pos) => OnRelease?.Invoke(pos);

    public static void TriggerZoom(float delta) => OnZoom?.Invoke(delta);
    public static void TriggerDrag(Vector2 delta) => OnDrag?.Invoke(delta);
    public static void TriggerSwipe(Vector2 dir) => OnSwipe?.Invoke(dir);
}