using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentLeakScript : MonoBehaviour
{
    [SerializeField] private GameObject drip;
    [Range(2f, 10f)] [SerializeField] private float initialDrop = 2f;
    [Range(4f, 10f)] [SerializeField] private float nextDrop = 4f;

    private bool firstDropDripped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.gameStarted || (!UI.gameRunning))
            return;
        if (!firstDropDripped)
        {
            Invoke("DropDrip", initialDrop);
            firstDropDripped = true;
        }
    }

    void DropDrip()
    {
        GameObject newDrip = Instantiate(drip, new Vector2(transform.position.x, transform.position.y - .15f), Quaternion.identity);
        Invoke("DropDrip", nextDrop);
    }
}
