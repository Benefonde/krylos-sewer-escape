using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonic : MonoBehaviour
{
    public void OuttaHere() => StartCoroutine(ImOuttaHere());
    IEnumerator ImOuttaHere()
    {
        GetComponent<AudioSource>().ignoreListenerPause = true;
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("outtaHere");
        GetComponent<UnityEngine.UI.Button>().interactable = false;
        rt = GetComponent<RectTransform>();
        yield return new WaitForSecondsRealtime(2);
        while (rt.anchoredPosition.y > -228)
        {
            gravit -= 1.2f;
            rt.anchoredPosition += new Vector2(6, gravit);
            rt.anchoredPosition = new Vector2(Mathf.Floor(rt.anchoredPosition.x), Mathf.Floor(rt.anchoredPosition.y));
            yield return new WaitForSecondsRealtime(1 / 60f);
        }
        Time.timeScale = 1;
        AudioListener.pause = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    RectTransform rt;
    float gravit = 21.2f;
}
