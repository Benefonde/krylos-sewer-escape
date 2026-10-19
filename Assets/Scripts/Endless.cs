using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endless : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (who == null)
        {
            other.transform.Translate(offset); return;
        }

        if (who.GetComponent<CharacterController>() != null) who.GetComponent<CharacterController>().enabled = false;
        if (who == other.transform) who.Translate(offset, Space.World);
        if (who.GetComponent<CharacterController>() != null) who.GetComponent<CharacterController>().enabled = true;
    }

    public Vector3 offset;
    public Transform who;
}
