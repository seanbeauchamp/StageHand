using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public Text timer;
    public Text stage;
    public Text statusLabel;
    public Image statusBar;

    public GameObject winObject;
    public GameObject loseObject;

    public float timeLeft = 60f;
    public int stageNum;

    public static bool gameRunning = true;
    public static bool gameStarted = false;
    public string winScene;

    // Start is called before the first frame update
    void Start()
    {
        //setTimer();
        stage.text = string.Format("Stage: {0}", stageNum);
        if (winScene == "")
            winScene = "title";
        StartCoroutine(OpeningLabelAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameRunning)
            return;

        updateTimer();
    }

    void updateTimer()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            setTimer();
        }
        else
            StartCoroutine(WinGame());
    }

    void setTimer()
    {
        int currentTime = Convert.ToInt32(timeLeft);
        float minutes = Mathf.Floor(currentTime / 60);
        string seconds = (currentTime % 60).ToString("00");
        timer.text = string.Format("{0}:{1}", minutes, seconds);
    }

    public void changeStaturBarFill(float amount)
    {
        statusBar.fillAmount += amount;

        if (statusBar.fillAmount == 0f)
            StartCoroutine(LoseGame());
    }

    IEnumerator OpeningLabelAnimation()
    {
        for (int n = 0; n <= 1; n++)
        {
            statusLabel.GetComponent<Text>().enabled = true;
            yield return new WaitForSeconds(.4f);
            statusLabel.GetComponent<Text>().enabled = false;
            yield return new WaitForSeconds(.4f);
        }
        statusLabel.GetComponent<Text>().text = "Showtime!!!";
        statusLabel.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(.8f);
        statusLabel.GetComponent<Text>().enabled = false;
        gameStarted = true;
        setTimer();
    }

    IEnumerator LoseGame()
    {
        gameRunning = false;
        loseObject.GetComponent<Image>().enabled = true;
        Text[] textChildren = loseObject.GetComponentsInChildren<Text>();
        textChildren[0].enabled = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }

    public IEnumerator WinGame()
    {
        gameRunning = false;
        winObject.GetComponent<Image>().enabled = true;
        Text[] textChildren = winObject.GetComponentsInChildren<Text>();
        textChildren[0].enabled = true;
        yield return new WaitForSeconds(3f);
        gameRunning = true;
        SceneManager.LoadScene(winScene, LoadSceneMode.Single);
    }
}
