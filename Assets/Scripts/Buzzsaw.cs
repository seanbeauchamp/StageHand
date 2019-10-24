using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzsaw : MonoBehaviour
{
    Dictionary<string, Vector3> directions = new Dictionary<string, Vector3>();
    string[] directionOrder;
    int orderIndex = 0;
    Vector3 origin;

    string currentDirection;
    [SerializeField]
    bool goingRight;
    [SerializeField]
    float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        directions.Add("up", new Vector3(0, 1));
        directions.Add("down", new Vector3(0, -1));
        directions.Add("left", new Vector3(-1, 0));
        directions.Add("right", new Vector3(1, 0));

        if (goingRight)
            directionOrder = new string[4] { "right", "down", "left", "up" };
        else
            directionOrder = new string[4] { "left", "down", "right", "up" };
        currentDirection = directionOrder[orderIndex];
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.GetComponent<Renderer>().bounds.center;
        moveSaw();
    }

    void moveSaw()
    {
        if (!stillOnPlatform())
            changeDirection();
         transform.position = transform.position + directions[currentDirection] * Time.deltaTime * moveSpeed;
    }

    bool stillOnPlatform()
    {
        Vector2 testPosition = origin + directions[currentDirection] * Time.deltaTime * moveSpeed;
        LayerMask mask = LayerMask.GetMask("Beams");
        Collider2D originCollisions = Physics2D.OverlapCircle(testPosition, 0.01f, mask);
        if (originCollisions)
            return true;
        return false;
    }

    void changeDirection()
    {
        orderIndex = (orderIndex >= 3 ? 0 : orderIndex + 1);
        currentDirection = directionOrder[orderIndex];
    }
}
