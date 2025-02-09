using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugButtons : MonoBehaviour
{
    public GameObject targetObject;

    void Start()
    {
        targetObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if(targetObject.activeSelf)
            {
                targetObject.SetActive(false);
            }
            else
            {
                targetObject.SetActive(true);
            }

        }
        
    }
}
