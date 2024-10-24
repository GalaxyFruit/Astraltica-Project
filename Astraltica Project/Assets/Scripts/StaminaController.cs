using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    public Slider staminaSlider;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;
    public float regenDelay = 3f;

    private bool isSprinting;
    private Coroutine regenCoroutine;

    void Start()
    {
        SetMaxStamina(maxStamina);
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (isSprinting)
        {
            DrainStamina();
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
        }

        SetStamina(currentStamina);
    }

    public void SetMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        currentStamina = stamina;
    }

    public void SetStamina(float staminaValue)
    {
        currentStamina = staminaValue;
        staminaSlider.value = staminaValue;
    }

    public void StartSprinting()
    {
        isSprinting = true;
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    public void StopSprinting()
    {
        isSprinting = false;

        if (regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(StartRegenAfterDelay());
        }
    }

    private IEnumerator StartRegenAfterDelay()
    {
        yield return new WaitForSeconds(regenDelay);

        while (currentStamina < maxStamina && !isSprinting)
        {
            RegenerateStamina();
            yield return null;
        }

        regenCoroutine = null;
    }

    private void DrainStamina()
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;
        if (currentStamina < 0)
        {
            currentStamina = 0;
            StopSprinting();
        }
    }

    private void RegenerateStamina()
    {
        currentStamina += staminaRegenRate * Time.deltaTime;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public bool CanSprint()
    {
        return currentStamina > 0;
    }
}
