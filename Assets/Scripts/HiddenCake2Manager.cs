using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenCake2Manager : MonoBehaviour
{
    void Start()
    {
        aud = GetComponent<AudioSource>();
        regions = GetComponentsInChildren<HiddenCakeNumberTwo>();
    }

    void Update()
    {
        if (timey > 0) timey -= Time.deltaTime;
        else if (GO) KillRegions(true);
    }

    void KillRegions(bool evil)
    {
        for (byte i = 0; i < regions.Length; i++) regions[i].region = false;
        if (evil) GO = false;
        else laps++;
    }

    public void RegionGot()
    {
        GO = true;
        timey = 3.5f;
        foreach (HiddenCakeNumberTwo hc in regions)
        {
            if (!hc.region) return;
        }
        aud.PlayOneShot(numbas[laps]);
        KillRegions(false);
        if (laps >= 5)
        {
            cake.SetActive(true);
            this.enabled = false;
        }
    }

    HiddenCakeNumberTwo[] regions = new HiddenCakeNumberTwo[4];
    float timey;
    bool GO;
    [SerializeField]
    byte laps;
    AudioSource aud;
    public AudioClip[] numbas;
    public GameObject cake;
}
