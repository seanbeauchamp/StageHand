using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Actor : Person
{
    private bool canAct = false;
    private bool isTargeting = false;
    private bool atTarget = false;
    private Collider2D collider;

    public string stepName;
    private List<Step> steps;
    private int currentStepIndex;
    private int maxSteps;

    private float moveDistance = .25f;
    private float targetDistance;
    private float boundsBuffer = .02f;
    public Vector2 newTarget; //gets used as movetowards target on movement
    public Vector2 position;

    public int minToTarget;
    public int maxToTarget;

    public int minActsBeforeTargeting;
    private int currentActNum;

    private Prop propScript;
    public GameObject[] props;

    //0 is down, then numbers move clockwise
    private Vector2[] moveDirections = new Vector2[]
    {
        Vector2.down, Vector2.down + Vector2.left, Vector2.left,
        Vector2.left + Vector2.up, Vector2.up, Vector2.up + Vector2.right,
        Vector2.right, Vector2.right + Vector2.down
    };

    private GameObject currentTargetProp;
    private Vector2 currentPropSide;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        props = GameObject.FindGameObjectsWithTag("Prop");

        RetrieveSteps();

        targetDistance = moveDistance - .1f;
        speed = .1f;
        ignoreStagePlatforms();
        StartCoroutine(stallBetweenActions(1f, 2.5f));
        currentActNum = 0;
        currentStepIndex = 0;
        maxSteps = steps.Count;
    }


    // Update is called once per frame
    void Update()
    {
        if (!UI.gameRunning)
            Destroy(gameObject);

        if (canAct)
        {
            //determineMovementType();
            takeNextAction();
        }
        else if (isMoving)
        {
            if (!isTargeting)
                blankMoveStep();
            else
                targetMoveStep();
            determineLayer();
        }
    }

    private void takeNextAction()
    {
        Step currentStep = steps[currentStepIndex];
        switch (currentStep.Action)
        {
            case "stall":
                stallBetweenActions((float)currentStep.Value, (float)currentStep.Value);
                break;
            case "target":
                initiateTargetMove((int)currentStep.Value);
                break;
            default:
                int directionIndex = currentStep.Action.IndexOf("-");
                string direction = currentStep.Action.Substring(directionIndex + 1);
                initiateBlankMove(direction);
                break;
        }

        currentStepIndex++;
        if (currentStepIndex >= maxSteps)
            currentStepIndex = 0;
    }

    private void initiateTargetMove(int targetIndex)
    {
        //currentTargetProp = findNearestProp(true);
        currentTargetProp = props[targetIndex];
        propScript = currentTargetProp.GetComponent<Prop>();
        propScript.flashing = true;
        float lineToRightCorner = Vector2.Distance(collider.bounds.center, propScript.lowerRightCorner);
        float lineToLeftCorner = Vector2.Distance(collider.bounds.center, propScript.lowerLeftCorner);
        if (lineToRightCorner < lineToLeftCorner)
            currentPropSide = propScript.lowerRightCorner;
        else
            currentPropSide = propScript.lowerLeftCorner;
        int horizontal = (currentPropSide.x > transform.position.x ? 1 : -1);
        flip(horizontal);
        isTargeting = true;
        isMoving = true;
        animator.SetBool("Run", isMoving);
        canAct = false;
        currentActNum = 0;
    }


    private void targetMoveStep()
    {
        position = transform.position;
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(position, currentPropSide, step);

        Vector2 position2d = new Vector2(transform.position.x, transform.position.y);
        if (position2d == currentPropSide)
        {
            propScript.flashing = false;
            isMoving = false;
            isTargeting = false;
            animator.SetBool("Run", isMoving);
            StartCoroutine(actAtProp(3f, 5f, propScript));
        }
    }


    private void initiateBlankMove(string directionText)
    {
        Vector2 direction = chooseDirection(directionText);
        if (direction != Vector2.zero)
        {
            currentActNum++;
            Vector3 center = collider.bounds.center;
            newTarget = new Vector2(center.x, center.y) + (direction * moveDistance);
            isMoving = true;
            int horizontal = (int)direction.x;
            flip(horizontal);
            animator.SetBool("Run", isMoving);
            canAct = false;
        }
    }


    private void blankMoveStep()
    {
        position = transform.position;
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(position, newTarget, step);

        Vector2 position2d = new Vector2(transform.position.x, transform.position.y);
        if (position2d == newTarget)
        {
            isMoving = false;
            animator.SetBool("Run", isMoving);
            StartCoroutine(stallBetweenActions(1f, 2.5f));
        }
    }


    IEnumerator stallBetweenActions(float min, float max, Prop propScript = null)
    {
        canAct = false;
        float newTimeFrame = Random.Range(min, max);
        if (propScript != null)
            propScript.actorArrived();
        yield return new WaitForSeconds(newTimeFrame);
        canAct = true;
        if (propScript != null)
            propScript.actorLeft();
    }

    //will include some kind of animation switch later. For now just a similar stall to the above method
    IEnumerator actAtProp(float min, float max, Prop propScript)
    {
        atTarget = true;
        canAct = false;
        propScript.actorReached = true;
        float newTimeFrame = Random.Range(min, max);
        yield return new WaitForSeconds(newTimeFrame);
        atTarget = false;
        canAct = true;
        propScript.actorLeft();
        //StartCoroutine(stallBetweenActions(1f, 2.5f));
    }

    //only needs to be called once, we'll never need these collisions re-enabled
    private void ignoreStagePlatforms()
    {
        GameObject[] platformObjects = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platformObject in platformObjects)
        {
            Collider2D ignorableCollision = platformObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignorableCollision);
        }
    }


    private void determineLayer()
    {
        GameObject currentProp = findNearestProp(false);
        if (transform.position.y > currentProp.transform.position.y)
            spriteRenderer.sortingLayerName = "ActorBack";
        else
            spriteRenderer.sortingLayerName = "ActorFront";
    }

    //find the prop closest to the actor (that isn't already targeted), which will be their new event target
    private GameObject findNearestProp(bool forTarget)
    {
        GameObject[] props = GameObject.FindGameObjectsWithTag("Prop");
        GameObject currentObject = null;
        for (int n = 0; n < props.Length; n++)
        {
            Prop propScript = props[n].GetComponent<Prop>();
            if (currentObject is null && !propScript.flashing)
                currentObject = props[n];
            else if (currentObject != null && !propScript.flashing)
            {
                float currentDistance = Vector2.Distance(currentObject.transform.position, transform.position);
                float newDistance = Vector2.Distance(props[n].transform.position, transform.position);
                if (newDistance < currentDistance)
                    currentObject = props[n];
            }
        }
        return currentObject;
    }

    private void RetrieveSteps()
    {
        foreach (ActorSteps actorSteps in StepsManager.actorSteps)
        {
            if (actorSteps.actorName == stepName)
            {
                steps = actorSteps.steps;
                break;
            }
        }
    }

    private Vector2 chooseDirection(string directionText)
    {
        switch (directionText)
        {
            case "up":
                return Vector2.up;
            case "left":
                return Vector2.left;
            case "right":
                return Vector2.right;
            case "down":
                return Vector2.down;
            default:
                Debug.Log("Error: Unrecognized direction value " + directionText);
                return Vector2.zero;    
        }
    }
}
