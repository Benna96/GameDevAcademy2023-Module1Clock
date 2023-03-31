using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class ClockScript : MonoBehaviour
{
    private const float
        hoursToDegrees = 360f / 12f,
        minutesToDegrees = 360f / 60f,
        secondsToSecondDegrees = 360f / 60f,
        secondsToMinuteDegrees = 360f / (float)(60 * 60),
        secondsToHourDegrees = 360f / (float)(60 * 60 * 12);


    public Transform? hourHandle, minuteHandle, secondHandle;

    [SerializeField]
    private AnimationCurve? curve;

    private TimeSpan previousTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Update3Star();
    }
    void Update1Star()
    {
        // Moves the handles suddenly when the relevant value changes

        TimeSpan time = DateTime.Now.TimeOfDay;

        if (!(Math.Floor(time.TotalSeconds) > Math.Floor(previousTime.TotalSeconds)))
            return;

        if (hourHandle != null
            && Math.Floor(time.TotalHours) > Math.Floor(previousTime.TotalHours))
            hourHandle.localEulerAngles = new Vector3(0, 0, hoursToDegrees * time.Hours);

        if (minuteHandle != null
            && Math.Floor(time.TotalMinutes) > Math.Floor(previousTime.TotalMinutes))
            minuteHandle.localEulerAngles = new Vector3(0, 0, minutesToDegrees * time.Minutes);

        if (secondHandle != null
            && Math.Floor(time.TotalSeconds) > Math.Floor(previousTime.TotalSeconds))
            secondHandle.localEulerAngles = new Vector3(0, 0, secondsToSecondDegrees * time.Seconds);

        previousTime = time;
    }
    void Update2Star()
    {
        // Constantly moves the handles smoothly

        TimeSpan time = DateTime.Now.TimeOfDay;
        
        if (hourHandle != null)
            hourHandle.localEulerAngles = new Vector3(0, 0, (float)(hoursToDegrees * time.TotalHours));

        if (minuteHandle != null)
            minuteHandle.localEulerAngles = new Vector3(0, 0, (float)(minutesToDegrees * time.TotalMinutes));

        if (secondHandle != null)
            secondHandle.localEulerAngles = new Vector3(0, 0, (float)(secondsToSecondDegrees * time.TotalSeconds));
    }
    void Update3Star() {
        // Moves the handles in a lifelike manner.
        // The movement starts at the beginning of each second and during the second,
        // the handle moves towards where it should be at the beginning of the next second
        // using an animation curve.

        TimeSpan time = DateTime.Now.TimeOfDay;
        double totalFullSeconds = Math.Floor(time.TotalSeconds);
        float fractionSecondsAdjustedForCurve = curve?.Evaluate(time.Milliseconds / 1000f) ?? time.Milliseconds / 1000f;
        double totalSecondsAdjustedForCurve = totalFullSeconds + fractionSecondsAdjustedForCurve;

        if (hourHandle != null)
            hourHandle.localEulerAngles = new Vector3(0, 0, (float)(secondsToHourDegrees * totalSecondsAdjustedForCurve));

        if (minuteHandle != null)
            minuteHandle.localEulerAngles = new Vector3(0, 0, (float)(secondsToMinuteDegrees * totalSecondsAdjustedForCurve));

        if (secondHandle != null)
            secondHandle.localEulerAngles = new Vector3(0, 0, (float)(secondsToSecondDegrees * totalSecondsAdjustedForCurve));
    }
}
