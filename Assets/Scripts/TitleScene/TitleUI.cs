using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public Text start;
    public Text credits;
    public Text howTo;
    public Text exit;

    float[] positions = new float[4];
    float cursorOrigin;
    
    public Image cursor;
    int currentCursorPos = 0;
    public bool yAxisInUse = false;

    private AudioSource audioSource;
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip confirmSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Screen.SetResolution(360, 640, false);
    }

    private void Awake()
    {
        positions[0] = start.transform.position.y;
        positions[1] = credits.transform.position.y;
        positions[2] = howTo.transform.position.y;
        positions[3] = exit.transform.position.y;

        cursor.transform.position = new Vector3(cursor.transform.position.x, positions[currentCursorPos], cursor.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        checkForInput();
    }

    void checkForInput()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && !yAxisInUse)
        {
            currentCursorPos = (currentCursorPos >= positions.Length - 1 ? 0 : currentCursorPos + 1);
            yAxisInUse = true;
            audioSource.Stop();
            audioSource.PlayOneShot(moveSound);
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && !yAxisInUse)
        {
            currentCursorPos = (currentCursorPos <= 0 ? positions.Length - 1 : currentCursorPos - 1);
            yAxisInUse = true;
            audioSource.Stop();
            audioSource.PlayOneShot(moveSound);
        }
        else if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(confirmSound);
            switch (currentCursorPos)
            {
                case 0:
                    SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
                    break;
                case 1:
                    SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
                    break;
                case 2:
                    SceneManager.LoadScene("HowToPlay", LoadSceneMode.Single);
                    break;
                case 3:
                    Application.Quit();
                    Debug.Log("Game Exited");
                    break;
                default:
                    break;
            }
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            yAxisInUse = false;
        }

        cursor.transform.position = new Vector3(cursor.transform.position.x, positions[currentCursorPos], cursor.transform.position.z);
    }
}
