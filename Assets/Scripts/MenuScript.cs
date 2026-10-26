using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void LOAD() => UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    public void MENU() => UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    public void DIE() => UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.FatalError);

    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (GameOver) return;
        volumey.value = PlayerPrefs.GetFloat("volumee", 0.75f);
        Looudnoises();
        System.TimeSpan timee = System.TimeSpan.FromSeconds(PlayerPrefs.GetFloat("time", 3599.999f));
        time.text = string.Format("Best Time:\n{0:00}:{1:00}:{2:000}", timee.Minutes, timee.Seconds, timee.Milliseconds);
        for (byte i = 0; i < cakes.Length; i++)
        {
            cakes[i].color = PlayerPrefs.GetInt($"cake{i}") == 1 ? Color.white : Color.black;
        }
        PlayerPrefs.Save();
    }
    public void Looudnoises() => AudioListener.volume = volumey.value;

    public bool GameOver;
    public TMPro.TMP_Text time;
    public UnityEngine.UI.Image[] cakes;
    public UnityEngine.UI.Slider volumey;
}