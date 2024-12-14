using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField, Range(0, 7)] // Create a slider in the Inspector with a range from 0 to 7
    private int id;

    private Animator animator;

    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        // This method is called when a value in the Inspector changes
        if (animator != null)
        {
            animator.SetInteger("AnimationID", id); // Update the Animator parameter
        }
    }

    private void Start()
    {
        // Initialize Animator with the default ID value
        if (animator != null)
        {
            animator.SetInteger("AnimationID", id);
        }
    }

    public void SetAnimationId(int id)
    {
        // Initialize Animator with the default ID value
        if (animator != null)
        {
            animator.SetInteger("AnimationID", id);
        }
    }
}