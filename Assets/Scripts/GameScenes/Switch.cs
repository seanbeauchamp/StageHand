using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Sprite offSprite;
    public Sprite onSprite;

    public GameObject player;
    private Player playerScript;

    public GameObject prop;
    private Prop propScript;

    private SpriteRenderer spriteRenderer;
    private Collider2D coll2d;

    private UI ui;

    public bool switched = false;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll2d = GetComponent<Collider2D>();
        playerScript = player.GetComponent<Player>();
        propScript = prop.GetComponent<Prop>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForPlayerFlip();
    }

    private void checkForPlayerFlip()
    {
        Collider2D[] overLaps = Physics2D.OverlapCircleAll(spriteRenderer.bounds.center, 0.02f);
        foreach(Collider2D overlap in overLaps)
        {
            if (overlap == playerScript.flipCollider && playerScript.flipping)
                flipSwitch();
        }
    }

    public void flipSwitch()
    {
        if (!switched)
            StartCoroutine(SwitchRoutine());
    }

    IEnumerator SwitchRoutine()
    {
        switched = true;
        propScript.switchOn();        
        spriteRenderer.sprite = onSprite;
        yield return new WaitForSeconds(.3f);
        spriteRenderer.sprite = offSprite;
        switched = false;
    }
}
