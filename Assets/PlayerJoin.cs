using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerJoin : MonoBehaviourPun, IPunObservable
{
    public PhotonView PV;
    public int myId;
    public string playerName;



    // Start is called before the first frame update
    void Start()
    {
        PV = photonView;
        myId = PV.ViewID;
        playerName = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
