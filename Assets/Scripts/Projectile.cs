using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rd;
    public float power;

    void Start()
    {
        print(this);
        rd = GetComponent<Rigidbody2D>();
        // print(power);
        rd.velocity = transform.up * power;
        Destroy(gameObject, 15);
    }

    public void Fire(float _power, GameObject attackPos) {
        Projectile instance = this;
        // print("Fire");
        instance.power = _power;
        Instantiate(instance, attackPos.transform.position, attackPos.transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stage") {
            Destroy(gameObject);
        }
    }
}
