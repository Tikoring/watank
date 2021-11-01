using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rd;
    public float power;
    private static bool firePermission = true;
    public bool FirePermission => firePermission;   //포탄이 발사된 동안 공격을 막기 위함
    void Start()
    {
        print(this);
        rd = GetComponent<Rigidbody2D>();
        // print(power);
        rd.velocity = transform.right * power;
        Destroy(gameObject, 15);
        firePermission = false;
    }

    public void Fire (float _power, GameObject attackPos) {
        Projectile instance = this;
        // print("Fire");
        instance.power = _power;
        Vector3 pos = attackPos.transform.rotation * Vector3.right;
        //firePermission = false;
        Instantiate(instance, attackPos.transform.position + pos, attackPos.transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stage") {
            Destroy(gameObject);
        }
        if (collision.tag == "OtherPlayer") {  //상대방을 인식
            collision.GetComponent<PlayerHP> ().TakeDamage (10);
            Destroy (gameObject);
        }
    }

    private void OnDestroy() {
        firePermission = true;
    }
}