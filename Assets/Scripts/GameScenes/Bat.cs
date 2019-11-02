using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 3f;
    [SerializeField]
    float frequency = 5f;
    [SerializeField]
    float magnitude = .3f;

    public bool facingRight = true;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

        if (!facingRight)
            transform.localScale = new Vector3(-transform.localScale.x,
                transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (facingRight)
            MoveRight();
        else
            MoveLeft();

        checkForRemoval();
    }

    void MoveRight()
    {
        position += transform.right * Time.deltaTime * moveSpeed;
        transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    void MoveLeft()
    {
        position -= transform.right * Time.deltaTime * moveSpeed;
        transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    void checkForRemoval()
    {
        //lazy, replace with amount based on measurement of screen size
        if (transform.position.x > 5f || transform.position.x < -5f)
            Destroy(gameObject);
    }
}
