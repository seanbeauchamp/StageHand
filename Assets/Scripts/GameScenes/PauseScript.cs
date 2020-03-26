using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool currentlyPaused;
    [SerializeField] private GameObject pausePanel;

    public Text[] pauseOptions;
    private int currentOption = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentlyPaused = false;
        pausePanel.SetActive(false);
        pausePanel.GetComponent<Image>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.gameRunning || !UI.gameStarted)
            return;

        if (currentlyPaused)
            pauseSelectionCheck();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentlyPaused)
            {
                setPause(false, 1);
            }
            else
            {
                setPause(true, 0);
            }
        }
    }

    void pauseSelectionCheck()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            int prevOption = currentOption;
            currentOption = (currentOption == 0 ? 1 : 0);

            pauseOptions[currentOption].color = Color.white;
            pauseOptions[prevOption].color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentOption == 0)
                setPause(false, 1);
            else
            {
                Time.timeScale = 1;
                UI.resetGame();
            }
        }
    }

    void setPause(bool status, int timeScale)
    {
        currentlyPaused = status;
        pausePanel.SetActive(status);
        Time.timeScale = timeScale;
    }
}
