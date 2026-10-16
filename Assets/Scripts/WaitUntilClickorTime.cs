using UnityEngine;

/// <summary>
/// Waits until the left mouse button is pressed or a certain amount of time has passed.
/// </summary>
public class WaitUntilClickorTime : CustomYieldInstruction
{

    /// <summary>
    /// The amount of time to wait for.
    /// </summary>
    private readonly float _timeoutTime;

    public override bool keepWaiting
    {
        get
        {
            return !Input.GetMouseButtonDown(0) && Time.time < _timeoutTime;
        }
    }

    /// <summary>
    /// Waits until the left mouse button is pressed or a certain amount of time has passed.
    /// </summary>
    public WaitUntilClickorTime(float duration)
    {
        _timeoutTime = Time.time + duration;
    }

}
