using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;


public class PlayerScript : MonoBehaviourPunCallbacks , IPunObservable
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public TankControll TC;

    Vector3 curPos;
    // Start is called before the first frame update
    void Start()
    {
        TC = GetComponent<TankControll>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            TC.TankState();


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

}
