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
    public GameObject InGameUI;


    public GameObject playerJoinID;
    public GameObject startBtn;

    // 초기 인원4 
    public Text[] PlayerList;

    // 동기화된 미사일이 지형파괴, 플레이어에게 데미지 주게 만들기
    // 카메라 해당 턴 및 미사일에 동기화
    // 근데 포톤이 맛이 갔어..
    // 

    void Awake()
    {
        Screen.SetResolution(1280, 720, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    // Start is called before the first frame update
    void Start()
    {
        setPlayerListEmpty();
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
        
        //LoginSuccess();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[SYSTEM] : OnConnectedToMaster");
        // Call back 성공시 OnJoinedRoom() 
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("[SYSTEM] : Join Room");
        LoginSuccess();

        Vector3 rPos = new Vector3(Random.Range(-40f, 40f),30f,0);
        PhotonNetwork.Instantiate("PlayerJoin", Vector3.zero, Quaternion.identity);

        //PhotonNetwork.Instantiate("AssetTank", rPos , Quaternion.identity);

        Vector3 pos1 = new Vector3(-22f, 10f, 0);
        Vector3 pos2 = new Vector3(25f, 10f, 0);


        foreach (GameObject playerJoinID in GameObject.FindGameObjectsWithTag("PlayerJoinID"))
        {
            // ViewId 에 따라서 플레이어를 구분하고 Lobby 위치를 달리 한다. 
            if (playerJoinID.GetPhotonView().ViewID == 1001)
            {
                PhotonNetwork.Instantiate("AssetTank", pos1, Quaternion.identity);
            }
            else if (playerJoinID.GetPhotonView().ViewID == 3001)
            {
                PlayerList[1].text = playerJoinID.GetComponent<PlayerJoin>().playerName;
            }
            else if (playerJoinID.GetPhotonView().ViewID == 2001)
            {
                PhotonNetwork.Instantiate("AssetTank", pos2, Quaternion.identity);
            }
            else if (playerJoinID.GetPhotonView().ViewID == 4001)
            {
                PlayerList[3].text = playerJoinID.GetComponent<PlayerJoin>().playerName;
            }
        }


    }

    void LoginSuccess()
    {
        Debug.Log("LoginSucces()");
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
        // 동기화 
        RoomPanel.GetComponent<RoomPanel>().PV.RPC("removeRoomPanel", RpcTarget.AllBuffered);
        //InGameUI.GetComponent<InGameUI>().PV.RPC("createIngameUI", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerList();
        if (PhotonNetwork.IsConnected && Input.GetKeyDown(KeyCode.Backspace)) PhotonNetwork.Disconnect();
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
            else if (playerJoinID.GetPhotonView().ViewID == 3001)
            {
                PlayerList[1].text = playerJoinID.GetComponent<PlayerJoin>().playerName;
            }
            else if (playerJoinID.GetPhotonView().ViewID == 2001)
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
