using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class PlayerScript : MonoBehaviourPunCallbacks , IPunObservable
{
    public Rigidbody2D RB;
    
    // �� ��������Ʈ
    public SpriteRenderer SR;

    // ������ ��������Ʈ
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

    // ��ũ �¿� ��������Ʈ ����ȭ. �׷��� ��������Ʈ �������� wheel�� ����.
    // �װ� �����ͼ� ���������.

    //[PunRPC]
    //void FlipXRPC(float axis) => MSR.flipX = axis == -1;

    // �ڼ� Animation �� ����ȭ �Ұ���. ���õ� ���(Animator�� PhotonView�� ���� ��ġ)���θ�
    // ����ȭ ������.

}
