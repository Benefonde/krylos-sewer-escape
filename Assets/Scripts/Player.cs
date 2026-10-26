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
        dayTimer = dayAmount - 0.15f;

        hungar.maxValue = dayAmount;

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
        dayTxt.text = $"Day {dayAmount+1-Mathf.Ceil(dayTimer)}";
        if (Input.GetMouseButtonDown(0)) Interact();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (FindObjectOfType<Sonic>() != null) if (FindObjectOfType<Sonic>().outtaHere)
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                return;
            }
            pause.SetActive(!pause.activeSelf);
            Time.timeScale = pause.activeSelf ? 0 : 1;
            Cursor.lockState = pause.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = pause.activeSelf;
            AudioListener.pause = pause.activeSelf;
        }
        PlayerMooove();

        if (transform.position.y < -50) dayTimer = 0;

        if (dayTimer <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }

        if (nightTime)
        {
            scaryLarry -= Time.deltaTime;
            if (scaryLarry <= 0)
            {
                scaryLarry = 1;
                if (Random.Range(0, 50) == 28)
                {
                    OogaBooga();
                    scaryLarry += 9;
                }
            }
        }
    }

    void OogaBooga()
    {
        AudioSource AH = Instantiate(scary, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        AH.gameObject.SetActive(true);
        switch (Random.Range(0, 4))
        {
            case 0: AH.transform.Translate(Vector3.up * 5); break;
            case 1: AH.transform.Translate(Vector3.left * 20); break;
            case 2: AH.transform.Translate(Vector3.forward * -20); break;
            case 3: AH.transform.Translate(Vector3.right * 20); break;
        }
        AH.clip = scaries[Random.Range(0, scaries.Length)];
        AH.Play();
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
        if (!tc.isGrounded) gravity -= (swimmy ? 15 : 50) * Time.deltaTime;
        else if (tc.isGrounded && Input.GetKeyDown(KeyCode.Space) && !swimmy)
        {
            tc.Move(Vector3.up * 0.1f);
            gravity = 15;
        }
        if (swimmy && Input.GetKey(KeyCode.Space)) gravity = 8;

        gravity = Mathf.Clamp(gravity, (swimmy ? -15 : -75), 99);

        Vector3 vertical = Vector3.up * gravity;
        Vector3 x = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 y = transform.forward * Input.GetAxisRaw("Vertical");
        tc.Move(speed * Time.deltaTime * (x + y).normalized);
        tc.Move(vertical * Time.deltaTime);
    }
    private void LateUpdate()
    {
        if (Time.timeScale == 0) return;
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
                this.night.volume += 0.0041666f;
                vigenentinete.color += Color.black * (1 / 180f);
                RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 50, 1 / 36f);
                RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 120, 1 / 36f);
                yield return new WaitForSeconds(1 / 35f);
            }
            day.volume = 0;
            this.night.volume = 0.5f;
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
                this.night.volume -= 0.0041666f;
                vigenentinete.color -= Color.black * (1/180f);
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
            if (raycastHit.transform.parent.name.Contains("Lever") & Vector3.Distance(raycastHit.transform.position, transform.position) < 15f & Cursor.lockState == CursorLockMode.Locked)
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
                speed += 6;
                aud.PlayOneShot(speedbonus);
                break;
            case "cake":
                foundThem.color = Color.white;
                aud.PlayOneShot(foundCake);
                StartCoroutine(Cakes());
                PlayerPrefs.SetInt($"cake{other.transform.GetChild(0).name}", 1);
                cakes++;
                if (cakes >= 3) PlayerPrefs.SetInt("FoundMyHiddenCakes", 1);
                break;
            case "EscapeFromHairBasics":
                print((12 - dayTimer) * dayLength);
                if (PlayerPrefs.GetFloat("time", 3599.999f) > (int)((12 - dayTimer) * dayLength)) PlayerPrefs.SetFloat("time", (dayAmount-dayTimer)*dayLength);
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
                break;
            case "Springy":
                if (gravity <= -5)
                {
                    other.transform.GetComponent<AudioSource>().Play();
                    gravity = 40;
                }
                break;
        }
        if (other.tag == "item") other.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "WaterStream")
        {
            swimmy = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "WaterStream")
        {
            swimmy = false;
        }
    }

    IEnumerator Cakes()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 25; i++)
        {
            foundThem.color -= new Color(0, 0, 0, 1 / 24f);
            yield return new WaitForSeconds(1 / 50f);
        }
    }

    public Slider hungar;
    public TMP_Text dayTxt;
    [SerializeField]
    float dayTimer = 12;
    public int dayAmount = 12;
    public int dayLength = 480;

    CharacterController tc;
    Transform cam;
    public Vector2 cameraRotation;
    float mouseSensitivity = 4;

    float gravity;
    bool swimmy;
    public int speed;

    public GameObject pause;
    public Image vigenentinete;
    public GameObject lights;
    public AudioSource day, night;
    bool nightTime;

    public AudioSource aud, scary;
    public AudioClip speedbonus, foundCake;
    public AudioClip[] scaries;

    byte cakes;
    public Image foundThem;

    float scaryLarry = 1;
}
