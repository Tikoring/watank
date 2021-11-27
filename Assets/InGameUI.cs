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


    // Master �� �����Ͽ� ������ UI�� �����ϰ� �ֱ⶧���� PV �������� �ؾ��Ѵ�. 
    [PunRPC]
    void createIngameUI()
    {
        Debug.Log("Create Ingame UI");
        gameObject.SetActive(true);
    }

}
