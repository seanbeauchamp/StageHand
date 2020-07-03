using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private UI ui;
    private Color defaultColour;
    private Color switchedColour;
    private Color greenColour;

    private IEnumerator switchCoroutine;
    private Coroutine flashCoroutine;
    private Collider2D collider;
    private SpriteRenderer spotlight;

    public bool switched = false;
    public bool eventTargeted = false;
    public float onTime;
    private float elapsedTime = 0f;

    public bool flashing = false;
    private bool flashRoutineRunning = false;

    public bool actorReached = false;
    private bool switchCorrectlyPressed = false;

    public Vector2 lowerLeftCorner;
    public Vector2 lowerRightCorner;

    private AudioSource audioSource;
    [SerializeField] AudioClip startSound;
    [SerializeField] AudioClip failSound;
    [SerializeField] AudioClip succeedSound;


    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        spotlight = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        defaultColour = spriteRenderer.color;
        switchedColour = new Color(1.0f, 0.5f, 0.5f);
        greenColour = new Color(0.6f, 1.0f, 0.6f);

        switchCoroutine = switchRoutine();
        lowerLeftCorner = new Vector2(collider.bounds.min.x, collider.bounds.center.y);
        lowerRightCorner = new Vector2(collider.bounds.min.x + collider.bounds.size.x, collider.bounds.center.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.gameStarted)
            return;
        if (!UI.gameRunning)
            Destroy(gameObject);

        checkForFlash();

        if (eventTargeted)
            checkForCorrectSwitch();

        if (actorReached)
            actorArrived();
    }

    public void switchOn()
    {
        if (switched || switchCorrectlyPressed)
            return;
        switchCoroutine = switchRoutine();
        StartCoroutine(switchCoroutine);
        switchCorrectlyPressed = ((flashing || actorReached) ? true : false);
        if (!switchCorrectlyPressed)
        {
            ui.changeStaturBarFill(-.05f);
            audioSource.Stop();
            audioSource.PlayOneShot(failSound);
        }
        else if (flashing)
        {
            ui.changeStaturBarFill(+.05f);
            audioSource.Stop();
            audioSource.PlayOneShot(succeedSound);
        }
    }

    public void actorArrived()
    {
        cancelFlashRoutine();
        if (switchCorrectlyPressed)
        {
            spotlight.enabled = true;
            spriteRenderer.color = defaultColour;
            
        }
        else
        {
            spriteRenderer.color = switchedColour;
            decreaseStatus();
            
        }
    }

    public void actorLeft()
    {
        spriteRenderer.color = defaultColour;
        spotlight.enabled = false;
        actorReached = false;
        switchCorrectlyPressed = false;
    }

    private void decreaseStatus()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1f)
        {
            ui.changeStaturBarFill(-.06f);
            elapsedTime = 0f;
        }
    }

    private IEnumerator switchRoutine()
    {
        switched = true;
        spriteRenderer.color = switchedColour;
        yield return new WaitForSeconds(onTime);
        switched = false;
        spriteRenderer.color = defaultColour;
    }

    private void checkForFlash()
    {
        if (flashing && !flashRoutineRunning)
        {
            flashCoroutine = StartCoroutine(flashRoutine());
        }
        else if (!flashing && flashRoutineRunning)
        {
            cancelFlashRoutine();
        }
    }

    private void checkForCorrectSwitch()
    {
        if (switched)
        {
            if (!actorReached && switchCorrectlyPressed)
                ui.changeStaturBarFill(.05f);
            switchCorrectlyPressed = true;
        }
    }

    private IEnumerator flashRoutine()
    {
        flashRoutineRunning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(startSound);
        while (flashing)
        {
            if (switchCorrectlyPressed)
                spriteRenderer.color = greenColour;
            else
                spriteRenderer.color = switchedColour;
            yield return new WaitForSeconds(.2f);
            spriteRenderer.color = defaultColour;
            yield return new WaitForSeconds(.2f);
        }
    }

    public void cancelFlashRoutine()
    {
        if (flashRoutineRunning)
        {
            flashRoutineRunning = false;
            StopCoroutine(flashCoroutine);
            spriteRenderer.color = defaultColour;
        }
    }
}
