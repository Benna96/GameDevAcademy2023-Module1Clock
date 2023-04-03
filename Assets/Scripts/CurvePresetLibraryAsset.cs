using UnityEngine;

[CreateAssetMenu]
public class CurvePresetLibraryAsset : ScriptableObject
{
    public AnimationCurve[] curves;

    public AnimationCurve getAnimationCurve(int index) => curves[index];
}
