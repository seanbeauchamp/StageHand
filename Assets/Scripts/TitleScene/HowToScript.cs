using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToScript : MonoBehaviour
{
    private int currentPage;
    private int lastPage;
    private bool xAxisInUse;
    [SerializeField] GameObject[] pages;
    Canvas[] canvases;

    // Start is called before the first frame update
    void Start()
    {
        currentPage = 0;
        xAxisInUse = false;
        lastPage = pages.Length - 1;
        canvases = new Canvas[pages.Length];
        for (int n = 0; n < pages.Length; n++)
        {
            canvases[n] = pages[n].GetComponent<Canvas>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkForInput();
    }

    private void checkForInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && !xAxisInUse)
        {
            xAxisInUse = true;
            if (Input.GetAxisRaw("Horizontal") > 0)
            {              
                currentPage = (currentPage < lastPage ? currentPage + 1 : currentPage);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                currentPage = (currentPage > 0 ? currentPage - 1 : currentPage);
            }

            for (int n = 0; n < pages.Length; n++)
            {
                if (currentPage == n)
                {
                    canvases[n].enabled = true; ;
                }
                else
                {
                    canvases[n].enabled = false;
                }
            }
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            xAxisInUse = false;
        }

        if (Input.GetButton("Cancel"))
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
