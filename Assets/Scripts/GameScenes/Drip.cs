using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drip : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    private bool dropped;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        StartCoroutine("StartDrop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartDrop()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("DropStarted", true);
        yield return new WaitForSeconds(.1f);
        rb2d.gravityScale = 1.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Destroy(gameObject);
    }
}
