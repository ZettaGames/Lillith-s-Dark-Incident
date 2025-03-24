using UnityEngine;

/// <summary>
/// A class that provides a time scale that can be controlled locally, independently of Unity's global time scale.
/// </summary>
public static class LocalTime
{
    private static float _timeScale = 1f;

    /// <summary>
    /// Value that represents the time scale. It is clamped between 0 and 1.
    /// </summary>
    public static float TimeScale
    {
        get { return _timeScale; }
        set { _timeScale = Mathf.Clamp(value, 0f, 1f); }
    }
}