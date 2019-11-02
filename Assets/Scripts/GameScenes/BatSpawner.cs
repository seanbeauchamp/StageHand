using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    [SerializeField]
    bool facingRight;
    public int level;
    public GameObject bat;

    // Start is called before the first frame update
    void Start()
    {
        //spawnBat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnBat()
    {
        GameObject newBat = Instantiate(bat, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        Bat batScript = newBat.GetComponent<Bat>();
        batScript.facingRight = facingRight;
    }
}
