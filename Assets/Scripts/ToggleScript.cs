using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (usePlayerPrefPlsss) ON = (byte)PlayerPrefs.GetInt(playerPref);
        if (playerPref == "fullscreen") ON = (byte)(Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? 1 : 0);
        hand.SetActive(ON == 1);
    }

    public void Change()
    {
        ON = (byte)(1-ON);
        hand.SetActive(ON == 1);
        switch (playerPref)
        {
            case "fullscreen": Fullscreen(); break;
            case "reset data": PlayerPrefs.DeleteAll(); UnityEngine.SceneManagement.SceneManager.LoadScene(0); break;
        }
    }

    void Fullscreen()
    {
        if(ON == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow; 
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(Display.main.systemWidth - 80, Display.main.systemHeight - 40, false);
        }
    }

    public string playerPref;
    public byte ON;
    public GameObject hand;
    public bool usePlayerPrefPlsss;
}
