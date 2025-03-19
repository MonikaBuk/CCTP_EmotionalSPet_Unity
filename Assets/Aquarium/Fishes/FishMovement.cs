using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class FishMovement : MonoBehaviour
{
    public float swimSpeed = 3f;
    public float verticalMotionRange = 0.2f;
    public float minDirectionChangeTime = 2f; 
    public float maxDirectionChangeTime = 4f;
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
    private float startY;
    private float directionChangeTime;
    private float verticalSpeed;
    private float phaseOffset;


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
        float spawnX = Mathf.Clamp(Random.Range(-cameraWidth, cameraWidth), -cameraWidth + 0.5f, cameraWidth - 0.5f);
        float spawnY = Mathf.Clamp(Random.Range(-cameraHeight, cameraHeight), -cameraHeight - 5f, cameraHeight + 0.5f);

        transform.position = new Vector3(spawnX, spawnY, 0);
        startY = transform.position.y;
        directionChangeTime = Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
        startY = transform.position.y;

        verticalSpeed = Random.Range(1.5f, 3f);
        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
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

        if (transform.position.x + 0.5f  >= cameraWidth || transform.position.x - 0.5f <= -cameraWidth )
        {
            ChangeDirection();
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -cameraWidth, cameraWidth), transform.position.y, transform.position.z);
        }
    }

    void ApplyVerticalMotion()
    {
        float verticalMotion = Mathf.Sin(Time.time * verticalSpeed + phaseOffset) * verticalMotionRange;
        float newYPosition = startY + verticalMotion;

        newYPosition = Mathf.Clamp(newYPosition, -cameraHeight + 2f, cameraHeight - 0.5f);

        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }

    void ChangeDirection()
    {
        swimDirection = -swimDirection;
        transform.position += new Vector3(swimDirection * 0.1f, 0, 0);
        directionChangeTime = Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
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


