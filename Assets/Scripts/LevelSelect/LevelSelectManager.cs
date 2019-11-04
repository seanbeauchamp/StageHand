using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public int numberOfLevels;
    public GameObject levelBox;

    private Vector2 firstBoxPos = new Vector2(32.4f, 115.9f);
    private float offsetIncrement = 25f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(levelBox.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
