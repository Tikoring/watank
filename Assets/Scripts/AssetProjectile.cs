using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetProjectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rd;
    public float power;
    public static AssetProjectile bullet;
    private static bool firePermission = true;
    private float beforeY;
    private bool coll;
    private Animator apAnimator;                    //projectile animator 제어
    public bool FirePermission => firePermission;   //포탄이 발사된 동안 공격을 막기 위함
    public AudioClip explosionClip;

    void Start()
    {
        print(this);
        rd = GetComponent<Rigidbody2D>();
        // print(power);
        rd.velocity = transform.right * power;
        Destroy(gameObject, 15);
        firePermission = false;
        beforeY = transform.position.y;
        apAnimator = GetComponent<Animator> ();
        coll = false;
    }

    public void Fire (float _power, GameObject attackPos) {
        AssetProjectile instance = this;
        // print("Fire");
        instance.power = _power;
        Vector3 pos = attackPos.transform.rotation * Vector3.right / 3;
        bullet = Instantiate(instance, attackPos.transform.position + pos, attackPos.transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Field") {
            coll = true;
            apAnimator.SetBool ("Explosion", true);

            rd.gravityScale = 0;
            rd.velocity = Vector2.zero;
            Audio.instance.PlaySound("Explosion", explosionClip);
            Destroy(gameObject, 0.67f);         //개선 필요함(animation이 종료될 시에 삭제되게)
        }
        if (collision.tag == "OtherPlayer") {   //상대방을 인식
            coll = true;
            apAnimator.SetBool ("Explosion", true);
            collision.GetComponent<PlayerHP> ().TakeDamage (10);
            rd.gravityScale = 0;
            rd.velocity = Vector2.zero;
            Audio.instance.PlaySound("Explosion", explosionClip);
            Destroy(gameObject, 0.67f);
        }
    }
    //projectile의 방향과 각도가 일치하게 update
    //rigidbody를 이용해 이동을 하기 때문에 fixed update를 해야 충돌이 없음
    private void FixedUpdate() {
        float afterY = transform.position.y;
        float angle = Vector2.Angle (Vector2.right, rd.velocity);
        if (afterY < beforeY) {angle *= -1;}
        beforeY = afterY;
        if (!coll) {transform.eulerAngles = new Vector3 (0, 0, angle);}
    }

    private void OnDestroy() {
        firePermission = true;
    }

}
