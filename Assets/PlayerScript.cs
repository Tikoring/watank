using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class PlayerScript : MonoBehaviourPun , IPunObservable 
{

    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public TankControll tankControllScript;
    public Transform TS;


    Vector3 curPos;


    // Start is called before the first frame update
    void Start()
    {
        TS = GetComponent<Transform>();
        transform.position = TS.position;
    }


    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine)
        {
            tankControllScript.TankControllFunctionInScript();
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);


    }
    /*
        [PunRPC]
        void spawnPlayer()
        {

            Debug.Log("SpawnPlayer");
            PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-40, 40), 20f, 0), Quaternion.identity);
        }

    */


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
