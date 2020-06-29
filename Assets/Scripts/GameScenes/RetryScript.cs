using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryScript : MonoBehaviour
{
    public static bool isOn;
    [SerializeField] private GameObject retryPanel;
    public Text[] retryOptions;
    private int currentOption = 0;
    private bool yAxisInUse = false;
    private bool submitPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        retryPanel.SetActive(false);
        retryPanel.GetComponent<Image>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
            RetrySelectionCheck();
    }

    private void RetrySelectionCheck()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && !yAxisInUse)
        {
            int prevOption = currentOption;
            currentOption = (currentOption == 0 ? 1 : 0);

            retryOptions[currentOption].color = Color.white;
            retryOptions[prevOption].color = Color.gray;
            yAxisInUse = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            yAxisInUse = false;
        }
    }

    public void EnableRestart()
    {
        isOn = true;
        retryPanel.SetActive(true);
        for (int n = 0; n < retryOptions.Length; n++)
            retryOptions[n].enabled = true;

    }
}
