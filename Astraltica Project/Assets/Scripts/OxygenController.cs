using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public Slider slider;
    public int oxygen;
    public int maxOxygen = 120;

    void Start()
    {
        SetMaxOxygen(maxOxygen);
        oxygen = maxOxygen;
        InvokeRepeating("DecreaseOxygen", 0, 1);
    }

    public void SetMaxOxygen(int setOxygen)
    {
        slider.maxValue = setOxygen;
        oxygen = setOxygen;
        slider.value = setOxygen;
    }

    public void SetOxygen(int oxygenValue)
    {
        oxygen = oxygenValue;
        slider.value = oxygen;
    }

    private void DecreaseOxygen()
    {
        if (oxygen > 0)
        {
            oxygen--;
            Debug.Log("Decreasing oxygen: " + oxygen);
            SetOxygen(oxygen);
        }
        else
        {
            CancelInvoke("DecreaseOxygen");
            Debug.Log("Out of oxygen!");
        }
    }

    public void TakeDamageAsOxygen(int damage)
    {
        oxygen -= damage;
    }
}
