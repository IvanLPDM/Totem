using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemigo golpeado: " + other.name);
            // Aquí puedes hacer: other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
