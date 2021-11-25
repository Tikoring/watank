using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetProjectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rd;
    private SpriteRenderer sr;   //sprite color 제어를 위한 render
    public float power;
    public static AssetProjectile bullet;
    private static bool firePermission = true;
    private float beforeY;
    private bool coll;
    private static Color projectileColor = Color.white; //projectile 색상 변경을 위한 static variable
    private static float gravityScale = 1.0f;           //projectile의 중력 영향 여부를 위한 값
    private static float expScale = 1f;
    private static float damage = 30f;
    private Animator apAnimator;                    //projectile animator 제어
    private GameObject player;
    private static bool teloport = false;
    private static bool exceptField = false;
    public bool FirePermission => firePermission;   //포탄이 발사된 동안 공격을 막기 위함
    public Color ProjetileColor {
        get {return projectileColor;}
        set {projectileColor = value;}
    }
    public float GravityScale {
        get {return gravityScale;}
        set {gravityScale = value;}
    }
    public float ExpScale {
        get {return expScale;}
        set {expScale = value;}
    }
    public float Damage {
        get {return damage;}
        set {damage = value;}
    }
    public bool Teleport {
        get {return teloport;}
        set {teloport = value;}
    }
    public bool ExceptField {
        get {return exceptField;}
        set {exceptField = value;}
    }
    public AudioClip explosionClip;

    void Start()
    {
        print(this);
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer> ();
        sr.color = projectileColor;
        //print(power);
        rd.gravityScale = gravityScale;
        rd.velocity = transform.right * power;
        Destroy(gameObject, 15);
        firePermission = false;
        beforeY = transform.position.y;
        apAnimator = GetComponent<Animator> ();
        player = GameObject.Find("AssetTank");
        coll = false;
    }

    //skill은 한번에 하나씩 발동되기 때문에, color값을 인자로 받는 fire가 기본, 받지 않는 fire가 다른 스킬을 사용할 때 사용
    public void Fire (float _power, GameObject attackPos) {
        AssetProjectile instance = this;
        //print("Fire");
        instance.power = _power;
        Vector3 pos = attackPos.transform.rotation * Vector3.right / 3;
        bullet = Instantiate(instance, attackPos.transform.position + pos, attackPos.transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!teloport) {
            if (collision.tag == "Field" && !exceptField) {
                rd.gravityScale = 0;
                rd.velocity = Vector2.zero;
                this.transform.localScale = new Vector3 (4.5f, 4.5f, 4.5f) * expScale;
                sr.color = Color.white;
                coll = true;

                apAnimator.SetBool ("Explosion", true);;
                AudioManager.Instance.PlaySFXSound("ExplosionSound");
                //폭발 후 wind 값 변경
                WindScript.setWind();
                Destroy(gameObject, 0.67f);         //개선 필요함(animation이 종료될 시에 삭제되게)
            }
            if (collision.tag == "OtherPlayer") {   //상대방을 인식
                rd.gravityScale = 0;
                rd.velocity = Vector2.zero;
                this.transform.localScale = new Vector3 (4.5f, 4.5f, 4.5f) * expScale;
                sr.color = Color.white;
                coll = true;
                
                apAnimator.SetBool ("Explosion", true);
                collision.GetComponent<PlayerHP> ().TakeDamage (damage);
                AudioManager.Instance.PlaySFXSound("ExplosionSound");
                //폭발 후 wind 값 변경
                WindScript.setWind();
                Destroy(gameObject, 0.67f);
            }
            if (collision.tag == "Out") {
                //폭발 후 wind 값 변경
                WindScript.setWind();
                Destroy(gameObject);
            }
        } else {
            if (collision.tag == "Field" || collision.tag == "OtherPlayer") {
                rd.gravityScale = 0;
                rd.velocity = Vector2.zero;
                player.transform.position = this.transform.position;
                //폭발 후 wind 값 변경
                WindScript.setWind();
                Destroy (gameObject);
            }
            if (collision.tag == "Out") {
                //폭발 후 wind 값 변경
                WindScript.setWind();
                Destroy (gameObject);
                player.GetComponent<PlayerHP> ().TakeDamage (player.GetComponent<PlayerHP> ().CurrentHP / 2);
            }
        }
    }
    //projectile의 방향과 각도가 일치하게 update
    //rigidbody를 이용해 이동을 하기 때문에 fixed update를 해야 충돌이 없음
    private void FixedUpdate() {
        float afterY = transform.position.y;
        float angle = Vector2.Angle (Vector2.right, rd.velocity);
        if (afterY < beforeY) {angle *= -1;}
        beforeY = afterY;
        if (!coll) {
            transform.eulerAngles = new Vector3 (0, 0, angle);
            rd.velocity += WindScript.getWind();
        }
    }

    private void OnDestroy() {
        projectileColor = Color.white;
        gravityScale = 1.0f;
        expScale = 1f;
        firePermission = true;
        player.GetComponent<TankControll> ().SkillLock = false;
        damage = 30f;
        teloport = false;
        exceptField = false;
    }

}