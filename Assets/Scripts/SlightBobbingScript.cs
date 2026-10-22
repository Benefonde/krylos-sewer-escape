using UnityEngine;
public class SlightBobbingScript : MonoBehaviour
{
    private void Start()
    {
        if (offset == Vector3.zero) offset = transform.localPosition;
        timeOffset = Random.Range(-1f, 1f) * Random.Range(-6f, 6f);
    }
    void Update()
    {
        time = Time.time + timeOffset;
        Vector3 uh = new Vector3(offset.x, offset.y + Mathf.Sin(time * speed) * maxAmplitude, offset.z);
        if (PixelPerfect) uh = new Vector3(Mathf.Round(uh.x), Mathf.Round(uh.y), Mathf.Round(uh.z));
        if (UI) GetComponent<RectTransform>().anchoredPosition = uh;
        else transform.localPosition = uh;
    }
    public float maxAmplitude;
    public float speed = 1;
    public Vector3 offset;
    float time, timeOffset;
    public bool PixelPerfect;
    public bool UI;
}