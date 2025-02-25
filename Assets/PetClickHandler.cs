using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PetClickHandler : MonoBehaviour
{

    private AnimationManager animationManager;
    private AudioSource m_audioSource;

    void Start()
    {
        animationManager = GetComponent<AnimationManager>();
        m_audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("hit");
                if (hit.collider.gameObject == gameObject)
                {
                    m_audioSource.Play();
                    animationManager.SetAnimationId(1);
                    StartCoroutine(ResetAnimationAfterDelay(4.0f));
                }
            }
        }
    }

    IEnumerator ResetAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_audioSource.Stop();
        animationManager.SetAnimationId(0);
    }
}

