using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


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

    public float speedBoostMultiplier = 2f; // Multiplier for speed boost
    public float speedBoostDuration = 2f;   // How long the boost lasts
    private float speedBoostEndTime = 0f;
    private bool isBoosted = false;
    private float originalSpeed;


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
        originalSpeed = swimSpeed;
    }
        void Update()
    {
        if (isBoosted && Time.time > speedBoostEndTime)
        {
            swimSpeed = originalSpeed; 
            isBoosted = false;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClick();
        }
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
    public void OnClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

       
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            ApplySpeedBoost();
        }
    }

    void ApplySpeedBoost()
    {
        if (!isBoosted)
        {
            swimSpeed *= speedBoostMultiplier;
            speedBoostEndTime = Time.time + speedBoostDuration;
            isBoosted = true;
            ChangeDirection();
        }
    }

}


