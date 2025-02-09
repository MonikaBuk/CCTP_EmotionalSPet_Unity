using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; 
    public float spawnInterval = 5f;
    public Vector3 spawnPosition = new Vector3(0f, -5f, 0f); 

    void Start()
    {
        InvokeRepeating("SpawnBubble", 0f, spawnInterval); 
    }

    void SpawnBubble()
    {
        Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
    }
}
