using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenCakeNumberTwo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == playe) region = true;
        if (transform.parent.GetComponent<HiddenCake2Manager>().isActiveAndEnabled) transform.parent.GetComponent<HiddenCake2Manager>().RegionGot();
    }
    public bool region;
    public Transform playe;
}
