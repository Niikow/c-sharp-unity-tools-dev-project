using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * Class for individual sounds
 */

[System.Serializable]
public class Sound
{
    public string Name = "";

    public AudioClip? Clip;

    [HideInInspector]
    public AudioSource? Source;

    public SoundType SoundType;


    // [Range(0, 1)]
    // public float Volume;

    public bool Loop;
}
public enum SoundType
{
    music,
    combat
};
