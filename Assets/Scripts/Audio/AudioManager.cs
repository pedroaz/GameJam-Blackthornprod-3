﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enumeration of the audio clip names
/// </summary>
public enum AudioClipNames
{
    None,
    ButtonClick,
    PlayerBullet,
    PlayerDamage,
    PlayerDeath,
    ShieldFullEnergy,
    ShieldDamage,
    UpgradeDroppedInSlot,
    UpgradeUpdateSingle,
    UpgradeUpdateDuo,
    UpgradeUpdateTrio,
    EnemyDamage,
    EnemyDeath,
    MainMenuMusic,
    GameplayMusic0,
    GameplayMusic1,
    GameplayMusic2,
    GameOverMusic,
    LevelEndedMusic
}

public class AudioManager : MonoBehaviour
{
    #region Fields

    static bool initialized = false;
    static AudioSource audioSource_BackgroundMusic;
    static AudioSource audioSource_SoundEffects;
    static Dictionary<AudioClipNames, AudioClip> audioClips = new Dictionary<AudioClipNames, AudioClip>();

    #endregion

    #region Properties

    /// <summary>
    /// Gets whether or not the audio manager has been initialized
    /// </summary>
    public static bool Initialized
    {
        get { return initialized; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the necessary variables for the audio manager
    /// </summary>
    /// <param name="sourceBGM">The audio source to be used as background music</param>
    /// <param name="sourceSFX">The audio source to be used as sound effects</param>
    public static void Initialize(AudioSource sourceBGM, AudioSource sourceSFX)
    {
        //Sets the manager as initialized
        initialized = true;

        //Set the audio source globally
        audioSource_BackgroundMusic = sourceBGM;
        audioSource_SoundEffects = sourceSFX;

        //Load all the audio clips
        audioClips.Add(AudioClipNames.ButtonClick, Resources.Load<AudioClip>("Audio/Effects/sfx_buttonClick"));
        audioClips.Add(AudioClipNames.PlayerBullet, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.PlayerDamage, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.PlayerDeath, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.ShieldFullEnergy, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.ShieldDamage, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.UpgradeDroppedInSlot, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.UpgradeUpdateSingle, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.UpgradeUpdateDuo, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.UpgradeUpdateTrio, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.EnemyDamage, Resources.Load<AudioClip>("Audio/Effects/"));
        audioClips.Add(AudioClipNames.EnemyDeath, Resources.Load<AudioClip>("Audio/Effects/"));

        audioClips.Add(AudioClipNames.MainMenuMusic, Resources.Load<AudioClip>("Audio/BGM/"));
        audioClips.Add(AudioClipNames.GameplayMusic0, Resources.Load<AudioClip>("Audio/BGM/music_stage"));
        audioClips.Add(AudioClipNames.GameplayMusic1, Resources.Load<AudioClip>("Audio/BGM/"));
        audioClips.Add(AudioClipNames.GameplayMusic2, Resources.Load<AudioClip>("Audio/BGM/"));
        audioClips.Add(AudioClipNames.GameOverMusic, Resources.Load<AudioClip>("Audio/BGM/"));
        audioClips.Add(AudioClipNames.LevelEndedMusic, Resources.Load<AudioClip>("Audio/BGM/"));
    }

    /// <summary>
    /// Unsynchronously plays the given audio clip
    /// </summary>
    /// <param name="clip">The audio clip to be played</param>
    public static void PlaySFX(AudioClipNames clip)
    {
        //Only play the sound if it was loaded was loaded
        audioClips.TryGetValue(clip, out var value);
        if (value == null) return;

        //Plays the sound
        audioSource_SoundEffects.PlayOneShot(value);
    }

    /// <summary>
    /// Synchronously plays the given audio clips
    /// </summary>
    /// <param name="clip">The audio clip to be played</param>
    /// <param name="loop">Should the audio be looped</param>
    public static void PlayBGM(AudioClipNames clip, bool loop = true)
    {
        //Only play the music if it was loaded was loaded
        audioClips.TryGetValue(clip, out var value);
        if (value == null) return;

        //Plays the music
        audioSource_BackgroundMusic.clip = audioClips[clip];
        audioSource_BackgroundMusic.loop = loop;
        audioSource_BackgroundMusic.Play();
    }

    /// <summary>
    /// Stops the synchronously playing sound
    /// </summary>
    public static void StopBGM()
    {
        //Checks if SFX are playing and stop them as needed
        if (audioSource_SoundEffects != null && audioSource_SoundEffects.isPlaying)
        {
            audioSource_SoundEffects.clip = null;
            audioSource_SoundEffects.Stop();
        }

        //Stops the BGM
        if (audioSource_BackgroundMusic == null) return;
        audioSource_BackgroundMusic.clip = null;
        audioSource_BackgroundMusic.Stop();
    }

    /// <summary>
    /// Gets the length of a specific audio clip pre loaded
    /// </summary>
    /// <param name="name">The name of the audio clip</param>
    /// <returns></returns>
    public static float GetAudioClipLength(AudioClipNames name)
    {
        return audioClips[name].length;
    }

    #endregion
}
