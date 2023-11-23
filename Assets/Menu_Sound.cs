using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Menu_Sound : MonoBehaviour
{
    public static event Action OnClose;
    public static event Action OnOpen;
    public static Menu_Sound instance { get; private set;}

    public Toggle SoundToggle;
    public Slider SoundVolume;

    public Toggle MusicToggle;
    public Slider MusicVolume;

    public AudioClip[] list;

    private PlayerContols inputActions;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        gameObject.SetActive(false);

        DontDestroyOnLoad(gameObject);

        handleToggle(SoundKey.MusicToggle, SoundManager.MusicToggle);
        handleToggle(SoundKey.SoundToggle, SoundManager.SoundToggle);

        handleVolume(SoundKey.MusicVolume, SoundManager.MusicVolume);
        handleVolume(SoundKey.SoundVolume, SoundManager.SoundVolume);

        SoundManager.OnToggle += handleToggle;
        SoundManager.OnChangeVolume += handleVolume;

        inputActions = new PlayerContols();
        inputActions.Enable();
        inputActions.Menu.SoundMenu.performed += handleSoundMenuButton;
    }

    private void OnDestroy()
    {
        inputActions.Disable();
        if (instance != this)
            return;

        inputActions.Menu.SoundMenu.performed -= handleSoundMenuButton;

        SoundManager.OnToggle -= handleToggle;
        SoundManager.OnChangeVolume -= handleVolume;
    }

    private void OnEnable()
    {
        OnOpen?.Invoke();
    }

    private void OnDisable()
    {
        OnClose?.Invoke();
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void handleVolume(SoundKey key, float state)
    {
        switch (key)
        {
            case SoundKey.SoundVolume: SoundVolume.value = state;  break;
            case SoundKey.MusicVolume: MusicVolume.value = state;  break;
        }
    }

    private void handleToggle(SoundKey key, bool state)
    {
        switch (key)
        {
            case SoundKey.SoundToggle: SoundToggle.isOn = state; break;
            case SoundKey.MusicToggle: MusicToggle.isOn = state; break;
        }
    }

    private void handleSoundMenuButton(InputAction.CallbackContext value)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


    public void OnSoundToggle(bool state) {
        SoundManager.instance.toggleSound(state);
    }
    public void OnSoundVolume(float state)
    {
        SoundManager.instance.changeSoundVolume(state);
    }

    public void OnMusicChange(int id)
    {
        SoundManager.instance.changeMusic(list[id]);
    }


    public void OnMusicToggle(bool state) {
        SoundManager.instance.toggleMusic(state);
    }

    public void OnMusicVolume(float state)
    {
        SoundManager.instance.changeMusicVolume(state);
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

}
