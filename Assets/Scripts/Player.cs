using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    void Start()
    {
        tc = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        dayTimer -= 0.15f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(CheckDayTimeThing());

        day.Play();
        night.Play();
        RenderSettings.fogStartDistance = 900;
        RenderSettings.fogEndDistance = 1000;
        RenderSettings.fog = true;
        day.time = night.time;
    }

    void Update()
    {
        dayTimer -= Time.deltaTime / dayLength;
        hungar.value = dayTimer;
        dayTxt.text = $"Day {13-Mathf.Ceil(dayTimer)}";
        if (Input.GetMouseButtonDown(0)) Interact();
        PlayerMooove();
    }

    IEnumerator CheckDayTimeThing()
    {
        yield return new WaitUntil(() => dayTimer % 1 <= 0.25f);
        StartCoroutine(DaytimeChangeYay(true));
        yield return new WaitUntil(() => dayTimer % 1 <= 0.8f && dayTimer%1 >= 0.25f);
        StartCoroutine(DaytimeChangeYay(false));
        StartCoroutine(CheckDayTimeThing());
    }

    void PlayerMooove() //i took bits from my older code lol
    {
        if (!tc.isGrounded) gravity -= 50 * Time.deltaTime;
        else if (tc.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            tc.Move(Vector3.up * 0.1f);
            gravity = 15;
        }

        Vector3 vertical = Vector3.up * gravity;
        Vector3 x = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 y = transform.forward * Input.GetAxisRaw("Vertical");
        tc.Move(speed * Time.deltaTime * (x + y).normalized);
        tc.Move(vertical * Time.deltaTime);
    }
    private void LateUpdate()
    {
        Vector2 mouseRotation = new Vector2(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")); //taken from bbs2 lol
        cameraRotation += -mouseRotation * mouseSensitivity;
        cameraRotation = new Vector2(Mathf.Clamp(cameraRotation.x, -80, 80), cameraRotation.y);
        cam.transform.rotation = Quaternion.Euler(cameraRotation);
        cam.transform.position = transform.position + Vector3.up;
        transform.rotation = Quaternion.Euler(cameraRotation * Vector2.up);
    }

    IEnumerator DaytimeChangeYay(bool night)
    {
        if ((night && nightTime) || (!night && !nightTime)) yield break;
        nightTime = night;
        if (night)
        {
            for (byte i = 0; i < 120; i++)
            {
                foreach (Light light in lights.GetComponentsInChildren<Light>())
                {
                    light.intensity -= (1/128f);
                }
                day.volume -= 0.00625f;
                this.night.volume += 0.00625f;
                vigenentinete.color += Color.black * (1 / 120f);
                RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 80, 1 / 36f);
                RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 120, 1 / 36f);
                yield return new WaitForSeconds(1 / 35f);
            }
            day.volume = 0;
            this.night.volume = 0.75f;
        }
        else
        {
            for (byte i = 0; i < 120; i++)
            {
                foreach (Light light in lights.GetComponentsInChildren<Light>())
                {
                    light.intensity += (1 / 128f);
                }
                day.volume += 0.00625f;
                this.night.volume -= 0.00625f;
                vigenentinete.color -= Color.black * (1/120f);
                RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 800, 1 / 36f);
                RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 1200, 1 / 36f);
                yield return new WaitForSeconds(1 / 24f);
            }
            day.volume = 0.75f;
            this.night.volume = 0;
        }
    }

    void Interact()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.transform.parent.name.Contains("Lever") & Vector3.Distance(raycastHit.transform.position, transform.position) < 10f & Cursor.lockState == CursorLockMode.Locked)
            {
                raycastHit.transform.parent.GetComponent<PullTheLeverOrWhatever>().Pully();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.name)
        {
            case "speed":
                speed += 4;
                aud.PlayOneShot(speedbonus);
                break;
        }
        if (other.tag == "item") other.gameObject.SetActive(false);
    }

    public Slider hungar;
    public TMP_Text dayTxt;
    [SerializeField]
    float dayTimer = 12;
    public int dayLength = 480;

    CharacterController tc;
    Transform cam;
    Vector2 cameraRotation;
    float mouseSensitivity = 4;

    float gravity;

    public int speed;

    public Image vigenentinete;
    public GameObject lights;
    public AudioSource day, night;
    bool nightTime;

    public AudioSource aud;
    public AudioClip speedbonus;
}
