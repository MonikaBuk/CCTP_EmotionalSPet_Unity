using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText; 
    [SerializeField] private GameObject nextGameObject; 
    [SerializeField] private float displayDuration = 3f; 
    void Start()
    {
        nextGameObject.gameObject.SetActive(false);
        if (tmpText != null)
        {
            tmpText.gameObject.SetActive(true); 
            StartCoroutine(DisplayTextThenSwitch());
        }
    }

    private IEnumerator DisplayTextThenSwitch()
    {
        yield return new WaitForSeconds(displayDuration); 

        if (tmpText != null)
        {
            tmpText.gameObject.SetActive(false); 
        }

        if (nextGameObject != null)
        {
            nextGameObject.SetActive(true);
        }
    }
}
