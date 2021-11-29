using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class PlayerScript : MonoBehaviourPunCallbacks , IPunObservable
{
    public Rigidbody2D RB;
    
    // 휠 스프라이트
    public SpriteRenderer SR;

    // 마스터 스프라이트
    public SpriteRenderer MSR;
    public PhotonView PV;
    public TankControll TC;
    public Attack ATK;


    Vector3 curPos;
    // Start is called before the first frame update
    void Start()
    {
        TC = GetComponent<TankControll>();
        ATK = GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            TC.TankState();
            ATK.AttackState();

        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();

        }
    }

    // 탱크 좌우 스프라이트 동기화. 그런데 스프라이트 렌더러는 wheel에 있음.
    // 그거 가져와서 뒤집으면됨.

    //[PunRPC]
    //void FlipXRPC(float axis) => MSR.flipX = axis == -1;

    // 자손 Animation 은 동기화 불가능. 제시된 방식(Animator와 PhotonView가 같은 위치)으로만
    // 동기화 가능함.

}
