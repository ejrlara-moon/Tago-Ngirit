using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour
{
    public Slider StaminaSlider;

    public void SetMaxValue(float maxValue)
    {
        StaminaSlider.maxValue = maxValue;
        StaminaSlider.value = maxValue; // Start the bar full
    }

    public void SetValue(float currentValue)
    {
        StaminaSlider.value = currentValue;
    }
}
