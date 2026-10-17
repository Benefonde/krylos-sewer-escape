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
    }

    void Update()
    {
        dayTimer -= Time.deltaTime / dayLength;
        hungar.value = dayTimer;
        dayTxt.text = $"Day {12-Mathf.Ceil(dayTimer)}";

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
        if (!tc.isGrounded) gravity -= 80 * Time.deltaTime;
        else if (tc.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            tc.Move(Vector3.up * 0.1f);
            gravity = 24;
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
            for (byte i = 0; i < 60; i++)
            {
                foreach (Light light in lights.GetComponentsInChildren<Light>())
                {
                    light.intensity -= (1/64f);
                }
                day.volume -= 0.0125f;
                this.night.volume += 0.0125f;
                vigenentinete.color += Color.black * (1 / 120f);
                yield return new WaitForSeconds(1 / 15f);
            }
            day.volume = 0;
            this.night.volume = 0.75f;
        }
        else
        {
            for (byte i = 0; i < 60; i++)
            {
                foreach (Light light in lights.GetComponentsInChildren<Light>())
                {
                    light.intensity += (1 / 70f);
                }
                day.volume += 0.0125f;
                this.night.volume -= 0.0125f;
                vigenentinete.color -= Color.black * (1/120f);
                yield return new WaitForSeconds(1 / 24f);
            }
            day.volume = 0.75f;
            this.night.volume = 0;
        }
    }

    public Slider hungar;
    public TMP_Text dayTxt;
    [SerializeField]
    float dayTimer = 12;
    public int dayLength = 480*2;

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
}
