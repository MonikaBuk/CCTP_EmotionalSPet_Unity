using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    public float fallSpeed = 2f;
    void Update()
    {
        // Move food downwards
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Destroy the food if it falls below the screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
