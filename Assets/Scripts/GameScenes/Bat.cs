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
    }

    // Update is called once per frame
    void Update()
    {
        if (facingRight)
            MoveRight();
        else
            MoveLeft();
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
}
