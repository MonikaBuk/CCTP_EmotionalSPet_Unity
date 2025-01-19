using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FishMovement : MonoBehaviour
{
    public float swimSpeed = 3f;
    public float verticalMotionRange = 0.2f;
    public float directionChangeTime = 2f;
    public float rotationSpeed = 5f;

    private float timeSinceLastDirectionChange = 0f;
    private int swimDirection = 1;

    private Camera mainCamera;
    private float cameraWidth;
    private float cameraHeight;

    void Start()
    {
        swimDirection = Random.value > 0.5f ? 1 : -1;
        mainCamera = Camera.main;
        cameraWidth = mainCamera.orthographicSize * mainCamera.aspect;
        cameraHeight = mainCamera.orthographicSize;
        Vector3 newSpawnPoint = new Vector3(
        Random.Range(-cameraWidth, cameraWidth),
        Random.Range(-cameraHeight, cameraHeight), 0);
        transform.position = newSpawnPoint;
    }
        void Update()
    {
        MoveFish();
        ApplyVerticalMotion();

        timeSinceLastDirectionChange += Time.deltaTime;
        if (timeSinceLastDirectionChange >= directionChangeTime)
        {
            ChangeDirection();
            timeSinceLastDirectionChange = 0f;
        }
        RotateFish();
    }

    void MoveFish()
    {
        transform.position += new Vector3(swimDirection * swimSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x > cameraWidth || transform.position.x < -cameraWidth)
        {
            ChangeDirection();
        }
    }

    void ApplyVerticalMotion()
    {
        float verticalMotion = Mathf.Sin(Time.time) * verticalMotionRange;
        float newYPosition = transform.position.y + verticalMotion;
        newYPosition = Mathf.Clamp(newYPosition, -cameraHeight, cameraHeight);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }

    void ChangeDirection()
    {
        swimDirection = -swimDirection;
    }

    void RotateFish()
    {
        Vector3 currentScale = transform.localScale;

        if (swimDirection == 1)
        {
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
    }
}


