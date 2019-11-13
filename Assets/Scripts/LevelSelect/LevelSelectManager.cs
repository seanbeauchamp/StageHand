using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject[] levelBoxes;
    public string[] levels;
    private int currentLevelIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentLevelIndex = 0;
        highlightCurrentBox();
    }

    // Update is called once per frame
    void Update()
    {
        checkForKeyPresses();
    }

    void checkForKeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLevelIndex = (currentLevelIndex >= levelBoxes.Length - 1 ? 0 : currentLevelIndex + 1);
            highlightCurrentBox();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLevelIndex = (currentLevelIndex <= 0 ? levelBoxes.Length - 1 : currentLevelIndex - 1);
            highlightCurrentBox();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(levels[currentLevelIndex], LoadSceneMode.Single);
    }

    void highlightCurrentBox()
    {
        for (int n = 0; n < levelBoxes.Length; n++)
        {
            if (n == currentLevelIndex)
                levelBoxes[n].GetComponent<Image>().color = Color.white;
            else
                levelBoxes[n].GetComponent<Image>().color = Color.green;
        }
    }
}
