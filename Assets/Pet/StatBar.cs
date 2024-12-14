using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void UpdateStatBar(int scale)
    {
        slider.value = scale;
      
    }
}
