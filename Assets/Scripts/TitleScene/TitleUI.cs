using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public Text start;
    public Text credits;
    public Text exit;

    float[] positions = new float[3];
    float cursorOrigin;
    
    public Image cursor;
    int currentCursorPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(360, 640, false);
    }

    private void Awake()
    {
        positions[0] = start.transform.position.y;
        positions[1] = credits.transform.position.y;
        positions[2] = exit.transform.position.y;

        cursor.transform.position = new Vector3(cursor.transform.position.x, positions[currentCursorPos], cursor.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        checkForInput();
    }

    void checkForInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentCursorPos = (currentCursorPos >= positions.Length - 1 ? 0 : currentCursorPos + 1);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentCursorPos = (currentCursorPos <= 0 ? positions.Length - 1 : currentCursorPos - 1);
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentCursorPos)
            {
                case 0:
                    SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
                    break;
                case 1:
                    SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
                    break;
                case 2:
                    Application.Quit();
                    Debug.Log("Game Exited");
                    break;
                default:
                    break;
            }
        }

        cursor.transform.position = new Vector3(cursor.transform.position.x, positions[currentCursorPos], cursor.transform.position.z);
    }
}
