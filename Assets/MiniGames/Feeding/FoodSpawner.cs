using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    public float spawnInterval = 1.5f; 
    private Camera cam;
    float screenLeft;
    float screenRight;
    float spawnY;


    void Start()
    {
        cam = Camera.main;
        screenLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        spawnY = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        InvokeRepeating(nameof(SpawnFood), 1f, spawnInterval); 
    }
    void SpawnFood()
    {
        float randomX = Random.Range(screenLeft + 1, screenRight - 1);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, -1);
        int randomIndex = Random.Range(0, foodPrefabs.Length);
        Instantiate(foodPrefabs[randomIndex], spawnPosition, Quaternion.identity);
    }


}
