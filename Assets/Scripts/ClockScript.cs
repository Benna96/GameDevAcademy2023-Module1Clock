using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class ClockScript : MonoBehaviour
{
    private const float
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
