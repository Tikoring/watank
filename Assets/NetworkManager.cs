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

    // �ʱ� �ο�4 
    public Text[] PlayerList;

    // ��ġ ����
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
       

        // ������ 4���� ��ġ�� �Ҵ��ϴ� ���. �׳� ��ü ���� ���� �������� �ٲ�.

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

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("[SYSTEM] : Join Room");
        LoginSuccess();

        // ������ ��ġ�� ����������, �׳� ���ʿ� ���� �������� ����������.
        
        PhotonNetwork.Instantiate("PlayerJoin", Vector3.zero, Quaternion.identity);
        
        //Debug.Log("SpawnPlayer");
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-40, 40), 20f, 0), Quaternion.identity);
       

        // Temp Player�� ����ȭ �׽�Ʈ ����
        //PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-40, 40), 20f, 0), Quaternion.identity);




        // player ���̵� ���� ��ġ�� �Ҵ���Ѽ� �̸� ����� ���´�. 
        // Master�� ���۽� ������ ������ �� �ְ�, ���������� ������ �� �ִ�.
        // ���� ������ �����ϴ°� �ξ� ���̵��� �������δ�.


        /* foreach ( GameObject playerJoinID in GameObject.FindGameObjectsWithTag("PlayerJoinID"))
         {
             if(playerJoinID.GetComponent<PlayerJoin>().PV.IsMine)
             {
                 pos = playerJoinID.GetComponent<PlayerJoin>().myId;
             }
         }*/

        // View id�� 0,1,2,3 ���� �ٲٱ�. ����, index error
        // �ܼ� �ݺ� �ڵ� �ۼ�

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

        RoomPanel.GetComponent<RoomPanel>().PV.RPC("removeRoomPanel", RpcTarget.AllBuffered);
  


        // ����ȭ 
        //InGameUIPanel.GetComponent<InGameUI>().PV.RPC("createIngameUI", RpcTarget.AllBuffered);
        //PhotonNetwork.Instantiate("AssetTank", )

    }

    // Update is called once per frame
    // �߰��� �����ٰ� �ؼ� ��Ͽ��� ������� ����. 
    void Update()
    {
        SetPlayerList();
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
