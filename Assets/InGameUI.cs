using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class InGameUI : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = photonView;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


    // Master 의 관리하에 상대방의 UI를 관리하고 있기때문에 PV 형식으로 해야한다. 
    [PunRPC]
    void createIngameUI()
    {
        Debug.Log("Create Ingame UI");
        gameObject.SetActive(true);
    }

}
