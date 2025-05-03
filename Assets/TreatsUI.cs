using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TreatsUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static int num = 0;
    void Start()
    {
        if (num > 0)
        {
            text.text = "+" + num;
            Invoke("HideText", 2f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void HideText()
    {
        gameObject.SetActive(false);
        num = 0; 
    }


}
