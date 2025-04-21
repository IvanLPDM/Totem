using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float max_live;
    public float live;
    public float damage;
    private Rigidbody2D rb;

    public float knockbackForce = 5f;


    // Start is called before the first frame update
    void Start()
    {
        live = max_live;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(live <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        live -= damage;

        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Movement player = other.GetComponent<Movement>();
            if (player != null)
            {
                player.TakeDamage(this.damage);
            }
        }
    }
}
