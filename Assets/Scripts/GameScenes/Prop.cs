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

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        defaultColour = spriteRenderer.color;
        switchedColour = new Color(1.0f, 0.5f, 0.5f);
        greenColour = new Color(0.6f, 1.0f, 0.6f);

        switchCoroutine = switchRoutine();
        //lowerLeftCorner = collider.bounds.min;
        //lowerRightCorner = collider.bounds.min + new Vector3(collider.bounds.size.x, 0, 0);
        lowerLeftCorner = new Vector2(collider.bounds.min.x, collider.bounds.center.y);
        lowerRightCorner = new Vector2(collider.bounds.min.x + collider.bounds.size.x, collider.bounds.center.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.gameRunning)
            return;

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
            ui.changeStaturBarFill(-.05f);
        else if (flashing)
            ui.changeStaturBarFill(+.05f);
    }

    public void actorArrived()
    {
        cancelFlashRoutine();
        if (switchCorrectlyPressed)
            spriteRenderer.color = greenColour;
        else
        {
            spriteRenderer.color = switchedColour;
            decreaseStatus();
        }
    }

    public void actorLeft()
    {
        spriteRenderer.color = defaultColour;
        actorReached = false;
        switchCorrectlyPressed = false;
    }

    private void decreaseStatus()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1f)
        {
            ui.changeStaturBarFill(-.075f);
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

    private void cancelFlashRoutine()
    {
        if (flashRoutineRunning)
        {
            flashRoutineRunning = false;
            StopCoroutine(flashCoroutine);
            spriteRenderer.color = defaultColour;
        }
    }
}
