using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    public float jumpForce = 5f;        
    public float gravity = -9.8f;       
    private bool isGrounded = true;     
    private float verticalVelocity = 0;
    private AudioSource jumpSound;

    private void Start()
    {
        jumpSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            jumpSound.Play();
            verticalVelocity = jumpForce;
            isGrounded = false;
        }

        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            transform.position += new Vector3(0, verticalVelocity * Time.deltaTime, 0);

            if (transform.position.y <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                verticalVelocity = 0;
                isGrounded = true;
            }
        }
    }
}
