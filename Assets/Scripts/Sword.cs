using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damage;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    private bool colliderActive = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        TryHitEnemy(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TryHitEnemy(other);
    }

    void TryHitEnemy(Collider2D other)
    {
        if (!colliderActive) return;

        if (other.CompareTag("Enemy") && !hitEnemies.Contains(other.gameObject))
        {
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

            other.GetComponent<Enemy>().TakeDamage(damage, knockbackDirection);
            hitEnemies.Add(other.gameObject);
        }
    }

    public void EnableCollider()
    {
        colliderActive = true;
        hitEnemies.Clear(); // Limpiamos la lista al empezar el ataque
        GetComponent<Collider2D>().enabled = true;
    }

    public void DisableCollider()
    {
        colliderActive = false;
        GetComponent<Collider2D>().enabled = false;
    }

}