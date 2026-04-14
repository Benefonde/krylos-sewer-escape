using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FINDTHEM : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Find());
    }
    IEnumerator Find()
    {
        transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), 1);
        yield return new WaitForSeconds(1 / 30f);
        StartCoroutine(Find());
    }
    private void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            gameObject.SetActive(false);
            good.SetActive(true);
        }
    }

    public GameObject good;
}
