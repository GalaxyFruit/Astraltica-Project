using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public OxygenBar oxygenBar;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        oxygenBar.TakeDamageAsOxygen(damage);
    }
}
