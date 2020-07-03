using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public static bool currentlyPaused;
    [SerializeField] private GameObject pausePanel;

    public Text[] pauseOptions;
    private int currentOption = 0;
    private bool yAxisInUse = false;
    private bool submitPressed = false;

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

        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("KeyPause")) && !currentlyPaused && !submitPressed)
        {
             setPause(true, 0);
        }
        submitPressed = false;
    }

    void pauseSelectionCheck()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && !yAxisInUse)
        {
            int prevOption = currentOption;
            currentOption = (currentOption == 0 ? 1 : 0);

            pauseOptions[currentOption].color = Color.white;
            pauseOptions[prevOption].color = Color.gray;
            yAxisInUse = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            yAxisInUse = false;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit"))
        {
            if (currentOption == 0)
            {
                setPause(false, 1);
                submitPressed = true;
            }
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
        for (int n = 0; n < pauseOptions.Length; n++)
            pauseOptions[n].enabled = status;
        Time.timeScale = timeScale;
    }
}
