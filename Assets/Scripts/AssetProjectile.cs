using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class AssetProjectile : MonoBehaviourPunCallbacks , IPunObservable
{
    [SerializeField]
    private Rigidbody2D rd;
    private SpriteRenderer sr;   //sprite color 제어를 위한 render
    public float power;
    //public static AssetProjectile bullet;
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

    //Server
    public PhotonView PV;
    int dir;
    public GameObject GroundObject;

    public PolygonCollider2D AssetProjectilePolygonCollider;

    public GameObject CameraControlObject;


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

    private void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        AssetProjectilePolygonCollider = GetComponent<PolygonCollider2D>();
        GroundObject = GameObject.FindGameObjectWithTag("Field");
        CameraControlObject = GameObject.FindGameObjectWithTag("MainCamera");

    }
    void Start()
    {
        print(this);
        sr.color = projectileColor;
        //print(power);
        rd.gravityScale = gravityScale;
        rd.velocity = transform.right * power;
        //Destroy(gameObject, 15);
        //PV.RPC("DestroyRPC", RpcTarget.AllBuffered);

        CameraControlObject.GetComponent<CameraControl>().assetProjectile
            = this.gameObject;


        firePermission = false;
        beforeY = transform.position.y;
        apAnimator = GetComponent<Animator> ();

        /*foreach(GameObject Go in GameObject.FindGameObjectsWithTag("Player"))
        {

            if (Go.GetComponent<PlayerScript>().PV.IsMine)
            {
                player = Go;

            }
            else
            {
                player = null;
                Debug.Log("Error! AssetProjectile can't find player object");
            }

        }*/
        coll = false;
    }

    public void setPlayer()
    {
        foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Player"))
        {

            if (Go.GetComponent<PlayerScript>().PV.IsMine)
            {
                player = Go;

            }
            else
            {
                player = null;
                Debug.Log("Error! AssetProjectile can't find player object");
            }

        }
    }

    //skill은 한번에 하나씩 발동되기 때문에, color값을 인자로 받는 fire가 기본, 받지 않는 fire가 다른 스킬을 사용할 때 사용
    //이게 헷갈렸던 원인이다. 미사일을 제어하는 함수는 Attack 에 있어야하는데,
    // 미사일을 발사하는 함수가 미사일 내부에 존재하는게 문제인것.

    // 이 부분을 Attack 으로 옮겨봄.
    /*   public void Fire (float _power, GameObject attackPos) {
           AssetProjectile instance = this;
           //print("Fire");
           instance.power = _power;
           Vector3 pos = attackPos.transform.rotation * Vector3.right / 3;

           // 이 부분이 불렛 할당 부분 
           //bullet = Instantiate(instance, attackPos.transform.position + pos, attackPos.transform.rotation);
           PhotonNetwork.Instantiate("AssetProjectile", attackPos.transform.position + pos, attackPos.transform.rotation);

       }*/


    // 여기서 지형파괴 함수를 호출하면 된다.

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
                //AudioManager.Instance.PlaySFXSound("ExplosionSound");
                //폭발 후 wind 값 변경
                //WindScript.setWind();
                //Destroy(gameObject, 0.67f);         //개선 필요함(animation이 종료될 시에 삭제되게)

                // 미사일이 필드에 도착해서 제거 
                Debug.Log("[System] : Missile hit Field");

                Debug.Log("Make a hole");
                // 이렇게 해버리면 텔레포트 , ExceptField 체크 못함.
                collision.GetComponent<Ground>().MakeAHole(AssetProjectilePolygonCollider, this.ExpScale);

                Debug.Log("Remove AssetProjectile");
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);

                // 이런식으로 매개변수 넣어주면 안된다.
                //PV.RPC("setGround", RpcTarget.AllBuffered, AssetProjectilePolygonCollider);
        
            }

            //플레이어 히트시 느린쪽에 맞춰서 판정 
            //!PV.IsMine && 
            if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine) {   //상대방을 인식
                Debug.Log("Hit Player!!");
                rd.gravityScale = 0;
                rd.velocity = Vector2.zero;
                this.transform.localScale = new Vector3 (4.5f, 4.5f, 4.5f) * expScale;
                sr.color = Color.white;
                coll = true;
                
                apAnimator.SetBool ("Explosion", true);
                //collision.GetComponent<PlayerScript> ().TakeDamage (damage);
                collision.GetComponent<PlayerScript>().playerHPScript.TakeDamage (damage);
                //AudioManager.Instance.PlaySFXSound("ExplosionSound");
                //폭발 후 wind 값 변경
                //WindScript.setWind();
                //Destroy(gameObject, 0.67f);

                //미사일이 타겟에 맞아서 제거 
                Debug.Log("[System] : Missile hit player");
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            if (collision.tag == "Out") {
                //폭발 후 wind 값 변경
                //WindScript.setWind();

                //미사일이 밖으로 나가서 제거
                Debug.Log("[System] : Missile Out");
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);

            }
        } else {
            if (collision.tag == "Field" || collision.tag == "OtherPlayer") {
                rd.gravityScale = 0;
                rd.velocity = Vector2.zero;

                // 텔레포트인 경우 player의 포지션을 옮기고자 한다. 스킬부분인데 구현하나?
                // player 변수는 동기화되어있다. 트랜스폼을 옮기면 동기화 가능함. 미사일로 player의 transform을 옮기는것도 이론상 가능하다.
                

                player.transform.position = this.transform.position;
                //폭발 후 wind 값 변경
                //Wind 값도 동기화 시켜줘야함. 동일하도록
                //WindScript.setWind();
                //Destroy (gameObject);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);

            }
            if (collision.tag == "Out") {
                //폭발 후 wind 값 변경
                //바람도 제각각 이므로 하나의 마스터 Client를 기준으로 동기화 해야함. 
                //턴이랑 같이 동기화 하면 될것같은데. 
                //WindScript.setWind();
                //Destroy (gameObject);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
                player.GetComponent<PlayerHP> ().TakeDamage (player.GetComponent<PlayerHP> ().CurrentHP / 2);
            }
        }
    }


    //projectile의 방향과 각도가 일치하게 update
    //rigidbody를 이용해 이동을 하기 때문에 fixed update를 해야 충돌이 없음
    private void FixedUpdate() {
        setPlayer();
        float afterY = transform.position.y;
        float angle = Vector2.Angle (Vector2.right, rd.velocity);
        if (afterY < beforeY) {angle *= -1;}
        beforeY = afterY;
        if (!coll) {
            transform.eulerAngles = new Vector3 (0, 0, angle);
            //rd.velocity += WindScript.getWind();
        }
    }


    // MonoBehaviour 가 제거될 때 호출됨.

    private void OnDestroy() {

        // 이 부분을 동기화 하는게 완벽하긴 함. 그런데 턴 방식으로 자기 턴이 아니면 조작이 불가능하게 만들어버리면, 한 턴에 생성되는 미사일은 하나이므로
        // local에서 변수가 바뀌는지 여부를 알 필요가 없을것으로 예상한다.

        if(PV.IsMine)
        {

            projectileColor = Color.white;
            gravityScale = 1.0f;
            expScale = 1f;
            firePermission = true;

            // 이부분 PV 체크 해야할수도 있음. 
            //player.GetComponent<TankControll>().SkillLock = false;
            damage = 30f;
            teloport = false;
            exceptField = false;

        }
    }


    //Set Direction -1, 1
    [PunRPC]
    void DirRPC(int dir) => this.dir = dir;

    
    //서버에서 실행하는 포탄 제거 함수 ( 모든 클라이언트에서 포탄이 제거되어야 하므로 )
    //생성도 마찬가지로 PhotonView.Instantiate 를 사용하여야 한다.
    //발사 하는건 AssetProjectile이 아니다.
   /* [PunRPC]
    void FireRPC(float _power, GameObject attackPos)
    {
        Fire(_power, attackPos);
    }*/


    // 매개변수로 collision 사용 하면 안됨 
    /*[PunRPC]
    void setGround(Collider2D collision)
    {
        GroundObject.GetComponent<Ground>().OnTriggerEnter2D(collision);
    }
*/
    [PunRPC]
    void DestroyRPC()
    {
        Debug.Log("DestroyPRc");
        Destroy(gameObject, 0.67f);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            stream.ReceiveNext();

        }
    }
}