using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneStart : MonoBehaviour
{
    public GameObject UI;
    void Start()
    {
        Time.timeScale = 0;
        UI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        Time.timeScale = 1;
        UI.SetActive(false);
    }
}
