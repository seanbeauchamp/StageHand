using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public GameObject warpTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float newX = warpTarget.transform.position.x;
            GameObject player = collision.gameObject;
            player.transform.position = new Vector3(newX, player.transform.position.y, player.transform.position.z);
        }
    }
}
