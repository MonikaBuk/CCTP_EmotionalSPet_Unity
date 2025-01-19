using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class BowlController : MonoBehaviour
{
    public float speed = 10.0f;
    private float score = 0;
    public TMP_Text scoreText;
 
    private void Update()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0));
        worldPosition.y = -1;
        worldPosition.z = 0; 
        transform.position = worldPosition;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Debug.Log("Food collected!");
            Destroy(other.gameObject);
            score++;
            scoreText.text = score.ToString();
            if (score >= 10)
            {
                score = 0;
                PetStats.wasFed = true;
                PlayerStats.AddMoney(5);
                SceneManager.LoadScene("PetScene");
            }
        }
    }
}

