using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacles : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            PetStats.wasPlayed = true;
            SceneManager.LoadScene("PetScene");
        }
        else if (other.CompareTag("WinZone"))
        {
            PetStats.wasPlayed = true;
            PlayerStats.AddMoney(5);
            SceneManager.LoadScene("PetScene");
        }
    }
}
