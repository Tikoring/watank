using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rd;
    public float power = 100f;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        print(transform.up);
        rd.velocity = transform.up * power;
        Destroy(gameObject, 5);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.tag);
    }
}
