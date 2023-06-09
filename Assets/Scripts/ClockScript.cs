using System;
using UnityEngine;

#nullable enable

public class ClockScript : MonoBehaviour
{
    private enum CurveSelector {
        teleport,
        smooth,
        lifelike,
        exaggerated
    }

    private const float
        secondsToSecondDegrees = 360f / 60f,
        secondsToMinuteDegrees = 360f / (float)(60 * 60),
        secondsToHourDegrees = 360f / (float)(60 * 60 * 12);
    private readonly TimeSpan oneSecond = new(0, 0, 1);

    [SerializeField]
    private Transform? hourHandle, minuteHandle, secondHandle;
    [SerializeField]
    private CurveSelector animationStyle;
    private AnimationCurve? curve;
    
    // Start is called before the first frame update
    void Start()
    {
        CurvePresetLibraryAsset? curvePresets = Resources.Load("Clock anim curves") as CurvePresetLibraryAsset;
        curve = curvePresets?.getAnimationCurve((int)animationStyle);
    }

    // Update is called once per frame
    void Update() {
        // Each second, the handles move from the previous second's position towards the current second's position.
        // The movement is governed by the selected animation curve.
        // In the animation curve, a value of of 0 stands for the previous second's position,
        // while a value of 1 stands for the current second's position.

        TimeSpan time = DateTime.Now.TimeOfDay.Subtract(oneSecond);
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
