using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Projectile : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Rigidbody2D rd;
    public float power;
    private static bool firePermission = true;
    public bool FirePermission => firePermission;   //포탄이 발사된 동안 공격을 막기 위함

    public PhotonView PV;
    int dir = 1;
   
    void Start()
    {
        print(this);
        rd = GetComponent<Rigidbody2D>();
        // print(power);
        rd.velocity = transform.right * power;
        Destroy(gameObject, 15);
        firePermission = false;
    }

    public void Fire(float _power, GameObject attackPos)
    {
        Projectile instance = this;
        // print("Fire");
        instance.power = _power;
        Vector3 pos = attackPos.transform.rotation * Vector3.right;
        //firePermission = false;
        //Instantiate(instance, attackPos.transform.position + pos, attackPos.transform.rotation);

        PhotonNetwork.Instantiate("projectile", attackPos.transform.position + pos, attackPos.transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stage") {
            Debug.Log("Bullet Hit Ground");
            PV.RPC("DestoryRPC", RpcTarget.AllBuffered);
            
        }

        // tag를 OtherPlayer 에서 Player로 바꿈.
        // 서버에서 나 자신과 다른사람을 구분할 수 있기 때문에, and 연산자를 사용하고 모든 플레이어 태그는 Player 사용.
        // 피격 판정은 느린쪽에 맞춰서 TakeDamage 판정 
        if (!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine) {  

            Debug.Log("Bullet Other Player");

            collision.GetComponent<PlayerHP> ().TakeDamage (10);
            //Destroy (gameObject);
            PV.RPC("DestoryRPC", RpcTarget.AllBuffered);
        }
    }

    private void OnDestroy() {
        firePermission = true;
    }

    [PunRPC]
    void DestoryRPC() => Destroy(gameObject);

    [PunRPC]
    void DirRPC(int dir) => this.dir = dir;

}
