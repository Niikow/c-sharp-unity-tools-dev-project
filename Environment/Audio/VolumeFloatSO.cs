using UnityEngine;

/*
 * Class for storing audio settings
 */

[CreateAssetMenu]
public class VolumeFloatSO : ScriptableObject
{
    public float musicVolume, combatVolume;

    public float musicValue { get => musicVolume; set => musicVolume = value; }

    public float combatValue { get => combatVolume; set => combatVolume = value; }
}
