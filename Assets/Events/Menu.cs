using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public string LevelName;

    private GameObject SoundMenu;
    private TMP_Text HighScore;


    private void Awake()
    {
        //SoundMenu = Menu_Sound.instance.gameObject;

        if (Menu_Sound.instance?.gameObject != null)
            SoundMenu = Menu_Sound.instance.gameObject;
        else
            SoundMenu = GameObject.FindAnyObjectByType<Menu_Sound>().gameObject;

        HighScore = GameObject.FindGameObjectWithTag(save.HighScore.ToString()).GetComponent<TMP_Text>();


        Menu_Sound.OnClose += handleSoundClose;
        Menu_Sound.OnOpen += handleSoundOpen;

        Debug.Log("High Score" + PlayerPrefs.GetInt(save.HighScore.ToString(), 0));
        HighScore.text = String.Format("High Score: {0}", PlayerPrefs.GetInt(save.HighScore.ToString(), 0));
    }

    private void OnDestroy()
    {
        Menu_Sound.OnClose -= handleSoundClose;
        Menu_Sound.OnOpen -= handleSoundOpen;
    }

    

    public void LoadLevel()
    {
        Debug.Log("load Scene:" + LevelName);
        SceneManager.LoadScene(LevelName);
    }

    public void OpenSoundMenu()
    {
        SoundMenu.SetActive(true);
    }

    private void handleSoundClose()
    {
        gameObject.SetActive(true);
    }

    private void handleSoundOpen()
    {
        gameObject.SetActive(false);
    }
}
