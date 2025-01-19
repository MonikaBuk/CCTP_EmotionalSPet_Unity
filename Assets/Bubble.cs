using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float riseDuration = 4f; 
    public float riseHeight = 5f; 
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1f); 
    private Animator bubbleAnimator;
    public float holdDuration = 4f;  
    private float holdTimer = 0f;
    private bool holdingAtTop = false;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 startScale;

    private float timer = 0f;
    private bool popped = false;


    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + Vector3.up * riseHeight;
        startScale = transform.localScale;
        bubbleAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!popped)
        {
            if (timer < 1f) // Rising phase
            {
                timer += Time.deltaTime / riseDuration;
                transform.position = Vector3.Lerp(startPosition, endPosition, timer);
                transform.localScale = Vector3.Lerp(startScale, maxScale, timer);
            }
            else if (!holdingAtTop) 
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= holdDuration) 
                {
                    PopBubble();
                    holdingAtTop = true; 
                }
            }
        }
    }
    void PopBubble()
    {
        popped = true;
        if (bubbleAnimator != null)
        {
            bubbleAnimator.SetTrigger("Pop");
        }
        else
        {
            DestroyBubble(); 
        }
    }
    public void DestroyBubble()
    {
        Destroy(gameObject);
    }
    public void Pop()
    {
        Debug.Log("Pop method called at: " + Time.time);
        DestroyBubble();
    }
}

