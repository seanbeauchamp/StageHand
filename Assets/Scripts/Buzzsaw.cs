using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzsaw : MonoBehaviour
{
    Dictionary<string, Vector2> directions = new Dictionary<string, Vector2>();
    [SerializeField]
    string currentDirection;
    [SerializeField]
    bool goingRight;
    Vector2 origin;

    // Start is called before the first frame update
    void Start()
    {
        directions.Add("up", new Vector2(0, 1));
        directions.Add("down", new Vector2(0, -1));
        directions.Add("left", new Vector2(-1, 0));
        directions.Add("right", new Vector2(1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.GetComponent<Renderer>().bounds.center;


    }

    void checkIfStillOnPlatform()
    {

    }
}
