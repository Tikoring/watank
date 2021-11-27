using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class RoomPanel : MonoBehaviourPunCallbacks,IPunObservable
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

    [PunRPC]
    void removeRoomPanel()
    {
        Debug.Log("Game Start, removeRoomPanel");
        gameObject.SetActive(false);
    }
}
