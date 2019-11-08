using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLogic : MonoBehaviour
{
    public GameObject player;
    public GameObject[] batSpawners;

    private Player playerScript;
    private Vector2 playerOrigin;

    public int currentLevel = 1;
    private int lastLevel = 0;
    private bool batCountingDown = false;
    public float batTime;

    private Coroutine countDown;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.gameRunning)
            return;

        checkPlayerLevel();
        compareLevels();
        //if no coroutines running after the compare, start countdown again with same level
        if (!batCountingDown)
            StartCoroutine(BatCountDown(chooseBatSpawner()));
    }

    private void checkPlayerLevel()
    {
        playerOrigin = player.transform.GetComponent<Renderer>().bounds.center;
        for (int n = 0; n < batSpawners.Length; n++)
        {
            Collider2D spawnCollider = batSpawners[n].GetComponent<Collider2D>();
            float minY = spawnCollider.bounds.center.y - (spawnCollider.bounds.size.y / 2);
            float maxY = spawnCollider.bounds.center.y + (spawnCollider.bounds.size.y / 2);
            if (playerOrigin.y > minY && playerOrigin.y < maxY)
            {
                currentLevel = batSpawners[n].GetComponent<BatSpawner>().level;
                break;
            }
        }
    }

    private void compareLevels()
    {
        if (currentLevel != lastLevel)
        {
            lastLevel = currentLevel;
            StopAllCoroutines();
            StartCoroutine(BatCountDown(chooseBatSpawner()));
        }
    }

    IEnumerator BatCountDown(GameObject batSpawner)
    {
        batCountingDown = true;
        yield return new WaitForSeconds(batTime);

        BatSpawner spawnerScript = batSpawner.GetComponent<BatSpawner>();
        spawnerScript.spawnBat();
        batCountingDown = false;
    }

    private GameObject chooseBatSpawner()
    {
        List<GameObject> applicableSpawners = new List<GameObject>();
        for (int n = 0; n < batSpawners.Length; n++)
        {
            int spawnerLevel = batSpawners[n].GetComponent<BatSpawner>().level;
            if (spawnerLevel == currentLevel)
                applicableSpawners.Add(batSpawners[n]);
        }

        int chosenSpawnerIndex = Random.Range(0, applicableSpawners.Count);
        return applicableSpawners[chosenSpawnerIndex];
    }
}
