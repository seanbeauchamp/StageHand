using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public static bool currentlyPaused = false;
    [SerializeField] private GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        pausePanel.GetComponent<Image>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.gameRunning || !UI.gameStarted)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentlyPaused)
            {
                currentlyPaused = false;
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                currentlyPaused = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
