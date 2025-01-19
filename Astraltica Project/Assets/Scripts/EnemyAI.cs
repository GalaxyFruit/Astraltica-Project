using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float checkTime = 1f;

    private EnemyAnimationController animationController;
    private float timer = 0;
    private bool isDead = false;

    private void Start()
    {
        animationController = GetComponent<EnemyAnimationController>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            if (isDead) return;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange)
            {
                float attackType = Random.Range(3f, 5f); // Left Punch = 3, Right Punch = 4
                animationController.SetAction(attackType);
            }
            else if (distance <= detectionRange)
            {
                animationController.SetAction(2); // Run
            }
            else
            {
                animationController.SetAction(0); // Idle
            }
        }
    }

    public void Kill()
    {
        isDead = true;
        animationController.Die();
    }
}
