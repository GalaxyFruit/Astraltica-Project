using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float sprintDrain = 10f; 
    [SerializeField] private float baseRegenRate = 20f; 
    [SerializeField] private float lowStaminaRegenDelay = 3f; 
    [SerializeField] private float halfStaminaRegenDelay = 1f; 
    [SerializeField] private Image staminaBar; 

    private float currentStamina;
    private bool isSprinting = false;
    private Coroutine regenCoroutine;

    public float CurrentStamina => currentStamina;

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    public void StartSprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
            StopRegen();
            StartCoroutine(SprintCoroutine());
        }
    }

    public void StopSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
            StartRegen();
        }
    }

    private IEnumerator SprintCoroutine()
    {
        while (isSprinting)
        {
            currentStamina -= sprintDrain * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            UpdateStaminaBar();

            if (currentStamina <= 0)
            {
                StopSprint(); // Pokud stamina klesne na nulu, sprint končí
                yield break;
            }

            yield return null;
        }
    }

    private void StartRegen()
    {
        if (regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(RegenCoroutine());
        }
    }

    private void StopRegen()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    private IEnumerator RegenCoroutine()
    {
        float delay = currentStamina < maxStamina / 2 ?
            (currentStamina == 0 ? lowStaminaRegenDelay : halfStaminaRegenDelay) : 0;

        yield return new WaitForSeconds(delay);

        while (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += baseRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            UpdateStaminaBar();

            yield return null;
        }

        regenCoroutine = null;
    }

    private void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
}
