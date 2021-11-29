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

    // �ʱ� �ο�4 
    public Text[] PlayerList;

    // ����ȭ�� �̻����� �����ı�, �÷��̾�� ������ �ְ� �����
    // ī�޶� �ش� �� �� �̻��Ͽ� ����ȭ
    // �ٵ� ������ ���� ����..
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

    //�ʱ� �ο�4 
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
        // Call back ������ OnJoinedRoom() 
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
            // ViewId �� ���� �÷��̾ �����ϰ� Lobby ��ġ�� �޸� �Ѵ�. 
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

        // ActorNumber �� identifying �� �� �ִ�.
        // ���常 start ��ư�� Ȱ��ȭ�ȴ�.
        // �̸� �������� �÷��̾���� ready ��ɵ� ������ �����ϴ�.
        // ���� RoomPanel �� photonView �� ������� �ʾƵ� �ȴ�.

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            startBtn.SetActive(true);
        }
        else
        {
            startBtn.SetActive(false);
        }


        // �α��� ������ ������ View id �� ������ �ִ� �÷��̾ ã�� �� �÷��̾���
        // ���� ��ư�� Ȱ��ȭ �����ش�.

        /*
        foreach (GameObject playerJoinID in GameObject.FindGameObjectsWithTag("PlayerJoinID"))
        {
            
            // �� id �� �����̰�, �� ������ �� �ڽ��̶��
            if (playerJoinID.GetComponent<PlayerJoin>().PV.IsMine)
            {
                startBtn.SetActive(true);
            }
            else
            // ������ �� �ڽ��� �ƴ϶�� 
            {
                startBtn.SetActive(false);
            }
        }
        */


    }

    public void GameStart()
    {
        // ����ȭ 
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
            // ViewId �� ���� �÷��̾ �����ϰ� Lobby ��ġ�� �޸� �Ѵ�. 
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
