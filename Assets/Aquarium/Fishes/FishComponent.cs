using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishComponent : MonoBehaviour
{
    public int fishID;  
    public string acvhievedDate;
    public string entry;
    private AchievementTextManager achievementTextManager;
    private AudioSource audioSource;

    private void Start()
    {
        achievementTextManager = FindObjectOfType<AchievementTextManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(int id, string date, string txtInput)
    {
        fishID = id;
        acvhievedDate = date;
        entry = txtInput;
        Debug.Log($"Fish {fishID} spawned!");
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);


        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            audioSource.Play();
            achievementTextManager.ActivateText(entry + " " + acvhievedDate + ".");
        }
    }
}
