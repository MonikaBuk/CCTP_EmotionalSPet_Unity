using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BreathingExcersiseTimer : MonoBehaviour
{
    public TMP_Text timer;
    public CustomSceneLoader sceneLoader;

    public float targetTime = 60.0f;
    private bool excersiseEnded = false;

    private void Start()
    {
        bool excersiseEnded = false;
    }

    private void Update()
    {

        targetTime -= Time.deltaTime;
        int countdown = Mathf.FloorToInt(targetTime);
        timer.text = countdown.ToString();

        if (targetTime <= 0.0f && !excersiseEnded)
        {
            timerEnded();
            excersiseEnded = true;
        }

    }

    void timerEnded()
    {
        AquariumManagger.AddNewFish("1day");
        sceneLoader.LoadSceneAsync("PetScene");
    }
}