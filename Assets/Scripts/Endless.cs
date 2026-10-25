using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endless : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (who == null)
        {
            other.transform.Translate(offset, Space.World);
            other.transform.Rotate(rotOffset);
            return;
        }

        if (who.GetComponent<CharacterController>() != null) who.GetComponent<CharacterController>().enabled = false;
        if (who == other.transform)
        {
            who.Translate(offset, Space.World);
            who.Rotate(rotOffset);
            if (who.name == "Krylos") who.GetComponent<Player>().cameraRotation += new Vector2(rotOffset.x, rotOffset.y);
        }
        if (who.GetComponent<CharacterController>() != null) who.GetComponent<CharacterController>().enabled = true;
    }

    public Vector3 offset;
    public Vector3 rotOffset;
    public Transform who;
}
