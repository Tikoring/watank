using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerHP playerHPScript;
    public GameObject NM;
    public float playerRealHP;

    public float tempPlayerRealHp;

    public Slider syncSlider;

    Vector3 curPos;
    // Start is called before the first frame update
    void Start()
    {
        TC = GetComponent<TankControll>();
        ATK = GetComponent<Attack>();
        playerHPScript = GetComponent<PlayerHP>();
        NM = GameObject.FindGameObjectWithTag("NetworkManager");
        syncSlider.value = 0;
        syncSlider.value = playerHPScript.headHpBar.value;
        

        
        
        //playerRealHP = playerHPScript.CurrentHP;
        

        //게임오브젝트 두개 할당
        /*foreach(GameObject Go in GameObject.FindGameObjectsWithTag("Player")){

            if (Go.GetComponent<PlayerScript>().PV.IsMine)
            {
                
                Debug.Log("Find player head bar");
                syncSlider = Go.GetComponent<Slider>();

            }
            else
            {
            }
        }*/
        //슬라이더 두개 할당
    }
    void checkPlayerDie()
    {
        if (playerHPScript.CurrentHP <= 0)
        {
            PV.RPC("DiePlayer", RpcTarget.AllBuffered); // AllBuffered로 해야 제대로 사라져 복제버그가 안 생긴다

        }
    }
    // Update is called once per frame
    void Update()
    {
        /*   foreach (GameObject Go in GameObject.FindGameObjectsWithTag("PlayerHeadHp"))
           {

               if (Go.GetComponent<PlayerScript>().PV.IsMine)
               {
                   synchHpbar = Go;
                   Debug.Log("Find player head bar");
                   syncSlider = Go.GetComponent<Slider>();
               }
               else
               {
               }
           }*/

        // 이부분이 있어야할까?
        //syncSlider = synchHpbar.GetComponent<Slider>();
        checkPlayerDie();
        if (PV.IsMine)
        {
            TC.TankState();
            ATK.AttackState();

          

        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
       
            //syncSlider = tempSlider.GetComponent<Slider>();
        }
        //HP 가 0 되면 서버에서 종료시켜버림
        //if(playerHPScript.CurrentHP == 0)
        //{
        //    PhotonNetwork.Disconnect();
        //    NM.GetComponent<NetworkManager>().LoginPanel.SetActive(true);
            
        //}
    }



    //stream.SendNext(playerRealHP);
    //stream.SendNext(synchHpbar);

    //tempPlayerRealHp = (float)stream.ReceiveNext(playerRealHP);
    //tempSlider = (GameObject)stream.ReceiveNext();

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(syncSlider.value);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            syncSlider.value = (float)stream.ReceiveNext();

        }
    }

    [PunRPC]
    void DiePlayer()
    {
        Destroy(gameObject);
    } 
    // 탱크 좌우 스프라이트 동기화. 그런데 스프라이트 렌더러는 wheel에 있음.
    // 그거 가져와서 뒤집으면됨.

    //[PunRPC]
    //void FlipXRPC(float axis) => MSR.flipX = axis == -1;

    // 자손 Animation 은 동기화 불가능. 제시된 방식(Animator와 PhotonView가 같은 위치)으로만
    // 동기화 가능함.

}
