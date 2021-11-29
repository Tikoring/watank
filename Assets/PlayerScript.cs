using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        

        //���ӿ�����Ʈ �ΰ� �Ҵ�
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
        //�����̴� �ΰ� �Ҵ�
    }
    void checkPlayerDie()
    {
        if (playerHPScript.CurrentHP <= 0)
        {
            PV.RPC("DiePlayer", RpcTarget.AllBuffered); // AllBuffered�� �ؾ� ����� ����� �������װ� �� �����

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

        // �̺κ��� �־���ұ�?
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
        //HP �� 0 �Ǹ� �������� ������ѹ���
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
    // ��ũ �¿� ��������Ʈ ����ȭ. �׷��� ��������Ʈ �������� wheel�� ����.
    // �װ� �����ͼ� ���������.

    //[PunRPC]
    //void FlipXRPC(float axis) => MSR.flipX = axis == -1;

    // �ڼ� Animation �� ����ȭ �Ұ���. ���õ� ���(Animator�� PhotonView�� ���� ��ġ)���θ�
    // ����ȭ ������.

}
