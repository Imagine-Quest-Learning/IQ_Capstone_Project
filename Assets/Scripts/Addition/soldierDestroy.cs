using UnityEngine;

// Handles soldier health, collision with cannonball, explosion, and answer checking
public class soldierDestroy : MonoBehaviour
{
    [Header("HP Settings")]
    public int maxHP = 1;
    private int currentHP;

    [Header("Explosion Prefab")]
    public GameObject boomPrefab;

    // Initialize health and load explosion prefab if not set
    private void Start()
    {
        currentHP = maxHP;

        if (boomPrefab == null)
            boomPrefab = Resources.Load<GameObject>("Boom1");
    }

    // Triggered when the soldier collides with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only respond to objects tagged "Ball"
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        // Prevent the same ball from triggering again
        collision.gameObject.tag = "UsedBall";

        // Calculate damage based on impact velocity
        float collisionMagnitude = collision.relativeVelocity.magnitude;
        int damage = (int)(collisionMagnitude * 8);
        currentHP -= damage;

        // If health drops to zero or below, trigger destruction
        if (currentHP <= 0)
        {
            PlayExplosionEffect();
            CheckAnswerAndShowDialog();
            Destroy(gameObject);
        }
    }

    // Checks this soldier's answer and informs the QABoardManager
    private void CheckAnswerAndShowDialog()
    {
        ShieldAnswer shield = GetComponent<ShieldAnswer>();
        if (shield == null) return;

        if (QABoardManager.Instance != null)
            QABoardManager.Instance.CheckAnswer(shield.answer);
    }

    // Plays the explosion animation and destroys the effect after delay
    private void PlayExplosionEffect()
    {
        if (boomPrefab == null) return;

        GameObject explosion = Instantiate(boomPrefab, transform.position, Quaternion.identity);

        if (!explosion) return;

        Animator boomAnimator = explosion.GetComponent<Animator>();
        if (boomAnimator != null)
        {
            boomAnimator.SetTrigger("explode");
        }

        Destroy(explosion, 5f);
    }
}
