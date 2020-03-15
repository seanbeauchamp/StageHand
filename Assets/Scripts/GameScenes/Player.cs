using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Person
{
    float climbSpeed = 1.5f;

    public float batTime;
   
    public bool canClimb = false;
    public bool climbing = false;
    public bool flipping = false;
    public bool reachedTop = false;
    public bool invincible = false;
    Vector2 currentDirection;

    private bool frozen = false;

    private float ladderX;
    public GameObject tilemapGameObject;
    Tilemap tileMap;
    float ladderMiddle = 0.125f;

    public Collider2D coll2d;
    public Collider2D flipCollider;
    public Sprite hurtSprite;
    public Sprite downSprite;

    private Collider2D ignorableCollision;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        tileMap = tilemapGameObject.GetComponent<Tilemap>();

        speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.gameRunning)
            Destroy(gameObject);

            Flip();
            Move();
            Climb();
    }

    void Flip()
    {
        if (!isMoving && !climbing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(flipSwitch());
        }
    }

    IEnumerator flipSwitch()
    {
        flipping = true;
        animator.SetBool("Flip", true);       

        yield return new WaitForSeconds(.1f);
        flipping = false;
        animator.SetBool("Flip", false);
    }

    //will run indefinitely until stopped within seperate routine
    IEnumerator flash()
    {
        while (true)
        {
            invincible = true;
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);           
        }
    }

    public IEnumerator HurtRoutine()
    {
        frozen = true;
        IEnumerator flashRoutine = flash();
        StartCoroutine(flashRoutine);
        animator.SetBool("Hurt", true);
        float xChange = (transform.localScale.x < 0 ? .1f : -.1f);
        transform.position += new Vector3(xChange, 0, 0);
        yield return new WaitForSeconds(2f);        
        animator.SetBool("Hurt", false);
        frozen = false;
        yield return new WaitForSeconds(1f);
        StopCoroutine(flashRoutine);
        spriteRenderer.enabled = true;
        invincible = false;
    }

    void Move()
    {
        if (flipping || frozen)
            return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");       

        if (horizontal != 0)
        {
            currentDirection = new Vector2((float)horizontal, 0);
            transform.position += new Vector3(currentDirection.x * speed * Time.deltaTime, 0,0);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        flip(horizontal);
        
        animator.SetBool("Run", isMoving);
    }  

    void Climb()
    {
        LadderCheck(0);
        if (!isMoving && !flipping && !frozen && canClimb)
        {
            int vertical = (int)Input.GetAxisRaw("Vertical");

            if (vertical != 0)
            {
                startClimb();
                animator.enabled = true;
                rb2d.gravityScale = 0;
                currentDirection = new Vector2(0, (float)vertical);
                LadderCheck(currentDirection.y);               
                if (canClimb)
                {
                    transform.position += new Vector3(0, currentDirection.y * climbSpeed * Time.deltaTime, 0);
                    ignoreBeam();
                }
                else
                {
                    revertCollisionIgnore();
                    stopClimb();
                }
            }          
            else if (climbing)
                animator.enabled = false;
        }
        else
        {
            stopClimb();
            revertCollisionIgnore();
            animator.enabled = true;
            rb2d.gravityScale = 1.3f;
        }

       animator.SetBool("Climb", climbing);
    }

    void LadderCheck(float yVal)
    {
        Vector3 center = spriteRenderer.bounds.center;
        LayerMask mask = LayerMask.GetMask("Ladder");
        float height = coll2d.bounds.size.y;
        float currentCast;
        if (climbing)
            currentCast = height / 2f;
        else
            currentCast = height / 2f + .02f;
        RaycastHit2D hit = Physics2D.Raycast(center, Vector2.down, currentCast, mask);
        if (hit.collider != null)
        {
            canClimb = true;
            //get the tile position from the map, then convert that to the actual game positioning
            Vector3Int tilePos = tileMap.WorldToCell(hit.point);
            Vector3 worldTilePos = tileMap.CellToWorld(tilePos);
            ladderX = worldTilePos.x + ladderMiddle;
        }
        else
            canClimb = false;
        //if player is trying to move down this turn
        if (yVal < -0.1f)
        {
            Vector2 tempPos = transform.position + new Vector3(0, yVal * climbSpeed * Time.deltaTime, 0);
            Vector2 pointPosition = new Vector2(tempPos.x, tempPos.y - height / 2f - 0.02f);
            Collider2D[] ladderCircles = Physics2D.OverlapCircleAll(pointPosition, 0.01f);
            foreach (Collider2D ladderCircle in ladderCircles)
            {
                if (ladderCircle.gameObject.tag == "Ladder")
                {
                    canClimb = true;
                    return;
                }
            }
            canClimb = false;
            return;
        }
    }

    void ignoreBeam()
    {
        Vector3 center = spriteRenderer.bounds.center;
        float height = coll2d.bounds.size.y;
        RaycastHit2D[] hits = Physics2D.RaycastAll(center, Vector2.down, height / 2f + .02f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Platform")
            {
                ignorableCollision = hit.collider.gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignorableCollision);
            }
        }
    }

    void revertCollisionIgnore()
    {
        if (ignorableCollision != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignorableCollision, false);
            ignorableCollision = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //LayerMask platforms = LayerMask.GetMask("Beams");
        if (collision.gameObject.tag == "Platform" && climbing)
        {
            ignorableCollision = collision.gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignorableCollision);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard" && !invincible)
        {
            StartCoroutine(HurtRoutine());
        }
    }

    void startClimb()
    {
        climbing = true;
        flipCollider.enabled = false;
        transform.position = new Vector3(ladderX, transform.position.y);
    }

    void stopClimb()
    {
        climbing = false;
        flipCollider.enabled = true;
    }
}
