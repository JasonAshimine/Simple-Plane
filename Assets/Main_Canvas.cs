using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Canvas : MonoBehaviour
{
    public GameObject SoundMenu;

    private void Awake()
    {
        SoundMenu = Menu_Sound.instance.gameObject;
        Menu_Sound.OnClose += handleSoundClose;
        Menu_Sound.OnOpen += handleSoundOpen;
    }

    private void OnDestroy()
    {
        Menu_Sound.OnClose -= handleSoundClose;
        Menu_Sound.OnOpen -= handleSoundOpen;
    }

    private GameState previous;

    public void OpenSoundMenu()
    {
        SoundMenu.SetActive(true);
    }

    private void handleSoundOpen()
    {
        previous = GameManager.instance.state;
        GameManager.instance.ChangeState(GameState.Menu);

        gameObject.SetActive(false);
    }

    public void handleSoundClose()
    {
        GameManager.instance.ChangeState(previous);
        gameObject.SetActive(true);
    }
}
