using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/*
 * Class for controlling audio in the game
 */

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds = Array.Empty<Sound>();

    [SerializeField] public float musicVolume, combatVolume;

    [SerializeField] private VolumeFloatSO volumeFloatSo = null!;

    private AudioSource? currentlyPlaying;

    #region Singleton
    public static AudioManager Instance { get; private set; } = null!;
    private void Awake()
    {
        musicVolume = volumeFloatSo.musicValue;
        combatVolume = volumeFloatSo.combatValue;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;

            switch (sound.SoundType)
            {
                case SoundType.music:
                    sound.Source.volume = musicVolume;
                    break;
                case SoundType.combat:
                    sound.Source.volume = combatVolume;
                    break;
                default:
                    sound.Source.volume = 0.2f;
                    break;
            }

            sound.Source.loop = sound.Loop;
        }
    }

    #endregion

    public void PlaySound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.Name == soundName && s.Source.isPlaying)
                return;
        }
        Array.Find(sounds, sound => sound.Name == soundName).Source?.Play();
        Debug.Log($"Playing: {soundName}");
    }

    public void StopSound(string soundName)
    {
        Array.Find(sounds, sound => sound.Name == soundName).Source?.Stop();
    }

    private void Start()
    {
        Array.Find(sounds, sound => sound.Name == "Background Music").Source?.Play();
    }

    public void updateMusicVolume(float volume)
    {
        musicVolume = volume;
        volumeFloatSo.musicValue = musicVolume;

        foreach (var sound in sounds)
        {
            if (sound.Source != null && sound.SoundType == SoundType.music)
            {
                sound.Source.volume = musicVolume;
            }
        }
    }

    public void updateCombatVolume(float volume)
    {
        combatVolume = volume;
        volumeFloatSo.combatValue = combatVolume;

        foreach (var sound in sounds)
        {
            if (sound.Source != null && sound.SoundType == SoundType.combat)
            {
                sound.Source.volume = combatVolume;
            }
        }
    }
}