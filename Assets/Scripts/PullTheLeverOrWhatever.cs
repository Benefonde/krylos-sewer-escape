using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTheLeverOrWhatever : MonoBehaviour
{
    public void Pully()
    {
        if (yes) return;
        yes = true;
        GetComponentInChildren<Animator>().SetTrigger("pull");
        gate.Translate(Vector3.up * 9);
        gate.GetComponent<AudioSource>().Play();
    }
    bool yes;
    public Transform gate;
}
