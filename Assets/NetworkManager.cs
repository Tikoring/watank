using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{

    [Header("LoginPanel")]
    public GameObject LoginPanel;
    public InputField NickNameInput;

    [Header("RoomPanel")]
    public GameObject RoomPanel;

    [Header("InGameUI")]
    public GameObject InGameUIPanel;


    public GameObject playerJoinID;
    public GameObject gamePlayer;

    public GameObject startBtn;

    // 초기 인원4 
    public Text[] PlayerList;

    // 위치 고정
    public GameObject[] spawnPos;
    public Vector3[] playerSpawn;


    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

    }

    // Start is called before the first frame update
    void Start()
    {
        setPlayerListEmpty();
        InGameUIPanel = GameObject.FindGameObjectWithTag("InGameUITag");
       

        // 지정된 4개의 위치를 할당하는 방식. 그냥 전체 범위 랜덤 스폰으로 바꿈.

   /*     
        spawnPos = new GameObject[4];
        playerSpawn = new Vector3[4];
*/
        /* for(int i = 0; i < 4; i++)
         {  
             Debug.Log("Player pos Set");
             string s = "spawnPos";
             string t = s + i.ToString();
             spawnPos[i] = GameObject.FindGameObjectWithTag(t);
         }*/

    /*    spawnPos[0] = GameObject.FindGameObjectWithTag("spawnPos0");
        spawnPos[1] = GameObject.FindGameObjectWithTag("spawnPos1");
        spawnPos[2] = GameObject.FindGameObjectWithTag("spawnPos2");
        spawnPos[3] = GameObject.FindGameObjectWithTag("spawnPos3");
    */
       /* for (int i = 0; i < 4; i++)
        {
            Debug.Log("Player spawn pos set");
            playerSpawn[i] = new Vector3(spawnPos[i].transform.position.x, spawnPos[i].transform.position.y,
                spawnPos[i].transform.position.z);
        }*/

    }


    PlayerScript FindPlayer()
    {
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Player.GetPhotonView().IsMine) return Player.GetComponent<PlayerScript>();
        }
        return null;
    }

    //초기 인원4 
    void setPlayerListEmpty()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerList[i].text = "Empty";
        }
    }
    public void Connect()
    {
        Debug.Log("[SYSTEM] : Conntect()");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("[SYSTEM] : Join Room");
        LoginSuccess();

        // 정해진 위치로 돌리지말고, 그냥 애초에 랜덤 스폰으로 만들어버리자.
        
        PhotonNetwork.Instantiate("PlayerJoin", Vector3.zero, Quaternion.identity);
        
        //Debug.Log("SpawnPlayer");
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-40, 40), 20f, 0), Quaternion.identity);
       

        // Temp Player로 동기화 테스트 진행
        //PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-40, 40), 20f, 0), Quaternion.identity);




        // player 아이디에 따라 위치를 할당시켜서 미리 만들어 놓는다. 
        // Master가 시작시 스폰을 진행할 수 있고, 개별적으로 스폰할 수 있다.
        // 개별 적으로 스폰하는게 훨씬 난이도가 쉬워보인다.


        /* foreach ( GameObject playerJoinID in GameObject.FindGameObjectsWithTag("PlayerJoinID"))
         {
             if(playerJoinID.GetComponent<PlayerJoin>().PV.IsMine)
             {
                 pos = playerJoinID.GetComponent<PlayerJoin>().myId;
             }
         }*/

        // View id를 0,1,2,3 으로 바꾸기. 실패, index error
        // 단순 반복 코드 작성

        //switch (pos)
        //{
        //    case 1001:
        //        //PhotonNetwork.Instantiate("AssetTank", playerSpawn[0], Quaternion.identity);

        //        // Player Making Test
        //        PhotonNetwork.Instantiate("Player", playerSpawn[0], Quaternion.identity);
        //        break;
        //    case 2001:
        //        PhotonNetwork.Instantiate("AssetTank", playerSpawn[1], Quaternion.identity);
        //        break;
        //    case 3001:
        //        PhotonNetwork.Instantiate("AssetTank", playerSpawn[2], Quaternion.identity);
        //        break;
        //    case 4001:
        //        PhotonNetwork.Instantiate("AssetTank", playerSpawn[3], Quaternion.identity);
        //        break;

        //}



    }

   
       
    void LoginSuccess()
    {
        LoginPanel.SetActive(false);
        RoomPanel.SetActive(true);

        // ActorNumber 로 identifying 할 수 있다.
        // 방장만 start 버튼이 활성화된다.
        // 이를 바탕으로 플레이어들의 ready 기능도 구현이 가능하다.
        // 굳이 RoomPanel 에 photonView 를 사용하지 않아도 된다.

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            startBtn.SetActive(true);
        }
        else
        {
            startBtn.SetActive(false);
        }


        // 로그인 성공시 방장의 View id 를 가지고 있는 플레이어를 찾아 그 플레이어의
        // 시작 버튼을 활성화 시켜준다.

        /*
        foreach (GameObject playerJoinID in GameObject.FindGameObjectsWithTag("PlayerJoinID"))
        {
            
            // 뷰 id 가 방장이고, 그 방장이 나 자신이라면
            if (playerJoinID.GetComponent<PlayerJoin>().PV.IsMine)
            {
                startBtn.SetActive(true);
            }
            else
            // 방장이 나 자신이 아니라면 
            {
                startBtn.SetActive(false);
            }
        }
        */


    }

    public void GameStart()
    {

        RoomPanel.GetComponent<RoomPanel>().PV.RPC("removeRoomPanel", RpcTarget.AllBuffered);
  


        // 동기화 
        //InGameUIPanel.GetComponent<InGameUI>().PV.RPC("createIngameUI", RpcTarget.AllBuffered);
        //PhotonNetwork.Instantiate("AssetTank", )

    }

    // Update is called once per frame
    // 중간에 나간다고 해서 목록에서 사라지지 않음. 
    void Update()
    {
        SetPlayerList();
    }

    void SetPlayerList()
    {
        foreach (GameObject playerJoinID in GameObject.FindGameObjectsWithTag("PlayerJoinID"))
        {
            // ViewId 에 따라서 플레이어를 구분하고 Lobby 위치를 달리 한다. 
            if (playerJoinID.GetPhotonView().ViewID == 1001)
            {
                PlayerList[0].text = playerJoinID.GetComponent<PlayerJoin>().playerName;
            }
            else if (playerJoinID.GetPhotonView().ViewID == 2001)
            {
                PlayerList[1].text = playerJoinID.GetComponent<PlayerJoin>().playerName;
            }
            else if (playerJoinID.GetPhotonView().ViewID == 3001)
            {
                PlayerList[2].text = playerJoinID.GetComponent<PlayerJoin>().playerName;
            }
            else if (playerJoinID.GetPhotonView().ViewID == 4001)
            {
                PlayerList[3].text = playerJoinID.GetComponent<PlayerJoin>().playerName;
            }
        }
    }


}
