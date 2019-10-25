using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Rigidbody2D rb2d;

    public float speed;
    public bool isMoving = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void flip(int horizontal)
    {
        bool flipSprite = (transform.localScale.x < 0 ? (horizontal > 0.01f) : (horizontal < -0.1f));
        if (flipSprite)
        {
            transform.localScale = new Vector3(-transform.localScale.x,
                transform.localScale.y, transform.localScale.z);
        }
    }
}
