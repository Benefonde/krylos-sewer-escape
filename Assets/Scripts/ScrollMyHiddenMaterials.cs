using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMyHiddenMaterials : MonoBehaviour
{
    float scrollSpeed = 1.28f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.mainTextureOffset = new Vector2(0, Time.time * -scrollSpeed);
    }
}
