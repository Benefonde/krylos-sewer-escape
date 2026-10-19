using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImStaringAtYou : MonoBehaviour
{
    private void Start() => cammy = Camera.main.transform;
    void Update() { if (Vector3.Distance(transform.position, cammy.position) <= 50) transform.rotation = cammy.rotation; }
    Transform cammy;
}