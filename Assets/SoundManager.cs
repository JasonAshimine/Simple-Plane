using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private AudioClip PlayerOnImpact;
    [SerializeField] private AudioClip PlayerOnDeath;
    [SerializeField] private AudioClip PlayerOnHeal;

    [Header("Enemy")]
    [SerializeField] private AudioClip EnemyOnImpact;
    [SerializeField] private AudioClip EnemyOnDeath;
    [SerializeField] private AudioClip EnemyOnSpawn;
    [SerializeField] private AudioClip EnemyOnSplit;

    [Header("Bullet")]
    [SerializeField] private AudioClip BulletOnSpawn;
    [SerializeField] private AudioClip BulletOnImpact;

    public static event Action<SoundKey, float> OnChangeVolume;
    public static event Action<SoundKey, bool> OnToggle;

    public static event Action<float> OnMusicVolumeChange;
    public static event Action<bool> OnMusicToggle;
    public static event Action<AudioClip> OnMusicChange;

    public static event Action<float> OnSoundVolumeChange;
    public static event Action<bool> OnSoundToggle;

    public AudioSource AudioSource;

    private static SoundManager _instance;
    public static SoundManager instance { get => _instance;}

    public static bool SoundToggle { get; private set; }
    public static bool MusicToggle { get; private set; }

    public static float SoundVolume { get; private set; }
    public static float MusicVolume { get; private set; }

    private void Awake()
    {
        Debug.Log("Awake" + _instance);
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        AudioSource = GetComponent<AudioSource>();

        toggleSound(PlayerPrefs.GetInt(SoundKey.SoundToggle.ToString(), 1) == 1);
        toggleMusic(PlayerPrefs.GetInt(SoundKey.MusicToggle.ToString(), 1) == 1);

        changeMusicVolume(PlayerPrefs.GetFloat(SoundKey.MusicVolume.ToString(), 0.5f));
        changeSoundVolume(PlayerPrefs.GetFloat(SoundKey.SoundVolume.ToString(), 0.5f));

        AudioSource.Play();
        if (!MusicToggle)
            AudioSource.Pause();
    }

    public void toggleSound(bool state)
    {
        if(SoundToggle != state)
            PlayerPrefs.SetInt(SoundKey.SoundToggle.ToString(), state ? 1 : 0);

        SoundToggle = state;
        OnSoundToggle?.Invoke(state);
        OnToggle?.Invoke(SoundKey.SoundToggle, state);
    }
    public void changeSoundVolume(float state)
    {
        if(SoundVolume != state)
            PlayerPrefs.SetFloat(SoundKey.SoundVolume.ToString(), state);

        SoundVolume = state;

        OnSoundVolumeChange?.Invoke(state);
        OnChangeVolume?.Invoke(SoundKey.SoundVolume, state);
    }

    public void changeMusicVolume(float state)
    {
        if(MusicVolume != state)
            PlayerPrefs.SetFloat(SoundKey.MusicVolume.ToString(), state);

        MusicVolume = state;
        AudioSource.volume = state;

        OnMusicVolumeChange?.Invoke(state);
        OnChangeVolume?.Invoke(SoundKey.MusicVolume, state);
    }

    public void toggleMusic(bool state)
    {
        if(MusicToggle != state)
            PlayerPrefs.SetInt(SoundKey.MusicToggle.ToString(), state ? 1 : 0);

        MusicToggle = state;

        if (state)
            AudioSource.UnPause();
        else
            AudioSource.Pause();

        OnMusicToggle?.Invoke(state);
        OnToggle?.Invoke(SoundKey.MusicToggle, state);
    }

    public void changeMusic(AudioClip audioClip)
    {
        AudioSource.Stop();
        AudioSource.clip = audioClip;
        AudioSource.Play();

        OnMusicChange?.Invoke(audioClip);
    }


    public void Player(AudioKey key)
    {
        Player(key, SoundVolume);
    }

    public void Player(AudioKey key, float vol = 1)
    {
        switch (key)
        {
            case AudioKey.Death: Play(PlayerOnDeath, vol); break;
            case AudioKey.Impact: Play(PlayerOnImpact, vol); break;
            case AudioKey.Heal: Play(PlayerOnHeal, vol); break;
        }
    }


    public void Enemy(AudioKey key) { Enemy(key, SoundVolume); }

    public void Enemy(AudioKey key, float vol = 1)
    {
        switch (key)
        {
            case AudioKey.Death: Play(EnemyOnDeath, vol); break;
            case AudioKey.Impact: Play(EnemyOnImpact, vol); break;
            case AudioKey.Split: Play(EnemyOnSplit, vol); break;
            case AudioKey.Spawn: Play(EnemyOnSpawn, vol); break;
        }
    }


    public void Bullet(AudioKey key) { Bullet(key, SoundVolume); }
    public void Bullet(AudioKey key, float vol = 1)
    {
        switch (key)
        {
            case AudioKey.Impact: Play(BulletOnImpact, vol); break;
            case AudioKey.Spawn: Play(BulletOnSpawn, vol); break;
        }
    }

    void Play(AudioClip source, float vol = 1)
    {
        if (SoundToggle == false)
            return;

        if(source != null)
            AudioSource.PlayOneShot(source, vol);
    }

}

public enum AudioKey
{
    Impact,
    Death,
    Heal,
    Split,
    Spawn
}

public enum SoundKey
{
    SoundVolume,
    MusicVolume,
    SoundToggle,
    MusicToggle
}