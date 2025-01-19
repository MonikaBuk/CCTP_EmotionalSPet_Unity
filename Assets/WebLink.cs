using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLink : MonoBehaviour
{
    //[SerializeField] private string url = "https://www.example.com";

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}


