using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text timer;
    public Text stage;
    public Image statusBar;

    public float timeLeft = 60f;
    public int stageNum;

    // Start is called before the first frame update
    void Start()
    {
        setTimer();
        stage.text = string.Format("Stage: {0}", stageNum);
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer();
    }

    void updateTimer()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            setTimer();
        }
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
    }
}
